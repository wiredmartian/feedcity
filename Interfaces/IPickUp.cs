using System.Collections.Generic;
using feeddcity.Data;
using feeddcity.Models.PickUp;

namespace feeddcity.Interfaces
{
    public interface IPickUp
    {
        int RequestPickUp(PickUpRequestModel model);
        int UpdatePickUpStatus(PickUpStatus status, int pickUpId);
        List<PickUpRequest> GetPickUpRequests(PickUpStatus status);
    }
}