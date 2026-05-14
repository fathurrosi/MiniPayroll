using App.Domain.Models.Request;
using App.Domain.Models.Response;
using AutoMapper;
 
using System.Linq.Expressions;
namespace App.Infrastructure.Extensions
{
    public static class PagedResultExtensions
    {
        public static PagedResponse<TDto> MapPaged<TEntity, TDto>(
            this PagedResponse<TEntity> source,
            IMapper mapper,
            DataTableRequest request)
        {
            return new PagedResponse<TDto>(
                mapper.Map<List<TDto>>(source.Items),
                source.TotalCount,
                source.TotalFilteredCount,
                request
            );
        }
      
    }
}
