using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Core;

namespace Persistence.Components {
	public class ColumnBuilder<TModel> {
		private readonly List<ColumnDef> _columns;

		public ColumnBuilder() {
			_columns = new List<ColumnDef>();
		}

		public CellAlignment this[int index] {
			get { return _columns[index].Alignment; }
		}

		public ColumnBuilder<TModel> Column(Expression<Func<TModel, object>> expression, CellAlignment alignment = CellAlignment.Left) {
			var member = expression.Body as MemberExpression ?? (MemberExpression) (((UnaryExpression) expression.Body).Operand);
			var name = member.Member.Name;

			_columns.Add(new ColumnDef {
				Name = name,
				Caption = name.ToSentenceCase(),
				Alignment = alignment
			});

			return this;
		}

		public ColumnBuilder<TModel> Column(Expression<Func<TModel, object>> expression, string caption, CellAlignment alignment = CellAlignment.Left) {
			var member = expression.Body as MemberExpression ?? (MemberExpression) (((UnaryExpression) expression.Body).Operand);
			var name = member.Member.Name;

			_columns.Add(new ColumnDef {
				Name = name,
				Caption = caption,
				Alignment = alignment
			});

			return this;
		}

		public object[] ToArray() {
			return
				_columns
					.Select(x => new {
						name = x.Name,
						caption = x.Caption,
						alignment = x.Alignment
					})
					.Cast<object>()
					.ToArray();
		}

		private class ColumnDef {
			public string Name { get; set; }

			public string Caption { get; set; }

			public CellAlignment Alignment { get; set; }
		}
	}
}