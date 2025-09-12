using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using App.Application.Interfaces.Repositories;
using App.Domain.Entities;
using App.Domain.Models.Result;
using System.Collections.Generic;

namespace POS.Api.Controllers
{    
    [EnableCors("AllowSpecificMethods")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RoleController : ControllerBase
    {
        private readonly ILogger<RoleController> _logger;
        private readonly IRoleRepository _roleRepository;

        public RoleController(ILogger<RoleController> logger, IRoleRepository roleRepository)
        {
            _logger = logger;
            _roleRepository = roleRepository;
        }
        
        [HttpGet]
        public ActionResult<List<Role>> Get()
        {
            try
            {
                var results = _roleRepository.GetAll();
                if (results == null) return NotFound();
                return this.Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return this.StatusCode(500, ex.Message);

            }           
        }

        [HttpGet("{param}")]
        public ActionResult Get(string param)
        {
            try
            {
                if (int.TryParse(param, out int id))
                {
                    var results = _roleRepository.GetById(id);
                    if (results == null) return NotFound();
                    return this.Ok(results);
                }
                else
                {
                    var results = _roleRepository.GetByUsername(param);
                    if (results == null) return NotFound();
                    return this.Ok(results);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return this.StatusCode(500, ex.Message);
            }
        }

        [HttpGet("Paging/{pageIndex}/{pageSize}")]
        public async Task<ActionResult<PagingResult<Usp_GetRolePagingResult>>> GetDataPaging(int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                var results = await _roleRepository.GetDataPaging(pageIndex, pageSize);
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
        public ActionResult Save(Role item)
        {
            try
            {
                var results = _roleRepository.Save(item);
                return this.Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var results = _roleRepository.Delete(id);
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
