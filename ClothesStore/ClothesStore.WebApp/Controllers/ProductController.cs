using ClothesStore.Model.Model.EF;
using ClothesStore.Model.ModelView;
using ClothesStore.Model.ModelView.ProductConfig;
using ClothesStore.Service.IService;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClothesStore.WebApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IBrandService _brandService;
        private readonly ICategoryService _categoryService;
        private readonly IColorService _colorService;

        public ProductController(IProductService productService, IBrandService brandService, ICategoryService categoryService, IColorService colorService)
        {
            _productService = productService;
            _brandService = brandService;
            _categoryService = categoryService;
            _colorService = colorService;
        }

        public async Task<IActionResult> Index(int? CateId, int? BrandId)
        {
            ViewBag.brand = (await _brandService.GetAll());
            ViewBag.category = (await _categoryService.GetAll());
            ViewBag.color = (await _colorService.GetAll());

            ViewBag.CateId = CateId;
            ViewBag.BrandId = BrandId;
            // set filter
            FilterProduct filterProduct = new FilterProduct();
            if (CateId != null)
                filterProduct.categoryId = (int)CateId;
            if (BrandId != null)
            {
                filterProduct.brands = new List<int>();
                filterProduct.brands.Add((int)BrandId);
            }    
            var data = (await _productService.GetListProduct(filterProduct, 0));
            return View(data);
        }

        [HttpPost]
        public async Task<PartialViewResult> GetProduct(FilterProduct filter)
        {
            var data = (await _productService.GetListProduct(filter, 0));
            return PartialView(data);
        }

        public async Task<IActionResult> Detail(int Id)
        {
            var product = await _productService.GetInforProduct(Id);
            return View(product);
        }

        [HttpPost]
        public async Task<PartialViewResult> GetInforProduct(int Id)
        {
            var data = await _productService.GetInforProduct(Id);
            return PartialView(data);
        }

        [HttpPost]
        public async Task<JsonResult> GetColor(int productId, int sizeId)
        {
            var data = await _productService.GetColorConfig(productId, sizeId);
            return Json(data);
        }
    }
}

