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
    public class MasterInboundRateImport
    {
        public List<Data.Domain.MasterInboundRate> GetInboundRate(ISheet sheet, string _fileName, string _Modul)
        {
            List<Data.Domain.MasterInboundRate> _dataInbound = new List<Data.Domain.MasterInboundRate>();

            try
            {
                if (sheet.GetRow(1) != null)
                {
                    for (int drRate = 1; drRate <= sheet.LastRowNum; drRate++)
                    {
                        var _itemInbound = new Data.Domain.MasterInboundRate();
                        if (sheet.GetRow(drRate).GetCell(0).StringCellValue == string.Empty)
                            break;
                        else
                        {
                            _itemInbound.Service_Code = sheet.GetRow(drRate).GetCell(0).StringCellValue.Trim();
                            _itemInbound.Origin_Code = sheet.GetRow(drRate).GetCell(1).StringCellValue.Trim();
                            _itemInbound.Destination_Code = sheet.GetRow(drRate).GetCell(3).StringCellValue.Trim();
                            _itemInbound.Currency = sheet.GetRow(drRate).GetCell(5).StringCellValue.Trim();
                            if (sheet.GetRow(drRate).GetCell(6).CellType == CellType.Numeric)
                                _itemInbound.Lead_Time = ImportFreight.getValue(sheet, drRate, 6).ToString();
                            else
                                _itemInbound.Lead_Time = sheet.GetRow(drRate).GetCell(6).StringCellValue.Trim();
                            _itemInbound.Port_Hub = sheet.GetRow(drRate).GetCell(7).StringCellValue.Trim();

                            _itemInbound.SSIN_Rate = ImportFreight.getValue(sheet, drRate, 8);
                            _itemInbound.HSIN_Rate = ImportFreight.getValue(sheet, drRate, 9);
                            _itemInbound.SINID_Rate = ImportFreight.getValue(sheet, drRate, 10);
                            _itemInbound.CC_Rate = ImportFreight.getValue(sheet, drRate, 11);
                            _itemInbound.DN_Rate = ImportFreight.getValue(sheet, drRate, 12);
                            _itemInbound.Rate_Inbound = ImportFreight.getValue(sheet, drRate, 13);
                            _itemInbound.ValidonMounth = Convert.ToInt32(ImportFreight.getValue(sheet, drRate, 14));
                            _itemInbound.ValidonYears = Convert.ToInt32(ImportFreight.getValue(sheet, drRate, 15));
                            _itemInbound.Remarks = sheet.GetRow(drRate).GetCell(16).StringCellValue.Trim();

                            var existDBInRate = _dataInbound.Select(s => s).Where(w => w.Origin_Code.Trim().ToLower() == _itemInbound.Origin_Code.Trim().ToLower())
                            .Where(w => w.Destination_Code.Trim().ToLower() == _itemInbound.Destination_Code.Trim().ToLower())
                            .Where(w => w.Service_Code.Trim().ToLower() == _itemInbound.Service_Code.Trim().ToLower())
                            .Where(w => w.Currency.Trim().ToLower() == _itemInbound.Currency.Trim().ToLower())
                            .Where(w => w.ValidonMounth == _itemInbound.ValidonMounth)
                            .Where(w => w.ValidonYears == _itemInbound.ValidonYears).FirstOrDefault();

                            if (existDBInRate == null)
                                _dataInbound.Add(_itemInbound);
                        }
                    }
                }
               

            }
            catch (Exception ex)
            {
                _dataInbound = new List<Data.Domain.MasterInboundRate>();
                throw new Exception("Detail: Error Message read sheet Inbound Rate : " + ex.Message);
            }
            return _dataInbound;
        }

        public void SaveInboundRate(List<Data.Domain.MasterInboundRate> list)
        {
            if (list.Count > 0)
            {
               
                using (var db = new Data.EfDbContext())
                {
                    var trans = db.Database.BeginTransaction();
                    try
                    {

                        ImportFreight.TruncateTbale("Master_InboundRate");
                        foreach (var g in list)
                        {

                            Data.Domain.MasterInboundRate DataInRate = new Data.Domain.MasterInboundRate();
                            Data.Domain.MasterInboundRateLog DataInRateLog = new Data.Domain.MasterInboundRateLog();

                            //set Master
                            DataInRate.Service_Code = g.Service_Code;
                            DataInRate.Origin_Code = g.Origin_Code;
                            DataInRate.Destination_Code = g.Destination_Code;
                            DataInRate.Currency = g.Currency;
                            DataInRate.Lead_Time = g.Lead_Time;
                            DataInRate.Port_Hub = g.Port_Hub;
                            DataInRate.SSIN_Rate = g.SSIN_Rate;
                            DataInRate.HSIN_Rate = g.HSIN_Rate;
                            DataInRate.SINID_Rate = g.SINID_Rate;
                            DataInRate.CC_Rate = g.CC_Rate;
                            DataInRate.DN_Rate = g.DN_Rate;
                            DataInRate.Rate_Inbound = g.Rate_Inbound;
                            DataInRate.ValidonMounth = g.ValidonMounth;
                            DataInRate.ValidonYears = g.ValidonYears;
                            DataInRate.Remarks = g.Remarks;
                            DataInRate.EntryBy = Domain.SiteConfiguration.UserName;
                            DataInRate.ModifiedBy = Domain.SiteConfiguration.UserName;
                            DataInRate.EntryDate = DateTime.Now;
                            DataInRate.ModifiedDate = DateTime.Now;

                            //set Log
                            DataInRateLog.Service_Code = g.Service_Code;
                            DataInRateLog.Origin_Code = g.Origin_Code;
                            DataInRateLog.Destination_Code = g.Destination_Code;
                            DataInRateLog.Currency = g.Currency;
                            DataInRateLog.Lead_Time = g.Lead_Time;
                            DataInRateLog.Port_Hub = g.Port_Hub;
                            DataInRateLog.SSIN_Rate = g.SSIN_Rate;
                            DataInRateLog.HSIN_Rate = g.HSIN_Rate;
                            DataInRateLog.SINID_Rate = g.SINID_Rate;
                            DataInRateLog.CC_Rate = g.CC_Rate;
                            DataInRateLog.DN_Rate = g.DN_Rate;
                            DataInRateLog.Rate_Inbound = g.Rate_Inbound;
                            DataInRateLog.ValidonMounth = g.ValidonMounth;
                            DataInRateLog.ValidonYears = g.ValidonYears;
                            DataInRateLog.Remarks = g.Remarks;
                            DataInRateLog.EntryBy = Domain.SiteConfiguration.UserName;
                            DataInRateLog.ModifiedBy = Domain.SiteConfiguration.UserName;
                            DataInRateLog.EntryDate = DateTime.Now;
                            DataInRateLog.ModifiedDate = DateTime.Now;

                            //Save DB
                            db.MasterInboundRate.Add(DataInRate);
                            db.MasterInboundRateLog.Add(DataInRateLog);
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

        public static void SaveInboundRateOLD(ISheet sheet, string _fileName, string _Modul)
        {
            var _itemInbound = new Data.Domain.MasterInboundRate();
            var _itemInboundLog = new Data.Domain.MasterInboundRateLog();
            var _logImport = new Data.Domain.LogImport();
            try
            {
                if (sheet.GetRow(1).GetCell(0) != null && sheet.GetRow(1).GetCell(2) != null)
                {
                    //ImportFreight.TruncateTbale("Master_InboundRate");
                    for (int drRate = 1; drRate < sheet.LastRowNum; drRate++)
                    {
                        if (sheet.GetRow(drRate).GetCell(0).StringCellValue == string.Empty)
                            break;
                        else
                        {
                            //try
                            //{
                            _itemInbound.Service_Code = sheet.GetRow(drRate).GetCell(0).StringCellValue.Trim();
                            _itemInbound.Origin_Code = sheet.GetRow(drRate).GetCell(1).StringCellValue.Trim();
                            _itemInbound.Destination_Code = sheet.GetRow(drRate).GetCell(3).StringCellValue.Trim();
                            _itemInbound.Currency = sheet.GetRow(drRate).GetCell(5).StringCellValue.Trim();
                            if (sheet.GetRow(drRate).GetCell(6).CellType == CellType.Numeric)
                                _itemInbound.Lead_Time = ImportFreight.getValue(sheet, drRate, 6).ToString();
                            else
                                _itemInbound.Lead_Time = sheet.GetRow(drRate).GetCell(6).StringCellValue.Trim();
                            _itemInbound.Port_Hub = sheet.GetRow(drRate).GetCell(7).StringCellValue.Trim();

                            _itemInbound.SSIN_Rate = ImportFreight.getValue(sheet, drRate, 8);
                            _itemInbound.HSIN_Rate = ImportFreight.getValue(sheet, drRate, 9);
                            _itemInbound.SINID_Rate = ImportFreight.getValue(sheet, drRate, 10);
                            _itemInbound.CC_Rate = ImportFreight.getValue(sheet, drRate, 11);
                            _itemInbound.DN_Rate = ImportFreight.getValue(sheet, drRate, 12);
                            _itemInbound.Rate_Inbound = ImportFreight.getValue(sheet, drRate, 13);
                            _itemInbound.ValidonMounth = Convert.ToInt32(ImportFreight.getValue(sheet, drRate, 14));
                            _itemInbound.ValidonYears = Convert.ToInt32(ImportFreight.getValue(sheet, drRate, 15));
                            _itemInbound.Remarks = sheet.GetRow(drRate).GetCell(16).StringCellValue.Trim();

                            #region Log

                            _itemInboundLog.Service_Code = sheet.GetRow(drRate).GetCell(0).StringCellValue.Trim();
                            _itemInboundLog.Origin_Code = sheet.GetRow(drRate).GetCell(1).StringCellValue.Trim();
                            _itemInboundLog.Destination_Code = sheet.GetRow(drRate).GetCell(3).StringCellValue.Trim();
                            _itemInboundLog.Currency = sheet.GetRow(drRate).GetCell(5).StringCellValue.Trim();
                            if (sheet.GetRow(drRate).GetCell(6).CellType == CellType.Numeric)
                                _itemInboundLog.Lead_Time = ImportFreight.getValue(sheet, drRate, 6).ToString();
                            else
                                _itemInboundLog.Lead_Time = sheet.GetRow(drRate).GetCell(6).StringCellValue.Trim();
                            _itemInboundLog.Port_Hub = sheet.GetRow(drRate).GetCell(7).StringCellValue.Trim();
                            _itemInboundLog.SSIN_Rate = ImportFreight.getValue(sheet, drRate, 8);
                            _itemInboundLog.HSIN_Rate = ImportFreight.getValue(sheet, drRate, 9);
                            _itemInboundLog.SINID_Rate = ImportFreight.getValue(sheet, drRate, 10);
                            _itemInboundLog.CC_Rate = ImportFreight.getValue(sheet, drRate, 11);
                            _itemInboundLog.DN_Rate = ImportFreight.getValue(sheet, drRate, 12);
                            _itemInboundLog.Rate_Inbound = ImportFreight.getValue(sheet, drRate, 13);
                            _itemInboundLog.ValidonMounth = Convert.ToInt32(ImportFreight.getValue(sheet, drRate, 14));
                            _itemInboundLog.ValidonYears = Convert.ToInt32(ImportFreight.getValue(sheet, drRate, 15));
                            _itemInboundLog.Remarks = sheet.GetRow(drRate).GetCell(16).StringCellValue.Trim();
                            #endregion

                            var existDBInRate = Service.Master.MasterInboundRate.GetExistDB(_itemInbound);
                            if (existDBInRate == null)
                            {
                                Service.Master.MasterInboundRate.crud(_itemInbound, "I");
                                Service.Master.MasterInboundRate.crud(_itemInboundLog, "I");
                            }
                            //}
                            //catch (Exception ex)
                            //{

                            //    ImportFreight.setException(_logImport, ex.Message.ToString(), _fileName, _Modul, "Inbound Rate");
                            //}
                        }
                    }
                }
                else
                {
                    ImportFreight.setException(_logImport, "No Data Inbound Rate", _fileName, _Modul, "Inbound Rate");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Detail: Error Message read sheet Inbound Rate : " + ex.Message);
            }
        }
    }
}
