namespace Theatre.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Common;

    public class Cast
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(ValidationConstants.CastFullNameMaxLength)]
        public string FullName { get; set; } = null!;

        [Required]
        public bool IsMainCharacter { get; set; }

        [Required]
        [MaxLength(ValidationConstants.CastPhoneNumberLength)]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        public int PlayId { get; set; }

        [Required]
        [ForeignKey(nameof(PlayId))]
        public virtual Play Play { get; set; } = null!;
    }
}
