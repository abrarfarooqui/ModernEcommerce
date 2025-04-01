using Ecommerce.Application.Common.Interfaces;
using Ecommerce.Application.Common.Utility;
using Ecommerce.Application.Services.Interface;
using Ecommerce.Domain.Entities;


namespace Ecommerce.Application.Services.Implementation
{

    public class VillaService : IVillaService
    {
        private readonly IUnitOfWork _unitOfWork;
        //private readonly IWebHostEnvironment _webHostEnvironment;
        public VillaService(IUnitOfWork unitOfWork)//, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            //_webHostEnvironment = webHostEnvironment;
        }
        public void CreateVilla(Villa villa)
        {
            //if (villa.Image != null)
            //{
            //    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(villa.Image.FileName);
            //    string imagePath = "";// Path.Combine(_webHostEnvironment.WebRootPath, @"images\VillaImage");

            //    using var fileStream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create);
            //    villa.Image.CopyTo(fileStream);
            //    villa.ImageUrl = @"\images\VillaImage\" + fileName;
            //}
            //else
            //{
            //    villa.ImageUrl = "hhtps://placehold.co/600x400";
            //}
            _unitOfWork.Villa.Add(villa);
            _unitOfWork.Save();
        }

        public bool DeleteVilla(int id)
        {
            try
            {
                Villa? villa = _unitOfWork.Villa.Get(u => u.Id == id);
                if (villa is not null)
                {
                    //if (!string.IsNullOrEmpty(villa.ImageUrl))
                    //{
                    //    var oldImagePath = "";// Path.Combine(_webHostEnvironment.WebRootPath, villa.ImageUrl.Trim('\\'));
                    //    if (System.IO.File.Exists(oldImagePath))
                    //    {
                    //        System.IO.File.Delete(oldImagePath);
                    //    }
                    //}
                    _unitOfWork.Villa.Remove(villa);
                    _unitOfWork.Save();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IEnumerable<Villa> GetAllVillas()
        {
            return _unitOfWork.Villa.GetAll(includeProperties: "VillaAmenity");
        }

        public IEnumerable<Villa> GetVillaAvailabilityByDate(int nights, DateOnly checkInDate)
        {
            var VillaList = _unitOfWork.Villa.GetAll().ToList();
            var villaNumberList = _unitOfWork.VillaNumber.GetAll().ToList();
            var bookedVillas = _unitOfWork.Booking.GetAll(u => u.status == SD.Status_Approved || u.status == SD.Status_CheckedIn).ToList();


            foreach (var villa in VillaList)
            {
                int roomAvailable = SD.VillaRoomsAvailable_Count(villa.Id, villaNumberList, checkInDate, nights, bookedVillas);
                villa.IsAvailable = roomAvailable > 0 ? true : false;
            }
            return VillaList;
        }

        public Villa GetVillaById(int id)
        {
            return _unitOfWork.Villa.Get(u => u.Id == id, includeProperties: "VillaAmenity");
        }

        public bool IsVillaAvailableByDate(int villaId, int nights, DateOnly checkInDate)
        {
            //check the Villa availability first
            var villaNumberList = _unitOfWork.VillaNumber.GetAll().ToList();
            var bookedVillas = _unitOfWork.Booking.GetAll(u => u.status == SD.Status_Approved || u.status == SD.Status_CheckedIn).ToList();

            int roomAvailable = SD.VillaRoomsAvailable_Count(villaId, villaNumberList, checkInDate, nights, bookedVillas);

            return roomAvailable > 0 ? true : false;
        }

        public void UpdateVilla(Villa villa)
        {
            //if (villa.Image != null)
            //{
            //    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(villa.Image.FileName);
            //    string imagePath = "";// Path.Combine(_webHostEnvironment.WebRootPath, @"images\VillaImage");

            //    if (!string.IsNullOrEmpty(villa.ImageUrl))
            //    {
            //        var oldImagePath = "";// Path.Combine(_webHostEnvironment.WebRootPath, obj.ImageUrl.Trim('\\'));
            //        if (System.IO.File.Exists(oldImagePath))
            //        {
            //            System.IO.File.Delete(oldImagePath);
            //        }
            //    }
            //    using var fileStream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create);
            //    villa.Image.CopyTo(fileStream);
            //    villa.ImageUrl = @"\images\VillaImage\" + fileName;
            //}
            _unitOfWork.Villa.Update(villa);
            _unitOfWork.Save();
        }
    }
}
