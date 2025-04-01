using Ecommerce.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ECommerce.Web.ViewModels
{
    public class VillaNumberVM
    {
        public VillaNumber? VillaNumber { get; set; }
        public IEnumerable<SelectListItem>? VillaList { get; set; }
    }
}
