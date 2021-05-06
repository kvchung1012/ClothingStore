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
    public class CustomerController : Controller
    {
        ICustomerService _customerService;
        public CustomerController (ICustomerService brandService)
        {
            _customerService = brandService;
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
                return PartialView(await _customerService.GetObjectById(Id));
        }

        [HttpPost]
        public async Task<PartialViewResult> ViewDetail(int Id)
        {
            var obj = await _customerService.GetObjectById(Id);
            var data = new ViewDetailObject<Customer>()
            {
                obj = obj,
                CreatedBy = "",
                UpdatedBy = ""
            };
            return PartialView(data);
        }

        [HttpPost]
        public async Task<PartialViewResult> GetListData(RequestData requestData)
        {
            var response = await _customerService.GetListData(requestData);
            return PartialView(response);
        }

        [HttpPost]
        public async Task<JsonResult> GetObjectById(int Id)
        {
            var response = await _customerService.GetObjectById(Id);
            return Json(response);
        }

        [HttpPost]
        public async Task<JsonResult> AddOrUpdate(Customer obj)
        {
            var response = await _customerService.AddOrUpdate(obj);
            return Json(new ResponseStatus() { success = response, error = null });
        }

        [HttpPost]
        public async Task<JsonResult> DeleteById(int Id)
        {
            var response = await _customerService.DeleteById(Id);
            return Json(response);
        }
    }
}
