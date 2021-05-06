using ClothesStore.Model.Model.EF;
using ClothesStore.Model.ModelView;
using ClothesStore.Service.IService;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClothesStore.WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminController : Controller
    {

        private readonly IEmployeeService _employeeService;

        public AdminController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<PartialViewResult> GetListData(RequestData requestData)
        {
            var data = await _employeeService.GetListData(requestData);
            return PartialView(data);
        }

        public async Task<JsonResult> DeleteById(int id)
        {
            var result = await _employeeService.DeleteById(id);
            return Json(result);
        }

        [HttpPost]
        public async Task<PartialViewResult> GetFormAddOrEdit(int Id)
        {
            if (Id == 0)
                return PartialView(null);
            else
                return PartialView(await _employeeService.GetObjectById(Id));
        }

        [HttpPost]
        public async Task<JsonResult> AddOrUpdate(Employee obj)
        {
            var response = await _employeeService.AddOrUpdate(obj);
            return Json(new ResponseStatus() { success = response, error = null });
        }

        [HttpPost]
        public async Task<PartialViewResult> ViewDetail(int id)
        {
            var obj = await _employeeService.GetObjectById(id);
            var data = new ViewDetailObject<Employee>()
            {
                obj = obj,
                CreatedBy = "",
                UpdatedBy = ""
            };
            return PartialView(data);
        }
    }
}
