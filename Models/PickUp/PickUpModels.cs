using System.ComponentModel.DataAnnotations;

namespace feeddcity.Models.PickUp
{
    public class PickUpRequestModel
    {
        [Required(AllowEmptyStrings = false)]
        [MaxLength(255)]
        public string Location { get; set; }
        
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please provide a drop of zone for this package")]
        public string DropOffZoneId { get; set; }
        
        [Required(AllowEmptyStrings = false)]
        [MaxLength(50)]
        public string Latitude { get; set; }
        
        [Required(AllowEmptyStrings = false)]
        [MaxLength(50)]
        public string Longitude { get; set; }
        
        [Required(AllowEmptyStrings = false)]
        [MaxLength(255)]
        public string ContactName { get; set; }
        
        [DataType(DataType.PhoneNumber)]
        [Required(AllowEmptyStrings = false)]
        public string ContactNumber { get; set; }
        
        [DataType(DataType.MultilineText)]
        [Required(AllowEmptyStrings = false)]
        public string Notes { get; set; }
    }
}