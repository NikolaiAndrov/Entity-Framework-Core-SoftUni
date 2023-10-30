namespace Invoices.Data.Models
{
    using Invoices.Common;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Address
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(ValidationConstants.StreetNameMaxLength)]
        public string StreetName { get; set; } = null!;

        [Required]
        public int StreetNumber { get; set; }

        [Required]
        public string PostCode { get; set; } = null!;

        [Required]
        [MaxLength(ValidationConstants.CityNameMaxLength)]
        public string City { get; set; } = null!;

        [Required]
        [MaxLength(ValidationConstants.CountryNameMaxLength)]
        public string Country { get; set; } = null!;

        [Required]
        public int ClientId { get; set; }

        [Required]
        [ForeignKey(nameof(ClientId))]
        public virtual Client Client { get; set; } = null!;
    }
}
