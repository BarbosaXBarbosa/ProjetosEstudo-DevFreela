using DevFreela.API.Models.Config;
using Microsoft.Extensions.Options;

namespace DevFreela.API.Services
{
    public interface IConfigService
    {
        decimal GetMinimumCost();
        decimal GetMaximumCost();

        decimal GetPlatformCommission(); // ex: 0.10
        int GetProposalExpirationDays();
    }

    public class ConfigService : IConfigService
    {
        private readonly FreelanceTotalCostConfig _costConfig;
        private readonly PlatformConfig _platformConfig;

        public ConfigService(
            IOptions<FreelanceTotalCostConfig> costOptions,
            IOptions<PlatformConfig> platformOptions)
        {
            _costConfig = costOptions.Value;
            _platformConfig = platformOptions.Value;
        }

        public decimal GetMinimumCost() => _costConfig.Minimum;
        public decimal GetMaximumCost() => _costConfig.Maximum;

        public decimal GetPlatformCommission() => _platformConfig.Commission;
        public int GetProposalExpirationDays() => _platformConfig.ProposalExpirationDays;
    }
}


