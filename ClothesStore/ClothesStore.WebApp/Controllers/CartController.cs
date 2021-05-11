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

            var CartSession = HttpContext.Session.GetString(Common.Constant.CART);
            var ListItem = new List<CartItemModelView>();

            if (CartSession != null)
            {
                ListItem = JsonSerializer.Deserialize<List<CartItemModelView>>(CartSession);
            }

            return View(ListItem);
        }

        [HttpPost]
        public async Task<JsonResult> AddToCart(CartItemModelView CartItem)
        {
            try
            {
                //get price by config
                var hasPrice = await _productService.GetListConfigProductByProductId(CartItem.ProductId);
               
                if(hasPrice.Count > 0)
                {
                    
                }
                
                var CartSession = HttpContext.Session.GetString(Common.Constant.CART);
                var ListItem = new List<CartItemModelView>();
                if (CartSession == null)
                {
                    ListItem.Add(CartItem);
                    HttpContext.Session.SetString(Common.Constant.CART, JsonSerializer.Serialize(ListItem));
                }
                else
                {
                    ListItem = JsonSerializer.Deserialize<List<CartItemModelView>>(CartSession);

                    CartItemModelView hasProduct = ListItem.FirstOrDefault(x => x.ProductId == CartItem.ProductId
                    && x.Size == CartItem.Size && x.Color == CartItem.Color);

                    //this item already exists in cart
                    if (hasProduct != null)
                    {
                        hasProduct.Quantity += CartItem.Quantity;
                    }
                    else
                    {
                        ListItem.Add(CartItem);
                    }

                    //put cart to session
                    HttpContext.Session.SetString(Common.Constant.CART, JsonSerializer.Serialize(ListItem));
                }
                return Json(new { status = true });
            }
            catch (Exception e)
            {
                return Json(new { status = false, message = e.Message });
            }

        }
    }
}
