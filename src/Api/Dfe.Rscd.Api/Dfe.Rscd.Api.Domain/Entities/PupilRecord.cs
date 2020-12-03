using System;
using System.Collections.Generic;
using System.Text;

namespace Dfe.Rscd.Api.Domain.Entities
{
    public class PupilRecord
    {
        public string Id { get; set; }
        public string ForeName { get; set; }
        public string Surname { get; set; }
        public string UPN { get; set; }
        public string ULN { get; set; }
    }
}
