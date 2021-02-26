using System;

namespace Dfe.Rscd.Api.Domain.Entities
{
    public class Result
    {
        public int? ExamNumber { get; set; }
        public int? ExamYear { get; set; }
        public string SeasonCode { get; set; }
        public string SubjectCode { get; set; }
        public string QualificationTypeCode { get; set; }
        public int? AwardingBodyNumber { get; set; }
        public string BoardSubjectNumber { get; set; }
        public int? RIncl { get; set; }
        public string QAN { get; set; }
        public int? MatchReg { get; set; }
        public int? SubLevelCode { get; set; }
        public string NationalCentreNumber { get; set; }
        public double? FineGrade { get; set; }
        public string TierCode { get; set; }
        public string TestMark { get; set; }
        public string GradeCode { get; set; }
        public int? PortlandResultID { get; set; }
        public DateTime ExamDate { get; set; }
        public string ScaledScore { get; set; }
    }
}