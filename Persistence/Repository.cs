using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using Persistence.Components;
using Persistence.Interfaces;

namespace Persistence {
	public class Repository : IRepository {
		private readonly PersistenceContext _context;
		private readonly ObjectContext _objectContext;

		public Repository(PersistenceContext context) {
			_context = context;
			_objectContext = ((IObjectContextAdapter) context).ObjectContext;
		}

		public string GetTableName<TEntity>() {
			var container = _objectContext.MetadataWorkspace.GetEntityContainer(_objectContext.DefaultContainerName, DataSpace.CSpace);
			var name = container.EntitySets.First(x => x.ElementType.Name == typeof (TEntity).Name).Name;

			return name;
		}

		public IQueryable<TEntity> AsQueryable<TEntity>() where TEntity : class {
			return _context.Set<TEntity>();
		}

		public async Task<int> ExecuteStoreCommandAsync(string commandText, params object[] parameters) {
			var result = await _objectContext.ExecuteStoreCommandAsync(commandText, parameters);

			return result;
		}

		public async Task<List<TEntity>> ExecuteStoreQueryAsync<TEntity>(string commandText, params object[] parameters) {
			var result = await _objectContext.ExecuteStoreQueryAsync<TEntity>(commandText, parameters);

			return result.ToList();
		}

		public async Task<List<TEntity>> GetAllAsync<TEntity>() where TEntity : class {
			var result =
				await
					_context
						.Set<TEntity>()
						.ToListAsync();

			return result;
		}

		public QueryBuilder<TEntity> GetPage<TEntity>(QueryParameters parameters) {
			return new QueryBuilder<TEntity>(this, parameters);
		}

		public QueryBuilder<TEntity, TModel> GetPage<TEntity, TModel>(QueryParameters parameters) {
			return new QueryBuilder<TEntity, TModel>(this, parameters);
		}

		public async Task<TEntity> GetAsync<TEntity>(int id) where TEntity : class, IEntity {
			var result =
				await
					_context
						.Set<TEntity>()
						.FirstOrDefaultAsync(x => x.Id == id);

			return result;
		}

		public async Task InsertAsync<TEntity>(TEntity entity) where TEntity : class, IEntity {
			entity.CreatedDate = DateTime.UtcNow;

			_context
				.Set<TEntity>()
				.Add(entity);

			await _context.SaveChangesAsync();
		}

		public async Task UpdateAsync<TEntity>(TEntity entity) where TEntity : class, IEntity {
			entity.ModifiedDate = DateTime.UtcNow;

			await _context.SaveChangesAsync();
		}

		public async Task DeleteAsync<TEntity>(TEntity entity) where TEntity : class {
			_context
				.Set<TEntity>()
				.Remove(entity);

			await _context.SaveChangesAsync();
		}
	}
}