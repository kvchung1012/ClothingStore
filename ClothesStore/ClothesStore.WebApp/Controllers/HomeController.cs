using ClothesStore.Model.ModelView;
using ClothesStore.Service.IService;
using ClothesStore.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ClothesStore.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;
        private readonly IBrandService _brandService;
        private readonly ICategoryService _categoryService;
        private readonly ISliderService _sliderService;

        public HomeController(IProductService productService,IBrandService brandService,ICategoryService categoryService,ISliderService sliderService)
        {
            _productService = productService;
            _brandService = brandService;
            _categoryService = categoryService;
            _sliderService = sliderService;
        }

        public async Task<IActionResult> Index()
        {
            var product = await _productService.GetListProductByQtyAndPosition(0, 3);
            ViewBag.Product = await _productService.GetListProductByQtyAndPosition(3, 3);
            //new
            ViewBag.brand = (await _brandService.GetAll()).Take(6);
            ViewBag.category = (await _categoryService.GetAll());
            ViewBag.Slider = (await _sliderService.GetAll());
            return View(product);
        }

        public async Task<PartialViewResult> GetProduct(int Id)
        {
            var data = (await _productService.GetListProduct(Id,16));
            return PartialView(data);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

    
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
