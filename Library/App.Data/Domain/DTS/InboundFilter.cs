using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Domain.DTS
{
    public class InboundFilter
    {
        public string IdString { get; set; }
        public string AjuNumber { get; set; }
        public string PoNumber { get; set; }
        public string Model { get; set; }
        public string SerialNumber { get; set; }
        public string BatchNumber { get; set; }
        public DateTime? RTSFrom { get; set; }
        public DateTime? RTSTo { get; set; }
        public DateTime? OnBoardVesselFrom  { get; set; }
        public DateTime? OnBoardVesselTo { get; set; }
        public DateTime? PortInFrom { get; set; }
        public DateTime? PortInTo { get; set; }
        public DateTime? PortOutFrom { get; set; }
        public DateTime? PortOutTo { get; set; }
        public string Status { get; set; }
        public string Position { get; set; }
    }
}
