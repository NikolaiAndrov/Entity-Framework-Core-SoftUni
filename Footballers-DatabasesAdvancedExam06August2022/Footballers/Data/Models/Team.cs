﻿namespace Footballers.Data.Models
{
    using Footballers.Common;
    using System.ComponentModel.DataAnnotations;

    public class Team
    {
        public Team()
        {
            this.TeamsFootballers = new HashSet<TeamFootballer>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(ValidationConstants.TeamNameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(ValidationConstants.TeamNationalityMaxLength)]
        public string Nationality { get; set; } = null!;

        [Required]
        public int Trophies { get; set; }
        
        public virtual ICollection<TeamFootballer> TeamsFootballers { get; set; }
    }
}
