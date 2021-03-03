namespace Dfe.Rscd.Web.ApiClient
{
    public partial class Gender
    {
        public static Gender Female => new() {Code = "F", Description = "Female"};
        public static Gender Male => new() {Code = "M", Description = "Male"};

        public static Gender FromCode(string code)
        {
            if (code == "M" || code == "Male")
                return Male;

            return Female;
        }
    }
}