using App.Application.Interfaces.Services.Settings;
using App.Domain.Enums;
using App.Domain.Models;
using App.Domain.Models.Dto.Masters;
using App.Domain.Models.Request;
using App.Domain.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace App.UI.Web.Controllers
{
    [Route("Bpjs")]
    public class BpjsController : BaseController
    {
        private readonly IBpjsKesehatanService _bpjsKesehatanService;
        private readonly IBpjsKetenagakerjaanService _bpjsKetenagakerjaanService;
        private readonly ILogger<BpjsController> _logger;

        public BpjsController(
            IBpjsKesehatanService bpjsKesehatanService,
            IBpjsKetenagakerjaanService bpjsKetenagakerjaanService,
            ILogger<BpjsController> logger)
        {
            _bpjsKesehatanService = bpjsKesehatanService;
            _bpjsKetenagakerjaanService = bpjsKetenagakerjaanService;
            _logger = logger;
        }

        [HttpGet("/Bpjs")]
        [HttpGet("/Bpjs/Index")]
        public IActionResult Index()
        {
            var model = new PageModel<BpjsKesehatanDto> { Title = "Konfigurasi BPJS" };
            return View(model);
        }

        #region BPJS Kesehatan
        [HttpPost("GetListKesehatan")]
        public async Task<IActionResult> GetListKesehatan([FromBody] DataTableRequest model)
        {
            try
            {
                var result = await _bpjsKesehatanService.GetPagedAsync(model);
                return Json(new
                {
                    draw = model.Draw,
                    recordsTotal = result.TotalCount,
                    recordsFiltered = result.TotalFilteredCount,
                    data = result.Items
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting BPJS Kesehatan list");
                return StatusCode(500, new { draw = model.Draw, recordsTotal = 0, recordsFiltered = 0, data = Array.Empty<object>() });
            }
        }

        [HttpGet("GetDataKesehatan")]
        public async Task<IActionResult> GetDataKesehatan(int id)
        {
            try
            {
                var result = await _bpjsKesehatanService.GetByIdAsync(id);
                return Json(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting BPJS Kesehatan data for ID {Id}", id);
                return Json(ActionResponse.Fail(ex.Message));
            }
        }

        [HttpPost("SaveKesehatanAsync")]
        public async Task<IActionResult> SaveKesehatanAsync(PageModel<BpjsKesehatanDto> model)
        {
            try
            {
                if (model?.Item == null)
                    return Json(ActionResponse.Fail("Request data is missing or invalid."));

                var result = await _bpjsKesehatanService.SaveAsync(model.Item);
                return result != null
                    ? Json(ActionResponse.Ok("Konfigurasi BPJS Kesehatan berhasil disimpan."))
                    : Json(ActionResponse.Fail("Gagal menyimpan konfigurasi BPJS Kesehatan."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving BPJS Kesehatan config");
                return Json(ActionResponse.Fail(ex.Message));
            }
        }

        [HttpDelete("DeleteKesehatan")]
        public async Task<IActionResult> DeleteKesehatan(int id)
        {
            try
            {
                var result = await _bpjsKesehatanService.DeleteAsync(id);
                return result > 0
                    ? Ok(ActionResponse.Ok("Konfigurasi BPJS Kesehatan berhasil dihapus."))
                    : Ok(ActionResponse.Fail("Gagal menghapus konfigurasi BPJS Kesehatan."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting BPJS Kesehatan config ID {Id}", id);
                return Json(ActionResponse.Fail(ex.Message));
            }
        }
        #endregion

        #region BPJS Ketenagakerjaan
        [HttpPost("GetListKetenagakerjaan")]
        public async Task<IActionResult> GetListKetenagakerjaan([FromBody] DataTableRequest model)
        {
            try
            {
                var result = await _bpjsKetenagakerjaanService.GetPagedAsync(model);
                return Json(new
                {
                    draw = model.Draw,
                    recordsTotal = result.TotalCount,
                    recordsFiltered = result.TotalFilteredCount,
                    data = result.Items
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting BPJS Ketenagakerjaan list");
                return StatusCode(500, new { draw = model.Draw, recordsTotal = 0, recordsFiltered = 0, data = Array.Empty<object>() });
            }
        }

        [HttpGet("GetDataKetenagakerjaan")]
        public async Task<IActionResult> GetDataKetenagakerjaan(int id)
        {
            try
            {
                var result = await _bpjsKetenagakerjaanService.GetByIdAsync(id);
                return Json(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting BPJS Ketenagakerjaan data for ID {Id}", id);
                return Json(ActionResponse.Fail(ex.Message));
            }
        }

        [HttpPost("SaveKetenagakerjaanAsync")]
        public async Task<IActionResult> SaveKetenagakerjaanAsync(PageModel<BpjsKetenagakerjaanDto> model)
        {
            try
            {
                if (model?.Item == null)
                    return Json(ActionResponse.Fail("Request data is missing or invalid."));

                var result = await _bpjsKetenagakerjaanService.SaveAsync(model.Item);
                return result != null
                    ? Json(ActionResponse.Ok("Konfigurasi BPJS Ketenagakerjaan berhasil disimpan."))
                    : Json(ActionResponse.Fail("Gagal menyimpan konfigurasi BPJS Ketenagakerjaan."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving BPJS Ketenagakerjaan config");
                return Json(ActionResponse.Fail(ex.Message));
            }
        }

        [HttpDelete("DeleteKetenagakerjaan")]
        public async Task<IActionResult> DeleteKetenagakerjaan(int id)
        {
            try
            {
                var result = await _bpjsKetenagakerjaanService.DeleteAsync(id);
                return result > 0
                    ? Ok(ActionResponse.Ok("Konfigurasi BPJS Ketenagakerjaan berhasil dihapus."))
                    : Ok(ActionResponse.Fail("Gagal menghapus konfigurasi BPJS Ketenagakerjaan."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting BPJS Ketenagakerjaan config ID {Id}", id);
                return Json(ActionResponse.Fail(ex.Message));
            }
        }
        #endregion
    }
}

