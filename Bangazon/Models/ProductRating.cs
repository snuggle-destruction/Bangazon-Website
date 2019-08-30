using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bangazon.Models
{
   
    public class ProductRating
    {
        [Key]
        public int ProductRatingId { get; set; }

        [Display(Name = "Product Rating")]
        [Required]
        public int ProductId { get; set; }

        [Required]
        public string UserId { get; set; }

        [Range(1,5, ErrorMessage = "Must be between 1 and 5")]
        public int Rating { get; set; }

        public Product Product { get; set; }

        public ApplicationUser User { get; set; }
    }

    
}
