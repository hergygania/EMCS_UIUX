#region License

// /****************************** Module Header ******************************\
// Module Name:  Library.cs
// Project:    Pis-WindowService.Library
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
using System.Configuration;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using CsvHelper;
using Microsoft.Exchange.WebServices.Data;
using WindowService.Library.Model;
using NPOI;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using System.Data.SqlClient;
using System.Web;

namespace WindowService.Library
{
    public static class Library
    {
        /// <summary>
        ///     Read email from exchange
        /// </summary>
        /// 
        public static void ReadEmailExchange()
        {
            CreateLog("Start Grab Email");
            try
            {


                string userName = ConfigurationManager.AppSettings["userName"];
                string userEmail = ConfigurationManager.AppSettings["userEmail"];
                string password = ConfigurationManager.AppSettings["password"];
                string folder = ConfigurationManager.AppSettings["ImagesData.document"] + "";
                int emailItem = int.Parse(ConfigurationManager.AppSettings["EmailItem"]);
                var service = new ExchangeService(ExchangeVersion.Exchange2007_SP1);
                service.Credentials = new NetworkCredential(userName, password, "JKT");
                service.Url = new Uri(ConfigurationManager.AppSettings["Uri"].ToString());
                //service.Url = new Uri("https://mail.tmt.co.id/ews/Exchange.asmx");

                //try
                //{
                //    //service.TraceEnabled = true;
                //    //service.AutodiscoverUrl(userEmail, RedirectionUrlValidationCallback);

                //}
                //catch(Exception ex)
                //{
                //    throw ex;
                //}

                var searchFilterCollection = new List<SearchFilter>();

                //filter only unread email
                searchFilterCollection.Add(new SearchFilter.SearchFilterCollection(LogicalOperator.And,
                    new SearchFilter.IsEqualTo(EmailMessageSchema.IsRead, false)));


                // Create the search filter.
                SearchFilter searchFilter = new SearchFilter.SearchFilterCollection(LogicalOperator.Or,
                    searchFilterCollection.ToArray());

                var inbox = new FolderId(WellKnownFolderName.Inbox);
                var iv = new ItemView(emailItem);

                IOrderedEnumerable<Item> items = null;
                try
                {
                    items = service.FindItems(inbox, searchFilter, iv).OrderBy(a => a.DateTimeReceived);
                }
                catch (Exception ex)
                {
                    CreateLog(ex.Message);
                }

                foreach (EmailMessage msg in items)
                {
                    if (!msg.IsRead)
                    {
                        CreateLog("Read Email: " + msg.Subject);
                        EmailMessage message = EmailMessage.Bind(service, msg.Id,
                            new PropertySet(BasePropertySet.IdOnly, ItemSchema.Attachments, ItemSchema.HasAttachments));

                        foreach (Attachment attachment in message.Attachments)
                        {
                            if (attachment is FileAttachment)
                            {
                                try
                                {
                                    var fileAttachment = attachment as FileAttachment;
                                    string path = folder + DateTime.Now.ToString("yyMMddHHss") + "_" + attachment.Name;
                                    bool exists = Directory.Exists(folder);

                                    if (!exists)
                                        Directory.CreateDirectory(folder);
                                    fileAttachment.Load(path);

                                    if (msg.Subject.ToUpper() == "H5LA9926")
                                    {
                                        SavePartsOrderDetail(path, msg.DateTimeReceived);
                                        DeleteFile(path);
                                    }
                                    else if (msg.Subject.ToUpper() == "H5LA9925")
                                    {
                                        SavePartOrderCase(path, msg.DateTimeReceived);
                                        DeleteFile(path);
                                    }
                                    else if (msg.Subject.ToUpper() == "H5LA9924")
                                    {
                                        SavePartOrder(path, msg.DateTimeReceived);
                                        DeleteFile(path);
                                    }
                                    else if (msg.Subject.ToUpper() == "BOMIT PTTU")
                                    {
                                        SaveBomit(path, msg.DateTimeReceived);
                                        DeleteFile(path);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    CreateLog(ex.InnerException.InnerException.Message);
                                    CreateLog(ex.Message);
                                }
                            }
                        }
                        msg.IsRead = true;
                        msg.Update(ConflictResolutionMode.AlwaysOverwrite);


                    }
                }
                CreateLog("Start Merge Part Order");
                MergePartsOrder();
                UpdatePartOrder();
                MergePartsOrderCase();
                MergePartsOrderDetail();
                MergeBomit();
            }
            catch (NullReferenceException e)
            {
                CreateLog(e.Message);
                throw;
            }
        }

        //private static void SavePartOrder(string path, DateTime dateTimeReceived)
        //{
        //    List<ShippingInstruction> shippingInstructions;
        //    using (var context = new PisContext())
        //    {
        //        shippingInstructions = context.ShippingInstructions.ToList();
        //    }
        //    var partOrders = new List<PartsOrderTMP>();
        //    using (var sr = new StreamReader(path))
        //    {
        //        var rs = new CsvReader(sr);
        //        while (rs.Read())
        //        {
        //            var partItem = new PartsOrderTMP();
        //            partItem.InvoiceNo = RemoveEndString(rs.GetField(0).Replace("\0", string.Empty));
        //            partItem.InvoiceDate = DateTime.Parse(RemoveEndString(rs.GetField(1).Replace("\0", string.Empty)));
        //            partItem.JCode = RemoveEndString(rs.GetField(2).Replace("\0", string.Empty));
        //            string instruction = RemoveEndString(rs.GetField(3).Replace(" \0", string.Empty));
        //            partItem.ShippingInstructionID =
        //                shippingInstructions.Where(
        //                    a => a.Description == instruction)
        //                    .Select(a => a.ShippingInstructionID)
        //                    .FirstOrDefault();
        //            partItem.TotalAmount =
        //                decimal.Parse(RemoveEndString(rs.GetField(4).Replace("\0", string.Empty).Trim()));
        //            partItem.TotalFOB = decimal.Parse(RemoveEndString(rs.GetField(5).Replace("\0", string.Empty).Trim()));
        //            partItem.IsHazardous =
        //                RemoveEndString(rs.GetField(6).Replace("\0", string.Empty)).Trim().Contains("HAZARD");
        //            partItem.ServiceCharges = decimal.Parse(RemoveEndString(rs.GetField(7).Replace("\0", string.Empty)));
        //            partItem.CoreDeposit = decimal.Parse(RemoveEndString(rs.GetField(8).Replace("\0", string.Empty)));
        //            partItem.OtherCharges = decimal.Parse(RemoveEndString(rs.GetField(9).Replace("\0", string.Empty)));
        //            partItem.FreightCharges = decimal.Parse(RemoveEndString(rs.GetField(10).Replace("\0", string.Empty)));
        //            partItem.ShippingIDASN = RemoveEndString(rs.GetField(11).Replace("\0", string.Empty));
        //            partItem.AgreementType = RemoveEndString(rs.GetField(12).Replace("\0", string.Empty));
        //            partItem.EmailDate = dateTimeReceived;
        //            partItem.EntryDate = DateTime.Now;
        //            partItem.EntryBy = "PisEmailService";
        //            partItem.ModifiedDate = DateTime.Now;
        //            partItem.ModifiedBy = "PisEmailService";
        //            partOrders.Add(partItem);
        //        }
        //    }


        //    try
        //    {
        //        using (var context = new PisContext())
        //        {
        //            context.PartsOrderTMPs.AddRange(partOrders.Distinct());
        //            context.SaveChanges();
        //        }
        //    }
        //    catch (DbEntityValidationException dbEx)
        //    {
        //        foreach (DbEntityValidationResult validationErrors in dbEx.EntityValidationErrors)
        //        {
        //            foreach (DbValidationError validationError in validationErrors.ValidationErrors)
        //            {
        //                CreateLog(String.Format("Property: {0} Error: {1}",
        //                    validationError.PropertyName,
        //                    validationError.ErrorMessage));
        //            }
        //        }
        //    }
        //}

        ////update to excel
        private static void SavePartOrder(string path, DateTime dateTimeReceived)
        {
            try
            {
                List<ShippingInstruction> shippingInstructions;
                HSSFWorkbook hssfwb;
                XSSFWorkbook xssfwb;
                ISheet sheet;
                string fileExt = Path.GetExtension(path).ToLower();
                using (FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    if (POIXMLDocument.HasOOXMLHeader(new BufferedStream(file)))
                    {
                        xssfwb = new XSSFWorkbook(file);
                        sheet = xssfwb.GetSheetAt(0);
                    }
                    else
                    {
                        hssfwb = new HSSFWorkbook(file);
                        sheet = hssfwb.GetSheetAt(0);
                    }
                }

                using (var context = new PisContext())
                {
                    shippingInstructions = context.ShippingInstructions.ToList();
                }

                var partOrders = new List<PartsOrderTMP>();

                int i = 1;
                while (sheet.GetRow(i) != null)
                {
                    if (sheet.GetRow(i).GetCell(0) != null)
                    {
                        try
                        {
                            var partItem = new PartsOrderTMP();
                            partItem.InvoiceNo = sheet.GetRow(i).GetCell(0).ToString().Trim();
                            partItem.InvoiceDate = HSSFDateUtil.GetJavaDate(sheet.GetRow(i).GetCell(1).NumericCellValue);
                            partItem.JCode = sheet.GetRow(i).GetCell(2).ToString().Trim();
                            string instruction = sheet.GetRow(i).GetCell(3).ToString().Trim();
                            partItem.ShippingInstructionID =
                                shippingInstructions.Where(
                                    a => a.Description == instruction)
                                    .Select(a => a.ShippingInstructionID)
                                    .FirstOrDefault();
                            partItem.TotalAmount = sheet.GetRow(i).GetCell(4).ToString().Trim() != "" ?
                                decimal.Parse(sheet.GetRow(i).GetCell(4).ToString().Trim()) : 0;
                            partItem.TotalFOB = sheet.GetRow(i).GetCell(5).ToString().Trim() != "" ?
                                decimal.Parse(sheet.GetRow(i).GetCell(5).ToString().Trim()) : 0;
                            partItem.IsHazardous = sheet.GetRow(i).GetCell(6).ToString().Trim().Contains("HAZARD");
                            partItem.ServiceCharges = sheet.GetRow(i).GetCell(7).ToString().Trim() != "" ?
                                decimal.Parse(sheet.GetRow(i).GetCell(7).ToString().Trim()) : 0;
                            partItem.CoreDeposit = sheet.GetRow(i).GetCell(8).ToString().Trim() != "" ?
                                decimal.Parse(sheet.GetRow(i).GetCell(8).ToString().Trim()) : 0;
                            partItem.OtherCharges = sheet.GetRow(i).GetCell(9).ToString().Trim() != "" ?
                                decimal.Parse(sheet.GetRow(i).GetCell(9).ToString().Trim()) : 0;
                            partItem.FreightCharges = sheet.GetRow(i).GetCell(10).ToString().Trim() != "" ?
                                decimal.Parse(sheet.GetRow(i).GetCell(10).ToString().Trim()) : 0;
                            partItem.ShippingIDASN = sheet.GetRow(i).GetCell(11).ToString().Trim();
                            partItem.AgreementType = sheet.GetRow(i).GetCell(12).ToString().Trim();
                            partItem.EmailDate = dateTimeReceived;
                            partItem.EntryDate = DateTime.Now;
                            partItem.EntryBy = "PisEmailService";
                            partItem.ModifiedDate = DateTime.Now;
                            partItem.ModifiedBy = "PisEmailService";
                            partOrders.Add(partItem);
                        }
                        catch (Exception ex)
                        {
                            CreateLog(String.Format("Property: {0} Error: {1}",
                                ex.Message,
                                sheet.GetRow(i)));
                        }
                        i++;
                    }
                    else
                    {
                        break;
                    }
                }
                try
                {
                    using (var context = new PisContext())
                    {
                        context.PartsOrderTMPs.AddRange(partOrders.Distinct());
                        context.SaveChanges();
                    }
                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (DbEntityValidationResult validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (DbValidationError validationError in validationErrors.ValidationErrors)
                        {
                            CreateLog(String.Format("Property: {0} Error: {1}",
                                validationError.PropertyName,
                                validationError.ErrorMessage));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CreateLog(ex.InnerException.InnerException.Message);
                CreateLog(ex.Message);
            }
        }


        private static void SaveBomit(string path, DateTime dateTimeReceived)
        {
            //List<ShippingInstruction> shippingInstructions;
            //using (var context = new PisContext())
            //{
            //    shippingInstructions = context.ShippingInstructions.ToList();
            //}
            var bomittmps = new List<BOMITTMP>();
            using (var sr = new StreamReader(path))
            {
                var rs = new CsvReader(sr);
                while (rs.Read())
                {
                    try
                    {
                        var bomittmp = new BOMITTMP();
                        bomittmp.JCode = RemoveEndString(rs.GetField(0).Replace("\0", string.Empty));
                        bomittmp.PrimPSO = RemoveEndString(rs.GetField(1).Replace("\0", string.Empty));
                        bomittmp.OrderReference = RemoveEndString(rs.GetField(2).Replace("\0", string.Empty));
                        bomittmp.FMS = RemoveEndString(rs.GetField(3).Replace("\0", string.Empty));
                        bomittmp.PartsNumber = RemoveEndString(rs.GetField(4).Replace("\0", string.Empty).Trim());
                        bomittmp.PartsName = RemoveEndString(rs.GetField(5).Replace("\0", string.Empty).Trim());
                        bomittmp.Pending = int.Parse(RemoveEndString(rs.GetField(6).Replace("\0", string.Empty)).Trim());
                        bomittmp.Class = RemoveEndString(rs.GetField(7).Replace("\0", string.Empty));
                        bomittmp.TransportedThrough = RemoveEndString(rs.GetField(8).Replace("\0", string.Empty));
                        bomittmp.OEDate =
                            DateTime.ParseExact(RemoveEndString(rs.GetField(9).Replace("\0", string.Empty)), "ddMMMyy",
                                null);
                        bomittmp.TotalMIT = int.Parse(RemoveEndString(rs.GetField(10).Replace("\0", string.Empty)));
                        bomittmp.NextReceiptQty = (RemoveEndString(rs.GetField(11).Replace("\0", string.Empty)) != "")
                            ? int.Parse(RemoveEndString(rs.GetField(11).Replace("\0", string.Empty)))
                            : (int?)null;
                        bomittmp.NextReceiptDate = (RemoveEndString(rs.GetField(12).Replace("\0", string.Empty)) != "")
                            ? DateTime.Parse(RemoveEndString(rs.GetField(12).Replace("\0", string.Empty)))
                            : (DateTime?)null;
                        bomittmp.TotalBO = int.Parse(RemoveEndString(rs.GetField(13).Replace("\0", string.Empty)));
                        bomittmp.FRZ = RemoveEndString(rs.GetField(14).Replace("\0", string.Empty));
                        bomittmp.UnitWeight = decimal.Parse(
                            RemoveEndString(rs.GetField(15).Replace("\0", string.Empty)), NumberStyles.Float);
                        bomittmp.EXT_WT = decimal.Parse(RemoveEndString(rs.GetField(16).Replace("\0", string.Empty)),
                            NumberStyles.Float);
                        bomittmp.UNIT_DN = decimal.Parse(RemoveEndString(rs.GetField(17).Replace("\0", string.Empty)),
                            NumberStyles.Float);
                        bomittmp.EXT_DN = decimal.Parse(RemoveEndString(rs.GetField(18).Replace("\0", string.Empty)),
                            NumberStyles.Float);
                        bomittmp.AgreementType = RemoveEndString(rs.GetField(19).Replace("\0", string.Empty));
                        bomittmp.BO_AGE = int.Parse(RemoveEndString(rs.GetField(20).Replace("\0", string.Empty)));
                        bomittmp.FU_DT_ORD = RemoveEndString(rs.GetField(21).Replace("\0", string.Empty));
                        bomittmp.DueDate = (RemoveEndString(rs.GetField(22).Replace("\0", string.Empty)) != "")
                            ? DateTime.Parse(RemoveEndString(rs.GetField(22).Replace("\0", string.Empty)))
                            : (DateTime?)null;
                        bomittmp.EmailDate = dateTimeReceived;
                        bomittmp.EntryDate = DateTime.Now;
                        bomittmp.EntryBy = "PisEmailService";
                        bomittmp.ModifiedDate = DateTime.Now;
                        bomittmp.ModifiedBy = "PisEmailService";
                        bomittmps.Add(bomittmp);
                    }
                    catch (Exception ex)
                    {
                        CreateLog(String.Format("Property: {0} Error: {1}",
                            ex.Message,
                            rs.CurrentRecord));
                    }
                }
            }


            try
            {
                using (var context = new PisContext())
                {
                    context.BOMITTMPs.AddRange(bomittmps.Distinct());
                    context.SaveChanges();
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (DbEntityValidationResult validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (DbValidationError validationError in validationErrors.ValidationErrors)
                    {
                        CreateLog(String.Format("Property: {0} Error: {1}",
                            validationError.PropertyName,
                            validationError.ErrorMessage));
                    }
                }
            }
        }

        //private static void SavePartOrderCase(string path, DateTime dateTimeReceived)
        //{
        //    var orderCases = new List<PartsOrderCaseTMP>();

        //    using (var sr = new StreamReader(path))
        //    {
        //        var rs = new CsvReader(sr);
        //        while (rs.Read())
        //        {
        //            try
        //            {
        //                var _partItem = new PartsOrderCaseTMP();
        //                _partItem.InvoiceNo = RemoveEndString(rs.GetField(0).Replace("\0", string.Empty));
        //                _partItem.InvoiceDate =
        //                    DateTime.Parse(RemoveEndString(rs.GetField(1).Replace("\0", string.Empty)));
        //                _partItem.CaseNo = RemoveEndString(rs.GetField(2).Replace("\0", string.Empty));
        //                _partItem.ShippingIDASN = RemoveEndString(rs.GetField(3).Replace(" \0", string.Empty));
        //                _partItem.CaseType = RemoveEndString(rs[4].Replace("\0", string.Empty));
        //                _partItem.CaseDescription = RemoveEndString(rs[5].Replace("\0", string.Empty));
        //                _partItem.WeightKG = decimal.Parse(RemoveEndString(rs.GetField(6).Replace("\0", string.Empty)),
        //                    NumberStyles.Float);
        //                _partItem.LengthCM = decimal.Parse(RemoveEndString(rs.GetField(7).Replace("\0", string.Empty)),
        //                    NumberStyles.Float);
        //                _partItem.WideCM = decimal.Parse(RemoveEndString(rs.GetField(8).Replace("\0", string.Empty)),
        //                    NumberStyles.Float);
        //                _partItem.HeightCM = decimal.Parse(RemoveEndString(rs.GetField(9).Replace("\0", string.Empty)),
        //                    NumberStyles.Float);
        //                _partItem.RouteID = RemoveEndString(rs.GetField(10).Replace("\0", string.Empty));
        //                _partItem.EmailDate = dateTimeReceived;
        //                _partItem.EntryDate = DateTime.Now;
        //                _partItem.EntryBy = "PisEmailService";
        //                _partItem.ModifiedDate = DateTime.Now;
        //                _partItem.ModifiedBy = "PisEmailService";
        //                orderCases.Add(_partItem);
        //            }
        //            catch (Exception ex)
        //            {
        //                CreateLog(String.Format("Property: {0} Error: {1}",
        //                    ex.Message,
        //                    rs.CurrentRecord));
        //                ;
        //            }
        //        }
        //    }
        //    try
        //    {
        //        using (var context = new PisContext())
        //        {
        //            context.PartsOrderCaseTMPs.AddRange(orderCases.Distinct());

        //            context.SaveChanges();
        //        }
        //    }
        //    catch (DbEntityValidationException dbEx)
        //    {
        //        foreach (DbEntityValidationResult validationErrors in dbEx.EntityValidationErrors)
        //        {
        //            foreach (DbValidationError validationError in validationErrors.ValidationErrors)
        //            {
        //                CreateLog(String.Format("Property: {0} Error: {1}",
        //                    validationError.PropertyName,
        //                    validationError.ErrorMessage));
        //            }
        //        }
        //    }
        //}

        //// Update to excel
        private static void SavePartOrderCase(string path, DateTime dateTimeReceived)
        {
            try
            {
                var orderCases = new List<PartsOrderCaseTMP>();

                HSSFWorkbook hssfwb;
                XSSFWorkbook xssfwb;
                ISheet sheet;
                string fileExt = Path.GetExtension(path).ToLower();
                using (FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    if (POIXMLDocument.HasOOXMLHeader(new BufferedStream(file)))
                    {
                        xssfwb = new XSSFWorkbook(file);
                        sheet = xssfwb.GetSheetAt(0);
                    }
                    else
                    {
                        hssfwb = new HSSFWorkbook(file);
                        sheet = hssfwb.GetSheetAt(0);
                    }
                }

                int i = 1;
                while (sheet.GetRow(i) != null)
                {
                    if (sheet.GetRow(i).GetCell(0) != null)
                    {
                        try
                        {
                            var _partItem = new PartsOrderCaseTMP();
                            _partItem.InvoiceNo = sheet.GetRow(i).GetCell(0).ToString().Trim();
                            _partItem.InvoiceDate = HSSFDateUtil.GetJavaDate(sheet.GetRow(i).GetCell(1).NumericCellValue);
                            //_partItem.InvoiceDate = DateTime.ParseExact(sheet.GetRow(i).GetCell(1).ToString().Trim(), "M/dd/yyyy", CultureInfo.InvariantCulture);
                            //_partItem.CaseNo = sheet.GetRow(i).GetCell(2).ToString().Trim();
                            _partItem.CaseNo = sheet.GetRow(i).GetCell(2).ToString().Trim() != "" ?
                                sheet.GetRow(i).GetCell(2).ToString().Trim() : "0";
                            _partItem.ShippingIDASN = sheet.GetRow(i).GetCell(3).ToString().Trim();
                            _partItem.CaseType = sheet.GetRow(i).GetCell(4).ToString().Trim() != "" ?
                                sheet.GetRow(i).GetCell(4).ToString().Trim() : "0";
                            _partItem.CaseDescription = sheet.GetRow(i).GetCell(5).ToString().Trim();
                            _partItem.WeightKG = sheet.GetRow(i).GetCell(6).ToString().Trim() != "" ?
                                decimal.Parse(sheet.GetRow(i).GetCell(6).ToString().Trim(), NumberStyles.Float) : 0;
                            _partItem.LengthCM = sheet.GetRow(i).GetCell(7).ToString().Trim() != "" ?
                                decimal.Parse(sheet.GetRow(i).GetCell(7).ToString().Trim(), NumberStyles.Float) : 0;
                            _partItem.WideCM = sheet.GetRow(i).GetCell(8).ToString().Trim() != "" ?
                                decimal.Parse(sheet.GetRow(i).GetCell(8).ToString().Trim(), NumberStyles.Float) : 0;
                            _partItem.HeightCM = sheet.GetRow(i).GetCell(9).ToString().Trim() != "" ?
                                decimal.Parse(sheet.GetRow(i).GetCell(9).ToString().Trim(), NumberStyles.Float) : 0;
                            _partItem.RouteID = sheet.GetRow(i).GetCell(10).ToString().Trim();
                            _partItem.EmailDate = dateTimeReceived;
                            _partItem.EntryDate = DateTime.Now;
                            _partItem.EntryBy = "PisEmailService";
                            _partItem.ModifiedDate = DateTime.Now;
                            _partItem.ModifiedBy = "PisEmailService";
                            orderCases.Add(_partItem);
                        }
                        catch (Exception ex)
                        {
                            CreateLog(String.Format("Property: {0} Error: {1}",
                                ex.Message,
                                sheet.GetRow(i)));
                        }
                        i++;
                    }
                    else
                    {
                        break;
                    }
                }
                try
                {
                    using (var context = new PisContext())
                    {
                        context.PartsOrderCaseTMPs.AddRange(orderCases.Distinct());

                        context.SaveChanges();
                    }
                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (DbEntityValidationResult validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (DbValidationError validationError in validationErrors.ValidationErrors)
                        {
                            CreateLog(String.Format("Property: {0} Error: {1}",
                                validationError.PropertyName,
                                validationError.ErrorMessage));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CreateLog(ex.InnerException.InnerException.Message);
                CreateLog(ex.Message);
            }

        }

        //private static void SavePartsOrderDetail(string path, DateTime dateTimeReceived)
        //{
        //    var partsOrderDetails = new List<PartsOrderDetailTMP>();
        //    using (var sr = new StreamReader(path))
        //    {
        //        var rs = new CsvReader(sr);

        //        while (rs.Read())
        //        {
        //            try
        //            {
        //                var detail = new PartsOrderDetailTMP();
        //                detail.InvoiceNo = RemoveEndString(rs.GetField(0).Replace("\0", string.Empty));
        //                detail.InvoiceDate =
        //                    DateTime.Parse(RemoveEndString(rs.GetField(1).Replace("\0", string.Empty)));
        //                detail.PrimPSO = RemoveEndString(rs.GetField(2).Replace("\0", string.Empty));
        //                detail.CaseNo = RemoveEndString(rs.GetField(3).Replace("\0", string.Empty));
        //                //     detail.PartsOrderID = rs[4].ToString().Replace("\0", string.Empty).Trim();
        //                //todo:
        //                detail.PartsNumber = RemoveEndString(rs.GetField(4).Replace("\0", string.Empty));
        //                // Console.WriteLine(rs[5]);
        //                detail.COO = RemoveEndString(rs.GetField(5).Replace("\0", string.Empty));
        //                detail.InvoiceItemNo = int.Parse(RemoveEndString(rs.GetField(6).Replace("\0", string.Empty)));
        //                detail.PartsDescriptionShort = RemoveEndString(rs.GetField(7).Replace("\0", string.Empty));
        //                detail.InvoiceItemQty = int.Parse(RemoveEndString(rs.GetField(8).Replace("\0", string.Empty)));
        //                detail.CustomerReff = RemoveEndString(rs.GetField(9).Replace("\0", string.Empty));
        //                detail.PartGrossWeight =
        //                    decimal.Parse(RemoveEndString(rs.GetField(10).Replace("\0", string.Empty)));
        //                //  detail. = (decimal?)rs[12];
        //                detail.ChargesDiscountAmount =
        //                    decimal.Parse(RemoveEndString(rs.GetField(12).Replace("\0", string.Empty)));
        //                detail.BECode = RemoveEndString(rs.GetField(13).Replace("\0", string.Empty));
        //                detail.OrderCLSCode = RemoveEndString(rs.GetField(14).Replace("\0", string.Empty));
        //                detail.Profile = int.Parse(RemoveEndString(rs.GetField(15).Replace("\0", string.Empty)));
        //                detail.UnitPrice = decimal.Parse(RemoveEndString(rs.GetField(16).Replace("\0", string.Empty)));
        //                detail.EmailDate = dateTimeReceived;
        //                detail.EntryDate = DateTime.Now;
        //                detail.EntryBy = "PisEmailService";
        //                detail.ModifiedDate = DateTime.Now;
        //                detail.ModifiedBy = "PisEmailService";
        //                partsOrderDetails.Add(detail);
        //            }
        //            catch (Exception ex)
        //            {
        //                CreateLog(String.Format("Property: {0} Error: {1}",
        //                    ex.Message,
        //                    rs.CurrentRecord));
        //                ;
        //            }
        //        }
        //    }
        //    try
        //    {
        //        using (var context = new PisContext())
        //        {
        //            foreach (PartsOrderDetailTMP detail in partsOrderDetails.Distinct())
        //            {
        //                context.PartsOrderDetailTMPs.Add(detail);
        //            }
        //            context.SaveChanges();
        //        }
        //    }
        //    catch (DbEntityValidationException dbEx)
        //    {
        //        foreach (DbEntityValidationResult validationErrors in dbEx.EntityValidationErrors)
        //        {
        //            foreach (DbValidationError validationError in validationErrors.ValidationErrors)
        //            {
        //                CreateLog(String.Format("Property: {0} Error: {1}",
        //                    validationError.PropertyName,
        //                    validationError.ErrorMessage));
        //            }
        //        }
        //    }
        //}

        //// Update to excel
        private static void SavePartsOrderDetail(string path, DateTime dateTimeReceived)
        {
            try
            {
                var partsOrderDetails = new List<PartsOrderDetailTMP>();

                HSSFWorkbook hssfwb;
                XSSFWorkbook xssfwb;
                ISheet sheet;
                string fileExt = Path.GetExtension(path).ToLower();
                using (FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    if (POIXMLDocument.HasOOXMLHeader(new BufferedStream(file)))
                    {
                        xssfwb = new XSSFWorkbook(file);
                        sheet = xssfwb.GetSheetAt(0);
                    }
                    else
                    {
                        hssfwb = new HSSFWorkbook(file);
                        sheet = hssfwb.GetSheetAt(0);
                    }
                }

                int i = 1;
                while (sheet.GetRow(i) != null)
                {
                    if (sheet.GetRow(i).GetCell(0) != null)
                    {
                        try
                        {
                            var detail = new PartsOrderDetailTMP();
                            detail.InvoiceNo = sheet.GetRow(i).GetCell(0).ToString().Trim();
                            detail.InvoiceDate = HSSFDateUtil.GetJavaDate(sheet.GetRow(i).GetCell(1).NumericCellValue);
                            detail.PrimPSO = sheet.GetRow(i).GetCell(2).ToString().Trim();
                            detail.CaseNo = sheet.GetRow(i).GetCell(3).ToString().Trim();
                            //     detail.PartsOrderID = rs[4].ToString().Replace("\0", string.Empty).Trim();
                            //todo:
                            detail.PartsNumber = sheet.GetRow(i).GetCell(4).ToString().Trim();
                            // Console.WriteLine(rs[5]);
                            detail.COO = sheet.GetRow(i).GetCell(5).ToString().Trim();
                            detail.InvoiceItemNo = sheet.GetRow(i).GetCell(6).ToString().Trim() != "" ?
                                int.Parse(sheet.GetRow(i).GetCell(6).ToString().Trim()) : 0;
                            detail.PartsDescriptionShort = sheet.GetRow(i).GetCell(7).ToString().Trim();
                            detail.InvoiceItemQty = sheet.GetRow(i).GetCell(8).ToString().Trim() != "" ?
                                int.Parse(sheet.GetRow(i).GetCell(8).ToString().Trim()) : 0;
                            detail.CustomerReff = sheet.GetRow(i).GetCell(9).ToString().Trim();
                            detail.PartGrossWeight = sheet.GetRow(i).GetCell(10).ToString().Trim() != "" ?
                                decimal.Parse(sheet.GetRow(i).GetCell(10).ToString().Trim()) : 0;
                            //  detail. = (decimal?)rs[12];
                            detail.ChargesDiscountAmount = sheet.GetRow(i).GetCell(12).ToString().Trim() != "" ?
                                decimal.Parse(sheet.GetRow(i).GetCell(12).ToString().Trim()) : 0;
                            detail.BECode = sheet.GetRow(i).GetCell(13).ToString().Trim();
                            detail.OrderCLSCode = sheet.GetRow(i).GetCell(14).ToString().Trim();
                            detail.Profile = sheet.GetRow(i).GetCell(15).ToString().Trim() != "" ?
                                int.Parse(sheet.GetRow(i).GetCell(15).ToString().Trim()) : 0;
                            detail.UnitPrice = sheet.GetRow(i).GetCell(16).ToString().Trim() != "" ?
                                decimal.Parse(sheet.GetRow(i).GetCell(16).ToString().Trim()) : 0;
                            detail.EmailDate = dateTimeReceived;
                            detail.EntryDate = DateTime.Now;
                            detail.EntryBy = "PisEmailService";
                            detail.ModifiedDate = DateTime.Now;
                            detail.ModifiedBy = "PisEmailService";
                            partsOrderDetails.Add(detail);
                        }
                        catch (Exception ex)
                        {
                            CreateLog(String.Format("Property: {0} Error: {1}",
                                ex.Message,
                                sheet.GetRow(i)));
                        }
                        i++;
                    }
                    else
                    {
                        break;
                    }
                }
                try
                {
                    using (var context = new PisContext())
                    {
                        foreach (PartsOrderDetailTMP detail in partsOrderDetails.Distinct())
                        {
                            context.PartsOrderDetailTMPs.Add(detail);
                        }
                        context.SaveChanges();
                    }
                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (DbEntityValidationResult validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (DbValidationError validationError in validationErrors.ValidationErrors)
                        {
                            CreateLog(String.Format("Property: {0} Error: {1}",
                                validationError.PropertyName,
                                validationError.ErrorMessage));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CreateLog(ex.InnerException.InnerException.Message);
                CreateLog(ex.Message);
            }

        }

        public static void UpdatePartOrder()
        {
            try
            {
                using (var context = new PisContext())
                {
                    context.Database.ExecuteSqlCommand("exec spUpdatePartsOrder");
                    //context.SaveChanges();
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (DbEntityValidationResult validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (DbValidationError validationError in validationErrors.ValidationErrors)
                    {
                        CreateLog(String.Format("Property: {0} Error: {1}",
                            validationError.PropertyName,
                            validationError.ErrorMessage));
                    }
                }
            }
        }

        public static void MergePartsOrderDetail()
        {
            CreateLog("Start Merge Data Parts Order Detail");

            try
            {
                using (var context = new PisContext())
                {
                    context.Database.CommandTimeout = (30 * 60);
                    context.Database.ExecuteSqlCommand("exec common.spMergePartsOrderDetail");
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (DbEntityValidationResult validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (DbValidationError validationError in validationErrors.ValidationErrors)
                    {
                        CreateLog(String.Format("Property: {0} Error: {1}",
                            validationError.PropertyName,
                            validationError.ErrorMessage));
                    }
                }
            }
        }

        public static void MergePartsOrder()
        {
            CreateLog("Start Merge Data Parts Order");

            try
            {
                using (var context = new PisContext())
                {
                    context.Database.CommandTimeout = (30 * 60);
                    context.Database.ExecuteSqlCommand("exec common.spMergePartsOrder");
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (DbEntityValidationResult validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (DbValidationError validationError in validationErrors.ValidationErrors)
                    {
                        CreateLog(String.Format("Property: {0} Error: {1}",
                            validationError.PropertyName,
                            validationError.ErrorMessage));
                    }
                }
            }
        }

        public static void MergePartsOrderCase()
        {
            CreateLog("Start Merge Data Parts Order Case");

            try
            {
                using (var context = new PisContext())
                {
                    context.Database.CommandTimeout = (30 * 60);
                    context.Database.ExecuteSqlCommand("exec common.spMergePartsOrderCase");
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (DbEntityValidationResult validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (DbValidationError validationError in validationErrors.ValidationErrors)
                    {
                        CreateLog(String.Format("Property: {0} Error: {1}",
                            validationError.PropertyName,
                            validationError.ErrorMessage));
                    }
                }
            }
        }

        public static void MergeBomit()
        {
            CreateLog("Start Merge Data Bomit");

            try
            {
                using (var context = new PisContext())
                {
                    context.Database.CommandTimeout = (30 * 60);
                    context.Database.ExecuteSqlCommand("exec common.spMergeBomit");
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (DbEntityValidationResult validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (DbValidationError validationError in validationErrors.ValidationErrors)
                    {
                        CreateLog(String.Format("Property: {0} Error: {1}",
                            validationError.PropertyName,
                            validationError.ErrorMessage));
                    }
                }
            }
        }

        //public static void VettingProcess()
        //{
        //    try
        //    {
        //        using (var context = new PisContext())
        //        {
        //            context.Database.ExecuteSqlCommand("exec spVettingProcess");
        //        }
        //    }
        //    catch (DbEntityValidationException dbEx)
        //    {
        //        foreach (DbEntityValidationResult validationErrors in dbEx.EntityValidationErrors)
        //        {
        //            foreach (DbValidationError validationError in validationErrors.ValidationErrors)
        //            {
        //                CreateLog(String.Format("Property: {0} Error: {1}",
        //                    validationError.PropertyName,
        //                    validationError.ErrorMessage));
        //            }
        //        }
        //    }
        //}

        public static void CleanData()
        {
            CreateLog("Start Clean Data");

            try
            {
                using (var context = new PisContext())
                {
                    context.Database.ExecuteSqlCommand("exec common.spCleansePartsOrder");
                    context.Database.ExecuteSqlCommand("exec common.spCleansePartsOrderCase");
                    context.Database.ExecuteSqlCommand("exec common.spCleansePartsOrderDetail");

                    //context.SaveChanges();
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (DbEntityValidationResult validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (DbValidationError validationError in validationErrors.ValidationErrors)
                    {
                        CreateLog(String.Format("Property: {0} Error: {1}",
                            validationError.PropertyName,
                            validationError.ErrorMessage));
                    }
                }
            }
        }


        public static void CreateLog(string logEvent)
        {
            const string sSource = "PISReadEmailService";
            const string sLog = "Application";
            try
            {
                if (!EventLog.SourceExists(sSource))
                    EventLog.CreateEventSource(sSource, sLog);

                EventLog.WriteEntry(sSource, logEvent);
            }
            catch (Exception ex)
            {
                using (var eventLog = new EventLog("Application"))
                {
                    eventLog.Source = "Application";
                    eventLog.WriteEntry("PISReadEmailError: " + ex.Message, EventLogEntryType.Error);
                }
            }
        }

        private static bool RedirectionUrlValidationCallback(string redirectionUrl)
        {
            // The default for the validation callback is to reject the URL.
            bool result = false;

            var redirectionUri = new Uri(redirectionUrl);

            // Validate the contents of the redirection URL. In this simple validation
            // callback, the redirection URL is considered valid if it is using HTTPS
            // to encrypt the authentication credentials. 
            if (redirectionUri.Scheme == "https")
            {
                result = true;
            }
            return result;
        }

        public static string RemoveEndString(string csvString)
        {
            string csvStringBefore = csvString.Trim();
            int length = csvStringBefore.Length;
            if (length > 0)
            {
                var regexItem = new Regex("^[a-zA-Z0-9_-]+$");
                if (!regexItem.IsMatch(csvStringBefore[length - 1].ToString()))
                {
                    return csvStringBefore.Substring(0, length - 1).Trim();
                }
            }

            return csvStringBefore;
        }

        public static void DeleteFile(string path)
        {
            CreateLog("DeleteFile: " + path);

            if (File.Exists(path))
            {
                // Use a try block to catch IOExceptions, to
                // handle the case of the file already being
                // opened by another process.
                try
                {
                    File.Delete(path);
                }
                catch (IOException e)
                {
                    CreateLog("DeleteFile: " + e.Message);
                }
            }
        }

        private static string GetCellAsString(ICell cell)
        {
            switch (cell.CellType)
            {
                case CellType.Boolean:
                    return cell.BooleanCellValue.ToString();
                case CellType.Error:
                    return cell.ErrorCellValue.ToString();
                case CellType.Formula:
                    return cell.CellFormula.ToString();
                //ALL CELLS IN THE EXEMPEL ARE NUMERIC AND GOES HERE
                case CellType.Numeric:
                    {
                        return DateUtil.IsCellDateFormatted(cell)
                            ? cell.DateCellValue.ToString()
                            : cell.NumericCellValue.ToString();
                    }
                case CellType.String:
                    return cell.StringCellValue.ToString();
                case CellType.Blank:
                case CellType.Unknown:
                default:
                    return "";
            }
        }

        #region POST Console
        public static void ReadEmailPOST()
        {
            //string Subject = "TRAKINDO_POST";

            CreateLog("Start Grab Email");
            try
            {

                bool isOffice365 = ConfigurationManager.AppSettings["IsOffice365"].ToString() == "1" ? true : false;
                string userName = ConfigurationManager.AppSettings["userNamePOST"];
                string userEmail = ConfigurationManager.AppSettings["userEmailPOST"];
                string password = ConfigurationManager.AppSettings["passwordPOST"];
                string folder = ConfigurationManager.AppSettings["ImagesData.document"] + "";
                int emailItem = int.Parse(ConfigurationManager.AppSettings["EmailItem"]);
                var service = new ExchangeService(ExchangeVersion.Exchange2007_SP1);
                var exchangeAddress = ConfigurationManager.AppSettings["Uri"].ToString();

                if (!isOffice365)
                {
                    service = new ExchangeService(ExchangeVersion.Exchange2007_SP1);
                    service.Credentials = new NetworkCredential(userName, password, "JKT");
                    //service.Url = new Uri("https://mail.tmt.co.id/EWS/Exchange.asmx");
                    service.Url = new Uri(exchangeAddress);
                }
                else
                {
                    service = new ExchangeService(ExchangeVersion.Exchange2013);
                    service.Credentials = new WebCredentials(userEmail, password);
                    service.TraceEnabled = true;
                    service.TraceFlags = TraceFlags.All;
                    service.AutodiscoverUrl(userEmail, RedirectionUrlValidationCallback);
                }

                var searchFilterCollection = new List<SearchFilter>();

                //filter only unread email
                searchFilterCollection.Add(new SearchFilter.SearchFilterCollection(LogicalOperator.And,
                    new SearchFilter.IsEqualTo(EmailMessageSchema.IsRead, false)));

                searchFilterCollection.Add(new SearchFilter.SearchFilterCollection(LogicalOperator.And,
                    new SearchFilter.IsEqualTo(EmailMessageSchema.HasAttachments, true)));

                // Create the search filter.
                SearchFilter searchFilter = new SearchFilter.SearchFilterCollection(LogicalOperator.And,
                    searchFilterCollection.ToArray());

                var inbox = new FolderId(WellKnownFolderName.Inbox);
                var iv = new ItemView(emailItem);

                IOrderedEnumerable<Item> items = null;
                try
                {
                    items = service.FindItems(inbox, searchFilter, iv).OrderByDescending(a => a.DateTimeCreated);
                }
                catch (Exception ex)
                {
                    CreateLog(ex.Message);
                }

                foreach (EmailMessage msg in items)
                {

                    using (var db = new postContext())
                    {

                        string PO_number = "";
                        string SubjectMessage = "";
                        string FinalPO_number = "";
                        string subj = msg.Subject;
                        if (subj.Contains("TRAKINDO PO") || subj.Contains("TRAKINDO_POST"))
                        {
                            var subjsplit = subj.Split(' ');

                            foreach (var item in subjsplit)
                            {
                                SubjectMessage = Regex.Match(item, @"\d+").Value;
                                if (SubjectMessage.Length > 0 && SubjectMessage.Length == 10 && PO_number.Length == 0)
                                {
                                    PO_number = "'" + SubjectMessage + "',";
                                }
                                else if (SubjectMessage.Length > 0 && SubjectMessage.Length == 10 && PO_number.Length > 0)
                                {
                                    PO_number += "'" + SubjectMessage + "',";
                                }
                            }
                            if (PO_number != "")
                            {
                                FinalPO_number = PO_number.Substring(0, PO_number.Length - 1);

                                var dataPO = db.Database.SqlQuery<TrPO>("SELECT * FROM dbo.TrPO where PO_number in(" + FinalPO_number + ") ").ToList();
                                if (dataPO != null)
                                {
                                    if (dataPO.Count > 0)
                                    {
                                        foreach (var tmpPO in dataPO)
                                        {
                                            CreateLog("Read Email: " + msg.Subject);
                                            EmailMessage message = EmailMessage.Bind(service, msg.Id,
                                                new PropertySet(BasePropertySet.IdOnly, ItemSchema.Attachments, ItemSchema.HasAttachments));

                                            foreach (Attachment attachment in message.Attachments)
                                            {
                                                if (attachment is FileAttachment)
                                                {
                                                    try
                                                    {
                                                        var fileAttachment = attachment as FileAttachment;
                                                        var fileNameFilter = Path.GetFileName(fileAttachment.Name).ToString();
                                                        if (fileNameFilter.Contains(tmpPO.PO_Number))
                                                        {
                                                            var deleteData = db.TrRequestAttachment.Where(a => a.RequestID == tmpPO.IdRequest && a.FileNameOri == fileNameFilter).FirstOrDefault();
                                                            if (deleteData != null)
                                                            {
                                                                db.TrRequestAttachment.Remove(deleteData);
                                                                db.SaveChanges();
                                                            }

                                                            var model = new TrRequestAttachment();
                                                            model.RequestID = tmpPO.IdRequest;
                                                            model.ItemID = 0;
                                                            model.Path = "";
                                                            model.FileName = fileAttachment.Name;
                                                            model.FileNameOri = Path.GetFileName(fileAttachment.Name);
                                                            model.CodeAttachment = "PO";
                                                            model.IsActive = true;
                                                            model.UploadedDate = DateTime.Now;
                                                            model.UploadedBy = "CONSOLE_SCHEDULE";
                                                            model.Name = "";
                                                            model.Progress = "";
                                                            model.IsApprove = false;
                                                            model.IsRejected = false;
                                                            db.TrRequestAttachment.Add(model);
                                                            //db.SaveChanges();

                                                            var fileName = "PO" + "_" + model.ID + "_" + fileAttachment.Name;
                                                            var folder_ = folder + tmpPO.IdRequest + Path.DirectorySeparatorChar;
                                                            string path = folder_ + fileName;
                                                            bool exists = Directory.Exists(folder_);

                                                            if (!exists)
                                                                Directory.CreateDirectory(folder_);
                                                            fileAttachment.Load(path);


                                                            model.FileName = fileName;
                                                            model.Path = path;
                                                            db.SaveChanges();
                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        CreateLog(ex.InnerException.InnerException.Message);
                                                        CreateLog(ex.Message);
                                                    }
                                                }
                                            }
                                        }
                                        msg.IsRead = true;
                                        msg.Update(ConflictResolutionMode.AutoResolve);
                                    }
                                }
                            }

                        }
                        else
                        {
                            msg.IsRead = true;
                            msg.Update(ConflictResolutionMode.AutoResolve);
                        }
                    }
                }
                CreateLog("Start Merge Part Order");
            }
            catch (NullReferenceException e)
            {
                CreateLog(e.Message);
                throw;
            }
        }
        #endregion

        #region POST KOFAX Console


        public static void ReUploadtoShareFolderKOFAX()
        {
            CreateLog("Start Get Data");

            try
            {
                using (var context = new postContext())
                {
                    List<TrRequestAttachment> data = new List<TrRequestAttachment>();

                    data = context.Database.SqlQuery<TrRequestAttachment>("exec SP_GetDataFailedUploadKOFAX").ToList();
                    if (data != null)
                    {
                        foreach (var item in data)
                        {
                            UploadFiletoShareFolderKOFAX("", item.Path, item.FileNameKOFAX, item.FileNameOri, item.ID);
                        }
                    }
                }
            }
            catch (NullReferenceException e)
            {
                CreateLog(e.Message);
                throw;
            }
        }

        public static string UploadFiletoShareFolderKOFAX(string filePaths, string path, string FileNameKOFAX, string fileName, Int64 AttachmentId)
        {
            string ShareFolderKOFAX = "";
            string UserNameFolderKOFAX = ConfigurationManager.AppSettings["UserNameFolderKOFAX"];
            string PasswordFolderKOFAX = ConfigurationManager.AppSettings["PasswordFolderKOFAX"];
            string ApplicationDevelopment = ConfigurationManager.AppSettings["ApplicationDevelopment"];
            string CreateBy = "Console";
            string Message = "";
            if (ApplicationDevelopment == "True")
            {
                ShareFolderKOFAX = @"\\tuhov036.tu.tmt.co.id\POST\";
            }
            else
            {
                ShareFolderKOFAX = @"\\tuhov036.tu.tmt.co.id\POST\Production\";
            }
            bool status = true;
            try
            {
                App.Service.Helper.NetworkShare.DisconnectFromShare(ShareFolderKOFAX, true); //Disconnect in case we are currently connected with our credentials;

                App.Service.Helper.NetworkShare.ConnectToShare(ShareFolderKOFAX, UserNameFolderKOFAX, PasswordFolderKOFAX); //Connect with the new credentials

                File.Copy(path, ShareFolderKOFAX + FileNameKOFAX);

                App.Service.Helper.NetworkShare.DisconnectFromShare(ShareFolderKOFAX, false); //Disconnect from the server.
                status = true;
            }
            catch (Exception)
            {
                status = false;
            }

            using (var context = new postContext())
            {
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@AttachmentId", AttachmentId));
                parameterList.Add(new SqlParameter("@CreateBy", CreateBy));
                parameterList.Add(new SqlParameter("@Message", Message));
                parameterList.Add(new SqlParameter("@Status", status));
                SqlParameter[] parameters = parameterList.ToArray();
                context.Database.ExecuteSqlCommand("exec SP_SaveKOFAXLog @AttachmentId,@Message,@CreateBy,@Status", parameters);
            }
            return "";
        }

        #endregion
    }
}