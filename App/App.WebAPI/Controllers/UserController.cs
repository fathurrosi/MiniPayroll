using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using App.Application.Interfaces.Repositories;
using App.Domain.Entities;
using App.Domain.Models.Result;
using System.IdentityModel.Tokens.Jwt;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace POS.Api.Controllers
{

    [EnableCors("AllowSpecificMethods")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserRepository _userRepository;


        public UserController(ILogger<UserController> logger, IUserRepository userRepository)//, IConfiguration configuration)
        {
            _logger = logger;
            _userRepository = userRepository;
        //    _configuration = configuration;
        }

        // GET: api/<UserController>
        [HttpGet(Name = "GetUser")]
        public IEnumerable<User> Get()
        {
            List<User> users = new List<User>();
            try
            {
                users = _userRepository.GetAll();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
            return users.ToArray();
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public User Get(string id)
        {
            User? user = null;
            try
            {
                user = _userRepository.GetByKey(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
            return user;
        }

        [HttpGet("Paging/{pageIndex}/{pageSize}")]
        public async Task<ActionResult<PagingResult<Usp_GetUserPagingResult>>> GetDataPaging(int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                var results = await _userRepository.GetDataPaging(pageIndex, pageSize);
                if (results == null) return NotFound();
                return this.Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return this.StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public ActionResult Save(User item)
        {
            try
            {
                var results = _userRepository.Save(item);
                return this.Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            try
            {
                var results = _userRepository.Delete(id);
                return this.Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return StatusCode(500, ex.Message);
            }
        }
    }
}
