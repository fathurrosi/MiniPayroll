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
    public class UserRepository : IUserRepository
    {
        readonly AppContext _context;

        public UserRepository(AppContext context) { _context = context; }
        
        public int DeleteAll()
        {
            return this._context.Users.ExecuteDelete<User>();
        }

        public int DeleteByFK(int UserID)
        {
            string commentText = @"
delete t1
from [User] t1
inner join [UserUser] t2 on t1.Username = t2.Username
where t2.UserID =@UserID
";
            var UserIdParam = new Microsoft.Data.SqlClient.SqlParameter("@UserID", UserID);
            int result = this._context.Database.ExecuteSqlRaw(commentText, UserIdParam);
            return result;
        }

        public List<User> GetAll()
        {
            List<User> list = new List<User>();
            try
            {
                list = this._context.Users.AsNoTracking().ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }

        public User GetByKey(string username)
        {
            User? item = this._context.Users.Find(username);
            if (item != null) this._context.Entry(item).State = EntityState.Detached;
            return item;
        }

        public int Update(User item)
        {
            this._context.Entry(item).State = EntityState.Modified;
            return this._context.SaveChanges();
        }

        public async Task<PagingResult<Usp_GetUserPagingResult>> GetDataPaging(int pageIndex, int pageSize)
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

            PagingResult<Usp_GetUserPagingResult> result = new PagingResult<Usp_GetUserPagingResult>(pageIndex, pageSize);
            result.Items = await _context.SqlQueryAsync<Usp_GetUserPagingResult>("EXEC [dbo].[Usp_GetUserPaging] @text = @text, @pageIndex = @pageIndex, @pageSize = @pageSize, @totalRecord = @totalRecord OUTPUT", sqlParameters, default);

            result.TotalCount = (int)paramTotalRecord.Value;

            return result;
        }


        public int Save(User item)
        {
            User existing = GetByKey(item.Username);
            if (existing != null)
            {
                return Update(item);
            }
            else
            {
                return Create(item);
            }
        }

        public int Create(User item)
        {
            this._context.Entry(item).State = EntityState.Added;
            return this._context.SaveChanges();
        }

        public int Delete(string username)
        {
            User? item = this._context.Users.Find(username);
            if (item != null)
            {
                //this._context.Users.Where(t => t.username == username).ExecuteDelete();
                this._context.Users.Remove(item);
                return this._context.SaveChanges();
            }
            else
            {
                throw new ArgumentNullException();
            }
        }

        public User GetByRefreshToken(string refreshToken)
        {
            User? item = this._context.Users.FirstOrDefault(t => t.RefreshToken == refreshToken);
            if (item != null) this._context.Entry(item).State = EntityState.Detached;
            return item;
        }


    }
}
