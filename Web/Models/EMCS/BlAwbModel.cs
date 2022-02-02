using App.Data.Domain.EMCS;

namespace App.Web.Models.EMCS
{
    public class BlAwbModel
    {
        public SpCargoDetail Data { get; set; }

        public BlAwb BlAwb { get; set; }
        public RequestCl Request { get; set; }
    }
}