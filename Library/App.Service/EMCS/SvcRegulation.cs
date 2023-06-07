using System.Collections.Generic;
using System.Linq;

namespace App.Service.EMCS
{

    /// <summary>
    /// Services Proses Shipment inbound.</summary>                
    public class SvcRegulation
    {
        public const string CacheName = "App.EMCS.Regulation";

        public static List<Data.Domain.EMCS.SpRegulations> GetRegulationList()
        {
            using (var db = new Data.EmcsContext())
            {
                db.Database.CommandTimeout = 600;
                var SQL = "EXEC sp_get_regulation_list";
                List<Data.Domain.EMCS.SpRegulations> data = db.Database.SqlQuery<Data.Domain.EMCS.SpRegulations>(SQL).ToList();
                List<Data.Domain.EMCS.SpRegulations> result = data
                    .Where(i => i.ParentId > 0)
                    .Select(i => new Data.Domain.EMCS.SpRegulations
                    {
                        Id = i.Id,
                        ParentId = i.ParentId,
                        Name = i.Instansi,
                        Instansi = i.Instansi,
                        Nomor = i.Nomor,
                        RegulationType = i.RegulationType,
                        Category = i.Category,
                        Reference = i.Reference,
                        Description = i.Description,
                        RegulationNo = i.RegulationNo,
                        TanggalPenetapan = i.TanggalPenetapan,
                        TanggalDiUndangkan = i.TanggalDiUndangkan,
                        TanggalBerlaku = i.TanggalBerlaku,
                        TanggalBerakhir = i.TanggalBerakhir,
                        Keterangan = i.Keterangan,
                        Files = i.Files,
                        CreateBy = i.CreateBy,
                        CreateDate = i.CreateDate,
                        UpdateBy = i.UpdateBy,
                        UpdateDate = i.UpdateDate,
                        IsDelete = i.IsDelete,
                        Children = GetRegulationSub(data, i.Id)
                    }).ToList();
                return result;
            }
        }

        public static List<Data.Domain.EMCS.SpRegulations> GetRegulationSub(List<Data.Domain.EMCS.SpRegulations> data, long parentId)
        {
            return data
                .Where(i => i.ParentId == parentId)
                .Select(i => new Data.Domain.EMCS.SpRegulations
                {
                    Id = i.Id,
                    ParentId = i.ParentId,
                    Name = i.Nomor == null ? i.Category : i.RegulationType,
                    Instansi = i.Instansi,
                    Nomor = i.Nomor,
                    RegulationType = i.RegulationType,
                    Category = i.Category,
                    Reference = i.Reference,
                    Description = i.Description,
                    RegulationNo = i.RegulationNo,
                    TanggalPenetapan = i.TanggalPenetapan,
                    TanggalDiUndangkan = i.TanggalDiUndangkan,
                    TanggalBerlaku = i.TanggalBerlaku,
                    TanggalBerakhir = i.TanggalBerakhir,
                    Keterangan = i.Keterangan,
                    Files = i.Files,
                    CreateBy = i.CreateBy,
                    CreateDate = i.CreateDate,
                    UpdateBy = i.UpdateBy,
                    UpdateDate = i.UpdateDate,
                    IsDelete = i.IsDelete,
                    //state = "closed",
                    Children = GetRegulationSub(data, i.Id).Count > 0 ? GetRegulationSub(data, i.Id) : null
                }).ToList();
        }

        public static Data.Domain.EMCS.MasterRegulation GetById(long id)
        {
            using (var db = new Data.EmcsContext())
            {
                db.Database.CommandTimeout = 600;
                var data = db.MasterRegulation.Where(a => a.Id == id).FirstOrDefault();
                return data;
            }
        }
    }
}
