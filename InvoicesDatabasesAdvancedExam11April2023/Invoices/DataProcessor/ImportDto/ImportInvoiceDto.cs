namespace Invoices.DataProcessor.ImportDto
{
    using Invoices.Common;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;

    public class ImportInvoiceDto
    {
        [Required]
        [Range(ValidationConstants.InvoiceNumberMinValue,
            ValidationConstants.InvoiceNumberMaxValue)]
        [JsonProperty("Number")]
        public int Number { get; set; }

        [Required]
        [JsonProperty("IssueDate")]
        public DateTime IssueDate { get; set; }// = null!;

        [Required]
        [JsonProperty("DueDate")]
        public DateTime DueDate { get; set; }// = null!;

        [Required]
        [JsonProperty("Amount")]
        public decimal Amount { get; set; }

        [Required]
        [Range(ValidationConstants.CurrencyTypeMinValue, 
            ValidationConstants.CurrencyTypeMaxValue)]
        [JsonProperty("CurrencyType")]
        public int CurrencyType { get; set; }

        [Required]
        [JsonProperty("ClientId")]
        public int ClientId { get; set; }
    }
}
