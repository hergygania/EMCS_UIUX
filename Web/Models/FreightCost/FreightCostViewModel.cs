using App.Data.Domain;
using App.Data.Domain.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.Web.Models
{
    public partial class FreightCostViewModel : V_FREIGHTCOST
    {
        public List<MasterGeneric> ServiceList { get; set; }

        public List<getCity> OriginCodeList { get; set; }

        public List<getCity> DestinationCodeList { get; set; }

        public List<getModaFactor> ModaFactorList { get; set; }

        public List<MasterGeneric> PackingCostList { get; set; }

        public List<ModaFleet> FleetList { get; set; }

        public string Note1 { get; set; }
        public string Note2 { get; set; }
        public string Note3 { get; set; }
        public string Note4 { get; set; }

   
    }
}