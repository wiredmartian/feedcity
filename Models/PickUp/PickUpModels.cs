using System.ComponentModel.DataAnnotations;

namespace feeddcity.Models.PickUp
{
    public class PickUpRequestModel
    {
        [Required(AllowEmptyStrings = false)]
        public string Location { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string Latitude { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string Longitude { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string ContactName { get; set; }
        [DataType(DataType.PhoneNumber)]
        [Required(AllowEmptyStrings = false)]
        public string ContactNumber { get; set; }
        [DataType(DataType.MultilineText)]
        [Required(AllowEmptyStrings = false)]
        public string Notes { get; set; }
    }
}