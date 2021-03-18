namespace Dfe.Rscd.Web.Application.Security.DTO
{
    public class Organisation
    {
        public string id { get; set; }
        public string name { get; set; }
        public Category category { get; set; }
        public Type type { get; set; }
        public string urn { get; set; }
        public object uid { get; set; }
        public string ukprn { get; set; }
        public string establishmentNumber { get; set; }
        public Status status { get; set; }
        public object closedOn { get; set; }
        public string address { get; set; }
        public string telephone { get; set; }
        public Region region { get; set; }
        public Localauthority localAuthority { get; set; }
        public Phaseofeducation phaseOfEducation { get; set; }
        public int? statutoryLowAge { get; set; }
        public int? statutoryHighAge { get; set; }
        public string legacyId { get; set; }
        public object companyRegistrationNumber { get; set; }
    }

    public class Category
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class Type
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class Status
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Region
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class Localauthority
    {
        public string id { get; set; }
        public string name { get; set; }
        public string code { get; set; }
    }

    public class Phaseofeducation
    {
        public int id { get; set; }
        public string name { get; set; }
    }
}
