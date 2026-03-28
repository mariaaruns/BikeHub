namespace BikeHub.Shared.Dto.Response.ServiceRes
{
    public class AssignedJobDto
    {

        public long serviceJobId { get; set;}
        public string JobCardNumber { get; set; }
        public string ServiceStatus { get; set; }
        public string ProblemDescription { get; set; }
        public string BikeModel { get; set; }
        public string CustomerName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public DateTime? StartTime { get; set; }
        public int EstimatedDuration { get; set; }
        public int ActualDuration { get; set; }
       
        public bool IsCompleted => CompletedDate.HasValue;
    }
}
