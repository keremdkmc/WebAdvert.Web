using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAdvert.Web.Models.AdvertManagement;
using WebAdvert.Web.Services;

namespace WebAdvert.Web.Controllers
{
    public class AdvertManagement : Controller
    {
        private readonly IFileUploader _fileUploader;

        public AdvertManagement(IFileUploader fileUploader)
        {
            _fileUploader = fileUploader;
        }
        public async Task<IActionResult> Create(CreateAdvertViewModel model)
        {
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAdvertViewModel model,IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                var id = "11111";
                //Here, you must make a call to Advert Api, create the advertisement into the database and return Id !

                var fileName = "";

                if(imageFile != null)
                {
                    fileName = !string.IsNullOrEmpty(imageFile.FileName) ? Path.GetFileName(imageFile.FileName) : id;
                    var filePath = $"{id}/{fileName}";

                    try
                    {
                        using (var readStream = imageFile.OpenReadStream())
                        {
                            var result = await _fileUploader.UploadFileAsync(filePath, readStream);

                            if (!result)
                                throw new Exception(
                                        message: "Could not upload image !"
                                    );
                        }
                    }
                    catch (Exception e)
                    {
                        // Call Advert Api and cancel the advertisement.
                        Console.WriteLine(e);
                    }
                }


                
            }

            return RedirectToAction("Index", controllerName: "Home");
        }
    }
}
