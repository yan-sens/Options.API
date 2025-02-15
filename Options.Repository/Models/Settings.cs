using System.ComponentModel.DataAnnotations;

namespace Options.DbContext.Models
{
    public class Settings
    {
        [Key]
        public required Guid Id { get; set; }

        public required Guid UserId { get; set; }

        public double RegulatoryFee { get; set; }

        public double Tax { get; set; }
    }
}
