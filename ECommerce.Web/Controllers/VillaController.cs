using Ecommerce.Application.Common.Utility;
using Ecommerce.Application.Services.Interface;
using Ecommerce.Domain.Entities;
using ECommerce.Web.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace ECommerce.Web.Controllers
{
    [Authorize(Roles = SD.Role_Admin)]
    public class VillaController : Controller
    {
        private readonly IVillaService _villaService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UploadFile _uploadFile;
        public VillaController(IVillaService villaService, IWebHostEnvironment webHostEnvironment, UploadFile uploadFile)
        {
            _villaService = villaService;
            _webHostEnvironment = webHostEnvironment;
            _uploadFile = uploadFile;
        }
        public IActionResult Index()
        {
            var Villas = _villaService.GetAllVillas();
            return View(Villas);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Villa obj)
        {
            if (obj.Name == obj.Description)
            {
                ModelState.AddModelError("", "The description cannot exactly match the Name");
            }
            if (ModelState.IsValid)
            {
                obj.ImageUrl = "hhtps://placehold.co/600x400";
                if (obj.Image != null)
                    obj.ImageUrl = _uploadFile.fileUpload(obj.Image, string.Empty);

                _villaService.CreateVilla(obj);
                TempData["success"] = "The villa has been created successfully.";
                return RedirectToAction(nameof(Index));
            }
            return View(obj);
        }
        public IActionResult Update(int villaId)
        {
            Villa? villa = _villaService.GetVillaById(villaId);
            if (villa == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(villa);
        }
        [HttpPost]
        public IActionResult Update(Villa obj)
        {
            if (ModelState.IsValid && obj.Id>0)
            {
                if (obj.Image != null)
                    obj.ImageUrl = _uploadFile.fileUpload(obj.Image, obj.ImageUrl);

                _villaService.UpdateVilla(obj);
                TempData["success"] = "The villa has been updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            return View(obj);
        }
        public IActionResult Delete(int villaId)
        {
            Villa? villa = _villaService.GetVillaById(villaId);
            if (villa is null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(villa);
        }
        [HttpPost]
        public IActionResult Delete(Villa obj)
        {
            bool deleted = _villaService.DeleteVilla(obj.Id);
            if (deleted)
            {
                _uploadFile.deleteFile(obj.ImageUrl);
                TempData["success"] = "The villa has been deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = "The villa could not be deleted.";
            }
            return View(obj);
        }
    }
}
