using App.Data.Domain.EMCS;
using System;
using System.Collections.Generic;

namespace App.Web.Models.EMCS
{
    public class BlAwbModel
    {
        public BlAwbModel()
        {
            BlAwbList = new List<BlAwbViewModel>();
            documentsList = new List<Documents>();
        }

        public SpCargoDetail Data { get; set; }
        public BlAwb BlAwb { get; set; }
        public RequestCl Request { get; set; }
        public List<BlAwbViewModel> BlAwbList { get; set; }
        public List<Data.Domain.EMCS.Documents> documentsList { get; set; }
        public string Status { get; set; }
    }
    public class BlAwbViewModel
    {
        public BlAwbViewModel()
        {
        }

        public long? Id { get; set; }
        public string Number { get; set; }
        public DateTime? MasterBlDate { get; set; }
        public string HouseBlNumber { get; set; }
        public DateTime? HouseBlDate { get; set; }
        public string Description { get; set; }
        public string FileName { get; set; }
        public string Publisher { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool IsDelete { get; set; }
        public long? IdCl { get; set; }
    }

}