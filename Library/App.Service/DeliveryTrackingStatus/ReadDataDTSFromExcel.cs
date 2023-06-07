using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using System.IO;
using ImportDTS = App.Service.DeliveryTrackingStatus.ImportDTS;
using MstDTS = App.Service.DeliveryTrackingStatus.MasterDTS;
using System.Globalization;
using App.Service.FreightCost;

namespace App.Service.DeliveryTrackingStatus
{
    public class ReadDataDTSFromExcel
    {
        public List<Data.Domain.DeliveryTrackingStatus> getDataDTS(string FilePath, string _fileName)
        {
            List<Data.Domain.DeliveryTrackingStatus> dataDTS = new List<Data.Domain.DeliveryTrackingStatus>();

            try
            {
                ISheet sheet = getSheet(FilePath);

                var _logImport = new Data.Domain.LogImport();
                var _file = new Data.Domain.DocumentUpload();
                for (int row = 0; row <= sheet.LastRowNum; row++)
                {

                    Data.Domain.DeliveryTrackingStatus data = new Data.Domain.DeliveryTrackingStatus();
                    Data.Domain.Master_Moda _itemModa = new Data.Domain.Master_Moda();
                    Data.Domain.Master_Status _itemStatus = new Data.Domain.Master_Status();
                    Data.Domain.Master_UnitType _ItemUnit = new Data.Domain.Master_UnitType();

                    if (sheet.GetRow(row) != null && sheet.GetRow(row).GetCell(0).StringCellValue != "Moda"
                        && sheet.GetRow(row).GetCell(0).StringCellValue != "")
                    {
                        _itemModa.ModaDescription = sheet.GetRow(row).GetCell(0).StringCellValue.Trim();
                        _ItemUnit.UnitTypeDescription = sheet.GetRow(row).GetCell(6).StringCellValue.Trim();
                        _itemStatus.Status = sheet.GetRow(row).GetCell(14).StringCellValue.Trim();

                        if (MstDTS.GetIDMsModa(sheet.GetRow(row).GetCell(0).StringCellValue.Trim()) == null
                            && sheet.GetRow(row).GetCell(0).StringCellValue.Trim() != string.Empty)
                            MstDTS.CRUDModa(_itemModa, "I");
                        if (MstDTS.GetIDMsStatus(sheet.GetRow(row).GetCell(14).StringCellValue.Trim()) == null
                            && sheet.GetRow(row).GetCell(14).StringCellValue.Trim() != string.Empty)
                            MstDTS.CRUDStatus(_itemStatus, "I");
                        if (MstDTS.GetIDMsUnitType(sheet.GetRow(row).GetCell(6).StringCellValue.Trim()) == null
                            && sheet.GetRow(row).GetCell(6).StringCellValue.Trim() != string.Empty)
                            MstDTS.CRUDUnitType(_ItemUnit, "I");

                        data.Moda = getModaID(setValue(sheet.GetRow(row).GetCell(0)));
                        data.Unit_Moda = setValue(sheet.GetRow(row).GetCell(1));
                        data.From = getCityCode(setValue(sheet.GetRow(row).GetCell(2)));
                        data.To = getCityCode(setValue(sheet.GetRow(row).GetCell(3)));
                        data.NODA = setValue(sheet.GetRow(row).GetCell(4));
                        data.NODI = setValue(sheet.GetRow(row).GetCell(5));
                        data.Unit_Type = getUnitTypeID(setValue(sheet.GetRow(row).GetCell(6)));
                        data.Model = setValue(sheet.GetRow(row).GetCell(7));
                        data.BatchNumber = setValue(sheet.GetRow(row).GetCell(8));
                        data.SN = sheet.GetRow(row).GetCell(9).StringCellValue;
                        if (!string.IsNullOrWhiteSpace(setValue(sheet.GetRow(row).GetCell(10))))
                            data.ETD = Convert.ToDateTime(setValue(sheet.GetRow(row).GetCell(10)));
                        if (!string.IsNullOrWhiteSpace(setValue(sheet.GetRow(row).GetCell(11))))
                            data.ATD = Convert.ToDateTime(setValue(sheet.GetRow(row).GetCell(11)));
                        if (!string.IsNullOrWhiteSpace(setValue(sheet.GetRow(row).GetCell(12))))
                            data.ETA = Convert.ToDateTime(setValue(sheet.GetRow(row).GetCell(12)));
                        if (!string.IsNullOrWhiteSpace(setValue(sheet.GetRow(row).GetCell(13))))
                            data.ATA = Convert.ToDateTime(setValue(sheet.GetRow(row).GetCell(13)));
                        data.Status = getStatusID(setValue(sheet.GetRow(row).GetCell(14)));
                        data.Cost = setValue(sheet.GetRow(row).GetCell(15));
                        data.Currency = setValue(sheet.GetRow(row).GetCell(16));
                        data.Ship_Doc = setValue(sheet.GetRow(row).GetCell(17));
                        data.Ship_Cost = setValue(sheet.GetRow(row).GetCell(18));
                        data.Entry_Sheet = setValue(sheet.GetRow(row).GetCell(19));
                        data.No_PI = setValue(sheet.GetRow(row).GetCell(20));
                        data.Reject = setValue(sheet.GetRow(row).GetCell(21));
                        data.Remarks = setValue(sheet.GetRow(row).GetCell(22));
                        data.EntryDate = DateTime.Today;
                        data.EntryBy = Domain.SiteConfiguration.UserName;

                        if (ImportDTS.GetExistDB(data) == null)
                            dataDTS.Add(data);
                    }

                }
            }
            catch (Exception ex)
            {
                dataDTS = new List<Data.Domain.DeliveryTrackingStatus>();
                throw new Exception("Detail: Error Message read sheet Shipment : " + ex.Message);
            }

            return dataDTS;
        }

