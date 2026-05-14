using App.Application.Interfaces.Repositories;
using App.Domain.Models.Request;
using App.Domain.Models.Response;
using App.Infrastructure.Data;
using App.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore; 
using System.Linq.Expressions; 

namespace App.Infrastructure.Repositories
{

    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly AppDBContext _context;
        public GenericRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            var entry = await _context.Set<TEntity>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entry.Entity;
        }
        public async Task<TEntity?> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return await _context.Set<TEntity>().FirstOrDefaultAsync(predicate);
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        public async Task<PagedResponse<TEntity>> GetPagedAsync(DataTableRequest request)
        {
            var entities = await _context.Set<TEntity>()
                .ApplyDataTableFilters(request)
                .ApplyDataTableSorting(request)
                .ApplyDataTablePaging(request)
                .AsNoTracking()
                .ToListAsync();

            var filteredRecords = await _context.Set<TEntity>().ApplyDataTableFilters(request).CountAsync();
            var totalRecords = await _context.Set<TEntity>().CountAsync();

            return new PagedResponse<TEntity>(entities, totalRecords, filteredRecords, request);
        }
        public async Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _context.Set<TEntity>()
                .Where(predicate)
                .ToListAsync();
        }
        //public async Task<PagedResponse<TEntity>> GetAllAsync(DataTableRequest request)
        //{
        //    var entities = await _context.Set<TEntity>()
        //                  .ApplyDataTableFilters(request)
        //                  .ApplyDataTableSorting(request)
        //                  .AsNoTracking()
        //                  .ToListAsync();

        //    var filteredRecords = await _context.Set<TEntity>().ApplyDataTableFilters(request).CountAsync();
        //    var totalRecords = await _context.Set<TEntity>().CountAsync();

        //    return new PagedResponse<TEntity>(entities, totalRecords, filteredRecords, request);
        //}
        public async Task<PagedResponse<TEntity>> GetPagedAsync(Expression<Func<TEntity, bool>> predicate, DataTableRequest request)
        {
            var entities = await _context.Set<TEntity>()
                .Where(predicate)
               .ApplyDataTableFilters(request)
               .ApplyDataTableSorting(request)
               .ApplyDataTablePaging(request)
               .AsNoTracking()
               .ToListAsync();

            var filteredRecords = await _context.Set<TEntity>().Where(predicate).ApplyDataTableFilters(request).CountAsync();
            var totalRecords = await _context.Set<TEntity>().Where(predicate).CountAsync();

            return new PagedResponse<TEntity>(entities, totalRecords, filteredRecords, request);
        }
        public async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _context.Set<TEntity>().AsNoTracking().Where(predicate).ToListAsync();
        }

        public async Task<List<TEntity>> GetListAsync()
        {
            return await _context.Set<TEntity>().AsNoTracking().ToListAsync();
        }
        public async Task<TEntity?> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _context.Set<TEntity>().AsNoTracking().Where(predicate).FirstOrDefaultAsync();
        }


        public async Task<TEntity?> GetFirstOrDefaultAsync()
        {
            return await _context.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<TEntity?> GetFirstOrDefaultAsync<TOrderKey>(Expression<Func<TEntity, TOrderKey>> orderBy, bool descending = false)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>().AsNoTracking();

            query = descending
                ? query.OrderByDescending(orderBy)
                : query.OrderBy(orderBy);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<PagedResponse<TEntity>> GetPagedAsync(DataTableRequest request, bool enableColumnSearch)
        {
            var entities = await _context.Set<TEntity>()
             .ApplyDataTableFilters(request, enableColumnSearch)
             .ApplyDataTableSorting(request)
             .ApplyDataTablePaging(request)
             .AsNoTracking()
             .ToListAsync();

            var filteredRecords = await _context.Set<TEntity>().ApplyDataTableFilters(request).CountAsync();
            var totalRecords = await _context.Set<TEntity>().CountAsync();

            return new PagedResponse<TEntity>(entities, totalRecords, filteredRecords, request);

        }

        public async Task<IEnumerable<TEntity>> UpdateRangeAsync(IEnumerable<TEntity> entities)
        {
            if (entities == null)
                return Enumerable.Empty<TEntity>();

            var entityList = entities.ToList();
            if (!entityList.Any())
                return entityList;

            foreach (var entity in entityList)
            {
                var entry = _context.Entry(entity);

                if (entry.State == EntityState.Detached)
                {
                    _context.Set<TEntity>().Attach(entity);
                }

                entry.State = EntityState.Modified;
            }

            await _context.SaveChangesAsync();

            return entityList;
        }

        public async Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities)
        {
            if (entities == null)
                return Enumerable.Empty<TEntity>();

            var entityList = entities.ToList();
            if (!entityList.Any())
                return entityList;

            await _context.Set<TEntity>().AddRangeAsync(entityList);
            await _context.SaveChangesAsync();

            return entityList;
        }

        public async Task<int> RemoveRangeAsync(List<TEntity> entities)
        {
            if (entities == null)
                return 0;

            var entityList = entities.ToList();
            if (!entityList.Any())
                return 0;

            foreach (var entity in entityList)
            {
                var entry = _context.Entry(entity);
                if (entry.State == EntityState.Detached)
                {
                    _context.Set<TEntity>().Attach(entity);
                }
            }

            _context.Set<TEntity>().RemoveRange(entityList);
            return await _context.SaveChangesAsync();
        }
        public async Task<int> Remove(TEntity entity)
        {
            var entry = _context.Entry(entity);

            if (entry.State == EntityState.Detached)
            {
                _context.Set<TEntity>().Attach(entity);
            }

            _context.Set<TEntity>().Remove(entity);
            return await _context.SaveChangesAsync();
        }
        public IQueryable<TEntity> Query()
        {
            return _context.Set<TEntity>().AsQueryable();
        }

        public IQueryable<TEntity> GetQueryable(Expression<Func<TEntity, bool>> predicate = null)
        {
            return predicate == null
                ? _context.Set<TEntity>().AsQueryable()
                : _context.Set<TEntity>().Where(predicate).AsQueryable();
        }

        public async Task<PagedResponse<TEntity>> GetPagedFromQueryAsync(IQueryable<TEntity> query, DataTableRequest model)
        {
            var total = await query.CountAsync();
            var items = await query
                .Skip(model.Start)
                .Take(model.Length > 0 ? model.Length : total)
                .AsNoTracking()
                .ToListAsync();

            return new PagedResponse<TEntity>(items, total, total, model);
        }
    }
}
