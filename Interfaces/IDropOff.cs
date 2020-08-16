using feeddcity.Data;
using feeddcity.Models.DropOff;

namespace feeddcity.Interfaces
{
    public interface IDropOff
    {
        int CreateDropOff(DropOffZoneModel model);
        DropOffZone GetDropOffZone(int zoneId);
    }
}