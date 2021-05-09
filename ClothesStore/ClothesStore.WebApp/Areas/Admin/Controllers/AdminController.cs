using ClothesStore.Model.Model.EF;
using ClothesStore.Model.ModelView;
using ClothesStore.Service.IService;
using ClothesStore.WebApp.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ClothesStore.WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminController : Controller
    {

        private readonly IEmployeeService _employeeService;
        private readonly ILoginService _loginService;

        public AdminController(IEmployeeService employeeService, ILoginService loginService)
        {
            _employeeService = employeeService;
            _loginService = loginService;
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

        [HttpPost]
        public async Task<JsonResult> Login(string Email, string Password)
        {
            var emp = await _loginService.Login(Email, Utilities.ComputeSha256Hash(Password));

            //this emp is not available
            if (emp == null)
                return Json(false);
            string jsonData = JsonSerializer.Serialize(emp);
            HttpContext.Session.SetString(Common.Constant.USER, jsonData);

            return Json(new { status = true, isAdmin = emp.IsAdmin });
        }

        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> Register(RegisterModelView model)
        {
            Employee emp = new Employee()
            {
                Id = 0,
                Name = model.Name,
                Phone = model.Phone,
                BirthDay = model.BirthDay,
                Email = model.Email,
                Password = Utilities.ComputeSha256Hash(model.Password)
            };

            await _employeeService.AddOrUpdate(emp);

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
            var hasUser = await _loginService.HasUser(Email, Phone); // if exists, return user

            //user not available
            if (hasUser == null)
                return Json(false);

            //cretae new password
            string newPass = Utilities.GenerateRandomString(8);
            hasUser.Password = Utilities.ComputeSha256Hash(newPass);
            bool canUpdate = await _employeeService.AddOrUpdate(hasUser);

            if (canUpdate)
            {
                MailService.SendEmail(hasUser.Email, newPass);
                return Json(true);
            }
            else
                return Json(false);
        }


        public async Task<PartialViewResult> GetListData(RequestData requestData)
        {
            var data = await _employeeService.GetListData(requestData);
            return PartialView(data);
        }

        public async Task<JsonResult> DeleteById(int id)
        {
            var result = await _employeeService.DeleteById(id);
            return Json(result);
        }

        [HttpPost]
        public async Task<PartialViewResult> GetFormAddOrEdit(int Id)
        {
            if (Id == 0)
                return PartialView(null);
            else
                return PartialView(await _employeeService.GetObjectById(Id));
        }

        [HttpPost]
        public async Task<JsonResult> AddOrUpdate(Employee obj)
        {
            if (obj.Id == 0)
                obj.Password = Utilities.ComputeSha256Hash(obj.Password);
            var response = await _employeeService.AddOrUpdate(obj);
            return Json(new ResponseStatus() { success = response, error = null });
        }

        [HttpPost]
        public async Task<PartialViewResult> ViewDetail(int id)
        {
            var obj = await _employeeService.GetObjectById(id);
            var data = new ViewDetailObject<Employee>()
            {
                obj = obj,
                CreatedBy = "",
                UpdatedBy = ""
            };
            return PartialView(data);
        }
    }
}
