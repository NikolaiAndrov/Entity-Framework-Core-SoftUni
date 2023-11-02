namespace Artillery.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class CountryGun
    {
        [Required]
        public int CountryId { get; set; }

        [Required]
        [ForeignKey(nameof(CountryId))]
        public virtual Country Country { get; set; } = null!;

        [Required]
        public int GunId { get; set; }

        [Required]
        [ForeignKey(nameof(GunId))]
        public virtual Gun Gun { get; set; } = null!;
    }
}
