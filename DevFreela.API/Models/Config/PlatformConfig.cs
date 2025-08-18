namespace DevFreela.API.Models.Config
{
    public class PlatformConfig
    {
        public decimal Commission { get; set; } // ex: 0.10 => 10%
        public int ProposalExpirationDays { get; set; }
    }
}
