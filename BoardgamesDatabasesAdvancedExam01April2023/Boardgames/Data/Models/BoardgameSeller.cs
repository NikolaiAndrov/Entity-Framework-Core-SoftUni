namespace Boardgames.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class BoardgameSeller
    {
        [Required]
        public int BoardgameId { get; set; }

        [Required]
        [ForeignKey(nameof(BoardgameId))]
        public virtual Boardgame Boardgame { get; set; } = null!;

        [Required]
        public int SellerId { get; set; }

        [Required]
        [ForeignKey(nameof(SellerId))]
        public virtual Seller Seller { get; set; } = null!;
    }
}
