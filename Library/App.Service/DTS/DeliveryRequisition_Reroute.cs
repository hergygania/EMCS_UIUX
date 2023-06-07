using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using App.Data.Domain;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
namespace App.Service.DTS
{
    public class DeliveryRequisition_Reroute
    {
        public static int crud(Data.Domain.DeliveryRequisition_Reroute item)
        {
         
            item.UpdateBy = Domain.SiteConfiguration.UserName;
            item.UpdateDate = DateTime.Now;

           
            try
            {
                using (var db = new Data.RepositoryFactory(new Data.DTSContext()))
                {
                        return db.CreateRepository<Data.Domain.DeliveryRequisition_Reroute>().CRUD("I",item);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static Data.Domain.DeliveryRequisition_Reroute GetId(Int64 ID)
        {
            var db = new Data.DTSContext();
            var tb = db.DeliveryRequisition_Reroute;
            var item = tb.Where(i => i.ID == ID).FirstOrDefault();
            return item;
        }

        public static Data.Domain.DeliveryRequisition_Reroute RerouteForm (App.Data.Domain.DeliveryRequisition header)
        {
            Data.Domain.DeliveryRequisition_Reroute routeform = new Data.Domain.DeliveryRequisition_Reroute();

            routeform.ID = header.ID;
            routeform.KeyCustom = header.KeyCustom;
            routeform.Model = header.Model;
            routeform.Origin = header.Origin;
            routeform.SerialNumber = header.SerialNumber;
            routeform.Batch = header.Batch;
            routeform.ReqID = header.ReqID;
            routeform.ReqName = header.ReqName;
            routeform.ReqHp = header.ReqHp;
            routeform.CustID = header.CustID;
            routeform.CustName = header.CustName;
            routeform.CustAddress = header.CustAddress;
            routeform.Kecamatan = header.Kecamatan;
            routeform.Kabupaten = header.Kabupaten;
            routeform.Province = header.Province;
            routeform.PicName = header.PicName;
            routeform.PicHP = header.PicHP;
            routeform.TermOfDelivery = header.TermOfDelivery;
            routeform.SupportingOfDelivery = header.SupportingOfDelivery;
            routeform.Incoterm = header.Incoterm;
            routeform.ExpectedTimeLoading = header.ExpectedTimeLoading;
            routeform.ExpectedTimeArrival = header.ExpectedTimeArrival;
            routeform.PenaltyLateness = header.PenaltyLateness;
            routeform.RejectNote = header.RejectNote;
            routeform.SoNo = header.SoNo;
            routeform.DoNo = header.DoNo;
            routeform.OdDate = header.OdDate;
            routeform.Status = header.Status;
            routeform.Referrence = header.Referrence;
            routeform.Unit = header.Unit;
            routeform.DINo = header.DINo;
            routeform.Transportation = header.Transportation;
            routeform.RefNoType = header.RefNoType;
            routeform.RefNo = header.RefNo;
            routeform.SoDate = header.SoDate;
            routeform.STONo = header.STONo;
            routeform.STODate = header.STODate;
            routeform.DIDate = header.DIDate;
            routeform.STRNo = header.STRNo;
            routeform.ActualTimeDeparture = header.ActualTimeDeparture;
            routeform.ActualTimeArrival = header.ActualTimeArrival;
            routeform.Sales1Name = header.Sales1Name;
            routeform.Sales1Hp = header.Sales1Hp;
            routeform.Sales2Name = header.Sales2Name;
            routeform.Sales2Hp = header.Sales2Hp;
            routeform.STRDate = header.STRDate;
            routeform.ModaTransport = header.ModaTransport;
            routeform.UnitDimWeight = header.UnitDimWeight;
            routeform.UnitDimWidth = header.UnitDimWidth;
            routeform.UnitDimLength = header.UnitDimLength;
            routeform.UnitDimHeight = header.UnitDimWeight;
            routeform.UnitDimVol = header.UnitDimVol;
            routeform.Sales1ID = header.Sales1ID;
            routeform.Sales2ID = header.Sales2ID;
            routeform.SupportingDocument = header.SupportingDocument;
            routeform.SupportingDocument1 = header.SupportingDocument1;
            routeform.SupportingDocument2 = header.SupportingDocument2;
            routeform.SupportingDocument3 = header.SupportingDocument3;
            routeform.SupportingDocument4 = header.SupportingDocument4;
            routeform.SupportingDocument5 = header.SupportingDocument5;
            routeform.SendEmailToCkb = header.SendEmailToCkb;
            routeform.ReRouted = header.ReRouted;
            routeform.CreateBy = header.CreateBy;
            routeform.CreateDate = header.CreateDate;
            routeform.UpdateBy = header.UpdateBy;
            routeform.UpdateDate = header.UpdateDate;

            return routeform;
        }
     

        public static List<Data.Domain.DeliveryRequisition_Reroute> GetDRRerouteHistory( string keyNumber)
        {
            
            try
            {

                using (var db = new Data.RepositoryFactory(new Data.DTSContext()))
                {
                    db.DbContext.Database.CommandTimeout = 600;
                    List<SqlParameter> parameterList = new List<SqlParameter>();
                    if (keyNumber != null)
                    {                        
                        keyNumber = Regex.Replace(keyNumber, @"\s+\$|\s+(?=\w+$)", "");
                    }


                   
                    parameterList.Add(new SqlParameter("@keyNumber", keyNumber == null ? "" : keyNumber));

                    SqlParameter[] parameters = parameterList.ToArray();
                    var data = db.DbContext.Database.SqlQuery<Data.Domain.DeliveryRequisition_Reroute>
                        (@"exec [dbo].[SP_GetDRReroute] @keyNumber", parameters)
                        .ToList();
                    return data;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
