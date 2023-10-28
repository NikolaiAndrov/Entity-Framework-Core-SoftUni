﻿namespace Trucks.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Trucks.Common;
    using Trucks.Data.Models.Enums;

    public class Truck
    {
        public Truck()
        {
            this.ClientsTrucks = new HashSet<ClientTruck>();
        }

        [Key]
        public int Id { get; set; }

        [MaxLength(ValidationConstants.RegistrationNumberLength)]
        public string? RegistrationNumber { get; set; }

        [Required]
        [MaxLength(ValidationConstants.VinNumber)]
        public string VinNumber { get; set; } = null!;

        [Required]
        public int TankCapacity { get; set; }

        [Required]
        public int CargoCapacity { get; set; }

        [Required]
        public CategoryType CategoryType { get; set; }

        [Required]
        public MakeType MakeType { get; set; }

        [Required]
        public int DespatcherId { get; set; }

        [Required]
        [ForeignKey(nameof(DespatcherId))]
        public virtual Despatcher Despatcher { get; set; } = null!;

        public virtual ICollection<ClientTruck> ClientsTrucks { get; set; }
    }
}
