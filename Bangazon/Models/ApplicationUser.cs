using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace Bangazon.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
             
        }

        [Required]
        [Display(Name ="First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name ="Last Name")]
        public string LastName { get; set; }

        [Required]
        public string StreetAddress { get; set; }
        
        public virtual ICollection<Product> Products { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

        public virtual ICollection<PaymentType> PaymentTypes { get; set; }

        [Display(Name = "Full Name")]
        public string FullName
        {
            get
            { return $"{FirstName} {LastName}"; }
        }

        [NotMapped]
        public double AvgRating
        {
            get
            {
                var pRatings = new List<int>();

                foreach (var product in Products)
                {
                    foreach(var rating in product.Ratings)

                    pRatings.Add(rating.Rating);
                }

                return pRatings.Average();
            }
        }
    }
}
