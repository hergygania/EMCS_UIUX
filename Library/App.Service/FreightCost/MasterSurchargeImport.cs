using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace App.Service.FreightCost
{
    public class MasterSurchargeImport
    {
        public List<Data.Domain.MasterSurcharge> GetSurcharge(ISheet sheet, string _fileName, string _Modul)
        {
            List<Data.Domain.MasterSurcharge> _dataSurcharge = new List<Data.Domain.MasterSurcharge>();

            
            var _logImport = new Data.Domain.LogImport();
            try
            {
                if (sheet.GetRow(1) != null)
                {
                    for (int drRate = 1; drRate <= sheet.LastRowNum; drRate++)
                    {
                        var _itemSurcharge = new Data.Domain.MasterSurcharge();
                        if (sheet.GetRow(drRate).GetCell(0).StringCellValue == string.Empty)
                            break;
                        else
                        {
                            _itemSurcharge.Service_Code = sheet.GetRow(drRate).GetCell(0).StringCellValue.Trim();
                            _itemSurcharge.Origin_Code = sheet.GetRow(drRate).GetCell(1).StringCellValue.Trim();
                            _itemSurcharge.Destination_Code = sheet.GetRow(drRate).GetCell(3).StringCellValue.Trim();
                            var ModaID = Service.Master.MasterGeneric.GetIDByCodeName("Moda", sheet.GetRow(drRate).GetCell(5).StringCellValue.Trim());
                            if (ModaID == null)
                                throw new Exception("Moda " + sheet.GetRow(drRate).GetCell(5).StringCellValue.Trim());

                            _itemSurcharge.Moda_Factor_ID = Convert.ToInt32(ModaID.ID);
                            if (sheet.GetRow(drRate).GetCell(6).CellType == CellType.Numeric)
                                _itemSurcharge.Surcharge = ImportFreight.getValue(sheet, drRate, 6).ToString();
                            else
                                _itemSurcharge.Surcharge = sheet.GetRow(drRate).GetCell(6).StringCellValue.Trim();

                            if (sheet.GetRow(drRate).GetCell(7).CellType == CellType.Numeric)
                                _itemSurcharge.Surcharge_50 = ImportFreight.getValue(sheet, drRate, 7).ToString();
                            else
                                _itemSurcharge.Surcharge_50 = sheet.GetRow(drRate).GetCell(7).StringCellValue.Trim();

                            if (sheet.GetRow(drRate).GetCell(8).CellType == CellType.Numeric)
                                _itemSurcharge.Surcharge_100 = ImportFreight.getValue(sheet, drRate, 8).ToString();
                            else
                                _itemSurcharge.Surcharge_100 = sheet.GetRow(drRate).GetCell(8).StringCellValue.Trim();

                            if (sheet.GetRow(drRate).GetCell(9).CellType == CellType.Numeric)
                                _itemSurcharge.Surcharge_200 = ImportFreight.getValue(sheet, drRate, 9).ToString();
                            else
                                _itemSurcharge.Surcharge_200 = sheet.GetRow(drRate).GetCell(9).StringCellValue.Trim();

                            _itemSurcharge.ValidonMounth = Convert.ToInt32(ImportFreight.getValue(sheet, drRate, 10));
                            _itemSurcharge.ValidonYears = Convert.ToInt32(ImportFreight.getValue(sheet, drRate, 11));
                            _itemSurcharge.Remarks = sheet.GetRow(drRate).GetCell(12).StringCellValue.Trim();

                            var exist = _dataSurcharge.Select(s => s).Where(w => w.Origin_Code.Trim().ToLower() == _itemSurcharge.Origin_Code.Trim().ToLower())
                            .Where(w => w.Destination_Code.Trim().ToLower() == _itemSurcharge.Destination_Code.Trim().ToLower())
                            .Where(w => w.Service_Code.Trim().ToLower() == _itemSurcharge.Service_Code.Trim().ToLower())
                            .Where(w => w.Moda_Factor_ID == _itemSurcharge.Moda_Factor_ID)
                            .Where(w => w.Surcharge.Trim().ToLower() == _itemSurcharge.Surcharge.Trim().ToLower())
                            .Where(w => w.ValidonMounth == _itemSurcharge.ValidonMounth)
                            .Where(w => w.ValidonYears == _itemSurcharge.ValidonYears).FirstOrDefault();

                            if (exist == null)
                                _dataSurcharge.Add(_itemSurcharge);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _dataSurcharge = new List<Data.Domain.MasterSurcharge>();
                throw new Exception("Detail: Error Message read sheet Inbound Rate : " + ex.Message);
            }
            return _dataSurcharge;
        }

        public void SaveSurcharge(List<Data.Domain.MasterSurcharge> list)
        {
            if (list.Count > 0)
            {

                using (var db = new Data.EfDbContext())
                {
                    var trans = db.Database.BeginTransaction();
                    try
                    {
                        ImportFreight.TruncateTbale("Master_Surcharge");
                        foreach (var g in list)
                        {

                            Data.Domain.MasterSurcharge DataSurcharge = new Data.Domain.MasterSurcharge();
                            Data.Domain.MasterSurchargeLog DataSurchargeLog = new Data.Domain.MasterSurchargeLog();

                            //set Master
                            DataSurcharge.Service_Code = g.Service_Code;
                            DataSurcharge.Origin_Code = g.Origin_Code;
                            DataSurcharge.Destination_Code = g.Destination_Code;
                            DataSurcharge.Moda_Factor_ID = g.Moda_Factor_ID;
                            DataSurcharge.Surcharge = g.Surcharge;
                            DataSurcharge.Surcharge_50 = g.Surcharge_50;
                            DataSurcharge.Surcharge_100 = g.Surcharge_100;
                            DataSurcharge.Surcharge_200 = g.Surcharge_200;
                            DataSurcharge.ValidonMounth = g.ValidonMounth;
                            DataSurcharge.ValidonYears = g.ValidonYears;
                            DataSurcharge.Remarks = g.Remarks;
                            DataSurcharge.EntryBy = Domain.SiteConfiguration.UserName;
                            DataSurcharge.ModifiedBy = Domain.SiteConfiguration.UserName;
                            DataSurcharge.EntryDate = DateTime.Now;
                            DataSurcharge.ModifiedDate = DateTime.Now;

                            //set Log
                            DataSurchargeLog.Service_Code = g.Service_Code;
                            DataSurchargeLog.Origin_Code = g.Origin_Code;
                            DataSurchargeLog.Destination_Code = g.Destination_Code;
                            DataSurchargeLog.Moda_Factor_ID = g.Moda_Factor_ID;
                            DataSurchargeLog.Surcharge = g.Surcharge;
                            DataSurchargeLog.Surcharge_50 = g.Surcharge_50;
                            DataSurchargeLog.Surcharge_100 = g.Surcharge_100;
                            DataSurchargeLog.Surcharge_200 = g.Surcharge_200;
                            DataSurchargeLog.ValidonMounth = g.ValidonMounth;
                            DataSurchargeLog.ValidonYears = g.ValidonYears;
                            DataSurchargeLog.Remarks = g.Remarks;
                            DataSurchargeLog.EntryBy = Domain.SiteConfiguration.UserName;
                            DataSurchargeLog.ModifiedBy = Domain.SiteConfiguration.UserName;
                            DataSurchargeLog.EntryDate = DateTime.Now;
                            DataSurchargeLog.ModifiedDate = DateTime.Now;

                            //Save DB
                            db.MasterSurcharge.Add(DataSurcharge);
                            db.MasterSurchargeLog.Add(DataSurchargeLog);
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


        public static void SaveSurchargeOld(ISheet sheet, string _fileName, string _Modul)
        {
            var _itemSurchage = new Data.Domain.MasterSurcharge();
            var _itemSurchageLog = new Data.Domain.MasterSurchargeLog();
            var _logImport = new Data.Domain.LogImport();
            try
            {
                if (sheet.GetRow(1) != null)
                {
                    ImportFreight.TruncateTbale("Master_Surcharge");
                    for (int drRate = 1; drRate < sheet.LastRowNum; drRate++)
                    {
                        if (sheet.GetRow(drRate).GetCell(0).StringCellValue == string.Empty)
                            break;
                        else
                        {
                            //try
                            //{
                            _itemSurchage.Service_Code = sheet.GetRow(drRate).GetCell(0).StringCellValue.Trim();
                            _itemSurchage.Origin_Code = sheet.GetRow(drRate).GetCell(1).StringCellValue.Trim();
                            _itemSurchage.Destination_Code = sheet.GetRow(drRate).GetCell(3).StringCellValue.Trim();
                            var ModaID = Service.Master.MasterGeneric.GetIDByCodeName("Moda", sheet.GetRow(drRate).GetCell(5).StringCellValue.Trim());
                            if (ModaID == null)
                                throw new Exception("Moda " + sheet.GetRow(drRate).GetCell(5).StringCellValue.Trim());

                            _itemSurchage.Moda_Factor_ID = Convert.ToInt32(ModaID.ID);
                            if (sheet.GetRow(drRate).GetCell(6).CellType == CellType.Numeric)
                                _itemSurchage.Surcharge = ImportFreight.getValue(sheet, drRate, 6).ToString();
                            else
                                _itemSurchage.Surcharge = sheet.GetRow(drRate).GetCell(6).StringCellValue.Trim();

                            if (sheet.GetRow(drRate).GetCell(7).CellType == CellType.Numeric)
                                _itemSurchage.Surcharge_50 = ImportFreight.getValue(sheet, drRate, 7).ToString();
                            else
                                _itemSurchage.Surcharge_50 = sheet.GetRow(drRate).GetCell(7).StringCellValue.Trim();

                            if (sheet.GetRow(drRate).GetCell(8).CellType == CellType.Numeric)
                                _itemSurchage.Surcharge_100 = ImportFreight.getValue(sheet, drRate, 8).ToString();
                            else
                                _itemSurchage.Surcharge_100 = sheet.GetRow(drRate).GetCell(8).StringCellValue.Trim();

                            if (sheet.GetRow(drRate).GetCell(9).CellType == CellType.Numeric)
                                _itemSurchage.Surcharge_200 = ImportFreight.getValue(sheet, drRate, 9).ToString();
                            else
                                _itemSurchage.Surcharge_200 = sheet.GetRow(drRate).GetCell(9).StringCellValue.Trim();

                            _itemSurchage.ValidonMounth = Convert.ToInt32(ImportFreight.getValue(sheet, drRate, 10));
                            _itemSurchage.ValidonYears = Convert.ToInt32(ImportFreight.getValue(sheet, drRate, 11));
                            _itemSurchage.Remarks = sheet.GetRow(drRate).GetCell(12).StringCellValue.Trim();

                            #region log
                            _itemSurchageLog.Service_Code = sheet.GetRow(drRate).GetCell(0).StringCellValue.Trim();
                            _itemSurchageLog.Origin_Code = sheet.GetRow(drRate).GetCell(1).StringCellValue.Trim();
                            _itemSurchageLog.Destination_Code = sheet.GetRow(drRate).GetCell(3).StringCellValue.Trim();
                            _itemSurchageLog.Moda_Factor_ID = Convert.ToInt32(ModaID.ID);
                            if (sheet.GetRow(drRate).GetCell(6).CellType == CellType.Numeric)
                                _itemSurchageLog.Surcharge = ImportFreight.getValue(sheet, drRate, 6).ToString();
                            else
                                _itemSurchageLog.Surcharge = sheet.GetRow(drRate).GetCell(6).StringCellValue.Trim();

                            if (sheet.GetRow(drRate).GetCell(7).CellType == CellType.Numeric)
                                _itemSurchageLog.Surcharge_50 = ImportFreight.getValue(sheet, drRate, 7).ToString();
                            else
                                _itemSurchageLog.Surcharge_50 = sheet.GetRow(drRate).GetCell(7).StringCellValue.Trim();

                            if (sheet.GetRow(drRate).GetCell(8).CellType == CellType.Numeric)
                                _itemSurchageLog.Surcharge_100 = ImportFreight.getValue(sheet, drRate, 8).ToString();
                            else
                                _itemSurchageLog.Surcharge_100 = sheet.GetRow(drRate).GetCell(8).StringCellValue.Trim();

                            if (sheet.GetRow(drRate).GetCell(9).CellType == CellType.Numeric)
                                _itemSurchageLog.Surcharge_200 = ImportFreight.getValue(sheet, drRate, 9).ToString();
                            else
                                _itemSurchageLog.Surcharge_200 = sheet.GetRow(drRate).GetCell(9).StringCellValue.Trim();

                            _itemSurchageLog.ValidonMounth = Convert.ToInt32(ImportFreight.getValue(sheet, drRate, 10));
                            _itemSurchageLog.ValidonYears = Convert.ToInt32(ImportFreight.getValue(sheet, drRate, 11));
                            _itemSurchageLog.Remarks = sheet.GetRow(drRate).GetCell(12).StringCellValue.Trim();
                            #endregion

                            var existDBSurcharge = Service.Master.MasterSurcharge.GetExistDB(_itemSurchage);
                            if (existDBSurcharge == null)
                            {
                                Service.Master.MasterSurcharge.crud(_itemSurchage, "I");
                                Service.Master.MasterSurcharge.crud(_itemSurchageLog, "I");
                            }
                            //}
                            //catch (Exception ex)
                            //{

                            //    ImportFreight.setException(_logImport, ex.Message.ToString(), _fileName, _Modul, "Surcharge");
                            //}
                        }
                    }
                }
                else
                {
                    ImportFreight.setException(_logImport, "No Data Surcharge", _fileName, _Modul, "Surcharge");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Detail: Error Message read sheet Surcharge : " + ex.Message);
            }
        }
    }
}
