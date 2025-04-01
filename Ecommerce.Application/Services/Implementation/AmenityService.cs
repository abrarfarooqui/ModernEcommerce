using Ecommerce.Application.Common.Interfaces;
using Ecommerce.Application.Services.Interface;
using Ecommerce.Domain.Entities;


namespace Ecommerce.Application.Services.Implementation
{
    public class AmenityService : IAmenityService
    {
        private readonly IUnitOfWork _unitOfWork;
        public AmenityService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public void CreateAmenity(Amenity amenity)
        {
            _unitOfWork.Amenity.Add(amenity);
            _unitOfWork.Save();
        }

        public bool DeleteAmenity(int id)
        {
            try
            {
                Amenity? amenity = _unitOfWork.Amenity.Get(u => u.Id == id);
                if (amenity is not null)
                {
                    _unitOfWork.Amenity.Remove(amenity);
                    _unitOfWork.Save();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return true;
            }
        }

        public IEnumerable<Amenity> GetAllAmenities()
        {
            return _unitOfWork.Amenity.GetAll();
        }

        public Amenity GetAmenityById(int id)
        {
            return _unitOfWork.Amenity.Get(u => u.Id == id);
        }

        public void UpdateAmenity(Amenity amenity)
        {
            _unitOfWork.Amenity.Update(amenity);
            _unitOfWork.Save();
        }
    }
}
