﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Dfe.CspdAlpha.Web.Domain.Entities
{
    public class ConfirmationRecord
    {
        public string UserId { get; set; }
        public string EstablishmentId { get; set; }
        public bool ReviewCompleted { get; set; }
        public bool DataConfirmed { get; set; }
    }
}
