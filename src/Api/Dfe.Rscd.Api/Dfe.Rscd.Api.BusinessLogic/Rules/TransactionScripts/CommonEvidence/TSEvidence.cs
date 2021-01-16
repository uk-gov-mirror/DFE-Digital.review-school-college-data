using System;
using System.Data.EntityClient;

using Web09.Checking.DataAccess;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSEvidence : Logic.TSBase
    {
        public TSEvidence()
        {
        }

        public static string CreateEvidenceEntry()
        {
            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {
                    Evidence newE = new Evidence();
                    newE.Barcode = "";
                    newE.DateCreated = DateTime.Now;
                    newE.DocumentLocation = "";
                    context.AddToEvidence(newE);
                    context.SaveChanges();

                    return newE.EvidenceID.ToString();
                }
            }
        }
    }
}
