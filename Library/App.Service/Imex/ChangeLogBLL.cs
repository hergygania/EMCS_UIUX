using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using App.Data.Domain.ChangeLog;
using App.Data;

namespace App.Service.Imex
{
    public class ChangeLogBLL
    {
        public List<ChangeLogModel> GetChangeLogNewest()
        {
            List<ChangeLogModel> result = new List<ChangeLogModel>();

            DataTable dt = DbTransaction.DbToDataTable("mp.spGetChangeLogNewest", true);

            foreach (DataRow row in dt.Rows)
            {
                ChangeLogModel resultItem = new ChangeLogModel();
                resultItem.ID = row["ID"].ToString();
                resultItem.PartsNumber = row["PartsNumber"].ToString();
                resultItem.HSCodeOld = row["HSCodeOld"].ToString();
                resultItem.HSCodeNew = row["HSCodeNew"].ToString();
                resultItem.OMOld = row["OMOld"].ToString();
                resultItem.OMNew = row["OMNew"].ToString();
                //resultItem.BeaMasukOld = row["BeaMasukOld"].ToString();
                //resultItem.BeaMasukNew = row["BeaMasukNew"].ToString();
                //resultItem.PPNBMOld = row["PPNBMOld"].ToString();
                //resultItem.PPNBMNew = row["PPNBMNew"].ToString();
                //resultItem.PrefTarifOld = row["PrefTarifOld"].ToString();
                //resultItem.PrefTarifNew = row["PrefTarifNew"].ToString();
                resultItem.UpdatedBy = row["UpdatedBy"].ToString();
                resultItem.UpdatedDate = row["UpdatedDate"].ToString();
                result.Add(resultItem);
            }

            return result;
        }
        public List<ChangeLogModel> GetChangeLogDaily()
        {
            List<ChangeLogModel> result = new List<ChangeLogModel>();

            DataTable dt = DbTransaction.DbToDataTable("mp.spGetChangeLogDaily", true);

            foreach (DataRow row in dt.Rows)
            {
                ChangeLogModel resultItem = new ChangeLogModel();
                resultItem.ID = row["ID"].ToString();
                resultItem.PartsNumber = row["PartsNumber"].ToString();
                resultItem.HSCodeOld = row["HSCodeOld"].ToString();
                resultItem.HSCodeNew = row["HSCodeNew"].ToString();
                resultItem.OMOld = row["OMOld"].ToString();
                resultItem.OMNew = row["OMNew"].ToString();
                //resultItem.BeaMasukOld = row["BeaMasukOld"].ToString();
                //resultItem.BeaMasukNew = row["BeaMasukNew"].ToString();
                //resultItem.PPNBMOld = row["PPNBMOld"].ToString();
                //resultItem.PPNBMNew = row["PPNBMNew"].ToString();
                //resultItem.PrefTarifOld = row["PrefTarifOld"].ToString();
                //resultItem.PrefTarifNew = row["PrefTarifNew"].ToString();
                resultItem.UpdatedBy = row["UpdatedBy"].ToString();
                resultItem.UpdatedDate = row["UpdatedDate"].ToString();
                result.Add(resultItem);
            }

            return result;
        }
        public List<ChangeLogModel> GetChangeLogWeekly()
        {
            List<ChangeLogModel> result = new List<ChangeLogModel>();

            DataTable dt = DbTransaction.DbToDataTable("mp.spGetChangeLogWeekly", true);

            foreach (DataRow row in dt.Rows)
            {
                ChangeLogModel resultItem = new ChangeLogModel();
                resultItem.ID = row["ID"].ToString();
                resultItem.PartsNumber = row["PartsNumber"].ToString();
                resultItem.HSCodeOld = row["HSCodeOld"].ToString();
                resultItem.HSCodeNew = row["HSCodeNew"].ToString();
                resultItem.OMOld = row["OMOld"].ToString();
                resultItem.OMNew = row["OMNew"].ToString();
                //resultItem.BeaMasukOld = row["BeaMasukOld"].ToString();
                //resultItem.BeaMasukNew = row["BeaMasukNew"].ToString();
                //resultItem.PPNBMOld = row["PPNBMOld"].ToString();
                //resultItem.PPNBMNew = row["PPNBMNew"].ToString();
                //resultItem.PrefTarifOld = row["PrefTarifOld"].ToString();
                //resultItem.PrefTarifNew = row["PrefTarifNew"].ToString();
                resultItem.UpdatedBy = row["UpdatedBy"].ToString();
                resultItem.UpdatedDate = row["UpdatedDate"].ToString();
                result.Add(resultItem);
            }

            return result;
        }
        public List<ChangeLogModel> GetChangeLogMonthly()
        {
            List<ChangeLogModel> result = new List<ChangeLogModel>();

            DataTable dt = DbTransaction.DbToDataTable("mp.spGetChangeLogMonthly", true);

            foreach (DataRow row in dt.Rows)
            {
                ChangeLogModel resultItem = new ChangeLogModel();
                resultItem.ID = row["ID"].ToString();
                resultItem.PartsNumber = row["PartsNumber"].ToString();
                resultItem.HSCodeOld = row["HSCodeOld"].ToString();
                resultItem.HSCodeNew = row["HSCodeNew"].ToString();
                resultItem.OMOld = row["OMOld"].ToString();
                resultItem.OMNew = row["OMNew"].ToString();
                //resultItem.BeaMasukOld = row["BeaMasukOld"].ToString();
                //resultItem.BeaMasukNew = row["BeaMasukNew"].ToString();
                //resultItem.PPNBMOld = row["PPNBMOld"].ToString();
                //resultItem.PPNBMNew = row["PPNBMNew"].ToString();
                //resultItem.PrefTarifOld = row["PrefTarifOld"].ToString();
                //resultItem.PrefTarifNew = row["PrefTarifNew"].ToString();
                resultItem.UpdatedBy = row["UpdatedBy"].ToString();
                resultItem.UpdatedDate = row["UpdatedDate"].ToString();
                result.Add(resultItem);
            }

            return result;
        }
        public List<ChangeLogModel> GetChangeLogByDate(object filter)
        {
            List<ChangeLogModel> result = new List<ChangeLogModel>();

            DataSet ds = new DataSet();
            ds = DbTransaction.DbToDataSet("mp.spGetChangeLogByDate", filter);
            DataTable dt = ds.Tables[0];

            foreach (DataRow row in dt.Rows)
            {
                ChangeLogModel resultItem = new ChangeLogModel();
                resultItem.ID = row["ID"].ToString();
                resultItem.PartsNumber = row["PartsNumber"].ToString();
                resultItem.HSCodeOld = row["HSCodeOld"].ToString();
                resultItem.HSCodeNew = row["HSCodeNew"].ToString();
                resultItem.OMOld = row["OMOld"].ToString();
                resultItem.OMNew = row["OMNew"].ToString();
                //resultItem.BeaMasukOld = row["BeaMasukOld"].ToString();
                //resultItem.BeaMasukNew = row["BeaMasukNew"].ToString();
                //resultItem.PPNBMOld = row["PPNBMOld"].ToString();
                //resultItem.PPNBMNew = row["PPNBMNew"].ToString();
                //resultItem.PrefTarifOld = row["PrefTarifOld"].ToString();
                //resultItem.PrefTarifNew = row["PrefTarifNew"].ToString();
                resultItem.UpdatedBy = row["UpdatedBy"].ToString();
                resultItem.UpdatedDate = row["UpdatedDate"].ToString();
                result.Add(resultItem);
            }

            return result;
        }
        public DataTable DownloadChangeLog(string SPName)
        {
            DataTable dt = DbTransaction.DbToDataTable(SPName, true);
            return dt;
        }
        public DataTable DownloadChangeLogByDate(object filter)
        {
            DataSet ds = new DataSet();
            ds = DbTransaction.DbToDataSet("mp.spGetChangeLogByDate", filter);
            DataTable dt = ds.Tables[0];
            return dt;
        }
    }
}
