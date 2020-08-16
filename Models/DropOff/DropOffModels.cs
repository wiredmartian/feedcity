using System.ComponentModel.DataAnnotations;

namespace feeddcity.Models.DropOff
{
    public class DropOffZoneModel
    {
        [Required(AllowEmptyStrings = false)]
        public string PhysicalAddress { get; set; }
        
        [Required]
        public decimal Latitude { get; set; }
        
        [Required]
        public decimal Longitude { get; set; }
        
        [Required(AllowEmptyStrings = false)]
        public string Province { get; set; }
        
        [Required(AllowEmptyStrings = false)]
        public string City { get; set; }
        
        [Required(AllowEmptyStrings = false)]
        public string StreetName { get; set; }
        
        [Required(AllowEmptyStrings = false)]
        public string ContactName { get; set; }
        
        [Required(AllowEmptyStrings = false)]
        [DataType(DataType.PhoneNumber)]
        public string ContactNumber { get; set; }
        
        [Required(AllowEmptyStrings = false)]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }

    }
}