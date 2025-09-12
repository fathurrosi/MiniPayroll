using App.Domain.Entities;
using App.Domain.Models.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Models.Response
{
    public class LoginResponse<User> : ApiResult<User>
    {
        public List<Role> Roles { get; set; }

        public List<VUserPrevillage> Previllages { get; set; }
    }
}
