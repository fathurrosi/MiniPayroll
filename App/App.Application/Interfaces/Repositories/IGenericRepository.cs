using App.Domain.Models.Request;
using App.Domain.Models.Response; 
using System.Linq.Expressions;
 
namespace App.Application.Interfaces.Repositories
{

    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<List<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate); 
        Task<PagedResponse<TEntity>> GetPagedAsync(DataTableRequest request);
        Task<PagedResponse<TEntity>> GetPagedAsync(DataTableRequest request, bool enableColumnSearch);
        Task<PagedResponse<TEntity>> GetPagedAsync(Expression<Func<TEntity, bool>> predicate, DataTableRequest request);
        Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> AddAsync(TEntity entity);
        Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate);
        Task<List<TEntity>> GetListAsync();
        Task<TEntity?> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity?> GetFirstOrDefaultAsync();

        Task<TEntity?> GetFirstOrDefaultAsync<TOrderKey>(Expression<Func<TEntity, TOrderKey>> orderBy, bool descending = false);
        Task<int> RemoveRangeAsync(List<TEntity> entities);
        Task<int> Remove(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity);
        Task<IEnumerable<TEntity>> UpdateRangeAsync(IEnumerable<TEntity> entities);
        Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities);
        IQueryable<TEntity> Query();
        IQueryable<TEntity> GetQueryable(Expression<Func<TEntity, bool>> predicate = null);
        Task<PagedResponse<TEntity>> GetPagedFromQueryAsync(IQueryable<TEntity> query, DataTableRequest model);
    }

}
