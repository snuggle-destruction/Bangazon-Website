using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bangazon.Models
{
    public class Product
    {
        [Key]
        public int ProductId {get;set;}

        [Required]
        [DataType(DataType.Date)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime DateCreated {get;set;}

        [Required]
        [StringLength(255)]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$",
         ErrorMessage = "Special characters are not allowed.")]
        public string Description { get; set; }

        [Required]
        [StringLength(55, ErrorMessage="Please shorten the product title to 55 characters")]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$",
         ErrorMessage = "Special characters are not allowed.")]
        public string Title { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:C}")]
        [Range(0,10000, ErrorMessage = "Product must be less than $10,000")]
        public double Price { get; set; }

        [Required]
        public int Quantity { get; set; }


        [Required]
        public string UserId {get; set;}

        public string City {get; set;}

        
        public string ImagePath { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
       

        public bool Active { get; set; }
        public bool SoldLocally { get; set; }

        [Required]
        [Display(Name = "Seller")]
        public ApplicationUser User { get; set; }

        [Required]
        [Display(Name="Product Category")]
        public int ProductTypeId { get; set; }

        public ProductType ProductType { get; set; }

        public virtual ICollection<OrderProduct> OrderProducts { get; set; }

        public Product()
        {
            Active = true;
        }

        [NotMapped]
        public int QuantityRemaining
        {
            get
            {
                if(OrderProducts != null )
                {
                return Quantity - OrderProducts.Count;
                }
                else
                {
                    return Quantity;
                }
            }
        }
    }
}
