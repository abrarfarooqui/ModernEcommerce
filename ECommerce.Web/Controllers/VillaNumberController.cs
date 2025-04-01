using Ecommerce.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ECommerce.Web.ViewModels;
using Ecommerce.Application.Common.Utility;
using Microsoft.AspNetCore.Authorization;
using Ecommerce.Application.Services.Interface;

namespace ECommerce.Web.Controllers
{
    [Authorize(Roles = SD.Role_Admin)]
    public class VillaNumberController : Controller
    {
        private readonly IVillaNumberService _villaNumberService;
        private readonly IVillaService _villaService;
        public VillaNumberController(IVillaNumberService villaNumberService, IVillaService villaService)
        {
            _villaNumberService = villaNumberService;
            _villaService = villaService;
        }
        public IActionResult Index()
        {
            var VillaNumbers = _villaNumberService.GetAllVillaNumbers();
            return View(VillaNumbers);
        }
        public IActionResult Create()
        {
            VillaNumberVM villaNumberVM = new()
            {
                VillaList = _villaService.GetAllVillas().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                })
            };
            return View(villaNumberVM);
        }
        [HttpPost]
        public IActionResult Create(VillaNumberVM obj)
        {
            ModelState.Remove("Villa");

            bool isVillaNumberExists = _villaNumberService.IsVillaNumberExsist(obj.VillaNumber.Villa_Number);

            if (ModelState.IsValid && !isVillaNumberExists)
            {
                _villaNumberService.CreateVillaNumber(obj.VillaNumber);
                TempData["success"] = "The villa Number has been created successfully.";
                return RedirectToAction(nameof(Index));
            }
            if (isVillaNumberExists)
            {
                TempData["error"] = "The Villa Number already exists";
            }

            obj.VillaList = _villaService.GetAllVillas().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });
            
            return View(obj);
        }
        public IActionResult Update(int VillaNumberId)
        {
            VillaNumberVM villaNumberVM = new()
            {
                VillaList = _villaService.GetAllVillas().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                VillaNumber = _villaNumberService.GetVillaNumberById(VillaNumberId)
            };
            if (villaNumberVM.VillaNumber == null) 
            {
                return RedirectToAction("Error", "Home");
            }
            return View(villaNumberVM);
        }
        [HttpPost]
        public IActionResult Update(VillaNumberVM villaNumberVM)
        {
            if (ModelState.IsValid)
            {
                _villaNumberService.UpdateVillaNumber(villaNumberVM.VillaNumber);
                TempData["success"] = "The villa Number has been updated successfully.";
                return RedirectToAction(nameof(Index));
            }

            villaNumberVM.VillaList = _villaService.GetAllVillas().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });
            return View(villaNumberVM);
        }
        public IActionResult Delete(int VillaNumberId)
        {
            VillaNumberVM villaNumberVM = new()
            {
                VillaList = _villaService.GetAllVillas().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                VillaNumber = _villaNumberService.GetVillaNumberById(VillaNumberId)
            };
            if (villaNumberVM.VillaNumber == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(villaNumberVM);
        }
        [HttpPost]
        public IActionResult Delete(VillaNumberVM villaNumberVM)
        {
            VillaNumber? objFromDB = _villaNumberService.GetVillaNumberById(villaNumberVM.VillaNumber.Villa_Number);
            if (objFromDB is not null)
            {
                _villaNumberService.DeleteVillaNumber(objFromDB.Villa_Number);
                TempData["success"] = "The villa number has been deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "The villa number could not be deleted.";
            return View();
        }
    }
}
