using Dfe.Rscd.Api.BusinessLogic.Contracts.Entities.Core.Enums;

namespace Dfe.Rscd.Api.BusinessLogic.Contracts.Services.Extensions
{
    public static class AllocationExtensions
    {
        public static Allocation ToAllocation(this string allocation)
        {
            switch (allocation)
            {
                case "AO":
                    return Allocation.AwardingOrganisation;
                case "SC":
                    return Allocation.SchoolCensus;
                case "ILR":
                    return Allocation.IndividualLearnerRecord;
                default:
                    return Allocation.NotAllocated;
            }
        }
    }
}