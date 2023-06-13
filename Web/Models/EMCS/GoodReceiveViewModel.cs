﻿using App.Data.Domain.EMCS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace App.Web.Models.EMCS
{
    public class GoodReceiveViewModel
    {
        public SpGoodReceive Data { get; set; }
        public List<MasterStatus> YesNo { get; set; }
        public List<SelectListItem> DoList { get; set; }

        public ProblemHistory Detail { get; set; }

        public List<ExcelGrDetailData> DetailGr { get; set; }

        public RequestGr DataRequest { get; set; }

        public List<SpGetGrItemList> DataItem { get; set; }
        public ShippingFleetViewModel ShippingFleet { get; set; }
    }
    public class ShippingFleetViewModel
    {
        public long Id { get; set; }
        public long? IdGr { get; set; }
        public long? IdCipl { get; set; }

        public string EdoNo { get; set; }
        public string DoNo { get; set; }
        [RegularExpression("^([a-zA-Z0-9 .&'-]+)$", ErrorMessage = "Invalid Character Input")]
        public string PicName { get; set; }

        [RegularExpression("^([a-zA-Z0-9 .&'-]+)$", ErrorMessage = "Invalid Character Input")]
        public string PhoneNumber { get; set; }

        [RegularExpression("^([a-zA-Z0-9 .&'-]+)$", ErrorMessage = "Invalid Character Input")]
        public string KtpNumber { get; set; }

        [RegularExpression("^([a-zA-Z0-9 .&'-]+)$", ErrorMessage = "Invalid Character Input")]
        public string SimNumber { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMM yyyy}")]
        [DataType(DataType.DateTime)]
        [DisplayName("Sim Expiry Date")]
        public DateTime? SimExpiryDate { get; set; }

        [RegularExpression("^([a-zA-Z0-9 .&'-]+)$", ErrorMessage = "Invalid Character Input")]
        public string StnkNumber { get; set; }
        [RegularExpression("^([a-zA-Z0-9 .&'-]+)$", ErrorMessage = "Invalid Character Input")]
        public string KirNumber { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMM yyyy}")]
        [DataType(DataType.DateTime)]
        [DisplayName("Kir Expire")]
        public DateTime? KirExpire { get; set; }

        [RegularExpression("^([a-zA-Z0-9 .&'-]+)$", ErrorMessage = "Invalid Character Input")]
        public string NopolNumber { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMM yyyy}")]
        [DataType(DataType.DateTime)]
        [DisplayName("Estimation Time Pickup")]
        public DateTime? EstimationTimePickup { get; set; }
        public Nullable<bool> Apar { get; set; }
        public Nullable<bool> Apd { get; set; }
        public string DaNo { get; set; }
    }
}