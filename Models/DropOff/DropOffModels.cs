using System.ComponentModel.DataAnnotations;

namespace feeddcity.Models.DropOff
{
    public class DropOffZoneModel
    {
        [Required(AllowEmptyStrings = false)]
        [MaxLength(255)]
        public string PhysicalAddress { get; set; }
        
        [Required(AllowEmptyStrings = false)]
        [MaxLength(50)]
        public string Latitude { get; set; }
        
        [Required(AllowEmptyStrings = false)]
        [MaxLength(50)]
        public string Longitude { get; set; }
        
        [Required(AllowEmptyStrings = false)]
        [MaxLength(100)]
        public string Province { get; set; }
        
        [Required(AllowEmptyStrings = false)]
        [MaxLength(100)]
        public string City { get; set; }
        
        [Required(AllowEmptyStrings = false)]
        [MaxLength(100)]
        public string StreetName { get; set; }
        
        [Required(AllowEmptyStrings = false)]
        [MaxLength(255)]
        public string ContactName { get; set; }
        
        [DataType(DataType.PhoneNumber)]
        [Required(AllowEmptyStrings = false)]
        public string ContactNumber { get; set; }
        
        [DataType(DataType.EmailAddress)]
        [Required(AllowEmptyStrings = false)]
        [MaxLength(255)]
        public string EmailAddress { get; set; }
    }
}