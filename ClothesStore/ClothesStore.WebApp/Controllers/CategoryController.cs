using ClothesStore.Service.IService;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClothesStore.WebApp.Controllers
{
    public class CategoryController : Controller
    {
        ICategoryService _categoryService;
        IBrandService _brandService;
        public CategoryController(ICategoryService categoryService,IBrandService brandService)
        {
            _categoryService = categoryService;
            _brandService = brandService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> _Navigation()
        {
            var categories = await _categoryService.GetAll();
            ViewBag.Brand = await _brandService.GetAll();
            return PartialView(categories);
        }
    }
}
