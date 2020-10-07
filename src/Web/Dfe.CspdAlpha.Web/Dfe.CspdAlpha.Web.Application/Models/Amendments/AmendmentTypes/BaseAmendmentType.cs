using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dfe.CspdAlpha.Web.Application.Models.Common;

namespace Dfe.CspdAlpha.Web.Application.Models.Amendments.AmendmentTypes
{
    public class BaseAmendmentType
    {
        public AmendmentType AmendmentType => AmendmentType.RemovePupil;
        public PupilDetails PupilDetails { get; set; }
    }
}
