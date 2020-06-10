using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;
using Dfe.CspdAlpha.Web.Domain.Core;
using Dfe.CspdAlpha.Web.Domain.Entities;

namespace Dfe.CspdAlpha.Web.Domain.Interfaces
{
    public interface IAmendmentService
    {
        IEnumerable<AmendmentRecord> Get(PupilId id);
        string Create(IEvent newEvent, Audit audit);

        void Update(IEvent newEvent, Audit audit);
    }
}
