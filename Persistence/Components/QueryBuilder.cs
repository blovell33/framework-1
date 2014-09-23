using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Core;
using Persistence.Interfaces;

namespace Persistence.Components {
	public class QueryBuilder<TEntity> : QueryBuilder<TEntity, TEntity> {
		public QueryBuilder(IRepository repository, QueryParameters parameters) : base(repository, parameters) {}
	}

	public class QueryBuilder<TEntity, TModel> {
		private readonly IRepository _repository;
		private readonly QueryParameters _parameters;
		private readonly string _alias;
		private readonly string _selectClause;
		private readonly string _joinClause;

		private readonly Dictionary<string, string[]> _aliases;
		private readonly string _filter;

		private readonly ColumnBuilder<TModel> _columnBuilder;
		private readonly CellBuilder<TModel> _cellBuilder;

		public QueryBuilder(IRepository repository, QueryParameters parameters) : this(repository, parameters, null, null) {}

		private QueryBuilder(IRepository repository, QueryParameters parameters, string selectClause, string joinClause) {
			_repository = repository;
			_parameters = parameters;

			_alias = "_" + typeof (TEntity).Name;

			_selectClause =
				string.IsNullOrWhiteSpace(selectClause)
					? string.Format("{0}.*", _alias)
					: selectClause;

			_joinClause =
				string.IsNullOrWhiteSpace(joinClause)
					? string.Format("FROM [{0}] {1}", _repository.GetTableName<TEntity>(), _alias)
					: joinClause;

			_aliases = GetAliases();
			_filter = GetFilter();

			_columnBuilder = new ColumnBuilder<TModel>();
			_cellBuilder = new CellBuilder<TModel>(_columnBuilder);
		}

		public object Stats(int records) {
			var pageStart =
				records == 0
					? 0
					: (_parameters.Index * _parameters.Size) + 1;
			var pageEnd = Math.Min(pageStart + _parameters.Size - 1, records);

			return new {
				pageStart,
				pageEnd,
				page = _parameters.Index + 1,
				pages = (int) Math.Ceiling((double) records / _parameters.Size),
				records
			};
		}

		public ColumnBuilder<TModel> Column(Expression<Func<TModel, object>> expression, CellAlignment alignment = CellAlignment.Left) {
			return _columnBuilder.Column(expression, alignment);
		}

		public ColumnBuilder<TModel> Column(Expression<Func<TModel, object>> expression, string caption, CellAlignment alignment = CellAlignment.Left) {
			return _columnBuilder.Column(expression, caption, alignment);
		}

		public CellBuilder<TModel> Cell(object caption) {
			return _cellBuilder.Cell(caption.ToString());
		}

		public CellBuilder<TModel> Cell(string caption) {
			return _cellBuilder.Cell(caption);
		}

		public CellBuilder<TModel> Cell(bool caption) {
			return _cellBuilder.Cell(caption);
		}

		public CellBuilder<TModel> Cell(DateTime caption) {
			return _cellBuilder.Cell(caption);
		}

		public CellBuilder<TModel> Cell(DateTime? caption) {
			return _cellBuilder.Cell(caption);
		}

		public QueryBuilder<TEntity, TModel> Select<TType>(Expression<Func<TEntity, TType>> expression, Expression<Func<TModel, TType>> asExpression = null) {
			var member = expression.Body as MemberExpression ?? (MemberExpression) (((UnaryExpression) expression.Body).Operand);
			var name = member.Member.Name;
			var builder = new StringBuilder();

			if (!_selectClause.EndsWith("*")) {
				builder
					.Append(_selectClause)
					.Append(", ");
			}

			builder.AppendFormat("{0}.[{1}]", _alias, name);

			if (asExpression == null) {
				return new QueryBuilder<TEntity, TModel>(_repository, _parameters, builder.ToString(), _joinClause);
			}

			var asMember = asExpression.Body as MemberExpression ?? (MemberExpression) (((UnaryExpression) asExpression.Body).Operand);
			var asName = asMember.Member.Name;

			builder.AppendFormat(" AS [{0}]", asName);

			return new QueryBuilder<TEntity, TModel>(_repository, _parameters, builder.ToString(), _joinClause);
		}

