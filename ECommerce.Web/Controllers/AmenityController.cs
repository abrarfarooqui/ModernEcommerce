using Ecommerce.Application.Common.Interfaces;
using ECommerce.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Ecommerce.Application.Common.Utility;
using Ecommerce.Application.Services.Interface;

namespace ECommerce.Web.Controllers
{
    [Authorize(Roles = SD.Role_Admin)]
    public class AmenityController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IVillaService _villaService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IAmenityService _amenityService;
        public AmenityController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment, IVillaService villaService, IAmenityService amenityService)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
            _villaService = villaService;
            _amenityService = amenityService;
        }
        public IActionResult Index()
        {
            var Amenities = _unitOfWork.Amenity.GetAll(includeProperties: "Villa");
            return View(Amenities);
        }
        public IActionResult Create()
        {
            AmenityVM amenityVM = new()
            {
                VillaList = _villaService.GetAllVillas().Select(u=> new SelectListItem 
                { 
                    Text = u.Name,
                    Value = u.Id.ToString()
                })
            };
            return View(amenityVM);
        }
        [HttpPost]
        public IActionResult Create(AmenityVM obj)
        {
            if (ModelState.IsValid)
            {
                _amenityService.CreateAmenity(obj.Amenity);

                TempData["success"] = "The Amenity has been created successfully.";
                return RedirectToAction(nameof(Index));
            }
            obj.VillaList = _villaService.GetAllVillas().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });
            return View(obj);
        }
        public IActionResult Update(int amenityId)
        {
            AmenityVM amenityVM = new()
            {
                VillaList = _villaService.GetAllVillas().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Amenity = _amenityService.GetAmenityById(amenityId)
            };
            if (amenityVM.Amenity == null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(amenityVM);
        }
        [HttpPost]
        public IActionResult Update(AmenityVM obj)
        {
            if (ModelState.IsValid && obj.Amenity.Id > 0)
            {
                _unitOfWork.Amenity.Update(obj.Amenity);
                _unitOfWork.Save();
                TempData["success"] = "The Amenity has been updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            obj.VillaList = _villaService.GetAllVillas().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });

            return View(obj);
        }
        public IActionResult Delete(int amenityId)
        {
            AmenityVM amenityVM = new()
            {
                VillaList = _villaService.GetAllVillas().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Amenity = _amenityService.GetAmenityById(amenityId)
            };
            if (amenityVM is null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(amenityVM);
        }
        [HttpPost]
        public IActionResult Delete(AmenityVM obj)
        {
            bool deleted = _amenityService.DeleteAmenity(obj.Amenity.Id);
            if (deleted)
            {
                TempData["success"] = "The amenity has been deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = "The amenity could not be deleted.";
            }
            return View(obj);
        }
    }
}
