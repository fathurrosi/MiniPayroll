using App.Domain.Entities;
using App.Domain.Models.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Interfaces.Repositories
{
    public interface ILogRepository
    {
        int Create(Log item);

        Task<PagingResult<Usp_GetLogPagingResult>> GetDataPaging(int pageIndex, int pageSize);
    }
}
