using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace App.Service.FreightCost
{
    public class MasterModaOfConditionTruckingRateImport
    {
        public List<Data.Domain.MasterTruckingRate> GetDataTruck(ISheet sheet, string _fileName, string _Modul)
        {
            List<Data.Domain.MasterTruckingRate> _dataTruck = new List<Data.Domain.MasterTruckingRate>();
           
            var _logImport = new Data.Domain.LogImport();
            try
            {
                if (sheet.GetRow(1) != null)
                {
                    for (int dr = 1; dr <= sheet.LastRowNum; dr++)
                    {
                        var _itemModa = new Data.Domain.ModaOfCondition();
                        var _itemTruck = new Data.Domain.MasterTruckingRate();

                        if (sheet.GetRow(dr).GetCell(0).StringCellValue == string.Empty)
                            break;
                        else
                        {
                            _itemModa.Moda = sheet.GetRow(dr).GetCell(4).StringCellValue.Trim();
                            _itemModa.Description = sheet.GetRow(dr).GetCell(6).StringCellValue.Trim();
                            var CodeExistModa = Service.Master.ModaOfCondition.GetModa(
                                sheet.GetRow(dr).GetCell(4).StringCellValue.Trim().ToLower());

                            if (CodeExistModa == null)
                                Service.Master.ModaOfCondition.crud(_itemModa, "I");

                            var ModaID = Service.Master.ModaOfCondition.GetModa(sheet.GetRow(dr).GetCell(4).StringCellValue.Trim());
                            if (ModaID == null)
                                throw new Exception("Moda of Condition " + sheet.GetRow(dr).GetCell(4).StringCellValue.Trim() + " not found");

                            _itemTruck.Origin_Code = sheet.GetRow(dr).GetCell(0).StringCellValue.Trim();
                            _itemTruck.Destination_Code = sheet.GetRow(dr).GetCell(2).StringCellValue.Trim();
                            _itemTruck.ConditionModa_ID = Convert.ToInt32(ModaID.ID);
                            _itemTruck.Rate_Per_Trip_IDR = Convert.ToDecimal(ImportFreight.getValue(sheet, dr, 5));
                            _itemTruck.ValidonMounth = Convert.ToInt32(ImportFreight.getValue(sheet, dr, 7));
                            _itemTruck.ValidonYears = Convert.ToInt32(ImportFreight.getValue(sheet, dr, 8));
                            _itemTruck.Remarks = sheet.GetRow(dr).GetCell(6).StringCellValue.Trim();
                            

                            var item = _dataTruck.Select(s => s).Where(w => w.Origin_Code.Trim().ToLower() == _itemTruck.Origin_Code.Trim().ToLower())
                                .Where(w => w.Destination_Code.Trim().ToLower() == _itemTruck.Destination_Code.Trim().ToLower())
                                .Where(w => w.ConditionModa_ID == _itemTruck.ConditionModa_ID)
                                .Where(w => w.Rate_Per_Trip_IDR == _itemTruck.Rate_Per_Trip_IDR)
                                .Where(w => w.ValidonMounth == _itemTruck.ValidonMounth)
                                .Where(w => w.ValidonYears == _itemTruck.ValidonYears).FirstOrDefault();

                            if (item == null)
                                _dataTruck.Add(_itemTruck);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _dataTruck = new List<Data.Domain.MasterTruckingRate>();
                throw new Exception("Detail: Error Message read sheet Trucking Rate : " + ex.Message);
            }
            return _dataTruck;
        }

        public void SaveDataTrucking(List<Data.Domain.MasterTruckingRate> listTruck)
        {
            if (listTruck.Count > 0)
            {

                using (var db = new Data.EfDbContext())
                {
                    var trans = db.Database.BeginTransaction();
                    try
                    {
                        ImportFreight.TruncateTbale("Master_TruckingRate");
                        foreach (var g in listTruck)
                        {

                            Data.Domain.MasterTruckingRate DataTrucking = new Data.Domain.MasterTruckingRate();
                            Data.Domain.MasterTruckingRateLog DataTruckingLog = new Data.Domain.MasterTruckingRateLog();

                            //set Master
                            DataTrucking.Origin_Code = g.Origin_Code;
                            DataTrucking.Destination_Code = g.Destination_Code;
                            DataTrucking.ConditionModa_ID = g.ConditionModa_ID;
                            DataTrucking.Rate_Per_Trip_IDR = g.Rate_Per_Trip_IDR;
                            DataTrucking.ValidonMounth = g.ValidonMounth;
                            DataTrucking.ValidonYears = g.ValidonYears;
                            DataTrucking.EntryBy = Domain.SiteConfiguration.UserName;
                            DataTrucking.ModifiedBy = Domain.SiteConfiguration.UserName;
                            DataTrucking.EntryDate = DateTime.Now;
                            DataTrucking.ModifiedDate = DateTime.Now;
                            DataTrucking.Remarks = g.Remarks;

                            //set Log
                            DataTruckingLog.Origin_Code = g.Origin_Code;
                            DataTruckingLog.Destination_Code = g.Destination_Code;
                            DataTruckingLog.ConditionModa_ID = g.ConditionModa_ID;
                            DataTruckingLog.Rate_Per_Trip_IDR = g.Rate_Per_Trip_IDR;
                            DataTruckingLog.ValidonMounth = g.ValidonMounth;
                            DataTruckingLog.ValidonYears = g.ValidonYears;
                            DataTruckingLog.EntryBy = Domain.SiteConfiguration.UserName;
                            DataTruckingLog.ModifiedBy = Domain.SiteConfiguration.UserName;
                            DataTruckingLog.EntryDate = DateTime.Now;
                            DataTruckingLog.ModifiedDate = DateTime.Now;
                            DataTruckingLog.Remarks = g.Remarks;

                            //Save DB
                            db.MasterTruckingRate.Add(DataTrucking);
                            db.MasterTruckingRateLog.Add(DataTruckingLog);
                            db.SaveChanges();

                        }
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        throw new Exception(ex.Message);
                    }                    
                }
            }
        }

    }
}
