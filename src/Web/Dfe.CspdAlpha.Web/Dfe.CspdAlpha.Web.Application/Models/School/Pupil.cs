namespace Dfe.CspdAlpha.Web.Application.Models.School
{
    public class Pupil
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PupilId { get; set; }

        public string Fullname => $"{FirstName.Trim()} {LastName.Trim()}";
        public string Label => $"{FirstName.Trim()} {LastName.Trim()} (Pupil ID: {PupilId})";
    }
}
