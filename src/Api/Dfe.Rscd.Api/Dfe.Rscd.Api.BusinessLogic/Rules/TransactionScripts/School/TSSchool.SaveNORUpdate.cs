using System;
using System.Data.EntityClient;
using System.Linq;
using System.Transactions;
using Web09.Checking.Business.Logic.Entities;
using Web09.Checking.DataAccess;
using Web09.Common.Exceptions;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSSchool : Logic.TSBase
    {
        private static SchoolValues CreateSchoolValuesObject(string type, string value, Changes changeObj, Schools school, Web09_Entities context)
        {
            SchoolValueTypes sType = null;

            if (type.Equals("PUPOST16"))
            {
                sType = (from st in context.SchoolValueTypes
                         where st.KeyStage == 5
                         && st.ValueTypeCode == type
                         select st).First();
            }
            else
            {
                sType = (from st in context.SchoolValueTypes
                         where st.KeyStage == 4
                         && st.ValueTypeCode == type
                         select st).First();
            }

            var existingSchoolValue = (from v in context.SchoolValues
                                       where v.Schools.DFESNumber == school.DFESNumber
                                       && v.DateEnd == null && v.SchoolValueTypes.ValueTypeCode == type
                                       select v).ToList();

            //if there is an existing value set its DateEnd = Now
            if (existingSchoolValue.Count > 0)
            {
                SchoolValues  sv = existingSchoolValue.Last();
                sv.DateEnd = DateTime.Now;
                context.ApplyPropertyChanges("SchoolValues", sv);               
            }

            //now create a new choolValue
            SchoolValues returnValue = new SchoolValues();
            returnValue.Changes = changeObj;            
            returnValue.Schools = school;
            returnValue.SchoolValueTypes = sType;
            returnValue.Value = value;
            context.AddToSchoolValues(returnValue);

            context.SaveChanges();

            return returnValue;
        }

        public static void SaveNORUpdate(string pupost16, string pupsatks4sensp, string pupsatks4sena, string boysatks4, string girlsatks4, string comments, string barcode, string documentPath, string documentType, bool isWithEvidence, int dfesNumber, UserContext userContext, bool autoAccept)
        {
            using (TransactionScope transaction = TransactionScopeFactory.CreateTransactionScope() )
            {
                using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
                {
                    conn.Open();

                    using (Web09_Entities context = new Web09_Entities(conn))
                    {
                        //check for pending nor request only if it is a normal nor update
                        if (string.IsNullOrEmpty(pupost16))
                        {
                            if (!TSSchool.ValidateSchoolHasNoPriorSchoolRequests(context, dfesNumber))
                            {
                                //since there as an exception if the save is for printed evidence
                                //delete the evidence entry that was created earlier since it will no longer be used                            
                                if (isWithEvidence)
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
                                }

                                throw Web09Exception.GetBusinessException(Web09MessageList.AnotherNORRequestExists);
                            }
                        }

                        Evidence newE = null;
                        bool isPrintedEvidence = false; 

                        try
                        {
                            //Create the change object.
                            Changes changeObj = CreateChangeObject(context, 1, userContext);
                            context.AddToChanges(changeObj);
                            context.SaveChanges();

                            Schools school = (from s in context.Schools
                                              where s.DFESNumber == dfesNumber
                                              select s).First();

                            //if this is a Post16 AAT request simply save and return
                            if (!string.IsNullOrEmpty(pupost16))
                            {
                                CreateSchoolValuesObject("PUPOST16", pupost16, changeObj, school, context);
                                context.SaveChanges();
                                transaction.Complete();
                                return;
                            }

                            //fill up the evidence object if evidence is attached
                            SchoolEvidence se = null;

                            if (isWithEvidence)
                            {
                                //printed evidence
                                isPrintedEvidence = false;
                                if (!barcode.Equals(string.Empty))
                                {
                                    isPrintedEvidence = true;
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

                                //get the last used SchoolEvidenceID
                                var query = (from s in context.SchoolEvidence
                                             orderby s.SchoolEvidenceID descending
                                             select s).ToList();

                                SchoolEvidence prevSE = null;
                                if (query.Count > 0)
                                {
                                    prevSE = query.First();
                                }

                                //create a school evidence entry
                                se = new SchoolEvidence();
                                if (prevSE != null)
                                    se.SchoolEvidenceID = prevSE.SchoolEvidenceID + 1;
                                else
                                    se.SchoolEvidenceID = 1;

                                se.Evidence = newE;
                                se.Schools = school;
                                se.Changes = changeObj;
                                context.AddToSchoolEvidence(se);
                            }

                            //now update values  
                            if (!pupsatks4sensp.Equals(string.Empty))
                                CreateSchoolValuesObject("S2SENSP", pupsatks4sensp, changeObj, school, context);
                            if (!pupsatks4sena.Equals(string.Empty))
                                CreateSchoolValuesObject("S2SENA", pupsatks4sena, changeObj, school, context);
                            if (!boysatks4.Equals(string.Empty))
                                CreateSchoolValuesObject("BOYSKS4", boysatks4, changeObj, school, context);
                            if (!girlsatks4.Equals(string.Empty))
                                CreateSchoolValuesObject("GIRLSKS4", girlsatks4, changeObj, school, context);

                            //create schoolrequest entry

                            //get the last used schoolrequestID
                            var lastrequest = (from sr in context.SchoolRequests
                                               orderby sr.SchoolRequestID descending
                                               select sr).ToList();

                            SchoolRequests prevSR = null;

                            SchoolRequests sRequest = new SchoolRequests();

                            if (lastrequest.Count > 0)
                            {
                                prevSR = lastrequest.First();
                                sRequest.SchoolRequestID = prevSR.SchoolRequestID + 1;
                            }
                            else
                                sRequest.SchoolRequestID = 1;

                            sRequest.Changes = changeObj;
                            sRequest.Schools = school;
                            if (isWithEvidence)
                            {
                                sRequest.SchoolEvidence = se;
                            }
                            context.AddToSchoolRequests(sRequest);
                     
                            //create new schoolrequestchanges entry
                            SchoolRequestChanges src = new SchoolRequestChanges();
                            src.Changes = changeObj;
                            src.Comments = comments;
                            src.SchoolRequests = sRequest;
                            if ( autoAccept )
                            {
                                src.ScrutinyStatus = (from ss in context.ScrutinyStatus
                                                      where ss.ScrutinyStatusDescription == "Accepted Automatically"
                                                      select ss).First();
                            }
                            else if (isWithEvidence && isPrintedEvidence==false)
                            {
                                src.ScrutinyStatus = (from ss in context.ScrutinyStatus
                                                      where ss.ScrutinyStatusDescription == "Pending Forvus"
                                                      select ss).First();
                            }
                            else if (isWithEvidence && isPrintedEvidence == true)
                            {
                                src.ScrutinyStatus = (from ss in context.ScrutinyStatus
                                                      where ss.ScrutinyStatusDescription == "Awaiting Evidence"
                                                      select ss).First();
                            }
                            else
                            {
                                src.ScrutinyStatus = (from ss in context.ScrutinyStatus
                                                      where ss.ScrutinyStatusDescription == "Pending Forvus"
                                                      select ss).First();
                            }

                            context.AddToSchoolRequestChanges(src);


                            context.SaveChanges();
                        }
                        catch
                        {
                            //since there as an exception if the save as for printed evidence
                            //delete the evidence entry that was created earlier since it will no longer be used                            
                            context.DeleteObject(newE);

                            throw Web09Exception.GetBusinessException(Web09MessageList.ErrorUpdatingSchoolNOR);
                        }

                    }
                }

                transaction.Complete();
            }
        }
    }
}
