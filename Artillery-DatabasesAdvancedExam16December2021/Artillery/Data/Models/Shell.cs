namespace Artillery.Data.Models
{
    using Artillery.Common;
    using System.ComponentModel.DataAnnotations;

    public class Shell
    {
        public Shell()
        {
            this.Guns = new HashSet<Gun>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public double ShellWeight { get; set; }

        [Required]
        [MaxLength(ValidationConstants.ShellCaliberMaxLength)]
        public string Caliber { get; set; } = null!;

        public virtual ICollection<Gun> Guns { get; set; }
    }
}
