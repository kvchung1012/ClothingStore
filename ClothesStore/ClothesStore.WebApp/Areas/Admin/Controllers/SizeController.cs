using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ClothesStore.Model.Model.EF;
using ClothesStore.Model.ModelView;
using ClothesStore.Service.IService;
using ClothesStore.WebApp.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClothesStore.WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SizeController : Controller
    {
        ISizeService _sizeService;
        IEmployeeService _employeeService;
        IHttpContextAccessor _HttpContextAccessor;

        public SizeController(ISizeService sizeService, IEmployeeService employeeService, IHttpContextAccessor HttpContextAccessor)
        {
            _sizeService = sizeService;
            _employeeService = employeeService;
            _HttpContextAccessor = HttpContextAccessor;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<PartialViewResult> GetFormAddOrEdit(int Id)
        {
            if (Id == 0)
                return PartialView(null);
            else
                return PartialView(await _sizeService.GetObjectById(Id));
        }

        [HttpPost]
        public async Task<PartialViewResult> ViewDetail(int Id)
        {
            var obj = await _sizeService.GetObjectById(Id);
            var empUpdated = obj.UpdatedBy != null ? await _employeeService.GetObjectById((int)obj.UpdatedBy) : new Employee();
            var empCreated = obj.CreatedBy != null ? await _employeeService.GetObjectById((int)obj.CreatedBy) : new Employee();

            var data = new ViewDetailObject<Size>()
            {
                obj = obj,
                CreatedBy = empCreated.Name,
                UpdatedBy = empUpdated.Name
            };
            return PartialView(data);
        }

        [HttpPost]
        public async Task<PartialViewResult> GetListData(RequestData requestData)
        {
            var response = await _sizeService.GetListData(requestData);
            return PartialView(response);
        }

        [HttpPost]
        public async Task<JsonResult> GetObjectById(int Id)
        {
            var response = await _sizeService.GetObjectById(Id);
            return Json(response);
        }

        [HttpPost]
        public async Task<JsonResult> AddOrUpdate(Size obj)
        {
            string jsonUser = _HttpContextAccessor.HttpContext.Session.GetString(Constant.USER);
            var emp = new Employee();
            if (jsonUser != null)
            {
                emp = JsonSerializer.Deserialize<Employee>(jsonUser) as Employee;
                if (obj.Id == 0)
                    obj.CreatedBy = emp.Id;
                else
                {
                    obj.UpdatedBy = emp.Id;
                }
            }
            var response = await _sizeService.AddOrUpdate(obj);
            return Json(new ResponseStatus() { success = response, error = null });
        }

        [HttpPost]
        public async Task<JsonResult> DeleteById(int Id)
        {
            var response = await _sizeService.DeleteById(Id);
            return Json(response);
        }
    }
}
