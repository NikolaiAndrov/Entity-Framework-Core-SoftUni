using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Trucks.Data.Models
{
    public class ClientTruck
    {
        [Required]
        public int ClientId { get; set; }

        [Required]
        [ForeignKey(nameof(ClientId))]
        public virtual Client Client { get; set; } = null!;

        [Required]
        public int TruckId { get; set; }

        [Required]
        [ForeignKey(nameof(TruckId))]
        public virtual Truck Truck { get; set; } = null!;
    }
}
