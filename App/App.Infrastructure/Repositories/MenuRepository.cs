using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using App.Application.Interfaces.Repositories;
using App.Domain.Entities;
using App.Domain.Models.Result;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace App.Infrastructure.Repositories
{
    public class MenuRepository : IMenuRepository
    {
        readonly AppContext _context;

        public MenuRepository(AppContext context) { _context = context; }

        public List<Menu> GetAll()
        {
            return this._context.Menus.AsNoTracking().ToList();
        }

        public Menu GetById(int id)
        {
            Menu? item = this._context.Menus.Find(id);
            if (item != null)
            {
                this._context.Entry(item).State = EntityState.Detached;
            }
            return item;
        }

        public List<Menu> GetByUsername(string username)
        {
            var pUsername = new SqlParameter("@Username", username);
            return _context.Menus.FromSqlRaw("exec [dbo].[Usp_GetMenuByUsername] @Username", pUsername).ToList();
        }

        public async Task<PagingResult<Usp_GetMenuPagingResult>> GetDataPaging(int pageIndex, int pageSize)
        {
            var paramTotalRecord = new SqlParameter("@totalRecord", SqlDbType.Int);
            paramTotalRecord.Direction = ParameterDirection.Output;

            var sqlParameters = new[]
            {
                new SqlParameter("@text", ""),
                new SqlParameter("@pageIndex", pageIndex),
                new SqlParameter("@pageSize", pageSize),
                paramTotalRecord,
            };

            PagingResult<Usp_GetMenuPagingResult> result = new PagingResult<Usp_GetMenuPagingResult>(pageIndex, pageSize);
            List<Usp_GetMenuPagingResult> items = await _context.SqlQueryAsync<Usp_GetMenuPagingResult>("EXEC [dbo].[Usp_GetMenuPaging] @text = @text, @pageIndex = @pageIndex, @pageSize = @pageSize, @totalRecord = @totalRecord OUTPUT", sqlParameters, default);

            result.Items = items;
            result.TotalCount = (int)paramTotalRecord.Value;

            return result;
        }

        public int Save(Menu item)
        {
            Menu existing = GetById(item.Id);
            if (existing != null)
            {
                return Update(item);
            }
            else
            {
                return Create(item);
            }
        }


        private int Update(Menu item)
        {

            item.ModifiedBy = "system";
            item.ModifiedDate = DateTime.Now;
            this._context.Entry(item).State = EntityState.Modified;
            return this._context.SaveChanges();
        }

        private int Create(Menu item)
        {

            item.CreatedBy = "system";
            item.CreatedDate = DateTime.Now;
            this._context.Entry(item).State = EntityState.Added;
            return this._context.SaveChanges();
        }

        public int Delete(int id)
        {
            Menu? item = this._context.Menus.Find(id);
            if (item != null)
            {
                this._context.Menus.Remove(item);
                return this._context.SaveChanges();
            }

            return -1;
        }

    }
}
