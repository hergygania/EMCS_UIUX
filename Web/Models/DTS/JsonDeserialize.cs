using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.Web.Models.DTS
{
    public class JsonDetail
    {
        [JsonProperty("RTSActual")]
        public string RTSActual { get; set; }
        [JsonProperty("RTSPlan")]
        public string RTSPlan { get; set; }
        [JsonProperty("OnBoardVesselActual")]
        public string OnBoardVesselActual { get; set; }
        [JsonProperty("OnBoardVesselPlan")]
        public string OnBoardVesselPlan { get; set; }
        [JsonProperty("PortInActual")]
        public string PortInActual { get; set; }
        [JsonProperty("PortInPlan")]
        public string PortInPlan { get; set; }
        [JsonProperty("PortOutActual")]
        public string PortOutActual { get; set; }
        [JsonProperty("PortOutPlan")]
        public string PortOutPlan { get; set; }
        [JsonProperty("PLBInActual")]
        public string PLBInActual { get; set; }
        [JsonProperty("PLBInPlan")]
        public string PLBInPlan { get; set; }
        [JsonProperty("PLBOutActual")]
        public string PLBOutActual { get; set; }
        [JsonProperty("PLBOutPlan")]
        public string PLBOutPlan { get; set; }
        [JsonProperty("YardInActual")]
        public string YardInActual { get; set; }
        [JsonProperty("YardInPlan")]
        public string YardInPlan { get; set; }
        [JsonProperty("YardOutActual")]
        public string YardOutActual { get; set; }
        [JsonProperty("YardOutPlan")]
        public string YardOutPlan { get; set; }
    }
}