using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeHub.Shared.Dto.Request.ServiceReq
{
    public class CreateAndAssignmentDto
    {
        // --- Core Service Details ---
        public int CustomerId { get; set; }
        public int StoreId { get; set; }
        public string BikeModel { get; set; } = string.Empty;
        public string BikeNumber { get; set; } = string.Empty;
        public string? ProblemDescription { get; set; }
        public string ServiceStatus { get; set; } = "Pending";
        public decimal EstimatedCost { get; set; }
        public decimal? FinalCost { get; set; }

        // --- Mechanic Assignment Details ---
        public int? MechanicId { get; set; } // Nullable if not yet assigned
        public DateTime? AssignedDate { get; set; }

        // EstimatedDuration can be a string (e.g., "2 Hours") or TimeSpan
        public string? EstimatedDuration { get; set; }

        public string AssignmentStatus { get; set; } = "Assigned";
        public string AssignedBy { get; set; } = string.Empty;

        // --- Audit Fields ---
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.Now;

    }
}
