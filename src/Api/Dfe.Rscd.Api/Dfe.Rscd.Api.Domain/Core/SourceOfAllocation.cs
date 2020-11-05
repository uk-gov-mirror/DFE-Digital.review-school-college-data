using Dfe.Rscd.Api.Domain.Core.Enums;

namespace Dfe.Rscd.Api.Domain.Core
{
    public class SourceOfAllocation
    {
        public int Year { get; set; }
        public Allocation Allocation { get; set; }

        public SourceOfAllocation(int year, Allocation allocation)
        {
            Year = year;
            Allocation = allocation;
        }
    }
}
