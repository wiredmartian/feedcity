using System.Collections.Generic;
using feeddcity.Data;
using feeddcity.Models.DropOff;

namespace feeddcity.Interfaces
{
    public interface IDropOff
    {
        int CreateDropOff(DropOffZoneModel model);
        DropOffZone GetDropOffZone(int zoneId);
        List<DropOffZone> GetAllDropOffZones();
        List<DropOffZone> SearchDropOffZones(string searchTerm);
        List<DropOffZone> GetProvincialDropOffZones(int provinceId);
    }
}