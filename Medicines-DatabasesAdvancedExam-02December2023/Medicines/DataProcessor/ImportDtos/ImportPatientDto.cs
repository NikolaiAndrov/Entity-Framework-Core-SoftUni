namespace Medicines.DataProcessor.ImportDtos
{
	using Newtonsoft.Json;
	using System.ComponentModel.DataAnnotations;

	public class ImportPatientDto
	{
        public ImportPatientDto()
        {
            this.Medicines = new List<int>();
        }

        [Required]
		[StringLength(100, MinimumLength = 5)]
		[JsonProperty("FullName")]
		public string FullName { get; set; } = null!;

		[Required]
		[Range(0, 2)]
		[JsonProperty("AgeGroup")]
		public int AgeGroup { get; set; }

		[Required]
		[Range(0, 1)]
		[JsonProperty("Gender")]
		public int Gender { get; set; }

		[JsonProperty("Medicines")]
		public ICollection<int> Medicines { get; set; }
	}
}
