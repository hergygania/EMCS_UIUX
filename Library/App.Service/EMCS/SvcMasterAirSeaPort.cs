using App.Data.Caching;
using System.Linq;

namespace App.Service.EMCS
{

    /// <summary>
    /// Services Proses Shipment inbound.</summary>                
    public class SvcMasterAirSeaPort
    {
        public const string CacheName = "App.EMCS.SvcMasterAirSeaPort";

        public readonly static ICacheManager CacheManager = new MemoryCacheManager();

        public static dynamic GetSelectOption(Data.Domain.EMCS.MasterAirSeaPort filter)
        {
            string type = filter.Type ?? "SeaPort";
            string search = filter.Name ?? "";
            string country = filter.Country ?? "";
            if (country != "")
            {
                search = country;
            }

            using (var db = new Data.EmcsContext())
            {
                var data = db.MasterAirSeaPorts
                    .Where(i => (i.Name.Contains(search) || i.Code.Contains(search) || i.Country.Contains(search)))
                    .OrderBy(i => i.Code)
                    .Skip(0).Take(100)
                    .Select(i => new
                    {
                        id = i.Id,
                        text = i.Name,
                        country = i.Country,
                        code = i.Code
                    }).ToList();

                return data.Select(i => new Data.Domain.EMCS.SelectItem2 { Id = i.code + " - " + i.text, Text = i.code + " - " + i.text + " - " + i.country }).ToList();
            }
        }

        public static dynamic GetLocalPortSelectOption(Data.Domain.EMCS.MasterAirSeaPort filter)
        {
            string type = filter.Type ?? "SeaPort";
            string search = filter.Name ?? "";
            using (var db = new Data.EmcsContext())
            {
                var data = db.MasterAirSeaPorts
                    .Where(i => (i.Name.Contains(search) || i.Code.Contains(search) || i.Country.Contains(search)) && i.Country == "Indonesia")
                    .OrderBy(i => i.Code)
                    .Skip(0).Take(100)
                    .Select(i => new
                    {
                        id = i.Id,
                        text = i.Name,
                        country = i.Country,
                        code = i.Code
                    }).ToList();

                return data.Select(i => new Data.Domain.EMCS.SelectItem2 { Id = i.code + " - " + i.text, Text = i.code + " - " + i.text + " - " + i.country }).ToList();
            }
        }
    }
}
