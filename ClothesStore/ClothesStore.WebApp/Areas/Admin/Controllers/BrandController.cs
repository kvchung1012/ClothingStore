using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClothesStore.Model.Model.EF;
using ClothesStore.Model.ModelView;
using ClothesStore.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace ClothesStore.WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BrandController : Controller
    {
        IBrandService _brandService;
        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
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
                return PartialView(await _brandService.GetObjectById(Id));
        }

        [HttpPost]
        public async Task<PartialViewResult> ViewDetail(int Id)
        {
            var obj = await _brandService.GetObjectById(Id);
            var data = new ViewDetailObject<Brand>()
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
            var response = await _brandService.GetListData(requestData);
            return PartialView(response);
        }

        [HttpPost]
        public async Task<JsonResult> GetObjectById(int Id)
        {
            var response = await _brandService.GetObjectById(Id);
            return Json(response);
        }

        [HttpPost]
        public async Task<JsonResult> AddOrUpdate(Brand obj)
        {
            var response = await _brandService.AddOrUpdate(obj);
            return Json(new ResponseStatus() { success = response, error = null });
        }

        [HttpPost]
        public async Task<JsonResult> DeleteById(int Id)
        {
            var response = await _brandService.DeleteById(Id);
            return Json(response);
        }
    }
}
