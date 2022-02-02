using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Service.Master
{
    public class ImportDeliveryTrackingStatus
    {

        private const string cacheName = "App.master.DeliveryTackingStatus";

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
            columns.Add("MODA");
            columns.Add("Unit Moda");
            columns.Add("FROM");
            columns.Add("TO");
            columns.Add("Delivery Advice");
            columns.Add("Delivery Instruction");
            columns.Add("Unit Type");
            columns.Add("MODEL");
            columns.Add("Serial Number");
            columns.Add("Estimate Time Departure");
            columns.Add("Actual Time Departure");
            columns.Add("Estimate Time Arrival");
            columns.Add("Actual Time Arrival");
            columns.Add("STATUS");
            columns.Add("COST");
            columns.Add("Currency");
            columns.Add("SHIPMENT DOC");
            columns.Add("SHIPMENT COST");
            columns.Add("ENTRY SHEET");
            columns.Add("Number PI");
            columns.Add("REJECT REASON");
            columns.Add("REMARKS");

            return columns;
        }

        //public static Data.Domain.PrimeProduct GetExistDB(Data.Domain.PrimeProduct itemList)
        //{ 
            
        //}

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
