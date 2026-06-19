using App.Domain.Entities;
using App.Domain.Models.Response;

namespace App.Application.Interfaces.Services
{
    public interface IJwtService
    {
        public LoginResponse<TblUser> Authenticate(Domain.Models.Request.LoginRequest request);
    }
}
