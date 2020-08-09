using System;

namespace feeddcity.Data
{
    public class PickUpRequest
    {
        public int Id { get; set; }
        public string Location { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string ContactName { get; set; }
        public string ContactNumber { get; set; }
        public int UserId { get; set; }
        public int AcceptedBy { get; set; }
        public string Notes { get; set; }
        public PickUpStatus Status { get; set; }
        public DateTime RequestedOn { get; set; }
        public DateTime? ClosedOn { get; set; }
    }

    public enum PickUpStatus
    {
        Pending,
        Accepted,
        Rejected,
        Completed,
        Cancelled
    }
}