using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Data.Domain.KPIDashboard;
using App.Data;

namespace App.Service.Imex
{
    public class KPIDashboardBLL
    {
        public DataTable GetSCISvsCCOS()
        {
            List<SCISvsCCOSModel> result = new List<SCISvsCCOSModel>();
            DataTable dt = DbTransaction.DbToDataTable("mp.spGetSCISvsCCOS", true);
            return dt;
        }
        public DataTable GetChangeLogChart()
        {
            DataTable dt = DbTransaction.DbToDataTable("mp.spGetChangeLogChart", true);
            return dt;
        }
        public DataTable GetChangeLogChartNewMapping()
        {
            DataTable dt = DbTransaction.DbToDataTable("mp.spGetChangeLogChartNewMapping", true);
            return dt;
        }
    }
}
