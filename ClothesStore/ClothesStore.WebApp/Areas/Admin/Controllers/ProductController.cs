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
        private readonly IBrandService _brandService;
        private readonly ICategoryService _categoryService;

        public ProductController(IProductService productService,IBrandService brandService,ICategoryService categoryService)
        {
            _productService = productService;
            _brandService = brandService;
            _categoryService = categoryService;
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

        public async Task<IActionResult> Create()
        {
            ViewBag.brand = await _brandService.GetAll();
            ViewBag.category = await _categoryService.GetAll();
            ViewBag.size = await _productService.GetAllSize();
            ViewBag.color = await _productService.GetAllColor();
            return View();
        }
        public IActionResult Edit()
        {
            return View();
        }
    }
}
