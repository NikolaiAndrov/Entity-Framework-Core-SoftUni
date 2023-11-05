namespace Theatre.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Ticket
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public sbyte RowNumber { get; set; }

        [Required]
        public int PlayId { get; set; }

        [Required]
        [ForeignKey(nameof(PlayId))]
        public virtual Play Play { get; set; } = null!;

        [Required]
        public int TheatreId { get; set; }

        [Required]
        [ForeignKey(nameof(TheatreId))]
        public virtual Theatre Theatre { get; set; } = null!;
    }
}
