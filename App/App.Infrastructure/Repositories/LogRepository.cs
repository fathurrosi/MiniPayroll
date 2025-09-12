using Microsoft.EntityFrameworkCore;
using App.Application.Interfaces.Repositories;
using App.Domain.Entities;
using App.Domain.Models.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Infrastructure.Repositories
{
    public class LogRepository : ILogRepository
    {
        readonly AppContext _context;

        public LogRepository(AppContext context) { _context = context; }
        public int Create(Log item)
        {
            this._context.Entry(item).State = EntityState.Added;
            return this._context.SaveChanges();
        }

        public Task<PagingResult<Usp_GetLogPagingResult>> GetDataPaging(int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }
    }
}
