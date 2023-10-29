namespace Trucks.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;
    using Trucks.Common;

    [XmlType("Truck")]
    public class ImportTruckDto
    {
        [MinLength(ValidationConstants.RegistrationNumberLength)]
        [MaxLength(ValidationConstants.RegistrationNumberLength)]
        [RegularExpression(ValidationConstants.RegistrationNumberRegex)]
        [XmlElement("RegistrationNumber")]
        public string? RegistrationNumber { get; set; }

        [Required]
        [MinLength(ValidationConstants.VinNumber)]
        [MaxLength(ValidationConstants.VinNumber)]
        [XmlElement("VinNumber")]
        public string VinNumber { get; set; } = null!;

        [Required]
        [Range(ValidationConstants.TankCapacityMinValue, ValidationConstants.TankCapacityMaxValue)]
        [XmlElement("TankCapacity")]
        public int TankCapacity { get; set; }

        [Required]
        [Range(ValidationConstants.CargoCapacityMinValue, ValidationConstants.CargoCapacityMaxValue)]
        [XmlElement("CargoCapacity")]
        public int CargoCapacity { get; set; }

        [Required]
        [Range(ValidationConstants.CategoryTypeMinValue, ValidationConstants.CategoryTypeMaxValue)]
        [XmlElement("CategoryType")]
        public int CategoryType { get; set; }

        [Required]
        [Range(ValidationConstants.MakeTypeMinValue, ValidationConstants.MakeTypeMaxValue)]
        [XmlElement("MakeType")]
        public int MakeType { get; set; }
    }
}
