using App.Domain.Entities;
using App.Domain.Models.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Interfaces.Repositories
{
    public interface IRoleRepository
    {
        List<Role> GetByUsername(string username);
        
        List<Role> GetAll();
        //int Create(Role item);
        //int Update(Role item);
        int Delete(int id);
        int Save(Role item);
        Role GetById(int id);
        Task<PagingResult<Usp_GetRolePagingResult>> GetDataPaging(int pageIndex, int pageSize);
    }
}
