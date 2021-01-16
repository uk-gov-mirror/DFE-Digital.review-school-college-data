using System;
using System.Data.EntityClient;
using System.Linq;
using System.Transactions;
using Web09.Checking.DataAccess;
using Web09.Common.Exceptions;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSResult : Logic.TSBase
    {
        public static void AttachEvidence(int resultId, int changeId, string barcode, string documentPath, string documentType)
        {
            //TODO: Galiya, 16-03-2010: This method doesn't behave correctly if the result has been previously withdrawn and the change involved is "un-withdrawal".
            // The changeID passed is available only in ResultRequestChanges table and not presented in ResultChanges

            // TODO
            // TFS 19542 : school 3715400, (German) pupil 40026, home page, print evidence, KS45 UAT             
            // ChrisB, 12 December 2012, neither does it work if we have had a late result, this has a different changeID, but we get passed the earlier changeid
            // rrcQ doesnt find a ResultRequestChange for this changeid, where the dateend is null ( beause its now the date of the dataload )
            // so it attempts to insert a new ResultRequestChanges row, which breaks the primary key constraint
            // because prevRRC is null at line 168

            try
            {
                using (TransactionScope transaction = TransactionScopeFactory.CreateTransactionScope() )
                {
                    using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
                    {
                        conn.Open();

                        using (Web09_Entities context = new Web09_Entities(conn))
                        {
                            ResultChanges rc = null;

                            //if the resultId is -1 it is from add results
                            if (resultId == -1)
                            {
                                rc = (from r in context.ResultChanges
                                      where r.ChangeID == changeId 
                                      select r).First();

                                resultId = rc.ResultID;
                            }
                            else
                            {
                                //get the result change entry of this request
                                rc = (from r in context.ResultChanges
                                      where r.ChangeID == changeId && r.ResultID == resultId
                                      select r).First();
                            }

                            //if last RC is not found it is an error 
                            if (rc == null)
                                throw new Exception();

                            rc.ResultsReference.Load();

                            
                            ResultRequests prevRR = (from rq in context.ResultRequests
                                                     where rq.Changes.ChangeID == changeId                                                     
                                                     orderby rq.ResultRequestID descending
                                                     select rq).First();

                            prevRR.ResultEvidenceReference.Load();
                            prevRR.ChangesReference.Load();

                            if (prevRR.ResultEvidence != null)
                            {
                                prevRR.ResultEvidence.EvidenceReference.Load();
                            }

                            ResultRequestChanges prevRRC = null;
                            var rrcQ = (from rqc in context.ResultRequestChanges
                                                     where rqc.Changes.ChangeID == changeId
                                                            && rqc.ResultRequests.ResultRequestID == prevRR.ResultRequestID
                                                            && rqc.DateEnd == null                                                     
                                                     select rqc).ToList();

                            if (rrcQ.Count > 0)
                            {
                                prevRRC = rrcQ.First();
                                prevRRC.ResultReasonsReference.Load();
                                prevRRC.ScrutinyStatusReference.Load();
                            }

                            //there is the exact same evidence for this request already there
                            //so do not create a new one
                            if (prevRR.ResultEvidence != null && prevRR.ResultEvidence.Evidence.Barcode.Equals(barcode)
                                && prevRR.ResultEvidence.Evidence.DocumentLocation.Equals(documentPath))
                            {
                                return;
                            }

                            Evidence newE = null;
                            ScrutinyStatus ss = null;

                            //this is the status for electronic evidence submittedand yet to be scrutinized
                            //at Forvus
                            ScrutinyStatus ssLogged = (from s in context.ScrutinyStatus
                                                       where s.ScrutinyStatusDescription == "Logged"
                                                       select s).First();

                            //this is for printed evidence yet to be scanned in
                            ScrutinyStatus ssAwaiting = (from s in context.ScrutinyStatus
                                                         where s.ScrutinyStatusDescription == "Awaiting Evidence"
                                                         select s).First();

                            //printed evidence
                            if (!barcode.Equals(string.Empty))
                            {
                                int evidenceId = Convert.ToInt32(barcode);

                                //retrieve the evidence entry that was created
                                newE = (from e in context.Evidence
                                        where e.EvidenceID == evidenceId
                                        select e).First();
                                newE.Barcode = barcode;
                                newE.DocumentLocation = documentPath;
                                newE.DocumentType = documentType;
                                context.ApplyPropertyChanges("Evidence", newE);

                                ss = ssAwaiting; //set it to awaiting evidence if it is printed
                            }

                            //electronic evidence
                            if (!documentPath.Equals(string.Empty))
                            {
                                //get the existing evidence entry if it was already created
                                var qE = (from e in context.Evidence
                                          where e.DocumentLocation.Equals(documentPath)
                                          select e).ToList();
                                if (qE.Count > 0)
                                {
                                    newE = qE.First();                                    
                                }
                                else
                                {
                                    //else create a new evidence entry
                                    newE = new Evidence();
                                    newE.Barcode = "";
                                    newE.DateCreated = DateTime.Now;
                                    newE.DocumentLocation = documentPath;
                                    newE.DocumentType = documentType; ;
                                    context.AddToEvidence(newE);
                                }

                                ss = ssLogged; //set it to Logged if it is electronic
                            }

                            
                            //create a result evidence entry
                            ResultEvidence re = new ResultEvidence();                         
                            re.Evidence = newE;                            
                            context.AddToResultEvidence(re);
                            
                            //now update the entry in ResultRequest table to point to attached evidence                                               
                            ResultRequests rrNew = prevRR;
                            rrNew.Results = rc.Results;
                            rrNew.ResultEvidence = re;
                            rrNew.ScrutinyStatus = ss;
                            context.Attach(prevRR);
                            context.ApplyPropertyChanges("ResultRequests", rrNew);

                            if (prevRRC != null)
                            {
                                //now update the entry in ResultRequestChanges table to have the new SS code                                               
                                ResultRequestChanges rrcNew = prevRRC;
                                rrcNew.ScrutinyStatus = ss;
                                context.Attach(prevRRC);
                                context.ApplyPropertyChanges("ResultRequestChanges", rrcNew);
                            }
                            else
                            {
                                //backward compatibility, since RRC table was introduced later
                                //add a rRc row if it doesnt exist
                                //TODO 19542, this can cause a primary key violation in some circumstances
                                ResultRequestChanges rrcNew = new ResultRequestChanges();
                                rrcNew.Changes = rrNew.Changes;
                                rrcNew.ResultRequests = rrNew;
                                rrcNew.ScrutinyStatus = ss;
                                context.AddToResultRequestChanges(rrcNew);
                            }

                            context.SaveChanges();
                        }
                    }

                    transaction.Complete();
                }
            }
            catch(Exception ex)
            {
               throw Web09Exception.GetBusinessException(Web09MessageList.ErrorAttachingEvidence);
            }
   
        }
    }
}
