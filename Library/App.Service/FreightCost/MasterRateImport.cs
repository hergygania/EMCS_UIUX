using App.Data.Domain;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace App.Service.FreightCost
{
    public class MasterRateImport
    {
        public List<Data.Domain.MasterRate> GetRate(ISheet sheet, string _fileName, string _Modul)
        {
            List<Data.Domain.MasterRate> _dataRate = new List<Data.Domain.MasterRate>();
            var _logImport = new Data.Domain.LogImport();
            try
            {
                if (sheet.GetRow(1) != null)
                {
                    for (int drRate = 1; drRate <= sheet.LastRowNum; drRate++)
                    {
                        var _itemdata = new Data.Domain.MasterRate();
                        if (sheet.GetRow(drRate).GetCell(0).StringCellValue == string.Empty)
                            break;
                        else
                        {
                            bool ir = false;
                            if (sheet.GetRow(drRate).GetCell(14).StringCellValue.Trim() == "IR")
                                ir = true;

                            #region Master Rate
                            _itemdata.Service_Code = sheet.GetRow(drRate).GetCell(0).StringCellValue.Trim();
                            _itemdata.Origin_Code = sheet.GetRow(drRate).GetCell(1).StringCellValue.Trim();
                            _itemdata.Destination_Code = sheet.GetRow(drRate).GetCell(3).StringCellValue.Trim();
                            _itemdata.Currency = sheet.GetRow(drRate).GetCell(5).StringCellValue.Trim();

                            if (sheet.GetRow(drRate).GetCell(6).CellType == CellType.Numeric)
                                _itemdata.Lead_Time = ImportFreight.getValue(sheet, drRate, 6).ToString();
                            else
                                _itemdata.Lead_Time = sheet.GetRow(drRate).GetCell(6).StringCellValue.Trim();

                            _itemdata.MIN_Rate = ImportFreight.getValue(sheet, drRate, 7);
                            _itemdata.WeightBreak1000 = sheet.GetRow(drRate).GetCell(8).NumericCellValue.ToString();
                            _itemdata.WeightBreak3999 = sheet.GetRow(drRate).GetCell(9).NumericCellValue.ToString();
                            _itemdata.WeightBreak99999 = sheet.GetRow(drRate).GetCell(10).NumericCellValue.ToString();
                            _itemdata.Via = sheet.GetRow(drRate).GetCell(11).StringCellValue.Trim();
                            _itemdata.Ragulated = sheet.GetRow(drRate).GetCell(12).StringCellValue.Trim();
                            if (sheet.GetRow(drRate).GetCell(6).CellType == CellType.Numeric)
                                _itemdata.Cost = ImportFreight.getValue(sheet, drRate, 13);
                            else
                                _itemdata.Cost = 0;
                            _itemdata.IR = ir;
                            _itemdata.ValidonMounth = Convert.ToInt32(ImportFreight.getValue(sheet, drRate, 15));
                            _itemdata.ValidonYears = Convert.ToInt32(ImportFreight.getValue(sheet, drRate, 16));
                            _itemdata.Remarks = sheet.GetRow(drRate).GetCell(17).StringCellValue.Trim();
                            #endregion

                            var exists = _dataRate.Select(s => s).Where(w => w.Origin_Code.Trim().ToLower() == _itemdata.Origin_Code.Trim().ToLower())
                                        .Where(w => w.Destination_Code.Trim().ToLower() == _itemdata.Destination_Code.Trim().ToLower())
                                        .Where(w => w.Service_Code.Trim().ToLower() == _itemdata.Service_Code.Trim().ToLower())
                                        .Where(w => w.WeightBreak1000.Trim().ToLower() == _itemdata.WeightBreak1000.Trim().ToLower())
                                        .Where(w => w.WeightBreak3999.Trim().ToLower() == _itemdata.WeightBreak3999.Trim().ToLower())
                                        .Where(w => w.WeightBreak99999.Trim().ToLower() == _itemdata.WeightBreak99999.Trim().ToLower())
                                        .Where(w => w.Currency.Trim().ToLower() == _itemdata.Currency.Trim().ToLower())
                                        .Where(w => w.ValidonMounth == _itemdata.ValidonMounth)
                                        .Where(w => w.ValidonYears == _itemdata.ValidonYears)
                                        .FirstOrDefault();

                            if (exists == null)
                                _dataRate.Add(_itemdata);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _dataRate = new List<Data.Domain.MasterRate>();
                throw new Exception("Detail: Error Message read sheet Rate : " + ex.Message);
            }


            return _dataRate;
        }

        public void SaveDataRate(List<Data.Domain.MasterRate> list)
        {
            if (list.Count > 0)
            {
                using (var db = new Data.EfDbContext())
                {
                    var trans = db.Database.BeginTransaction();
                    try
                    {
                        ImportFreight.TruncateTbale("Master_Rate");
                        foreach (var g in list)
                        {
                            Data.Domain.MasterRate DataRate = new Data.Domain.MasterRate();
                            Data.Domain.MasterRateLog DataRateLog = new Data.Domain.MasterRateLog();

                            //set Master
                            DataRate.Origin_Code = g.Origin_Code;
                            DataRate.Destination_Code = g.Destination_Code;
                            DataRate.Service_Code = g.Service_Code;
                            DataRate.WeightBreak1000 = g.WeightBreak1000;
                            DataRate.WeightBreak3999 = g.WeightBreak3999;
                            DataRate.WeightBreak99999 = g.WeightBreak99999;
                            DataRate.Currency = g.Currency;
                            DataRate.MIN_Rate = g.MIN_Rate;
                            DataRate.Lead_Time = g.Lead_Time;
                            DataRate.Via = g.Via;
                            DataRate.Cost = g.Cost;
                            DataRate.Ragulated = g.Ragulated;
                            DataRate.IR = g.IR;
                            DataRate.ValidonMounth = g.ValidonMounth;
                            DataRate.ValidonYears = g.ValidonYears;
                            DataRate.Remarks = g.Remarks;
                            DataRate.EntryBy = Domain.SiteConfiguration.UserName;
                            DataRate.ModifiedBy = Domain.SiteConfiguration.UserName;
                            DataRate.EntryDate = DateTime.Now;
                            DataRate.ModifiedDate = DateTime.Now;

                            //set Log
                            DataRateLog.Origin_Code = g.Origin_Code;
                            DataRateLog.Destination_Code = g.Destination_Code;
                            DataRateLog.Service_Code = g.Service_Code;
                            DataRateLog.WeightBreak1000 = g.WeightBreak1000;
                            DataRateLog.WeightBreak3999 = g.WeightBreak3999;
                            DataRateLog.WeightBreak99999 = g.WeightBreak99999;
                            DataRateLog.Currency = g.Currency;
                            DataRateLog.MIN_Rate = g.MIN_Rate;
                            DataRateLog.Lead_Time = g.Lead_Time;
                            DataRateLog.Via = g.Via;
                            DataRateLog.Cost = g.Cost;
                            DataRateLog.Ragulated = g.Ragulated;
                            DataRateLog.IR = g.IR;
                            DataRateLog.ValidonMounth = g.ValidonMounth;
                            DataRateLog.ValidonYears = g.ValidonYears;
                            DataRateLog.Remarks = g.Remarks;
                            DataRateLog.EntryBy = Domain.SiteConfiguration.UserName;
                            DataRateLog.ModifiedBy = Domain.SiteConfiguration.UserName;
                            DataRateLog.EntryDate = DateTime.Now;
                            DataRateLog.ModifiedDate = DateTime.Now;

                            //Save DB
                            db.MasterRate.Add(DataRate);
                            db.MasterRateLog.Add(DataRateLog);
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

        //public static void SaveRate(ISheet sheet, string _fileName, string _Modul)
        //{
        //    var _itemRate = new MasterRate();
        //    var _itemRateLog = new MasterRateLog();
        //    var _logImport = new Data.Domain.LogImport();
        //    try
        //    {
        //        if (sheet.GetRow(1).GetCell(0) != null && sheet.GetRow(1).GetCell(2) != null)
        //        {
        //            ImportFreight.TruncateTbale("Master_Rate");
        //            for (int drRate = 1; drRate <= sheet.LastRowNum; drRate++)
        //            {
        //                if (sheet.GetRow(drRate).GetCell(0).StringCellValue == string.Empty)
        //                    break;
        //                else
        //                {
        //                    bool ir = false;
        //                    if (sheet.GetRow(drRate).GetCell(14).StringCellValue.Trim() == "IR")
        //                        ir = true;

        //                    #region Master Rate
        //                    _itemRate.Service_Code = sheet.GetRow(drRate).GetCell(0).StringCellValue.Trim();
        //                    _itemRate.Origin_Code = sheet.GetRow(drRate).GetCell(1).StringCellValue.Trim();
        //                    _itemRate.Destination_Code = sheet.GetRow(drRate).GetCell(3).StringCellValue.Trim();
        //                    _itemRate.Currency = sheet.GetRow(drRate).GetCell(5).StringCellValue.Trim();

        //                    if (sheet.GetRow(drRate).GetCell(6).CellType == CellType.Numeric)
        //                        _itemRate.Lead_Time = ImportFreight.getValue(sheet, drRate, 6).ToString();
        //                    else
        //                        _itemRate.Lead_Time = sheet.GetRow(drRate).GetCell(6).StringCellValue.Trim();

        //                    _itemRate.MIN_Rate = ImportFreight.getValue(sheet, drRate, 7);
        //                    _itemRate.WeightBreak1000 = sheet.GetRow(drRate).GetCell(8).NumericCellValue.ToString();
        //                    _itemRate.WeightBreak3999 = sheet.GetRow(drRate).GetCell(9).NumericCellValue.ToString();
        //                    _itemRate.WeightBreak99999 = sheet.GetRow(drRate).GetCell(10).NumericCellValue.ToString();
        //                    _itemRate.Via = sheet.GetRow(drRate).GetCell(11).StringCellValue.Trim();
        //                    _itemRate.Ragulated = sheet.GetRow(drRate).GetCell(12).StringCellValue.Trim();
        //                    if (sheet.GetRow(drRate).GetCell(6).CellType == CellType.Numeric)
        //                        _itemRate.Cost = ImportFreight.getValue(sheet, drRate, 13);
        //                    else
        //                        _itemRate.Cost = 0;
        //                    _itemRate.IR = ir;
        //                    _itemRate.ValidonMounth = Convert.ToInt32(ImportFreight.getValue(sheet, drRate, 15));
        //                    _itemRate.ValidonYears = Convert.ToInt32(ImportFreight.getValue(sheet, drRate, 16));
        //                    _itemRate.Remarks = sheet.GetRow(drRate).GetCell(17).StringCellValue.Trim();
        //                    #endregion

        //                    #region Master Rate Log
        //                    _itemRateLog.Service_Code = sheet.GetRow(drRate).GetCell(0).StringCellValue.Trim();
        //                    _itemRateLog.Origin_Code = sheet.GetRow(drRate).GetCell(1).StringCellValue.Trim();
        //                    _itemRateLog.Destination_Code = sheet.GetRow(drRate).GetCell(3).StringCellValue.Trim();
        //                    _itemRateLog.Currency = sheet.GetRow(drRate).GetCell(5).StringCellValue.Trim();

        //                    if (sheet.GetRow(drRate).GetCell(6).CellType == CellType.Numeric)
        //                        _itemRateLog.Lead_Time = ImportFreight.getValue(sheet, drRate, 6).ToString();
        //                    else
        //                        _itemRateLog.Lead_Time = sheet.GetRow(drRate).GetCell(6).StringCellValue.Trim();

        //                    _itemRateLog.MIN_Rate = ImportFreight.getValue(sheet, drRate, 7);
        //                    _itemRateLog.WeightBreak1000 = sheet.GetRow(drRate).GetCell(8).NumericCellValue.ToString();
        //                    _itemRateLog.WeightBreak3999 = sheet.GetRow(drRate).GetCell(9).NumericCellValue.ToString();
        //                    _itemRateLog.WeightBreak99999 = sheet.GetRow(drRate).GetCell(10).NumericCellValue.ToString();
        //                    _itemRateLog.Via = sheet.GetRow(drRate).GetCell(11).StringCellValue.Trim();
        //                    _itemRateLog.Ragulated = sheet.GetRow(drRate).GetCell(12).StringCellValue.Trim();
        //                    if (sheet.GetRow(drRate).GetCell(6).CellType == CellType.Numeric)
        //                        _itemRateLog.Cost = ImportFreight.getValue(sheet, drRate, 13);
        //                    else
        //                        _itemRateLog.Cost = 0;
        //                    _itemRateLog.IR = ir;
        //                    _itemRateLog.ValidonMounth = Convert.ToInt32(ImportFreight.getValue(sheet, drRate, 15));
        //                    _itemRateLog.ValidonYears = Convert.ToInt32(ImportFreight.getValue(sheet, drRate, 16));
        //                    _itemRateLog.Remarks = sheet.GetRow(drRate).GetCell(17).StringCellValue.Trim();
        //                    #endregion

        //                    var existDbRate = Service.Master.MasterRate.GetExistDB(_itemRate);
        //                    if (existDbRate == null)
        //                    {
        //                        Service.Master.MasterRate.crud(_itemRate, "I");
        //                        Service.Master.MasterRateLog.CRUD_Log(_itemRateLog, "I");
        //                    }
        //                }
        //            }
        //        }
        //        else
        //        {
        //            ImportFreight.setException(_logImport, "No Data Rate in sheet Rate", _fileName, _Modul, "");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Detail: Error Message read sheet Rate : " + ex.Message);
        //    }
        //}
    }
}
