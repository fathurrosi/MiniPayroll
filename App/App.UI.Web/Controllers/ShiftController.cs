using App.Application.Interfaces.Services.Masters;
using App.Domain.Enums;
using App.Domain.Models;
using App.Domain.Models.Dto;
using App.Domain.Models.Request;
using App.Domain.Models.Response;
using App.Infrastructure.Services.Masters;
using App.UI.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace App.UI.Web.Controllers
{
    public class ShiftController : BaseController
    {
        private readonly ILogger<ShiftController> _logger;
        private readonly IEmployeeService _employeeService;
        private readonly IDepartmentService _departmentService;
        private readonly IShiftPatternService _ShiftPatternService;
        private readonly IShiftService _ShiftService;
        public ShiftController(IShiftService ShiftService
            , IDepartmentService departmentService
            , IEmployeeService employeeService
            , IShiftPatternService ShiftPatternService
            , ILogger<ShiftController> logger)
        {
            _employeeService = employeeService;
            _ShiftService = ShiftService;
            _ShiftPatternService = ShiftPatternService;
            _departmentService = departmentService;
            _logger = logger;
        }


        //#region Shift_Pattern



        //[HttpDelete]
        //public async Task<IActionResult> DeletePattern(int id)
        //{
        //    if (id <= 0)
        //        return BadRequest("model ID is required");

        //    try
        //    {
        //        var result = await _ShiftPatternService.DeleteAsync(id);
        //        if (result > 0)
        //            return Ok(ActionResponse.Ok($"model {id} deleted successfully"));

        //        return Ok(ActionResponse.Fail("Failed to delete model"));
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(ActionResponse.Fail(ex.Message));
        //    }
        //}


        //public async Task<IActionResult> Pattern(int? year, int? month)
        //{
        //    var model = new PageModel<ShiftPatternDto>() { Title = "Shift" };
        //    model.Item = new ShiftPatternDto();
        //    return View(model);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> SavePattern(ShiftPatternModel model)
        //{
        //    try
        //    {
        //        await _ShiftPatternService.SaveAsync(model.Item);
        //        return RedirectToAction(nameof(Pattern));
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex.ToString());
        //    }

        //    model.Shifts = await _ShiftService.GetListAsync();
        //    return View("Detail", model);
        //}

        //public async Task<IActionResult> Detail(int id)
        //{
        //    var model = new ShiftPatternModel() { Title = "Shift Pattern" };

        //    var shiftPatteen = await _ShiftPatternService.GetByIdAsync(id);
        //    if (shiftPatteen == null)
        //    {
        //        shiftPatteen = new ShiftPatternDto();
        //        shiftPatteen.Details = new List<ShiftPatternDetailDto>();
        //    }

        //    model.Item = shiftPatteen;
        //    model.Shifts = await _ShiftService.GetListAsync();
        //    return View(model);
        //}


        //[HttpPost]
        //public async Task<IActionResult> GetPatternList([FromBody] DataTableRequest model)
        //{
        //    try
        //    {
        //        var result = await _ShiftService.GetPagedPatternAsync(model);

        //        return Json(new
        //        {
        //            draw = model.Draw,
        //            recordsTotal = result.TotalCount,
        //            recordsFiltered = result.TotalFilteredCount,
        //            data = result.Items
        //        });
        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(500, new
        //        {
        //            draw = model.Draw,
        //            recordsTotal = 0,
        //            recordsFiltered = 0,
        //            data = Array.Empty<object>()
        //        });
        //    }
        //}

        //#endregion


        #region Shift 

        [HttpPost]
        public async Task<IActionResult> GetList([FromBody] DataTableRequest model)
        {
            try
            {
                var result = await _ShiftService.GetPagedAsync(model);

                return Json(new
                {
                    draw = model.Draw,
                    recordsTotal = result.TotalCount,
                    recordsFiltered = result.TotalFilteredCount,
                    data = result.Items
                });
            }
            catch (Exception)
            {
                return StatusCode(500, new
                {
                    draw = model.Draw,
                    recordsTotal = 0,
                    recordsFiltered = 0,
                    data = Array.Empty<object>()
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetData(int id)
        {
            try
            {
                var model = await _ShiftService.GetByIdAsync(id);
                return Json(model);
            }
            catch (Exception ex)
            {
                return Json(ActionResponse.Fail(ex.Message));
            }

        }




        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
                return BadRequest("model ID is required");

            try
            {
                var result = await _ShiftService.DeleteAsync(id);
                if (result > 0)
                    return Ok(ActionResponse.Ok($"model {id} deleted successfully"));

                return Ok(ActionResponse.Fail("Failed to delete model"));
            }
            catch (Exception ex)
            {
                return Json(ActionResponse.Fail(ex.Message));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(PageModel<ShiftDto> model)
        {
            try
            {
                if (model.Mode == FormMode.Create)
                {
                    var existingItem = await _ShiftService.GetByIdAsync(model.Item.Id);
                    if (existingItem != null) return Json(ActionResponse.Fail($"model {model.Item.Id} already exist!"));
                }
                var result = await _ShiftService.SaveAsync(model.Item);
                return (result != null) ? Json(ActionResponse.Ok("model saved successfully")) : Json(ActionResponse.Fail("model saved failed"));
            }
            catch (Exception ex)
            {
                return Json(ActionResponse.Fail(ex.Message));
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetDataShift(int id)
        {
            try
            {
                var model = await _ShiftService.GetByIdAsync(id);
                return Json(model);
            }
            catch (Exception ex)
            {
                return Json(ActionResponse.Fail(ex.Message));
            }

        }


        [HttpDelete]
        public async Task<IActionResult> DeleteShift(int id)
        {
            if (id <= 0)
                return BadRequest("model ID is required");

            try
            {
                var result = await _ShiftService.DeleteAsync(id);
                if (result > 0)
                    return Ok(ActionResponse.Ok($"model {id} deleted successfully"));

                return Ok(ActionResponse.Fail("Failed to delete model"));
            }
            catch (Exception ex)
            {
                return Json(ActionResponse.Fail(ex.Message));
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddShift(PageModel<ShiftDto> model)
        {
            try
            {
                if (model.Mode == FormMode.Create)
                {
                    var existingItem = await _ShiftService.GetByIdAsync(model.Item.Id);
                    if (existingItem != null) return Json(ActionResponse.Fail($"model {model.Item.Id} already exist!"));
                }
                var result = await _ShiftService.SaveAsync(model.Item);
                return (result != null) ? Json(ActionResponse.Ok("model saved successfully")) : Json(ActionResponse.Fail("model saved failed"));
            }
            catch (Exception ex)
            {
                return Json(ActionResponse.Fail(ex.Message));
            }
        }


        #endregion


        public async Task<IActionResult> Index(int? year, int? month)
        {
            var model = new PageModel<ShiftDto>() { Title = "Shift" };
            model.Item = new ShiftDto();
            return View(model);
        }




        #region CRUD_Shift_Schedule
        public async Task<IActionResult> Employee(int? year, int? month)
        {
            int selectedYear = year ?? DateTime.Now.Year;
            int selectedMonth = month ?? DateTime.Now.Month;

            DateTime dateFrom = new DateTime(selectedYear, selectedMonth, 1);
            DateTime dateTo = dateFrom.AddMonths(1).AddDays(-1);
            var model = new ShiftScheduleModel
            {
                Year = year ?? DateTime.Now.Year,
                Month = month ?? DateTime.Now.Month,
                ModalDateFrom = dateFrom,
                ModalDateTo = dateTo,
                ShiftPatterns = await _ShiftPatternService.GetListAsync(),
                Departments = await _departmentService.GetListAsync(),
                Employees = await _employeeService.GetListAsync(),
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> GenerateSchedule(GenerateScheduleRequest request)
        {
            try
            {
                if (request.DateFrom > request.DateTo)
                {
                    throw new Exception("From Date cannot be greater than To Date.");
                }

                await _ShiftService.GenerateAsync(request);
                return Json(ActionResponse.Ok("Schedule generated successfully"));
            }
            catch (Exception ex)
            {
                return Json(ActionResponse.Fail(ex.Message));
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetEmployeeShiftList([FromBody] ScheduleDataTableRequest model)
        {
            try
            {
                var result = await _ShiftService.GetPagedEmplooyeeScheduleAsync(model);

                return Json(new
                {
                    draw = model.Draw,
                    recordsTotal = result.TotalCount,
                    recordsFiltered = result.TotalFilteredCount,
                    data = result.Items
                });
            }
            catch (Exception)
            {
                return StatusCode(500, new
                {
                    draw = model.Draw,
                    recordsTotal = 0,
                    recordsFiltered = 0,
                    data = Array.Empty<object>()
                });
            }
        }
        #endregion

    }
}
