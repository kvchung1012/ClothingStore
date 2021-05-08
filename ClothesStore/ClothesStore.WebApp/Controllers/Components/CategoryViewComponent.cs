using ClothesStore.Service.IService;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClothesStore.WebApp.Controllers.Components
{
    public class CategoryViewComponent : ViewComponent
    {
        ICategoryService _categoryService;
        IBrandService _brandService;
        public CategoryViewComponent(ICategoryService categoryService, IBrandService brandService)
        {
            _categoryService = categoryService;
            _brandService = brandService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _categoryService.GetAll();
            ViewBag.Brand = await _brandService.GetAll();
            return View(categories);
        }
    }
}
