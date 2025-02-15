namespace Options.Domain.Models
{
    public class UpdateSettingsRequestModel
    {
        public required Guid UserId { get; set; }

        public double RegulatoryFee { get; set; }

        public double Tax { get; set; }
    }
}
