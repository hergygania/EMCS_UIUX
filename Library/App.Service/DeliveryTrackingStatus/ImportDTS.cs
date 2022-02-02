using App.Data;
using App.Data.Caching;
using App.Data.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Service.DeliveryTrackingStatus
{
    public class ImportDTS
    {

        private const string cacheName = "App.master.ImportDeliveryTrackingStatus";

        private readonly static ICacheManager _cacheManager = new MemoryCacheManager();

        public static string CheckTemplateTable(DataTable sourceTable, string Modul)
        {
            StringBuilder sb = new StringBuilder();
            List<string> mandatoryColumns = new List<string>();

            if (Modul == "Shipment")
                mandatoryColumns = GetMandatoryColumnsTamplateShipment();

            DataColumnCollection columns = sourceTable.Columns;

            foreach (string s in mandatoryColumns)
            {
                if (!columns.Contains(s))
                {
                    sb.AppendLine(string.Format("There is no column {0} in sheet " + Modul + " your document template. || ", s));
                }
            }
            return sb.ToString();
        }

        static List<string> GetMandatoryColumnsTamplateShipment()
        {
            List<string> columns = new List<string>();
            columns.Add("Moda");
            columns.Add("Unit_Moda");
            columns.Add("From");
            columns.Add("To");
            columns.Add("NODA");
            columns.Add("NODI");
            columns.Add("Unit_Type");
            columns.Add("Model");
            columns.Add("SN");
            columns.Add("ETD");
            columns.Add("ATD");
            columns.Add("ETA");
            columns.Add("ATA");
            columns.Add("Status");
            columns.Add("Cost");
            columns.Add("Ship_Doc");
            columns.Add("Ship_Cost");
            columns.Add("Entry_Sheet");
            columns.Add("No_PI");
            columns.Add("Reject");
            columns.Add("Remarks");

            return columns;
        }

        public static Data.Domain.DeliveryTrackingStatus GetExistDB(Data.Domain.DeliveryTrackingStatus itemList)
        {
            var item = GetDTS()
                .Where(w => w.NODI.Trim().ToLower() == itemList.NODI.Trim().ToLower())
                .Where(w => w.NODA.Trim().ToLower() == itemList.NODA.Trim().ToLower())
                .Where(w => w.SN.Trim().ToLower() == itemList.SN.Trim().ToLower())
                .Where(w => w.Model.Trim().ToLower() == itemList.Model.Trim().ToLower())
                .Where(w => w.Status == itemList.Status).FirstOrDefault();
            return item;
        }

        public static List<Data.Domain.DeliveryTrackingStatus> GetDTS()
        {
            string key = string.Format(cacheName);

            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                var tbl = db.CreateRepository<Data.Domain.DeliveryTrackingStatus>().Table.Select(e => e);
                return tbl.ToList();
            }
        }

        public static Data.Domain.DeliveryTrackingStatus GetDTSByID(int ID)
        {
            var item = GetDTS().Where(p => p.ID == ID).FirstOrDefault();
            return item;
        }

        public static List<Data.Domain.DeliveryTrackingStatus> GetDTSByNODI(string NODI)
        {
            var item = GetDTS().Where(p => p.NODI == NODI).ToList();
            return item;
        }

        public static int crud(Data.Domain.DeliveryTrackingStatus item, string dml)
        {
            if (dml == "I")
            {
                item.EntryDate = DateTime.Now;
                item.EntryBy = Domain.SiteConfiguration.UserName;
            }

            _cacheManager.Remove(cacheName);

            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                return db.CreateRepository<Data.Domain.DeliveryTrackingStatus>().CRUD(dml, item);
            }
        }

    }
}