        private ISheet getSheet(string FilePath)
        {
            string extension = Path.GetExtension(FilePath);
            HSSFWorkbook hssfwb;
            XSSFWorkbook xssfwb;
            ISheet sheet = null;

            try
            {
                using (FileStream file = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
                {
                    if (extension == ".xls")
                    {
                        hssfwb = new HSSFWorkbook(file);
                        sheet = hssfwb.GetSheet("Shipment");
                    }
                    else if (extension == ".xlsx")
                    {
                        xssfwb = new XSSFWorkbook(file);
                        sheet = xssfwb.GetSheet("Shipment");
                    }
                    else
                        throw new Exception("File extension is not valid.");
                }

                if (sheet == null)
                    throw new Exception("Excel is not valid.");
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                    throw new Exception("Error when read Sheet. Error Message: " + ex.Message);
                else
                    throw new Exception("Error when read Sheet. Error Message: " + ex.InnerException.Message);
            }

            return sheet;
        }

        private int getModaID(string modaName)
        {
            int _ID = 0;
            try
            {
                var data = MstDTS.GetIDMsModa(modaName.Trim().ToLower());
                if (data != null)
                    _ID = data.ModaID;
                else
                    throw new Exception("Moda " + modaName + " is not found in data master.");
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                    throw new Exception("Error when read data Moda. Error Message: " + ex.Message);
                else
                    throw new Exception("Error when read data Moda. Error Message: " + ex.InnerException.Message);
            }
            return _ID;
        }

        private string getCityCode(string storeName)
        {
            string _code = string.Empty;
            try
            {
                var data = MstDTS.GetIDCity(storeName.Trim().ToLower());
                if (data != null)
                    _code = data.Code;
                else
                    throw new Exception("City " + storeName + " is not found in data master.");
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                    throw new Exception("Error when read data City. Error Message: " + ex.Message);
                else
                    throw new Exception("Error when read data City. Error Message: " + ex.InnerException.Message);
            }
            return _code;
        }

        private int getUnitTypeID(string unitType)
        {
            int _ID = 0;
            try
            {
                var data = MstDTS.GetIDMsUnitType(unitType.Trim().ToLower());
                if (data != null)
                    _ID = data.UnitTypeID;
                else
                    throw new Exception("Unit Type " + unitType + " is not found in data master.");
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                    throw new Exception("Error when read data Unit Type. Error Message: " + ex.Message);
                else
                    throw new Exception("Error when read data Unit Type. Error Message: " + ex.InnerException.Message);
            }
            return _ID;
        }

        private int getStatusID(string status)
        {
            int _ID = 0;
            try
            {
                var data = MstDTS.GetIDMsStatus(status.Trim().ToLower());
                if (data != null)
                    _ID = data.StatusID;
                else
                    throw new Exception("Status " + status + " is not found in data master.");
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                    throw new Exception("Error when read data Status. Error Message: " + ex.Message);
                else
                    throw new Exception("Error when read data Status. Error Message: " + ex.InnerException.Message);
            }
            return _ID;
        }

        private string setValue(ICell cell)
        {
            DataFormatter dataFormatter = new DataFormatter(CultureInfo.CurrentCulture);
            return dataFormatter.FormatCellValue(cell);
        }
    }
}
