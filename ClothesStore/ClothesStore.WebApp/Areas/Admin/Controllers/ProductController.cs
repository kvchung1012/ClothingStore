using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ClothesStore.Model.Model.EF;
using ClothesStore.Model.ModelView;
using ClothesStore.Service.IService;
using ClothesStore.WebApp.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClothesStore.WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IBrandService _brandService;
        private readonly ICategoryService _categoryService;
        IEmployeeService _employeeService;
        IHttpContextAccessor _HttpContextAccessor;

        public ProductController(IProductService productService,IBrandService brandService,ICategoryService categoryService,
            IEmployeeService employeeService, IHttpContextAccessor HttpContextAccessor)
        {
            _productService = productService;
            _brandService = brandService;
            _categoryService = categoryService;
            _employeeService = employeeService;
            _HttpContextAccessor = HttpContextAccessor;
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
            string jsonUser = _HttpContextAccessor.HttpContext.Session.GetString(Constant.USER);
            var emp = new Employee();
            if (jsonUser != null)
            {
                emp = JsonSerializer.Deserialize<Employee>(jsonUser) as Employee;
                if (obj.Id == 0)
                    obj.CreatedBy = emp.Id;
                else
                {
                    obj.UpdatedBy = emp.Id;
                }
            }
            var res = await _productService.AddOrUpdate(obj, config, images);
            return Json(res);
        }
    }
}
