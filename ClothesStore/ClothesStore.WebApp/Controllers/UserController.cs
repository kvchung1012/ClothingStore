using ClothesStore.Model.Model.EF;
using ClothesStore.Model.ModelView;
using ClothesStore.Service.IService;
using ClothesStore.WebApp.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace ClothesStore.WebApp.Controllers
{
    public class UserController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly ILoginService _loginService;
        private readonly ISendMailService _sendMailService;
        private readonly IHttpContextAccessor _HttpContextAccessor;


        public UserController(ICustomerService customerService, ILoginService loginService, ISendMailService sendMailService, IHttpContextAccessor HttpContextAccessor)
        {
            _customerService = customerService;
            _loginService = loginService;
            _sendMailService = sendMailService;
            _HttpContextAccessor = HttpContextAccessor;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> ChangePassword(ChangePassViewModel change)
        {
            string jsonUser = HttpContext.Request.Cookies[Constant.USER];
            if (jsonUser == null)
                return Json(false);
            var emp =  JsonSerializer.Deserialize<Customer>(jsonUser) as Customer;
            if (String.IsNullOrEmpty(change.OldPassword) || Common.Utilities.ComputeSha256Hash(change.OldPassword) != emp.Password)
            {
                return Json(1);
            }
            if (String.IsNullOrEmpty(change.NewPassword) && change.NewPassword.Length < 8)
            {
                return Json(2);
            }
            if (change.NewPassword != change.ConfirmedPassword)
            {
                return Json(3);
            }
            emp.Password = Utilities.ComputeSha256Hash(change.NewPassword);
            await _customerService.AddOrUpdate(emp);
            HttpContext.Response.Cookies.Delete(Constant.USER);
            return Json(true);
        }

        [HttpPost]
        public async Task<JsonResult> Login(string Email, string Password)
        {
            var emp = await _loginService.CustomerLogin(Email, Utilities.ComputeSha256Hash(Password));

            //this emp is not available
            if (emp == null)
                return Json(false);
            string jsonData = JsonSerializer.Serialize(emp);
            HttpContext.Response.Cookies.Append(Common.Constant.USER, jsonData);
            return Json(true);
        }

        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Logout()
        {
            Response.Cookies.Delete(Constant.USER);
            return RedirectToAction("Index","Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> Register(RegisterModelView model)
        {
            Customer emp = new Customer()
            {
                Id = 0,
                Name = model.Name,
                Phone = model.Phone,
                Email = model.Email,
                Password = Utilities.ComputeSha256Hash(model.Password)
            };

            await _customerService.AddOrUpdate(emp);

            return Json(true);
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> ForgotPassword(string Email, string Phone)
        {
            var hasUser = await _loginService.CustomerHasUser(Email, Phone); // if exists, return user

            //user not available
            if (hasUser == null)
                return Json(false);

            //cretae new password
            string newPass = Utilities.GenerateRandomString(8);
            hasUser.Password = Utilities.ComputeSha256Hash(newPass);
            bool canUpdate = await _customerService.AddOrUpdate(hasUser);

            if (canUpdate)
            {
                MailContent mail = new MailContent();
                mail.To = "nguyenkhanh21102000@gmail.com";
                mail.Subject = "ForgotPassword";
                mail.Body = "Mật khẩu mới của bạn là :" + newPass;
                await _sendMailService.SendMail(mail);
                return Json(true);
            }
            else
                return Json(false);
        }
    }
}
