namespace Dfe.Rscd.Api.BusinessLogic.Entities
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