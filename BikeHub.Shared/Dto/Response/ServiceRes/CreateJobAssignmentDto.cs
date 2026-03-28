namespace BikeHub.Shared.Dto.Response.ServiceRes
{
    public class CreateJobAssignmentDto
    {
        // --- Service Job Details ---
        public int CustomerId { get; set; }
        public int StoreId { get; set; }
        public string BikeModel { get; set; }
        public string BikeNumber { get; set; }
        public string ProblemDescription { get; set; }
        public int ServiceStatus { get; set; }
        public decimal EstimatedCost { get; set; }
        public decimal? FinalCost { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        // --- Assignment Details ---
        public int MechanicId { get; set; }
        public DateTime AssignedDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public int AssignmentStatus { get; set; } 
        public int AssignedBy { get; set; }
        public int EstimatedDuration { get; set; }
        public DateTime StartTime { get; set; }
    }
}
