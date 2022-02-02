using App.Data.Caching;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Service.CAT
{
    public class ImportExcel
    {
        private const string cacheName = "App.CAT.ImportExcel";

        private readonly static ICacheManager _cacheManager = new MemoryCacheManager();

        /// <summary>
        /// geberate sheet dari file excel.
        /// </summary>
        /// <param name="FilePath"></param>
        /// <param name="SheetName"></param>
        /// <returns></returns>
        private ISheet getSheet(string FilePath, string SheetName)
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
                        sheet = hssfwb.GetSheet(SheetName);
                    }
                    else if (extension == ".xlsx")
                    {
                        xssfwb = new XSSFWorkbook(file);
                        sheet = xssfwb.GetSheet(SheetName);
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

        /// <summary>
        /// pengambilan data Part Info Detail dari file excel.
        /// </summary>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        public List<Data.Domain.PartInfoDetail> getCompDetail(string FilePath)
        {
            List<Data.Domain.PartInfoDetail> CompDetail = new List<Data.Domain.PartInfoDetail>();
            string AltPartNo = "";
            try
            {
                ISheet sheet = getSheet(FilePath, "DETAIL");
                var _logImport = new Data.Domain.LogImport();
                var _file = new Data.Domain.DocumentUpload();

                for (int row = 1; row <= sheet.LastRowNum; row++)
                {
                    Data.Domain.PartInfoDetail data = new Data.Domain.PartInfoDetail();
                    data.AltPartNo = setValue(sheet.GetRow(row).GetCell(0)).ToString().Trim();
                    AltPartNo = setValue(sheet.GetRow(row).GetCell(0)).ToString().Trim();
                    data.RefPartNo = setValue(sheet.GetRow(row).GetCell(1)).ToString().Trim();
                    data.CoreModel = setValue(sheet.GetRow(row).GetCell(2)).ToString();
                    data.Family = setValue(sheet.GetRow(row).GetCell(3)).ToString();
                    data.Model = setValue(sheet.GetRow(row).GetCell(4)).ToString();
                    data.AppPrefix = setValue(sheet.GetRow(row).GetCell(5)).ToString();
                    data.SMCS = setValue(sheet.GetRow(row).GetCell(6)).ToString();
                    data.Component = setValue(sheet.GetRow(row).GetCell(7)).ToString();
                    data.MOD = setValue(sheet.GetRow(row).GetCell(8)).ToString();
                    data.CRCTAT = setValue(sheet.GetRow(row).GetCell(9));
                    data.MajorMinorCyl = setValue(sheet.GetRow(row).GetCell(10));
                    CompDetail.Add(data);
                }
            }
            catch (Exception ex)
            {
                CompDetail = new List<Data.Domain.PartInfoDetail>();
                if (!string.IsNullOrWhiteSpace(AltPartNo))
                    throw new Exception("Detail: Error Message read sheet Component Detail [On Part Number : " + AltPartNo + "] : " + ex.Message);
                else
                    throw new Exception("Detail: Error Message read sheet Component Detail : " + ex.Message);
            }
            return CompDetail;
        }

        /// <summary>
        /// pengambilan data Inventory Allocation dari file excel.
        /// </summary>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        public List<Data.Domain.InventoryAllocation> getInvAllocation(string FilePath)
        {
            List<Data.Domain.InventoryAllocation> invallocation = new List<Data.Domain.InventoryAllocation>();
            try
            {
                ISheet sheet = getSheet(FilePath, "ALLOCATION");
                var _logImport = new Data.Domain.LogImport();
                var _file = new Data.Domain.DocumentUpload();

                for (int row = 2; row <= sheet.LastRowNum; row++)
                {
                    string KAL = setValue(sheet.GetRow(row).GetCell(0)).ToString();

                    Data.Domain.InventoryAllocation data = new Data.Domain.InventoryAllocation();
                    if (!isValidAllocation(setValue(sheet.GetRow(row).GetCell(0))))
                        throw new Exception("KAL : " + KAL + " not found in inventory list, on excel row : " + (row + 1).ToString());


                    if (!string.IsNullOrWhiteSpace(setValue(sheet.GetRow(row).GetCell(3)))) // Cycle 1 before 1
                    {
                        data = new Data.Domain.InventoryAllocation();
                        data.KAL = KAL;
                        data.UnitNo = setValue(sheet.GetRow(row).GetCell(3)).ToString();//before 1
                        data.SerialNo = setValue(sheet.GetRow(row).GetCell(4)).ToString();//before 2
                        data.PONumber = setValue(sheet.GetRow(row).GetCell(1)).ToString();//setValue(sheet.GetRow(row).GetCell(3)).ToString();
                        data.StoreID = getStoreID(setValue(sheet.GetRow(row).GetCell(5)).ToString());//before 4
                        data.CUSTOMER_ID = getCustomerID(setValue(sheet.GetRow(row).GetCell(6)).ToString());//before 5
                        data.Customer = setValue(sheet.GetRow(row).GetCell(6)).ToString();//before 5
                        try
                        {
                            if (!string.IsNullOrWhiteSpace(setValue(sheet.GetRow(row).GetCell(2)).ToString()))//before 6
                                data.OriginalSchedule = Convert.ToDateTime(setValue(sheet.GetRow(row).GetCell(2)).ToString());//before 6
                            else
                                throw new Exception("Error Cycle 1 [KAL : " + KAL + "]: Original Schedule is mandatory, on excel row : " + (row + 1).ToString());
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Error Cycle 1 :" + ex.Message + ", on excel row : " + (row + 1).ToString());
                        }
                        data.IsActive = true;
                        data.IsUsed = false;
                        data.Cycle = 1;
                        invallocation.Add(data);
                    }
                    else
                    {
                        data = new Data.Domain.InventoryAllocation();
                        data.KAL = KAL;
                        data.Cycle = 1;
                        invallocation.Add(data);
                    }
                    if (!string.IsNullOrWhiteSpace(setValue(sheet.GetRow(row).GetCell(9)))) // Cycle 2 before 7
                    {
                        data = new Data.Domain.InventoryAllocation();
                        data.KAL = KAL;
                        data.UnitNo = setValue(sheet.GetRow(row).GetCell(9)).ToString();//before 7
                        data.SerialNo = setValue(sheet.GetRow(row).GetCell(10)).ToString();//before 8
                        data.PONumber = setValue(sheet.GetRow(row).GetCell(7)).ToString();//before 9
                        data.StoreID = getStoreID(setValue(sheet.GetRow(row).GetCell(11)).ToString());//before 10
                        data.CUSTOMER_ID = getCustomerID(setValue(sheet.GetRow(row).GetCell(12)).ToString());//before 11
                        data.Customer = setValue(sheet.GetRow(row).GetCell(12)).ToString();//before 11
                        try
                        {
                            if (!string.IsNullOrWhiteSpace(setValue(sheet.GetRow(row).GetCell(8)).ToString()))//before 12
                                data.OriginalSchedule = Convert.ToDateTime(setValue(sheet.GetRow(row).GetCell(8)).ToString());//before 12
                            else
                                throw new Exception("Error Cycle 2 [KAL : " + KAL + "]: Original Schedule is mandatory, on excel row : " + (row + 1).ToString());
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Error Cycle 2 : " + ex.Message + ", on excel row : " + (row + 1).ToString());
                        }
                        data.IsActive = false;
                        data.IsUsed = false;
                        data.Cycle = 2;
                        invallocation.Add(data);
                    }
                    else
                    {
                        data = new Data.Domain.InventoryAllocation();
                        data.KAL = KAL;
                        data.Cycle = 2;
                        invallocation.Add(data);
                    }
                    if (!string.IsNullOrWhiteSpace(setValue(sheet.GetRow(row).GetCell(15)))) // Cycle 3 before 13
                    {
                        data = new Data.Domain.InventoryAllocation();
                        data.KAL = KAL;
                        data.UnitNo = setValue(sheet.GetRow(row).GetCell(15)).ToString();//before 13
                        data.SerialNo = setValue(sheet.GetRow(row).GetCell(16)).ToString();//before 14
                        data.PONumber = setValue(sheet.GetRow(row).GetCell(13)).ToString();//before 15
                        data.StoreID = getStoreID(setValue(sheet.GetRow(row).GetCell(17)).ToString());//before 16
                        data.CUSTOMER_ID = getCustomerID(setValue(sheet.GetRow(row).GetCell(18)).ToString());//before 17
                        data.Customer = setValue(sheet.GetRow(row).GetCell(18)).ToString();//before 17
                        try
                        {
                            if (!string.IsNullOrWhiteSpace(setValue(sheet.GetRow(row).GetCell(14)).ToString()))//before 18
                                data.OriginalSchedule = Convert.ToDateTime(setValue(sheet.GetRow(row).GetCell(14)).ToString());//before 18
                            else
                                throw new Exception("Error Cycle 3 [KAL : " + KAL + "]: Original Schedule is mandatory, on excel row : " + (row + 1).ToString());
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Error Cycle 3 : " + ex.Message + ", on excel row : " + (row + 1).ToString());
                        }
                        data.IsActive = false;
                        data.IsUsed = false;
                        data.Cycle = 3;
                        invallocation.Add(data);
                    }
                    else
                    {
                        data = new Data.Domain.InventoryAllocation();
                        data.KAL = KAL;
                        data.Cycle = 3;
                        invallocation.Add(data);
                    }
                    if (!string.IsNullOrWhiteSpace(setValue(sheet.GetRow(row).GetCell(21)))) // Cycle 4 before 19
                    {
                        data = new Data.Domain.InventoryAllocation();
                        data.KAL = KAL;
                        data.UnitNo = setValue(sheet.GetRow(row).GetCell(21)).ToString();//before 19
                        data.SerialNo = setValue(sheet.GetRow(row).GetCell(22)).ToString();//before 20
                        data.PONumber = setValue(sheet.GetRow(row).GetCell(19)).ToString();//before 21
                        data.StoreID = getStoreID(setValue(sheet.GetRow(row).GetCell(23)).ToString());//before 22
                        data.CUSTOMER_ID = getCustomerID(setValue(sheet.GetRow(row).GetCell(24)).ToString());//before 23
                        data.Customer = setValue(sheet.GetRow(row).GetCell(24)).ToString();//before 23
                        try
                        {
                            if (!string.IsNullOrWhiteSpace(setValue(sheet.GetRow(row).GetCell(20)).ToString()))//before 24
                                data.OriginalSchedule = Convert.ToDateTime(setValue(sheet.GetRow(row).GetCell(20)).ToString());//before 24
                            else
                                throw new Exception("Error Cycle 4 [KAL : " + KAL + "]: Original Schedule is mandatory, on excel row : " + (row + 1).ToString());
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Error Cycle 4 : " + ex.Message + ", on excel row : " + (row + 1).ToString());
                        }
                        data.IsActive = false;
                        data.IsUsed = false;
                        data.Cycle = 4;
                        invallocation.Add(data);
                    }
                    else
                    {
                        data = new Data.Domain.InventoryAllocation();
                        data.KAL = KAL;
                        data.Cycle = 4;
                        invallocation.Add(data);
                    }
                    if (!string.IsNullOrWhiteSpace(setValue(sheet.GetRow(row).GetCell(27)))) // Cycle 5 before 25
                    {
                        data = new Data.Domain.InventoryAllocation();
                        data.KAL = KAL;
                        data.UnitNo = setValue(sheet.GetRow(row).GetCell(27)).ToString();//before 25
                        data.SerialNo = setValue(sheet.GetRow(row).GetCell(28)).ToString();//before 26
                        data.PONumber = setValue(sheet.GetRow(row).GetCell(25)).ToString();//before 27
                        data.StoreID = getStoreID(setValue(sheet.GetRow(row).GetCell(29)).ToString());//before 28
                        data.CUSTOMER_ID = getCustomerID(setValue(sheet.GetRow(row).GetCell(30)).ToString());//before 29
                        data.Customer = setValue(sheet.GetRow(row).GetCell(30)).ToString();//before 29
                        try
                        {
                            if (!string.IsNullOrWhiteSpace(setValue(sheet.GetRow(row).GetCell(26)).ToString()))//before 30
                                data.OriginalSchedule = Convert.ToDateTime(setValue(sheet.GetRow(row).GetCell(26)).ToString());//before 30
                            else
                                throw new Exception("Error Cycle 5 [KAL : " + KAL + "]: Original Schedule is mandatory, on excel row : " + (row + 1).ToString());
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Error Cycle 5 : " + ex.Message + ", on excel row : " + (row + 1).ToString());
                        }
                        data.IsActive = false;
                        data.IsUsed = false;
                        data.Cycle = 5;
                        invallocation.Add(data);
                    }
                    else
                    {
                        data = new Data.Domain.InventoryAllocation();
                        data.KAL = KAL;
                        data.Cycle = 5;
                        invallocation.Add(data);
                    }
                }

            }
            catch (Exception ex)
            {
                invallocation = new List<Data.Domain.InventoryAllocation>();
                throw new Exception("Detail: Error Message read sheet Inventory Allocation : " + ex.Message);
            }
            return invallocation;
        }

        /// <summary>
        /// pengambilan data Inventory dari file excel.
        /// </summary>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        public List<Data.Domain.InventoryList> getInventory(string FilePath)
        {
            List<Data.Domain.InventoryList> inventoryList = new List<Data.Domain.InventoryList>();
            int errRow = 0;
            try
            {
                ISheet sheet = getSheet(FilePath, "INVENTORY");
                var _logImport = new Data.Domain.LogImport();
                var _file = new Data.Domain.DocumentUpload();

                for (int row = 2; row <= sheet.LastRowNum; row++)
                {
                    Data.Domain.InventoryList inv = new Data.Domain.InventoryList();
                    errRow = row;

                    if (string.IsNullOrWhiteSpace(setValue(sheet.GetRow(row).GetCell(1))))
                        throw new Exception("Detail: AlternetPartNumber can't be BLANK, on excel row : " + row.ToString());
                    inv.AlternetPartNumber = setValue(sheet.GetRow(row).GetCell(1)).ToString().Trim();

                    //inv.ApplicableModel = setValue(sheet.GetRow(row).GetCell(2)).ToString().Trim();
                    inv.Component = setValue(sheet.GetRow(row).GetCell(3)).ToString().Trim();

                    if (string.IsNullOrWhiteSpace(setValue(sheet.GetRow(row).GetCell(4))))
                        throw new Exception("Detail: Status can't be BLANK, on excel row : " + row.ToString());
                    inv.LastStatus = setValue(sheet.GetRow(row).GetCell(4)).ToString().Trim();

                    if (string.IsNullOrWhiteSpace(setValue(sheet.GetRow(row).GetCell(5))))
                        throw new Exception("Detail: Store Number can't be BLANK, on excel row : " + row.ToString());
                    inv.StoreNumber = setValue(sheet.GetRow(row).GetCell(5)).ToString().Trim();

                    if (string.IsNullOrWhiteSpace(setValue(sheet.GetRow(row).GetCell(6))))
                        throw new Exception("Detail: SOS can't be BLANK, on excel row : " + row.ToString());
                    inv.SOS = setValue(sheet.GetRow(row).GetCell(6)).ToString().Trim();

                    if (string.IsNullOrWhiteSpace(setValue(sheet.GetRow(row).GetCell(7))))
                        throw new Exception("Detail: KAL can't be BLANK, on excel row : " + row.ToString());
                    inv.KAL = setValue(sheet.GetRow(row).GetCell(7)).ToString().Trim();

                    inv.InventoryAllocation = new List<Data.Domain.InventoryAllocation>();

                    if (!string.IsNullOrEmpty(setValue(sheet.GetRow(row).GetCell(8)))) // Cycle 1
                    {
                        Data.Domain.InventoryAllocation data = new Data.Domain.InventoryAllocation();
                        data.KAL = inv.KAL;
                        data.UnitNo = setValue(sheet.GetRow(row).GetCell(8)).ToString();
                        data.SerialNo = setValue(sheet.GetRow(row).GetCell(9)).ToString();
                        data.PONumber = setValue(sheet.GetRow(row).GetCell(10)).ToString();
                        data.StoreID = getStoreID(setValue(sheet.GetRow(row).GetCell(11)).ToString());
                        data.Customer = setValue(sheet.GetRow(row).GetCell(12)).ToString();
                        data.OriginalSchedule = Convert.ToDateTime(setValue(sheet.GetRow(row).GetCell(13)).ToString());
                        data.IsActive = true;
                        data.IsUsed = false;
                        data.Cycle = 1;
                        inv.InventoryAllocation.Add(data);
                    }
                    if (!string.IsNullOrEmpty(setValue(sheet.GetRow(row).GetCell(14)))) // Cycle 2
                    {
                        Data.Domain.InventoryAllocation data = new Data.Domain.InventoryAllocation();
                        data.KAL = inv.KAL;
                        data.UnitNo = setValue(sheet.GetRow(row).GetCell(14)).ToString();
                        data.SerialNo = setValue(sheet.GetRow(row).GetCell(15)).ToString();
                        data.PONumber = setValue(sheet.GetRow(row).GetCell(16)).ToString();
                        data.StoreID = getStoreID(setValue(sheet.GetRow(row).GetCell(17)).ToString());
                        data.Customer = setValue(sheet.GetRow(row).GetCell(18)).ToString();
                        data.OriginalSchedule = Convert.ToDateTime(setValue(sheet.GetRow(row).GetCell(19)).ToString());
                        data.IsActive = false;
                        data.IsUsed = false;
                        data.Cycle = 2;
                        inv.InventoryAllocation.Add(data);
                    }
                    if (!string.IsNullOrEmpty(setValue(sheet.GetRow(row).GetCell(20)))) // Cycle 3
                    {
                        Data.Domain.InventoryAllocation data = new Data.Domain.InventoryAllocation();
                        data.KAL = inv.KAL;
                        data.UnitNo = setValue(sheet.GetRow(row).GetCell(20)).ToString();
                        data.SerialNo = setValue(sheet.GetRow(row).GetCell(21)).ToString();
                        data.PONumber = setValue(sheet.GetRow(row).GetCell(22)).ToString();
                        data.StoreID = getStoreID(setValue(sheet.GetRow(row).GetCell(23)).ToString());
                        data.Customer = setValue(sheet.GetRow(row).GetCell(24)).ToString();
                        data.OriginalSchedule = Convert.ToDateTime(setValue(sheet.GetRow(row).GetCell(25)).ToString());
                        data.IsActive = false;
                        data.IsUsed = false;
                        data.Cycle = 3;
                        inv.InventoryAllocation.Add(data);
                    }
                    if (!string.IsNullOrEmpty(setValue(sheet.GetRow(row).GetCell(26)))) // Cycle 4
                    {
                        Data.Domain.InventoryAllocation data = new Data.Domain.InventoryAllocation();
                        data.KAL = inv.KAL;
                        data.UnitNo = setValue(sheet.GetRow(row).GetCell(26)).ToString();
                        data.SerialNo = setValue(sheet.GetRow(row).GetCell(27)).ToString();
                        data.PONumber = setValue(sheet.GetRow(row).GetCell(28)).ToString();
                        data.StoreID = getStoreID(setValue(sheet.GetRow(row).GetCell(29)).ToString());
                        data.Customer = setValue(sheet.GetRow(row).GetCell(30)).ToString();
                        data.OriginalSchedule = Convert.ToDateTime(setValue(sheet.GetRow(row).GetCell(31)).ToString());
                        data.IsActive = false;
                        data.IsUsed = false;
                        data.Cycle = 4;
                        inv.InventoryAllocation.Add(data);
                    }
                    if (!string.IsNullOrEmpty(setValue(sheet.GetRow(row).GetCell(32)))) // Cycle 5
                    {
                        Data.Domain.InventoryAllocation data = new Data.Domain.InventoryAllocation();
                        data.KAL = inv.KAL;
                        data.UnitNo = setValue(sheet.GetRow(row).GetCell(32)).ToString();
                        data.SerialNo = setValue(sheet.GetRow(row).GetCell(33)).ToString();
                        data.PONumber = setValue(sheet.GetRow(row).GetCell(34)).ToString();
                        data.StoreID = getStoreID(setValue(sheet.GetRow(row).GetCell(35)).ToString());
                        data.Customer = setValue(sheet.GetRow(row).GetCell(36)).ToString();
                        data.OriginalSchedule = Convert.ToDateTime(setValue(sheet.GetRow(row).GetCell(37)).ToString());
                        data.IsActive = false;
                        data.IsUsed = false;
                        data.Cycle = 5;
                        inv.InventoryAllocation.Add(data);
                    }

                    inventoryList.Add(inv);
                }
            }
            catch (Exception ex)
            {
                inventoryList = new List<Data.Domain.InventoryList>();
                throw new Exception("Detail: Error excel row " + errRow.ToString() + ", Error Message read sheet Inventory Allocation : " + ex.Message);
            }
            return inventoryList;
        }

        private string validateuploadedit(string status, int row, Data.Domain.InventoryList item)
        {

            if (string.IsNullOrEmpty(item.AlternetPartNumber))
            {
                return "Alt Part no : not found in inventory list, on excel row : " + (row + 1).ToString();
            }

            if (string.IsNullOrEmpty(item.StoreNumber))
            {
                return "Store Number : not found in inventory list, on excel row : " + (row + 1).ToString();
            }

            if (string.IsNullOrEmpty(item.SOS))
            {
                return "SOS : not found in inventory list, on excel row : " + (row + 1).ToString();
            }

            if (string.IsNullOrEmpty(item.LastStatus))
            {
                return "Status : not found in inventory list, on excel row : " + (row + 1).ToString();
            }

            if (status == "OH")
            {
                if (string.IsNullOrEmpty(item.RGNumber))
                {
                    return "RG Number : not found in inventory list, on excel row : " + (row + 1).ToString();
                }
            }
            else if (status == "WOC")
            {
                if (string.IsNullOrEmpty(item.DocSales))
                {
                    return "Doc Sales : not found in inventory list, on excel row : " + (row + 1).ToString();
                }

                if (string.IsNullOrEmpty(item.NewWO6F))
                {
                    if (item.SOS == "500")
                    {
                        return "Workorder : not found in inventory list, on excel row : " + (row + 1).ToString();
                    }
                }
            }
            else if (status == "TTC")
            {
                if (string.IsNullOrEmpty(item.DocSales))
                {
                    return "Doc Sales : not found in inventory list, on excel row : " + (row + 1).ToString();
                }

                if (string.IsNullOrEmpty(item.DocWCSL))
                {
                    return "Doc WCSL : not found in inventory list, on excel row : " + (row + 1).ToString();
                }
            }
            else if (status == "WIP")
            {
                if (string.IsNullOrEmpty(item.DocSales))
                {
                    return "Doc Sales : not found in inventory list, on excel row : " + (row + 1).ToString();
                }

                if (item.SOS == "500")
                {
                    if (string.IsNullOrEmpty(item.NewWO6F))
                    {
                        return "Workorder : not found in inventory list, on excel row : " + (row + 1).ToString();
                    }
                }

                if (item.SOS == "800")
                {
                    if (string.IsNullOrEmpty(item.DocWCSL))
                    {
                        return "Doc WCSL : not found in inventory list, on excel row : " + (row + 1).ToString();
                    }
                }

                if (string.IsNullOrEmpty(item.MO))
                {
                    return "Doc MO : not found in inventory list, on excel row : " + (row + 1).ToString();
                }
            }
            else if (status == "ST")
            {
                if (string.IsNullOrEmpty(item.DocTransfer))
                {
                    return "Doc Transfer : not found in inventory list, on excel row : " + (row + 1).ToString();
                }
            }

            return "";
        }

        /// <summary>
        /// pengambilan data Part Info Detail dari file excel.
        /// </summary>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        public List<Data.Domain.InventoryList> getInventoryForEdit(string FilePath)
        {
            List<Data.Domain.InventoryList> CompDetail = new List<Data.Domain.InventoryList>();
            string KAL = "";
            try
            {
                ISheet sheet = getSheet(FilePath, "Sheet1");
                var _logImport = new Data.Domain.LogImport();
                var _file = new Data.Domain.DocumentUpload();

                for (int row = 1; row <= sheet.LastRowNum; row++)
                {
                    Data.Domain.InventoryList data = Service.CAT.InventoryList.GetDataByKAL(sheet.GetRow(row).GetCell(0).ToString().Trim());

                    if (data == null)
                    {
                        throw new Exception("KAL : " + sheet.GetRow(row).GetCell(0).ToString().Trim() + " not found in inventory list, on excel row : " + (row + 1).ToString());
                    }

                    data.KAL = setValue(sheet.GetRow(row).GetCell(0)).ToString().Trim();
                    KAL = setValue(sheet.GetRow(row).GetCell(0)).ToString().Trim();
                    data.AlternetPartNumber = setValue(sheet.GetRow(row).GetCell(1)).ToString().Trim();
                    data.LastStatus = setValue(sheet.GetRow(row).GetCell(2)).ToString();
                    data.StoreNumber = setValue(sheet.GetRow(row).GetCell(3)).ToString();
                    data.SOS = setValue(sheet.GetRow(row).GetCell(4)).ToString();
                    data.Surplus = setValue(sheet.GetRow(row).GetCell(5)).ToString();
                    data.UnitNumber = setValue(sheet.GetRow(row).GetCell(6)).ToString();
                    data.EquipmentNumber = setValue(sheet.GetRow(row).GetCell(7)).ToString();
                    data.CUSTOMER_ID = setValue(sheet.GetRow(row).GetCell(8)).ToString();
                    if (!string.IsNullOrWhiteSpace(setValue(sheet.GetRow(row).GetCell(9)).ToString()))
                        data.DocDate = Convert.ToDateTime(setValue(sheet.GetRow(row).GetCell(9)).ToString());
                    else
                        data.DocDate = null;
                    if (!string.IsNullOrWhiteSpace(setValue(sheet.GetRow(row).GetCell(10)).ToString()))
                        data.DocDateTransfer = Convert.ToDateTime(setValue(sheet.GetRow(row).GetCell(10)).ToString());
                    else
                        data.DocDateTransfer = null;

                    data.DocTransfer = setValue(sheet.GetRow(row).GetCell(11)).ToString();
                    data.NewWO6F = setValue(sheet.GetRow(row).GetCell(12)).ToString();
                    data.MO = setValue(sheet.GetRow(row).GetCell(13)).ToString();
                    data.DocSales = setValue(sheet.GetRow(row).GetCell(14)).ToString();
                    data.DocReturn = setValue(sheet.GetRow(row).GetCell(15)).ToString();
                    data.DocWCSL = setValue(sheet.GetRow(row).GetCell(16)).ToString();
                    data.RGNumber = data.DocTransfer;

                    string val = validateuploadedit(data.LastStatus, row, data);
                    if (!string.IsNullOrEmpty(val))
                    {
                        throw new Exception(val);
                    }

                    CompDetail.Add(data);
                }
            }
            catch (Exception ex)
            {
                CompDetail = new List<Data.Domain.InventoryList>();
                if (!string.IsNullOrWhiteSpace(KAL))
                    throw new Exception("Detail: Error Message read sheet Component Detail [On KAL : " + KAL + "] : " + ex.Message);
                else
                    throw new Exception("Detail: Error Message read sheet Component Detail : " + ex.Message);
            }
            return CompDetail;
        }

        private string ValidationStatus(Data.Domain.InventoryList item)
        {

            if (!string.IsNullOrEmpty(item.LastStatus))
            {// && (new [] {"OH","WOC",""}).Contains(item.LastStatus)
                if (item.AlternetPartNumber == "")
                {
                    return "Detail: Error Message read sheet Alternet Part Number [On KAL : " + item.KAL + "] : field is empty.";
                }

                if (item.StoreNumber == "")
                {
                    return "Detail: Error Message read sheet Store Number [On KAL : " + item.KAL + "] : field is empty.";
                }

                if (item.SOS == "")
                {
                    return "Detail: Error Message read sheet SOS [On KAL : " + item.KAL + "] : field is empty.";
                }

                if (item.DocTransfer == "" && (new[] { "ST" }).Contains(item.LastStatus))
                {
                    return "Detail: Error Message read sheet Doc Transfer [On KAL : " + item.KAL + "] : field is empty.";
                }

                if (item.NewWO6F == "" && (new[] { "WOC", "WIP", "SQ", "BER", "JC" }).Contains(item.LastStatus) && item.SOS != "800")
                {
                    return "Detail: Error Message read sheet Workorder [On KAL : " + item.KAL + "] : field is empty.";
                }

                if (item.MO == "" && (new[] { "TCC", "WIP", "SQ", "BER", "JC" }).Contains(item.LastStatus) && item.SOS == "800")
                {
                    return "Detail: Error Message read sheet MO [On KAL : " + item.KAL + "] : field is empty.";
                }

                if (item.DocSales == "" && (new[] { "WOC", "TCC", "WIP", "SQ", "BER", "JC" }).Contains(item.LastStatus))
                {
                    return "Detail: Error Message read sheet Doc Sale [On KAL : " + item.KAL + "] : field is empty.";
                }

                if (item.DocSales == "" && (new[] { "WOC", "TCC", "WIP", "SQ", "BER", "JC" }).Contains(item.LastStatus))
                {
                    return "Detail: Error Message read sheet Doc Sale [On KAL : " + item.KAL + "] : field is empty.";
                }

                if (item.DocReturn == "" && (new[] { "TCC", "WIP", "SQ", "BER", "JC" }).Contains(item.LastStatus) && item.SOS == "800")
                {
                    return "Detail: Error Message read sheet Doc Return [On KAL : " + item.KAL + "] : field is empty.";
                }

                if (item.DocWCSL == "" && (new[] { "TCC", "WIP", "SQ", "BER", "JC" }).Contains(item.LastStatus) && item.SOS == "800")
                {
                    return "Detail: Error Message read sheet Doc WCSL [On KAL : " + item.KAL + "] : field is empty.";
                }
                return "";
            }
            else
            {
                return "";
            }


        }


        /// <summary>
        /// cek allocation punya allocation atau tidak.
        /// </summary>
        /// <param name="KAL"></param>
        /// <returns></returns>
        private bool isValidAllocation(string KAL)
        {
            using (var db = new Data.EfDbContext())
            {
                var inventorylist = db.InventoryList.Where(i => i.KAL.Trim().ToUpper().Equals(KAL.Trim().ToUpper())).FirstOrDefault();

                return inventorylist != null;
            }
        }

        /// <summary>
        /// generate value sesuai object cell.
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        private static string setValue(ICell cell)
        {
            DataFormatter dataFormatter = new DataFormatter(CultureInfo.CurrentCulture);
            return dataFormatter.FormatCellValue(cell);
        }

        /// <summary>
        /// Pengambilan value StoreID berdasrkan StoreName.
        /// </summary>
        /// <param name="StoreName"></param>
        /// <returns></returns>
        public int getStoreID(string StoreName)
        {
            using (var db = new Data.EfDbContext())
            {
                var data = db.Stores.Where(w => w.Name.Trim().ToUpper() == StoreName.Trim().ToUpper()).FirstOrDefault();
                if (data != null) return data.StoreID;

                return 0;
            }
        }

        /// <summary>
        /// Pengambilan value CUSTOMER_ID berdasrkan CustName.
        /// </summary>
        /// <param name="CustName"></param>
        /// <returns></returns>
        public string getCustomerID(string CustName)
        {
            using (var db = new Data.EfDbContext())
            {
                var data = db.MasterCustomer.Where(w => w.CUSTOMERNAME.Trim().ToUpper() == CustName.Trim().ToUpper()).FirstOrDefault();
                if (data != null) return data.CUSTOMER_ID.ToString();

                return "0";
            }
        }

        /// <summary>
        /// insert update delete data part info detail.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="dml"></param>
        /// <returns></returns>
        public static int crudPartInfoDetail(Data.Domain.PartInfoDetail item, string dml)
        {
            if (dml == "I")
            {
                item.EntryDate = DateTime.Now;
                item.EntryBy = Domain.SiteConfiguration.UserName;
            }

            _cacheManager.Remove(cacheName);

            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                return db.CreateRepository<Data.Domain.PartInfoDetail>().CRUD(dml, item);
            }
        }

        /// <summary>
        /// insert update delete data part info detail.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="dml"></param>
        /// <returns></returns>
        public static int UpdateInventoryEdit(Data.Domain.InventoryList item, string dml)
        {
            if (dml == "I")
            {
                item.UpdateDate = DateTime.Now;
            }

            _cacheManager.Remove(cacheName);

            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                return db.CreateRepository<Data.Domain.InventoryList>().CRUD(dml, item);
            }
        }

        /// <summary>
        /// Update data Inventory menggunakan Store Procedure By ID.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static int SP_UpdateDataInventoryFromExcel(Data.Domain.InventoryList item, string dml)
        {
            try
            {
                using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
                {
                    db.DbContext.Database.CommandTimeout = 600;
                    List<SqlParameter> parameterList = new List<SqlParameter>();
                    parameterList.Add(new SqlParameter("@InventoryID", item.ID));
                    parameterList.Add(new SqlParameter("@KAL", item.KAL));
                    parameterList.Add(new SqlParameter("@AlternetPartNumber", item.AlternetPartNumber));
                    parameterList.Add(new SqlParameter("@RGNumber", item.RGNumber));
                    parameterList.Add(new SqlParameter("@DocWCSL", item.DocWCSL));
                    parameterList.Add(new SqlParameter("@UnitNumber", item.UnitNumber));
                    parameterList.Add(new SqlParameter("@TUID", ""));
                    parameterList.Add(new SqlParameter("@DocSales", item.DocSales));
                    parameterList.Add(new SqlParameter("@LastStatus", item.LastStatus));
                    if (item.DocDate == null)
                    {
                        parameterList.Add(new SqlParameter("@DocDate", DBNull.Value));
                    }
                    else
                    {
                        parameterList.Add(new SqlParameter("@DocDate", item.DocDate));
                    }

                    parameterList.Add(new SqlParameter("@EquipmentNumber", item.EquipmentNumber));
                    parameterList.Add(new SqlParameter("@MO", item.MO));
                    parameterList.Add(new SqlParameter("@DocReturn", item.DocReturn));
                    parameterList.Add(new SqlParameter("@NewWO6F", item.NewWO6F));
                    parameterList.Add(new SqlParameter("@Customer_ID", item.CUSTOMER_ID));
                    parameterList.Add(new SqlParameter("@DocTransfer", item.DocTransfer));
                    if (item.DocDateTransfer == null)
                    {
                        parameterList.Add(new SqlParameter("@DocDateTransfer", DBNull.Value));
                    }
                    else
                    {
                        parameterList.Add(new SqlParameter("@DocDateTransfer", item.DocDateTransfer));
                    }

                    parameterList.Add(new SqlParameter("@SOS", item.SOS));
                    parameterList.Add(new SqlParameter("@Store", item.StoreNumber));

                    SqlParameter[] parameters = parameterList.ToArray();

                    db.DbContext.Database.ExecuteSqlCommand(@" exec [cat].[UpdateInventoryDatafrombtn] @InventoryID,@KAL, @AlternetPartNumber , @RGNumber ,
	                                                            @DocWCSL , @UnitNumber , @TUID , @DocSales , @LastStatus ,
	                                                            @DocDate , @EquipmentNumber , @MO , @DocReturn , @NewWO6F ,
	                                                            @Customer_ID , @DocTransfer , @DocDateTransfer , @SOS , @Store ", parameters);

                    return 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// insert update delete data Inventory Allocation.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static int crudInventoryAllocation(Data.Domain.InventoryAllocation item)
        {
            var data = Service.CAT.InventoryAllocation.GetByKAL(item.KAL, item.Cycle);
            if (data != null)
            {
                if (item.UnitNo == null)
                {
                    item.ID = data.ID;
                    _cacheManager.Remove(cacheName);

                    using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
                    {
                        return db.CreateRepository<Data.Domain.InventoryAllocation>().CRUD("D",item);
                    }
                }
                else
                {
                    item.ID = data.ID;
                    item.CreatedBy = data.CreatedBy;
                    item.CreatedDate = data.CreatedDate;
                    item.UpdatedDate = item.UpdatedDate = DateTime.Now;
                    item.UpdatedBy = item.UpdatedBy = Domain.SiteConfiguration.UserName;

                    _cacheManager.Remove(cacheName);

                    using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
                    {
                        return db.CreateRepository<Data.Domain.InventoryAllocation>().CRUD("U", item);
                    }
                }
            }
            else
            {
                if (item.UnitNo == null)
                {
                    return 0;
                }
                else
                {
                    item.CreatedDate = item.UpdatedDate = DateTime.Now;
                    item.CreatedBy = item.UpdatedBy = Domain.SiteConfiguration.UserName;
                    item.UpdatedDate = item.UpdatedDate = DateTime.Now;
                    item.UpdatedBy = item.UpdatedBy = Domain.SiteConfiguration.UserName;

                    _cacheManager.Remove(cacheName);

                    using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
                    {
                        return db.CreateRepository<Data.Domain.InventoryAllocation>().CRUD("I", item);
                    }
                } 
            }
        }

        /// <summary>
        /// haspus (Truncate) data Part Info Detail.
        /// </summary>
        public void ClearPartInfoDetail()
        {
            //using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            //{
            //    IEnumerable<Data.Domain.PartInfoDetail> partinfodetail = db.CreateRepository<Data.Domain.PartInfoDetail>().Table.Select(e => e).ToList();
            //    if (partinfodetail.Count() > 0)
            //        db.DbContext.Database.ExecuteSqlCommand("TRUNCATE TABLE CAT.PNComponentDetail");
            //}
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                db.DbContext.Database.ExecuteSqlCommand("TRUNCATE TABLE CAT.PNComponentDetail");
            }
        }

        /// <summary>
        /// haspus (Truncate) data Inventory Allocation.
        /// </summary>
        public void ClearInventoryAllocation(Data.Domain.InventoryAllocation item)
        {

            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                db.DbContext.Database.ExecuteSqlCommand("delete cat.InventoryAllocation where KAL = '" + item.KAL + "' and IsUsed <> 1 ");
            }
        }
    }
}
