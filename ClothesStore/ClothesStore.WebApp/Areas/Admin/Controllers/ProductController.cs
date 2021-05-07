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
        public async Task<IActionResult> Index()
        {
            ViewBag.brand = await _brandService.GetAll();
            ViewBag.category = await _categoryService.GetAll();
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
        public async Task<IActionResult> Edit(int Id)
        {
            var obj = await _productService.GetObjectById(Id);
            ViewBag.brand = await _brandService.GetAll();
            ViewBag.category = await _categoryService.GetAll();
            ViewBag.size = await _productService.GetAllSize();
            ViewBag.color = await _productService.GetAllColor();

            // selected 
            ViewBag.config = await _productService.GetListConfigProductByProductId(Id);
            ViewBag.image = await _productService.GetListProductImageByProductId(Id);
            return View(obj);
        }

        [HttpPost]
        public async Task<JsonResult> AddOrUpdate(Product obj, List<ProductImage> images, List<ConfigProduct> config)
        {
            var res = await _productService.AddOrUpdate(obj, config, images);
            return Json(res);
        }
    }
}
