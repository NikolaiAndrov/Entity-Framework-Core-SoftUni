﻿namespace Invoices.Data.Models
{
    using Invoices.Common;
    using Invoices.Data.Models.Enums;
    using System.ComponentModel.DataAnnotations;

    public class Product
    {
        public Product()
        {
            this.ProductsClients = new HashSet<ProductClient>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(ValidationConstants.ProductNameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        public decimal Price { get; set; }

        [Required]
        public CategoryType CategoryType { get; set; }

        public virtual ICollection<ProductClient> ProductsClients { get; set; }
    }
}
