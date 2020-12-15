using Dfe.Rscd.Api.Domain.Core.Enums;

namespace Dfe.Rscd.Api.Domain.Core
{
    public class SourceOfAllocation
    {
        public SourceOfAllocation(int year, Allocation allocation)
        {
            Year = year;
            Allocation = allocation;
        }

        public int Year { get; set; }
        public Allocation Allocation { get; set; }
    }
}