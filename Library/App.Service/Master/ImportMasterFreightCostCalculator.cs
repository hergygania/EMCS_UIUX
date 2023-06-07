using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Service.Master
{
    public class ImportMasterFreightCostCalculator
    {
        public static string CheckTemplateExcelOLD(DataTable sourceTable, string Modul)
        {
            StringBuilder sb = new StringBuilder();
            List<string> mandatoryColumns = new List<string>();

            if (Modul == "Rate")
                mandatoryColumns = GetMandatoryColumnsTemplateRate();
            if(Modul == "Inbound Rate")
                mandatoryColumns = GetMandatoryColumnsTemplateInboundRate();
            if(Modul == "Surcharge")
                mandatoryColumns = GetMandatoryColumnsTemplateSurcharge();
            if (Modul == "Trucking Rate")
                mandatoryColumns = GetMandatoryColumnsTemplateTrucking();

            DataColumnCollection columns = sourceTable.Columns;
            
            foreach (string s in mandatoryColumns)
            {
                if (!columns.Contains(s))
                {                  
                    sb.AppendLine(string.Format("There is no column {0} in sheet " + Modul + " your document template.", s));

                }
            }

            return sb.ToString();
        }

        //public static string CheckTemplateExcelOLD(DataTable sourceTable, string Modul)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    List<string> mandatoryColumns = new List<string>();

        //    sb.AppendLine(string.Format("There is no column {0} in sheet " + Modul + " your document template.", s));
        //}

        static List<string> GetMandatoryColumnsTemplateRate()
        {
            List<string> columns = new List<string>();
            columns.Add("ORIGIN CITY CODE");
            columns.Add("DESTINATION CITY CODE");
            columns.Add("SERVICE");
            columns.Add("0-1000");
            columns.Add("1001-3999");
            columns.Add("4000-99999");
            columns.Add("CURRENCY");
            columns.Add("MIN RATE");
            columns.Add("LEAD TIME");
            columns.Add("VIA");
            columns.Add("RAGULATED AGENT");
            columns.Add("COST");
            columns.Add("IR");
            columns.Add("REMARKS");
            columns.Add("Valid on Mounth");
            columns.Add("Valid on Years");

            return columns;
        }

        static List<string> GetMandatoryColumnsTemplateInboundRate()
        {
            List<string> columns = new List<string>();
            columns.Add("SERVICE");
            columns.Add("ORIGIN CITY CODE");
            columns.Add("DESTINATION CITY CODE");            
            columns.Add("CURRENCY");
            columns.Add("LEAD TIME");
            columns.Add("PORT HUB");
            columns.Add("SOURCE - SIN RATE");
            columns.Add("HANDLING AT SIN RATE");
            columns.Add("SIN-ID PORT RATE");
            columns.Add("CUSTOM CLEARANCE RATE");
            columns.Add("DEBIT NOTE RATE");
            columns.Add("RATE INBOUND");
            columns.Add("Valid on Mounth");
            columns.Add("Valid on Years");
            columns.Add("REMARKS");
            
            return columns;
        }

        static List<string> GetMandatoryColumnsTemplateSurcharge()
        {
            List<string> columns = new List<string>();
            columns.Add("ORIGIN CITY CODE");
            columns.Add("DESTINATION CITY CODE");
            columns.Add("MODA");
            columns.Add("SERVICE");
            columns.Add("SURCHARGE");
            columns.Add("SURCHARGE 50%");
            columns.Add("SURCHARGE 100%");
            columns.Add("SURCHARGE 200%");
            columns.Add("Valid on Mounth");
            columns.Add("Valid on Years");
            columns.Add("REMARKS");

            return columns;
        }

        static List<string> GetMandatoryColumnsTemplateTrucking()
        {
            List<string> columns = new List<string>();
            columns.Add("ORIGIN CITY CODE");
            columns.Add("ORIGIN DETAIL");
            columns.Add("DESTINATION CITY CODE");
            columns.Add("DESTINATION DETAIL");
            columns.Add("CONDITION OF MODA");
            columns.Add("RATE PER TRIP (IDR)");
            columns.Add("REMARKS");
            columns.Add("VALID ON MOUNTH");
            columns.Add("VALID ON YEARS");

            return columns;
        }

        public static void TruncateTbale(string _tableName)
        {
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                db.DbContext.Database.ExecuteSqlCommand("truncate table [dbo].[" + _tableName + "]");
            }
        }

    }
}
