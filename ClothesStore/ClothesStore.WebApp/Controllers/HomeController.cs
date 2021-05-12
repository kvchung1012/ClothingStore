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
        private readonly IColorService _colorService;
        private readonly ISliderService _sliderService;
        public HomeController(IProductService productService, IBrandService brandService, ICategoryService categoryService, IColorService colorService, ISliderService sliderService)
        {
            _productService = productService;
            _brandService = brandService;
            _categoryService = categoryService;
            _colorService = colorService;
            _sliderService = sliderService;
        }

        // trang chủ
        public async Task<IActionResult> Index()
        {
            ViewBag.brand = (await _brandService.GetAll());
            ViewBag.category = (await _categoryService.GetAll());
            ViewBag.color = (await _colorService.GetAll());
            ViewBag.slider = (await _sliderService.GetAll()).OrderBy(x => x.OrderBy).Take(6).ToList();
            return View();
        }


        // get product by filter
        [HttpPost]
        public async Task<PartialViewResult> GetProduct(FilterProduct filter)
        {
            var data = (await _productService.GetListProduct(filter, 16));
            return PartialView(data);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
