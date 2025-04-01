using Ecommerce.Domain.Entities;
namespace Ecommerce.Application.Common.Interfaces
{
    public interface IAmenityRepository : IRepository<Amenity>
    {
        void Update(Amenity entity);
    }
}
