using System.Collections.Generic;
using System.Linq;
using App.Data;
using App.Data.Domain;
using System.Data.SqlClient;
using System;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using Point = DotNet.Highcharts.Options.Point;
using DotNet.Highcharts;
using System.Drawing;

namespace App.Service.Report
{
    public class DeliveryTrackingStatus
    {
        public static List<RptDeliveryTrackingStatus> GetList(int Moda, string From, string To, int Status, int UnitType, string ETD, string ATD, string ETA, string ATA, string NODA)
        {
            using (var db = new EfDbContext())
            {
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@Moda", Moda));
                parameterList.Add(new SqlParameter("@From", From.Trim()));
                parameterList.Add(new SqlParameter("@To", To.Trim()));
                parameterList.Add(new SqlParameter("@Status", Status));
                parameterList.Add(new SqlParameter("@UnitType", UnitType));
                parameterList.Add(new SqlParameter("@ETD", ETD.Trim()));
                parameterList.Add(new SqlParameter("@ATD", ATD.Trim()));
                parameterList.Add(new SqlParameter("@ETA", ETA.Trim()));
                parameterList.Add(new SqlParameter("@ATA", ATA.Trim()));
                parameterList.Add(new SqlParameter("@NODA", NODA.Trim()));

                SqlParameter[] parameters = parameterList.ToArray();

                string query = "dbo.spGetDeliveryTrackingStatus @Moda, @From, @To, @Status, @UnitType, @ETD, @ATD, @ETA, @ATA, @NODA";

                var data = db.Database.SqlQuery<Data.Domain.RptDeliveryTrackingStatus>(query, parameters).AsQueryable();
                parameterList.Clear();

                return data.ToList();
            }
        }
    }
}
