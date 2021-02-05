namespace Dfe.Rscd.Api.Infrastructure.CosmosDb.Config
{
    public class AllocationYearConfig : IAllocationYearConfig
    {
        public string Value { get; set; }

        // TODO: Find out if this is same every year
        public string CensusDate { get;set; }
    }

    public interface IAllocationYearConfig
    {
        string Value { get; set; }
        string CensusDate { get; set; }
    }
}