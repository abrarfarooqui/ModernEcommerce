using Ecommerce.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ECommerce.Web.ViewModels
{
    public class AmenityVM
    {
        public Amenity? Amenity { get; set; }
        public IEnumerable<SelectListItem>? VillaList { get; set; }
    }
}
