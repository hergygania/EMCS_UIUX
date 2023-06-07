namespace App.Data.Domain.EMCS
{
    using System.Collections.Generic;

    public class WizardData
    {
        public List<IdData> Ciplid { get; set; }
        public List<IdData> Grid { get; set; }
        public List<IdData> Cargoid { get; set; }
        public List<IdData> Ssid { get; set; }
        public List<IdData> Siid { get; set; }
        public List<IdData> Npeid { get; set; }
        public List<IdData> Blid { get; set; }
        public bool HasSs { get; set; }
        public int Progress { get; set; }
    }
}
