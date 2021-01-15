using System;
using System.Data.EntityClient;
using System.Linq;
using System.Transactions;
using Web09.Checking.Business.Logic.Entities;
using Web09.Checking.Business.Logic.Validation;
using Web09.Checking.DataAccess;
using Web09.Common.Exceptions;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSStudent    
    {
        public static void SavePupilAdjustmentWithEvidence(CompletedStudentAdjustment completedStudentAdjustment, string barcode, string documentPath, string documentType, UserContext uc)
        {
            using (TransactionScope transaction = TransactionScopeFactory.CreateTransactionScope())
            {

                using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
                {
                    conn.Open();

                    using (Web09_Entities context = new Web09_Entities(conn))
                    {
                        Changes changeObj;
                        
                        if (completedStudentAdjustment.StudentRequestID.HasValue && completedStudentAdjustment.StudentRequestID.Value != 0)
                        {
                        
                            //The adjustment has been previously saved. use the same change object for the attachment
                            //of the evidence.
                            changeObj = context.StudentRequests
                                .Where(sr => sr.StudentRequestID == completedStudentAdjustment.StudentRequestID.Value)
                                .Select(sr => sr.Changes)
                                .FirstOrDefault();

                            if (changeObj == null)
                                throw new Exception(String.Format("Error occurred retrieving student request change object"));

                        }
                        else
                        {

                            if (!Student.ValidateStudentHasNoPriorStudentRequests(context, completedStudentAdjustment.StudentID))
                            {

                                //printed evidence
                                if (!barcode.Equals(string.Empty))
                                {
                                    int evidenceId = Convert.ToInt32(barcode);

                                    //retrieve the evidence entry that was created
                                    Evidence ev = (from e in context.Evidence
                                                   where e.EvidenceID == evidenceId
                                                   select e).First();
                                    context.DeleteObject(ev);
                                }

                                throw new Exception(String.Format("A student adjustment request has already been created for student id {0}", completedStudentAdjustment.StudentID.ToString()));
                            }

                            //Create the change object.
                            changeObj = CreateChangeObject(context, 1, uc);
                            context.AddToChanges(changeObj);
                            context.SaveChanges();

                            //Process the Adjustment Request
                            TSIncludeRemovePupil.SaveAdjustmentRequest(context, completedStudentAdjustment, changeObj);

                            //Submit all inserts also consider data concurreny issue while saving this request.
                            context.AcceptAllChanges();

                        }

                        //now do the attach evidence part
                        Evidence newE = null;
                        try
                        {
                            //get the StudentRequest obj for this change
                            StudentRequests prevSR = (from rq in context.StudentRequests
                                                      where rq.Changes.ChangeID == changeObj.ChangeID
                                                             && rq.Students.StudentID == completedStudentAdjustment.StudentID
                                                      select rq).First();

                            prevSR.StudentEvidenceReference.Load();
                            prevSR.StudentsReference.Load();

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
                            }

                            //electronic evidence
                            if (!documentPath.Equals(string.Empty))
                            {
                                //create a new evidence entry
                                newE = new Evidence();
                                newE.Barcode = "";
                                newE.DateCreated = DateTime.Now;
                                newE.DocumentLocation = documentPath;
                                newE.DocumentType = documentType;
                                context.AddToEvidence(newE);
                            }

                            //this is the status when evidence has been submitted is yet to be received
                            //at Forvus
                            ScrutinyStatus ss = (from s in context.ScrutinyStatus
                                                 where s.ScrutinyStatusDescription == "Logged"
                                                 select s).First();

                            StudentRequestChanges src = new StudentRequestChanges();
                            //see if a StudentRequestChanges entry has already been created
                            var query1 = (from sc in context.StudentRequestChanges
                                          where sc.Changes.ChangeID == changeObj.ChangeID
                                          && sc.StudentRequests.StudentRequestID == prevSR.StudentRequestID
                                          select sc).ToList();

                            //if a studentrequestchange entry exists, simply chage the ScrutinyCode
                            if (query1.Count > 0)
                            {
                                src = query1.First();                                
                                StudentRequestChanges srcNew = src;
                                srcNew.ScrutinyStatus = ss;
                                context.Attach(src);
                                context.ApplyPropertyChanges("StudentRequestChanges", srcNew);
                            }
                            else
                            {
                                //create a new studentRequestchange object and set the SSCode.                                
                                src.Changes = changeObj;
                                src.ScrutinyStatus = ss;
                                src.StudentRequests = prevSR;
                                context.AddToStudentRequestChanges(src);
                            }

                            //get the last used StudentEvidenceID
                            var query = (from r in context.StudentEvidence
                                                      orderby r.StudentEvidenceID descending
                                                      select r).ToList();
                            
                            StudentEvidence prevRE = null;
                            if (query.Count > 0)
                            {
                                prevRE = query.First();
                            }
                            
                            //create a student evidence entry
                            StudentEvidence se = new StudentEvidence();
                            if (prevRE != null)
                                se.StudentEvidenceID = prevRE.StudentEvidenceID + 1;
                            else
                                se.StudentEvidenceID = 1;

                            se.Evidence = newE;
                            se.Students = prevSR.Students;
                            context.AddToStudentEvidence(se);


                            //now update the entry in ResultRequest table to point to attached evidence                                               
                            StudentRequests srNew = prevSR;
                            srNew.StudentEvidence = se;
                            context.Attach(prevSR);
                            context.ApplyPropertyChanges("StudentRequests", srNew);


                            context.SaveChanges();
                        }
                        catch
                        {
                            throw Web09Exception.GetBusinessException(Web09MessageList.ErrorAttachingEvidence);
                        }
               
                    }
                }

                transaction.Complete();
            }
            
        }
    }
}
