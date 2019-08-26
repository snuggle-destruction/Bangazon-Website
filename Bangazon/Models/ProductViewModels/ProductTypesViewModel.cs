using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bangazon.Models.ProductViewModels
{
    public class ProductTypesViewModel
    {
        public ICollection<GroupedProducts> GroupedProducts { get; set; }
    }
}