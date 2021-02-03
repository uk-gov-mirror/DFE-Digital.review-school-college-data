using Dfe.Rscd.Api.Domain.Entities;

namespace Dfe.Rscd.Api.Services
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