using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using App.Application.Interfaces.Repositories;
using App.Domain.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace POS.Api.Controllers
{
    [EnableCors("AllowSpecificMethods")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PrevillageController : ControllerBase
    {
        private readonly ILogger<PrevillageController> _logger;
        private readonly IPrevillageRepository _previllageRepository;

        public PrevillageController(ILogger<PrevillageController> logger, IPrevillageRepository previllageRepository)
        {
            _logger = logger;
            _previllageRepository = previllageRepository;
        }

        [HttpGet("{username}")]
        public List<VUserPrevillage> Get(string username)
        {
            List<VUserPrevillage> items = new List<VUserPrevillage>();
            try
            {
                items = _previllageRepository.GetByUsername(username);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }

            return items;
        }

    }
}
