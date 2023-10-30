namespace Invoices.DataProcessor.ImportDto
{
    using Invoices.Common;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
     
    public class ImportProductDto
    {
        public ImportProductDto()
        {
            this.Clients = new HashSet<int>();
        }

        [Required]
        [MinLength(ValidationConstants.ProductNameMinLength)]
        [MaxLength(ValidationConstants.ProductNameMaxLength)]
        [JsonProperty("Name")]
        public string Name { get; set; } = null!;

        [Required]
        [JsonProperty("Price")]
        public decimal Price { get; set; }

        [Required]
        [Range(ValidationConstants.CategoryTypeMinValue, 
            ValidationConstants.CategoryTypeMaxValue)]
        [JsonProperty("CategoryType")]
        public int CategoryType { get; set; }

        [JsonProperty("Clients")]
        public ICollection<int> Clients { get; set; }
    }
}
