﻿namespace Footballers.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class TeamFootballer
    {
        [Required]
        [ForeignKey(nameof(Team))]
        public int TeamId { get; set; }

        [Required]
        public virtual Team Team { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(Footballer))]
        public int FootballerId { get; set; }

        [Required]
        public virtual Footballer Footballer { get; set; } = null!;
    }
}
