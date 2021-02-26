namespace Dfe.Rscd.Api.Infrastructure.CosmosDb.DTOs
{
    public class ResultDTO
    {
        public int? ExamNumber { get; set; }
        public int? ExamYear { get; set; }
        public string SeasonCode { get; set; }
        public string SubjectCode { get; set; }
        public string QualificationTypeCode { get; set; }
        public int? AwardingBodyNumber { get; set; }
        public string BoardSubjectNumber { get; set; }
        public int R_INCL { get; set; }
        public string QAN { get; set; }
        public int? MATCHREG { get; set; }
        public int? SubLevelCode { get; set; }
        public string NationalCentreNumber { get; set; }
        public double? FineGrade { get; set; }
        public string TierCode { get; set; }
        public string TestMark { get; set; }
        public string GradeCode { get; set; }
        public int? PortlandResultID { get; set; }
        public string Exam_Date { get; set; }
        public string ScaledScore { get; set; }
    }
}