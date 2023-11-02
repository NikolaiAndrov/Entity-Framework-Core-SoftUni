namespace Artillery.Data.Models
{
    using Artillery.Common;
    using System.ComponentModel.DataAnnotations;

    public class Manufacturer
    {
        public Manufacturer()
        {
            this.Guns = new HashSet<Gun>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(ValidationConstants.ManufacturerNameMaxLength)]
        public string ManufacturerName { get; set; } = null!;

        [Required]
        [MaxLength(ValidationConstants.FoundedMaxLength)]
        public string Founded { get; set; } = null!;

        public virtual ICollection<Gun> Guns { get; set; }
    }
}
