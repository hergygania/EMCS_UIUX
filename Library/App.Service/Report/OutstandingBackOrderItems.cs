#region License

// /****************************** Module Header ******************************\
// Module Name:  OutstandingBackorderItems.cs
// Project:    Pis-Service
// Copyright (c) Microsoft Corporation.
// 
// <Description of the file>
// This source is subject to the Microsoft Public License.
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
// All other rights reserved.
// 
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
// WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// \***************************************************************************/

#endregion

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using App.Data;
using App.Data.Domain;

namespace App.Service.Report
{
    public class OutstandingBackorderItems
    {

        public static List<RptOutstandingBackorderItem> GetList
            (string groupType, string filterBy, string storeNo, string[] rackCustID,string userId)
        {
            using (var db = new EfDbContext())
            {
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@UserId", userId));
                SqlParameter[] parameters = parameterList.ToArray();

                var tbl = db.Database.SqlQuery<Data.Domain.RptOutstandingBackorderItem>("dbo.spGetReportOutstandingBackorderItemUserId @UserId", parameters).AsQueryable();


             //   tbl = tbl.Where(w => w.obkitm_CreatedOn.Date <= DateTime.Now.AddDays(-1).Date);
                if (!string.IsNullOrEmpty(storeNo))
                    tbl = tbl.Where(w => w.obkitm_Store.Trim() == storeNo.Trim());
                else
                {
                    if (!string.IsNullOrEmpty(filterBy))
                    {
                        var filter = int.Parse(filterBy);
                        tbl = groupType.ToUpper() == "AREA" ? tbl.Where(w => w.obkitm_AreaID == filter) : tbl.Where(w => w.obkitm_HubID == filter);
                    }
                }
                if (rackCustID != null)
                    tbl = tbl.Where(w => rackCustID.Contains(w.obkitm_CustNo));
                return tbl.ToList();

            }
        }
        public static List<RptOutstandingBackorderItem> GetListCustomers()
        {
            using (var db = new ReportDbContext())
            {
                var tbl = db.RptOutstandingBackorderItems.GroupBy(d => d.obkitm_CustNo).Select(d => d.FirstOrDefault());
                return tbl.ToList();
            }
        }
    }
}