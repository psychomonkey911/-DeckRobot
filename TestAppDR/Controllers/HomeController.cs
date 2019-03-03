using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using TestAppDeckRobot.Models.Home;
using Microsoft.Extensions.FileProviders;
using TestAppDR;
using System;

namespace TestAppDeckRobot.Controllers
{
    public class HomeController : Controller
    {
        private readonly IFileProvider fileProvider;
        private Presentations PresenstationConvertor { get; }
        public HomeController(IFileProvider fileProvider)
        {
            this.fileProvider = fileProvider;
            PresenstationConvertor = new Presentations();
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> UploadFiles(List<IFormFile> files, int fontSize, string fontName)
        {
            if (files == null || files.Count == 0)
                return Content("files not selected");

            foreach (var file in files)
            {
                var guid = Guid.NewGuid();
                var path = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot", 
                        file.GetFilename());
                
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                PresenstationConvertor.ModFiles(path, fontName, fontSize);
            }

            return RedirectToAction("Files");
        }

        public IActionResult Files()
        {
            var model = new FilesViewModel();
            foreach (var item in this.fileProvider.GetDirectoryContents(""))
            {
                model.Files.Add(
                    new FileDetails { Name = item.Name, Path = item.PhysicalPath });
            }
            return View(model);
        }

        public async Task<IActionResult> Download(string filename)
        {
            if (filename == null)
                return Content("filename not present");

            var path = Path.Combine(
                           Directory.GetCurrentDirectory(),
                           "wwwroot", filename);

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }

            memory.Position = 0;
            return File(memory, GetContentType(path), Path.GetFileName(path));
        }

        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".ppt", "application/vnd.ms-powerpoint"},
                {".pptx ", "application/vnd.openxmlformats-officedocument.presentationml.presentation"},
                {".pptm", "application/vnd.ms-powerpoint.presentation.macroEnabled.12"}
            };
        }
    }
}
