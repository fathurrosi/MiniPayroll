using App.Domain.Entities; 

namespace App.Domain.Models.Response
{
    public class LoginResponse<User> : ApiResponse<User>
    {
        public List<Role> Roles { get; set; }

        public List<VUserPrevillage> Previllages { get; set; }
    }
}
