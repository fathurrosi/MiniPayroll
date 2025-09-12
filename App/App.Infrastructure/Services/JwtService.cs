using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using App.Application.Interfaces.Repositories;
using App.Application.Interfaces.Services;
using App.Domain.Entities;
using App.Domain.Models.Response;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace App.Infrastructure.Services
{
    public class JwtService: IJwtService
    {
        private readonly ILogger<JwtService> _logger;
        private readonly IConfiguration _configuration;        
        private readonly IUserRepository _userRepository;


        public JwtService(IConfiguration configuration, ILogger<JwtService> logger, IUserRepository userRepository)
        {
            _logger = logger;
            _configuration = configuration;            
            _userRepository = userRepository;
        }

        private string GenerateToken(string userId)
        {
            string secretKey = _configuration.GetValue<string>("Jwt:Key", "")!;
            string issuer = _configuration.GetValue<string>("Jwt:Issuer")!;
            string audience = _configuration.GetValue<string>("Jwt:Audience")!;
            int tokenLifetimeInMinutes = _configuration.GetValue<int>("Jwt:TokenExpirationInMinutes")!;

            byte[] key = Encoding.UTF8.GetBytes(secretKey);
            var claims = new[] { new Claim(ClaimTypes.NameIdentifier, userId) };

            var ssKey = new SymmetricSecurityKey(key);
            var creds = new SigningCredentials(ssKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(issuer, audience, claims, expires: DateTime.UtcNow.AddMinutes(tokenLifetimeInMinutes), signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        public LoginResponse<User> Authenticate(Domain.Models.Request.LoginRequest request)
        {
            try
            {
                int tokenLifetimeInMinutes = _configuration.GetValue<int>("Jwt:TokenExpirationInMinutes")!;
                User user = _userRepository.GetByKey(request.Username);
                if (user != null && Application.BCryptPasswordHasher.VerifyPassword(request.Password, user.Password))
                {
                    string token = GenerateToken(user.Username);
                    return new LoginResponse<User>
                    {
                        Token = token,
                        Success = true,
                        Message = "Login successful!",
                        StatusCode = 200,
                        RefreshToken = token,
                        ExpiresAt = DateTime.UtcNow.AddMinutes(tokenLifetimeInMinutes),
                        Data = user
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }

            return new LoginResponse<User>
            {
                Success = false,
                Message = "Invalid username or password",
                StatusCode = 401
            };
        }

    }
}