		public QueryBuilder<TJoinedEntity, TModel> Join<TJoinedEntity>(Expression<Func<TEntity, TJoinedEntity>> expression) {
			var member = expression.Body as MemberExpression ?? (MemberExpression) (((UnaryExpression) expression.Body).Operand);
			var name = member.Member.Name;
			var builder = new StringBuilder();

			builder.AppendFormat("INNER JOIN [{0}] {1} ON {2}.[{3}Id] = {1}.[Id]", _repository.GetTableName<TJoinedEntity>(), "_" + name, _alias, name);

			return new QueryBuilder<TJoinedEntity, TModel>(_repository, _parameters, _selectClause, _joinClause + " " + builder);
		}

		public async Task<List<TModel>> QueryAsync() {
			var builder = new StringBuilder();

			string[] value;

			if (!_aliases.TryGetValue(_parameters.Name, out value)) {
				value = new[] {_alias, _parameters.Name};
			}

			builder
				.AppendFormat("SELECT TOP {0} *", _parameters.Size).AppendLine()
				.AppendFormat("FROM (SELECT {0}, row_number() OVER (ORDER BY {1}.[{2}] {3}) AS [row_number] {4}{5}) AS [extent]", _selectClause, value[0], value[1], _parameters.Direction, _joinClause, _filter).AppendLine()
				.AppendFormat("WHERE [extent].[row_number] > {0}", _parameters.Index * _parameters.Size).AppendLine()
				.AppendFormat("ORDER BY [extent].[{0}] {1}", _parameters.Name, _parameters.Direction).AppendLine();

			var result = await _repository.ExecuteStoreQueryAsync<TModel>(builder.ToString());

			return result;
		}

		public async Task<int> CountAsync() {
			var builder = new StringBuilder();

			builder.AppendFormat("SELECT COUNT(*) {0}{1}", _joinClause, _filter).AppendLine();

			var result = await _repository.ExecuteStoreQueryAsync<int>(builder.ToString());

			return result.First();
		}

		private Dictionary<string, string[]> GetAliases() {
			var aliases = new Dictionary<string, string[]>();

			if (_selectClause.EndsWith("*")) {
				return aliases;
			}

			var raw = _selectClause.Replace("[", null).Replace("]", null);
			var columns = raw.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList();

			foreach (var column in columns) {
				var pair = column.Split(new[] {'.'}, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList();
				var names = pair[1].Split(new[] {"AS"}, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList();

				aliases.Add(names.Last(), new[] {pair.First(), names.First()});
			}

			return aliases;
		}

		private string GetFilter() {
			if (string.IsNullOrWhiteSpace(_parameters.Filter)) {
				return null;
			}

			var properties =
				typeof (TModel)
					.GetProperties(BindingFlags.Public | BindingFlags.Instance)
					.Where(x => Attribute.IsDefined(x, typeof (FilterableAttribute)))
					.ToList();

			if (properties.Count == 0) {
				return null;
			}

			var filter = _parameters.Filter.Replace("'", "''");
			var builder = new StringBuilder();
			var prefix = "WHERE";

			foreach (var property in properties) {
				builder.AppendFormat(" {0} ", prefix);

				prefix = "OR";

				var underlyingType = Nullable.GetUnderlyingType(property.PropertyType);
				var type = underlyingType ?? property.PropertyType;

				string[] value;

				if (!_aliases.TryGetValue(property.Name, out value)) {
					value = new[] {_alias, property.Name};
				}

				if (type == typeof (string)) {
					builder.AppendFormat("charindex('{0}', {1}.[{2}] collate Latin1_General_CI_AS) != 0", filter, value[0], value[1]);
				}
				else if (type == typeof (bool)) {
					builder.AppendFormat("charindex('{0}', iif({1}.[{2}] = 1, 'Yes', 'No') collate Latin1_General_CI_AS) != 0", filter, value[0], value[1]);
				}
				else if (type == typeof (DateTime)) {
					builder.AppendFormat("charindex('{0}', convert(varchar(2), month({1}.[{2}])) + '/' + convert(varchar(2), day({1}.[{2}])) + '/' + convert(varchar(4), year({1}.[{2}])) collate Latin1_General_CI_AS) != 0", filter, value[0], value[1]);
				}
				else {
					builder.AppendFormat("charindex('{0}', convert(nvarchar, {1}.[{2}]) collate Latin1_General_CI_AS) != 0", filter, value[0], value[1]);
				}
			}

			return builder.ToString();
		}
	}
}