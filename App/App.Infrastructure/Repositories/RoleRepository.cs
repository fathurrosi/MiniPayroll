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
using System.Threading.Tasks;

namespace App.Infrastructure.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        readonly AppContext _context;

        public RoleRepository(AppContext context) { _context = context; }

        public int Delete(int id)
        {
            Role? item = this._context.Roles.Find(id);
            if (item != null)
            {
                this._context.Roles.Remove(item);
                return this._context.SaveChanges();
            }
            return -1;
        }


        public List<Role> GetAll()
        {
            return this._context.Roles.AsNoTracking().ToList();
        }

        public Role GetById(int id)
        {
            Role? item = this._context.Roles.Find(id);
            if (item != null)
            {
                this._context.Entry(item).State = EntityState.Detached;
            }
            return item;
        }


        public List<Role> GetByUsername(string username)
        {
            var userParam = new SqlParameter("@Username", username);
            List<Role> items = _context.Roles.FromSqlRaw("EXECUTE [dbo].[Usp_GetRoleByUsername] @Username", userParam).ToList();
            return items;
        }

        public async Task<PagingResult<Usp_GetRolePagingResult>> GetDataPaging(int pageIndex, int pageSize)
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

            PagingResult<Usp_GetRolePagingResult> result = new PagingResult<Usp_GetRolePagingResult>(pageIndex, pageSize);
            result.Items = await _context.SqlQueryAsync<Usp_GetRolePagingResult>("EXEC [dbo].[Usp_GetRolePaging] @text = @text, @pageIndex = @pageIndex, @pageSize = @pageSize, @totalRecord = @totalRecord OUTPUT", sqlParameters, default);
            result.TotalCount = (int)paramTotalRecord.Value;

            return result;
        }

        public int Save(Role item)
        {
            Role existing = GetById(item.Id);
            if (existing != null)
            {
                return Update(item);
            }
            else
            {
                return Create(item);
            }
        }

        private int Create(Role item)
        {

            item.CreatedBy = "system";
            item.CreatedDate = DateTime.Now;
            this._context.Entry(item).State = EntityState.Added;
            return this._context.SaveChanges();
        }

        private int Update(Role item)
        {
            item.ModifiedBy = "system";
            item.ModifiedDate = DateTime.Now;
            this._context.Entry(item).State = EntityState.Modified;
            return this._context.SaveChanges();
        }
    }
}
