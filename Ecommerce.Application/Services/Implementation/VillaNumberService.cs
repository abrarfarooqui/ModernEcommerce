
using Ecommerce.Application.Common.Interfaces;
using Ecommerce.Application.Services.Interface;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Services.Implementation
{

    public class VillaNumberService : IVillaNumberService
    {
        private readonly IUnitOfWork _unitOfWork;
        public VillaNumberService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public void CreateVillaNumber(VillaNumber villaNumber)
        {
            _unitOfWork.VillaNumber.Add(villaNumber);
            _unitOfWork.Save();
        }
        public bool DeleteVillaNumber(int id)
        {
            try
            {
                VillaNumber? objFromDB = _unitOfWork.VillaNumber.Get(u => u.Villa_Number == id);
                if (objFromDB is not null)
                {
                    _unitOfWork.VillaNumber.Remove(objFromDB);
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

        public VillaNumber GetVillaNumberById(int id)
        {
            return _unitOfWork.VillaNumber.Get(u => u.Villa_Number == id, includeProperties: "Villa");
        }

        public IEnumerable<VillaNumber> GetAllVillaNumbers()
        {
            return _unitOfWork.VillaNumber.GetAll(includeProperties: "Villa");
        }

        public void UpdateVillaNumber(VillaNumber villaNumber)
        {
            _unitOfWork.VillaNumber.Update(villaNumber);
            _unitOfWork.Save();
        }

        public bool IsVillaNumberExsist(int villa_Number)
        {
            return _unitOfWork.VillaNumber.Any(u => u.Villa_Number == villa_Number);
        }
    }
}
