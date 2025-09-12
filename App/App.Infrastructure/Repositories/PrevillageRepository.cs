using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using App.Application.Interfaces.Repositories;
using App.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace App.Infrastructure.Repositories
{
    public class PrevillageRepository : IPrevillageRepository
    {
        readonly AppContext _context;

        public PrevillageRepository(AppContext context) { _context = context; }

        //public int Create(Role item)
        //{
        //    this._context.Entry(item).State = EntityState.Added;
        //    return this._context.SaveChanges();
        //}

        public List<VUserPrevillage> GetByUsername(string username)
        {
            var userParam = new SqlParameter("@Username", username);
            //List<Previllage> items = _context.Previllages.FromSqlInterpolated($"EXECUTE dbo.Usp_GetPrevillageByUsername @Username=@Username").ToList();
            List<VUserPrevillage> items = _context.VUserPrevillages.FromSqlRaw("EXECUTE [dbo].[Usp_GetUserPrevillageByUsername] @Username", userParam).ToList();
            return items;
        }
    }
}
