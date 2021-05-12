using ClothesStore.Model.ModelView;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ClothesStore.Service.Service;
using ClothesStore.Service.IService;
using ClothesStore.Model.Model.EF;
using Newtonsoft.Json;

namespace ClothesStore.WebApp.Controllers
{
    public class CartController : Controller
    {

        private readonly IProductService _productService;

        public CartController(IProductService productService)
        {
            _productService = productService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> AddToCart(Order order, List<OrderDetail> orderDetails)
        {
            var user = HttpContext.Request.Cookies[Common.Constant.USER];
            if (user == null)
                return Json(1);
            order.EmployeeId = 0;
            order.CustomerId = (JsonConvert.DeserializeObject<Customer>(user)).Id;
            if (order.Address == null)
                order.Address = (JsonConvert.DeserializeObject<Customer>(user)).Address;
            order.Status = false;
            order.IsDeleted = false;
            var check = await _productService.Order(order, orderDetails);
            if (check)
                return Json(0);
            return Json(2);
        }
    }
}
