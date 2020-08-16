namespace feeddcity.Data
{
    public class DropOffZone
    {
        public int Id { get; set; }
        public string PhysicalAddress { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string ProvinceId { get; set; }
        public string City { get; set; }
        public bool Active { get; set; }
        public string StreetName { get; set; }
        public string ContactName { get; set; }
        public string ContactNumber { get; set; }
        public string EmailAddress { get; set; }
    }
}