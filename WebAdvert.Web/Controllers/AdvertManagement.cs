using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AdvertApi.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAdvert.Web.Models.AdvertManagement;
using WebAdvert.Web.ServiceClient;
using WebAdvert.Web.Services;

namespace WebAdvert.Web.Controllers
{
    public class AdvertManagement : Controller
    {
        private readonly IFileUploader _fileUploader;
        private readonly IAdvertApiClient _advertApiClient;
        private readonly IMapper _mapper;
        public AdvertManagement(IFileUploader fileUploader,IAdvertApiClient advertApiClient, IMapper mapper)
        {
            _fileUploader = fileUploader;
            _advertApiClient = advertApiClient;
            _mapper = mapper;
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

                var createAdvertModel = _mapper.Map<CreateAdvertModel>(model);

                var apiCallResponse = await _advertApiClient.Create(createAdvertModel);

                var id = apiCallResponse.Id;

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

                        var confirmModel = new ConfirmAdvertRequest
                        {
                            Id = id,
                            Status = AdvertStatus.Active
                        };

                        var canConfirm = await _advertApiClient.Confirm(confirmModel);

                        if (!canConfirm)
                        {
                            throw new Exception(message: $"Cannot confirm advert of id = {id}");
                        }

                        return RedirectToAction("Index", controllerName: "Home");

                    }
                    catch (Exception e)
                    {

                        var confirmModel = new ConfirmAdvertRequest
                        {
                            Id = id,
                            Status = AdvertStatus.Pending
                        };
                        var canConfirm = await _advertApiClient.Confirm(confirmModel);
                    }
                }


                
            }

            return View("Create",model);
        }
    }
}
