using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using App.Application;
using App.Application.Interfaces.Repositories;
using App.Domain.Entities;
using App.Domain.Models.Request;
using App.Domain.Models.Response;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace POS.Api.Controllers
{
    [EnableCors("AllowSpecificMethods")]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        private IRoleRepository _roleRepository;
        private IPrevillageRepository _previllageRepository;
        public AuthController(IConfiguration configuration,
            ILogger<AuthController> logger,
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            IPrevillageRepository previllageRepository)
        {
            _logger = logger;
            _configuration = configuration;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _previllageRepository = previllageRepository;
        }


        [HttpPost("Login")]
        [AllowAnonymous]
        public IActionResult Login(App.Domain.Models.Request.LoginRequest item)
        {
            try
            {
                var response = this.Authenticate(item);
                if (response.Success) return Ok(response);
                else return Unauthorized();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Unauthorized();
            }
        }

        [HttpPost("Refresh")]
        public IActionResult RefreshToken(RefreshTokenRequest item)
        {
            // Validate the refresh token...
            var user = _userRepository.GetByRefreshToken(item.RefreshToken);
            if (user == null || user.RefreshTokenExpires < DateTime.UtcNow)
            {
                return Unauthorized();
            }

            var newAccessToken = GenerateToken(user.Username);
            var newRefreshToken = GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpires = DateTime.UtcNow.AddMinutes(5); // Set new expiration for refresh token

            _userRepository.Save(user);

            return Ok(new RefreshTokenResponse { AccessToken = newAccessToken, RefreshToken = newRefreshToken });
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


        private LoginResponse<User> Authenticate(App.Domain.Models.Request.LoginRequest request)
        {
            try
            {
                int tokenLifetimeInMinutes = _configuration.GetValue<int>("Jwt:TokenExpirationInMinutes")!;
                User user = _userRepository.GetByKey(request.Username);
                if (user != null && BCryptPasswordHasher.VerifyPassword(request.Password, user.Password!))
                {
                    List<Role> roles = _roleRepository.GetByUsername(user.Username);
                    List<VUserPrevillage> previllages = _previllageRepository.GetByUsername(user.Username);
                    user.RefreshToken = GenerateRefreshToken();
                    user.RefreshTokenExpires = DateTime.UtcNow.AddMinutes(5);

                    _userRepository.Save(user);
                    return new LoginResponse<User>
                    {
                        Token = GenerateToken(user.Username),
                        RefreshToken = user.RefreshToken,
                        Data = user,
                        Success = true,
                        Message = "Login successful!",
                        StatusCode = 200,

                        Roles = roles,
                        ExpiresAt = DateTime.UtcNow.AddMinutes(tokenLifetimeInMinutes),
                        Previllages = previllages
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


        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

    }

}