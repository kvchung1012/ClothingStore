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
        private readonly ISizeService _sizeService;
        private readonly IColorService _colorService;
        private readonly IConfigProductService _configProductService;

        public ProductController(IProductService productService, IColorService colorService, ISizeService sizeService, IConfigProductService configProductService)
        {
            _productService = productService;
            _colorService = colorService;
            _sizeService = sizeService;
            _configProductService = configProductService;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Detail(int id = 1)
        {
            var product = await _productService.GetProductDetailById(id);
            List<Size> listSize = new List<Size>();
            List<Color> listColor = new List<Color>();

            foreach (var item in product.configs)
            {
                listSize.Add(await _sizeService.GetObjectById((int)item.SizeId));
                listColor.Add(await _colorService.GetObjectById((int)item.ColorId));
            }

            ViewBag.Size = listSize;
            ViewBag.Color = listColor;
            ViewBag.RelatedPro = await _productService.GetRelatedProduct(id, 4);

            return View(product);
        }

        [HttpPost]
        public async Task<JsonResult> GetColor(int productId,int sizeId)
        {
            var data = await _productService.GetColorConfig(productId, sizeId); 
            return Json(data);
        }

        public async Task<IActionResult> SingleProduct(int Id)
        {
            var productViewModel = await _productService.GetProductDetailById(Id);
            ViewBag.allSize = await _sizeService.GetAll();
            ViewBag.allColor = await _colorService.GetAll();
            ViewBag.RelatedPro = await _productService.GetRelatedProduct(Id, 4);

            //default price with no config
            ViewBag.originalPrice = await _configProductService.GetMinimumPrice(Id);

            return View(productViewModel);
        }
        [HttpPost]
        public async Task<JsonResult> GetConfig(RequestConfig requestConfig)
        {
            ResponseConfig res = await _configProductService.GetPriceAndStock(requestConfig);
            return Json(res);
        }
    }
}
