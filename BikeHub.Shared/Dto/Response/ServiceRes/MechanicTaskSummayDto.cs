namespace BikeHub.Shared.Dto.Response.ServiceRes
{
    public class MechanicTaskSummayDto
    {
        public int Pending { get; set; }

        public int InProgress { get; set; }

        public int DoneToday { get; set; }

        public int ThisMonth { get; set; }
    }
}
