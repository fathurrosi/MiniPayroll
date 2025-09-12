using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Models.Result
{
    public class PagingResult<T>
    {
        public PagingResult() // Parameterless constructor
        {
        }
        public PagingResult(int pageIndex, int pageSize)
        {
            Items = new List<T>();
            PageIndex = pageIndex;
            PageSize = pageSize;
        }    

        public PagingResult(List<T> items, int totalCount, int pageIndex, int pageSize)
        {
            Items = items;
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = totalCount;            
        }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages
        {
            get
            {
                return (int)Math.Ceiling(TotalCount / (double)PageSize);
            }
        }
        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;
        public List<T> Items { get; set; }
    }

}
