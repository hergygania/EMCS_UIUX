using System.Collections.Generic;
using App.Data.Domain.EMCS;

namespace App.Web.Models.EMCS
{
    public class PebNpeModel
    {
        public SpCargoDetail Data { get; set; }
        public NpePeb NpePeb { get; set; }
        public List<Documents> Document { get; set; }
        public RequestCl Request { get; set; }
    }
}