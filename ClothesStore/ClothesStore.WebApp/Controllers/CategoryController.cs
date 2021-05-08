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

        public CategoryController(ICategoryService categoryService,IBrandService brandService)
        {
            _categoryService = categoryService;
   
        }
        public IActionResult Index()
        {
            return View();
        }

      
    }
}
