namespace Invoices.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class ProductClient
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        [ForeignKey(nameof(ProductId))]
        public virtual Product Product { get; set; } = null!;

        [Required]
        public int ClientId { get; set; }

        [Required]
        [ForeignKey(nameof(ClientId))]
        public virtual Client Client { get; set; } = null!;
    }
}
