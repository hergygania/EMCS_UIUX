using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Domain.Extensions
{
    public partial class DocumentUpload
    {
        public string ModulName { get; set; }
        public string FileName { get; set; }
        public string EntryBy { get; set; }
        public DateTime? EntryDate { get; set; }
        public string URL { get; set; }
        public string Status { get; set; }
    }
}
