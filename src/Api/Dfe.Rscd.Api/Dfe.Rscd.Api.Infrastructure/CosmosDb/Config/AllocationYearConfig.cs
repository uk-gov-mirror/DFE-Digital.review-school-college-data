namespace Dfe.Rscd.Api.Infrastructure.CosmosDb.Config
{
    public class AllocationYearConfig : IAllocationYearConfig
    {
        public string Value { get; set; }
    }

    public interface IAllocationYearConfig
    {
        string Value { get; set; }
    }
}