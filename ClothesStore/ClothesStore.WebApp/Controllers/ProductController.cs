using ClothesStore.Model.Model.EF;
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
        
        public ProductController(IProductService productService, IColorService colorService, ISizeService sizeService)
        {
            _productService = productService;
            _colorService = colorService;
            _sizeService = sizeService;
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

        public async Task<IActionResult> SingleProduct(int Id)
        {
            var productViewModel = await _productService.GetProductDetailById(Id);
            ViewBag.allSize = await _sizeService.GetAll();
            ViewBag.allColor = await _colorService.GetAll();
            ViewBag.RelatedPro = await _productService.GetRelatedProduct(Id, 4);
            return View(productViewModel);
        }
    }
}
