using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClothesStore.Model.ModelView;
using ClothesStore.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace ClothesStore.WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<PartialViewResult> GetListData(RequestData requestData)
        {
            var data =await _productService.GetListData(requestData);
            return PartialView(data);

        }

        public IActionResult Create()
        {
            return View();
        }
        public IActionResult Edit()
        {
            return View();
        }
    }
}
