using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace App.Web.Models
{
    public class ManualVettingModel
    {
        public List<string> selPartsList_Ids { get; set; }
        public List<int> selHSCodeList_Ids { get; set; }
        public List<string> selOrderMethods { get; set; }
    }
}