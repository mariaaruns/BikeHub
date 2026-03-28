namespace BikeHub.Shared.Dto.Response.ServiceRes
{
    public class ServiceJobDetailDto
    {
        public int ServiceJobId { get; set; }
        public string JobId { get; set; } 
        public string Mechanic { get; set; }
        public string Service { get; set; } 
        public string CustomerName { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public string BikeModel { get; set; }
        public string BikeNumber { get; set; }
        public DateTime? CompletedDate { get; set; }
        public decimal? EstimatedCost { get; set; }
        public decimal? FinalCost { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? AssignedDate { get; set; }
        public string Email { get; set; }
        public DateTime? StartTime { get; set; }
        public int EstimatedDuration { get; set; }
        public int ActualDuration { get; set; }
        public string Address { get; set; }
    }
}
