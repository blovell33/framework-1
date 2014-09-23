using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Persistence.Components;

namespace Persistence.Interfaces {
	public interface IRepository {
		string GetTableName<TEntity>();

		IQueryable<TEntity> AsQueryable<TEntity>() where TEntity : class;

		Task<int> ExecuteStoreCommandAsync(string commandText, params object[] parameters);

		Task<List<TEntity>> ExecuteStoreQueryAsync<TEntity>(string commandText, params object[] parameters);

		Task<List<TEntity>> GetAllAsync<TEntity>() where TEntity : class;

		QueryBuilder<TEntity> GetPage<TEntity>(QueryParameters parameters);

		QueryBuilder<TEntity, TModel> GetPage<TEntity, TModel>(QueryParameters parameters);

		Task<TEntity> GetAsync<TEntity>(int id) where TEntity : class, IEntity;

		Task InsertAsync<TEntity>(TEntity entity) where TEntity : class, IEntity;

		Task UpdateAsync<TEntity>(TEntity entity) where TEntity : class, IEntity;

		Task DeleteAsync<TEntity>(TEntity entity) where TEntity : class;
	}
}