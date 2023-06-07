using App.Web.Models.DTS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Demo.Data
{
    public static class FreightEstimateCalculator
    {
        public static List<FreightEstimateCalculatorModel> GetList()
        {
            List<FreightEstimateCalculatorModel> list = new List<FreightEstimateCalculatorModel>();

            list.Add(new FreightEstimateCalculatorModel
            {
                No = 1,
                Unit = "MACHINE",
                Destination = "Toba Samosir",
                Origin = "Jakarta",
                Area = "SUMATERA",
                Propinsi = "SUMATERA",
                Kota = "Toba Samosir",
                KabupatenKota = "-",
                Model120K = "40,000,000",
                Model320D2 = "45,000,000",
                Model320D2_GC = "45,000,000",
                Model330D2L = "65,000,000",
            });

            list.Add(new FreightEstimateCalculatorModel
            {
                No = 2,
                Unit = "FORKLIFT",
                Destination = "Kota Pekanbaru",
                Origin = "Jakarta",
                Area = "SUMATERA",
                Propinsi = "RIAU",
                Kota = "Kota Pekanbaru",
                KabupatenKota = "-",
                Model120K = "33,125,000",
                Model320D2 = "37,000,000",
                Model320D2_GC = "37,000,000",
                Model330D2L = "47,000,000",
            });

            return list;
        }
    }
}