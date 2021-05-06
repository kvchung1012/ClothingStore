using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace ClothesStore.WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public HomeController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult UploadFileImage(IFormFile upload)
        {
            try
            {
                var fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + upload.FileName;
                var path = Path.Combine(Directory.GetCurrentDirectory(), _hostingEnvironment.WebRootPath, "Image/Upload", fileName);
                var stream = new FileStream(path, FileMode.Create);
                upload.CopyToAsync(stream);
                return Json(new { uploaded = true, url = "/Image/Upload/" + fileName });
            }
            catch (Exception)
            {
                return Json(new { uploaded = false });
            }
        }

        [HttpPost]
        public JsonResult UploadListFileImage()
        {
            try
            {
                var ListImage = Request.Form.Files;
                if (ListImage != null)
                {
                    if (ListImage.Count > 0)
                    {
                        foreach (var upload in ListImage)
                        {
                            var fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + upload.FileName;
                            var path = Path.Combine(Directory.GetCurrentDirectory(), _hostingEnvironment.WebRootPath, "Image/Upload", fileName);
                            var stream = new FileStream(path, FileMode.Create);
                            upload.CopyToAsync(stream);
                        }
                    }
                    else
                    {
                        return Json(false);
                    }
                }
                else
                {
                    return Json(false);
                }
                return Json(true);
            }
            catch (Exception)
            {
                return Json(false);
            }
        }

        [HttpPost]
        public JsonResult DeleteImageByName([FromBody] string fileName)
        {
            var path = _hostingEnvironment.WebRootPath + "/Image/Upload/" + fileName;
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
                return Json(true);
            }
            else
            {
                return Json(false);
            }
        }

        [HttpGet]
        public IActionResult FileBrower()
        {
            var dir = new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(),
                                                    _hostingEnvironment.WebRootPath, "Image/Upload"));
            ViewBag.data = dir.GetFiles();
            return View();
        }

        [HttpGet]
        public IActionResult ReadFileBrower()
        {
            var dir = new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(),
                                                    _hostingEnvironment.WebRootPath, "Image/Upload"));
            ViewBag.data = dir.GetFiles();
            return PartialView();
        }
    }
}
