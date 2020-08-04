using feeddcity.Models.PickUp;

namespace feeddcity.Interfaces
{
    public interface IPickUp
    {
        int RequestPickUp(PickUpRequestModel model);
    }
}