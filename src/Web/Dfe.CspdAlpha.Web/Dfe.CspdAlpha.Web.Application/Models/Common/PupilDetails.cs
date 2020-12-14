using System;
using System.Linq;
using Dfe.Rscd.Web.ApiClient;

namespace Dfe.CspdAlpha.Web.Application.Models.Common
{
    public class PupilDetails : Pupil
    {
        public Keystage KeyStage { get; set; }

        public new string FullName => string.Join(" ", new[] { ForeName, LastName }.Where(n => !string.IsNullOrEmpty(n)));
        public string GetPupilDetailsLabel => KeyStage == Keystage.KS5 ? "View student details" : "View pupil details";
    }
}
