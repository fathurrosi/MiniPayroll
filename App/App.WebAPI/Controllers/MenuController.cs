using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using App.Application.Interfaces.Repositories;
using App.Domain.Entities;
using App.Domain.Models.Result;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace POS.Api.Controllers
{
    
    [EnableCors("AllowSpecificMethods")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class MenuController : ControllerBase
    {
        private readonly ILogger<MenuController> _logger;
        private readonly IMenuRepository _menuRepository;

        public MenuController(ILogger<MenuController> logger, IMenuRepository menuRepository)
        {
            _logger = logger;
            _menuRepository = menuRepository;
        }

        [HttpGet]
        public ActionResult<List<Menu>> Get()
        {
            try
            {
                var results = _menuRepository.GetAll();
                if (results == null) return NotFound();
                return this.Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return this.StatusCode(500, "Internal Server Error");
            }

        }
        [HttpGet("{param}")]
        public ActionResult Get(string param)
        {
            try
            {
                if (int.TryParse(param, out int id))
                {
                    var results = _menuRepository.GetById(id);
                    if (results == null) return NotFound();
                    return this.Ok(results);
                }
                else
                {
                    var results = _menuRepository.GetByUsername(param);
                    if (results == null) return NotFound();
                    return this.Ok(results);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return this.StatusCode(500, "Internal Server Error");
            }
        }




        [HttpGet("Paging/{pageIndex}/{pageSize}")]
        public async Task<ActionResult<PagingResult<Usp_GetMenuPagingResult>>> GetDataPaging(int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                var results = await _menuRepository.GetDataPaging(pageIndex, pageSize);
                if (results == null) return NotFound();
                return this.Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return this.StatusCode(500, "Internal Server Error");
            }

        }

        [HttpPost]
        public ActionResult Save(Menu item)
        {
            try
            {

                var results = _menuRepository.Save(item);
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
                var results = _menuRepository.Delete(id);
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
