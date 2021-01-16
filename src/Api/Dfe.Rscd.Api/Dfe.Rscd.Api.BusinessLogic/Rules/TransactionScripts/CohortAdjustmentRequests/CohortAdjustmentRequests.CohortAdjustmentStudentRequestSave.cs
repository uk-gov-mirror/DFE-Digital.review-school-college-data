using System;
using System.Data.EntityClient;
using System.Linq;
using Web09.Checking.Business.Logic.Entities;
using Web09.Checking.DataAccess;
using Web09.Common.Exceptions;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSCohortAdjustmentRequests: Logic.TSBase
    {
        public static void CohortAdjustmentStudentRequestSave(CohortAdjustmentRequestEntity request, UserContext userContext)
        {
            try
            {
                using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
                {
                    conn.Open();

                    using (Web09_Entities context = new Web09_Entities(conn))
                    {
                        StudentRequestChanges sr = context.StudentRequestChanges.Include("Changes").Where(r => r.StudentRequestID == request.StudentRequest.StudentRequestID && r.DateEnd==null).FirstOrDefault();
                        if (sr == null)
                            throw new BusinessLevelException("Request does not exists");

                        if (sr.Changes.ChangeID != request.StudentRequest.StudentRequestChanges.FirstOrDefault().Changes.ChangeID)
                            throw new BusinessLevelException("Request has been updated by another user.");

                        StudentChanges stdChange = context.StudentChanges
                            .Include("Changes")
                            .Where(r => r.StudentID == request.StudentRequest.Students.StudentID && r.DateEnd==null)
                            .FirstOrDefault();

                        if (request.ParentInfochangeID > 0 && stdChange.Changes.ChangeID != request.ParentInfochangeID)
                            throw new BusinessLevelException("Student information has been updated by another user.");

                        // create change object
                        Changes newChange = CreateChangeObject(context, 1, userContext);
                        context.AddToChanges(newChange);

                        // create new request
                        StudentRequestChanges srNew = new StudentRequestChanges();
                        
                        String amdCode=request.StudentRequest.StudentRequestChanges.First().AmendCodes.AmendCode;
                        srNew.AmendCodes = amdCode!=""?context.AmendCodes.Where(c => c.AmendCode == amdCode).FirstOrDefault(): null;

                        srNew.Changes = newChange;
                        srNew.Comments = request.StudentRequest.StudentRequestChanges.FirstOrDefault().Comments;
                        srNew.DCSFNotification = request.StudentRequest.StudentRequestChanges.FirstOrDefault().DCSFNotification;
                        
                        int reasonID=0;
                        if(request.StudentRequest.StudentRequestChanges.FirstOrDefault().Reasons.ReasonID > 0)
                            reasonID=request.StudentRequest.StudentRequestChanges.FirstOrDefault().Reasons.ReasonID;

                        srNew.Reasons = reasonID > 0 ? context.Reasons.Where(r => r.ReasonID == reasonID).FirstOrDefault() : null;
                        
                        String scrutinyCode=request.StudentRequest.StudentRequestChanges.First().ScrutinyStatus.ScrutinyStatusCode;
                        srNew.ScrutinyStatus = context.ScrutinyStatus.Where(ss => ss.ScrutinyStatusCode == scrutinyCode).First();
                        srNew.StudentRequests = context.StudentRequests.Where(s => s.StudentRequestID== request.StudentRequest.StudentRequestID).First();                            

                        // new request status
                        context.AddToStudentRequestChanges(srNew);

                        // turn off the old one
                        sr.DateEnd = DateTime.Now;
                        context.ApplyPropertyChanges("StudentRequestChanges", sr);
                        
                        context.SaveChanges();
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void CohortAdjustmentSchoolRequestSave(CohortAdjustmentRequestEntity request, UserContext userContext)
        {
            try
            {
                using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
                {
                    conn.Open();

                    using (Web09_Entities context = new Web09_Entities(conn))
                    {
                        SchoolRequestChanges sr = context.SchoolRequestChanges.Include("Changes").Where(r => r.SchoolRequestID == request.SchoolRequest.SchoolRequestID && r.DateEnd == null).FirstOrDefault();
                        if (sr == null)
                            throw new BusinessLevelException("Request does not exists");

                        if (sr.Changes.ChangeID != request.SchoolRequest.SchoolRequestChanges.FirstOrDefault().Changes.ChangeID)
                            throw new BusinessLevelException("Request has been updated by another user.");

                        //SchoolChanges stdChange = context.SchoolChanges
                        //    .Include("Changes")
                        //    .Where(r => r.SchoolID == request.SchoolRequest.Schools.SchoolID && r.DateEnd == null)
                        //    .FirstOrDefault();

                        //if (request.ParentInfochangeID > 0 && stdChange.Changes.ChangeID != request.ParentInfochangeID)
                        //    throw new BusinessLevelException("School information has been updated by another user.");

                        // create change object
                        Changes newChange = CreateChangeObject(context, 1, userContext);
                        context.AddToChanges(newChange);

                        // create new request
                        var srNew = new SchoolRequestChanges();

                        //String amdCode = request.SchoolRequest.SchoolRequestChanges.First().AmendCodes.AmendCode;
                        //srNew.AmendCodes = amdCode != "" ? context.AmendCodes.Where(c => c.AmendCode == amdCode).FirstOrDefault() : null;

                        srNew.Changes = newChange;
                        srNew.Comments = request.SchoolRequest.SchoolRequestChanges.FirstOrDefault().Comments;
                        srNew.DCSFNotification = request.SchoolRequest.SchoolRequestChanges.FirstOrDefault().DCSFNotification;

                        //CC11-66 Add rejection reason to NOR update rejection
                        srNew.SchoolReasons = context.SchoolReasons.Where(x => x.SchoolReasonId == request.SchoolReasonId).FirstOrDefault();
                        //int reasonID = 0;
                        //if (request.SchoolRequest.SchoolRequestChanges.FirstOrDefault().Reasons.ReasonID > 0)
                        //    reasonID = request.SchoolRequest.SchoolRequestChanges.FirstOrDefault().Reasons.ReasonID;

                        //srNew.Reasons = reasonID > 0 ? context.Reasons.Where(r => r.ReasonID == reasonID).FirstOrDefault() : null;

                        String scrutinyCode = request.SchoolRequest.SchoolRequestChanges.First().ScrutinyStatus.ScrutinyStatusCode;
                        srNew.ScrutinyStatus = context.ScrutinyStatus.Where(ss => ss.ScrutinyStatusCode == scrutinyCode).First();
                        srNew.SchoolRequests = context.SchoolRequests.Where(s => s.SchoolRequestID == request.SchoolRequest.SchoolRequestID).First();

                        // new request status
                        context.AddToSchoolRequestChanges(srNew);

                        // turn off the old one
                        sr.DateEnd = DateTime.Now;
                        context.ApplyPropertyChanges("SchoolRequestChanges", sr);

                        context.SaveChanges();
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
