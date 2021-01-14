namespace Dfe.Rscd.Api.Infrastructure.CosmosDb.DTOs
{
    public class AddressDTO
    {
        public string DFESNumber { get; set; }
        public string ADDRESS1 { get; set; }
        public string ADDRESS2 { get; set; }
        public string ADDRESS3 { get; set; }
        public string TOWN { get; set; }
        public string COUNTY { get; set; }
        public string PCODE { get; set; }
        public string TELNUM { get; set; }
    }
}