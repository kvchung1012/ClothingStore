﻿using System;
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
    public class CategoryController : Controller
    {
        ICategoryService _categoryService;
        IEmployeeService _employeeService;
        IHttpContextAccessor _HttpContextAccessor;

        public CategoryController(ICategoryService categoryService, IEmployeeService employeeService, IHttpContextAccessor HttpContextAccessor)
        {
            _categoryService = categoryService;
            _employeeService = employeeService;
            _HttpContextAccessor = HttpContextAccessor;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<PartialViewResult> GetFormAddOrEdit(int Id)
        {
            if (Id == 0)
                return PartialView(null);
            else
                return PartialView(await _categoryService.GetObjectById(Id));
        }

        [HttpPost]
        public async Task<PartialViewResult> ViewDetail(int Id)
        {
            var obj = await _categoryService.GetObjectById(Id);
            var empUpdated = obj.UpdatedBy != null ? await _employeeService.GetObjectById((int)obj.UpdatedBy) : new Employee();
            var empCreated = obj.CreatedBy != null ? await _employeeService.GetObjectById((int)obj.CreatedBy) : new Employee();

            var data = new ViewDetailObject<Category>()
            {
                obj = obj,
                CreatedBy = empCreated.Name,
                UpdatedBy = empUpdated.Name
            };
            return PartialView(data);
        }

        [HttpPost]
        public async Task<PartialViewResult> GetListData(RequestData requestData)
        {
            var response = await _categoryService.GetListData(requestData);
            return PartialView(response);
        }

        [HttpPost]
        public async Task<JsonResult> GetObjectById(int Id)
        {
            var response = await _categoryService.GetObjectById(Id);
            return Json(response);
        }

        [HttpPost]
        public async Task<JsonResult> AddOrUpdate(Category obj)
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
            var response = await _categoryService.AddOrUpdate(obj);
            return Json(new ResponseStatus() { success = response, error = null });
        }

        [HttpPost]
        public async Task<JsonResult> DeleteById(int Id)
        {
            var response = await _categoryService.DeleteById(Id);
            return Json(response);
        }

    }
}
