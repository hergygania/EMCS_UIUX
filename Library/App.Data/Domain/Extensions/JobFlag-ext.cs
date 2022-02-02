using System.ComponentModel.DataAnnotations.Schema;

namespace App.Data.Domain
{
    public partial class JobFlag
    {
        [NotMapped]
        public bool SelectedStatus { get; set; }
    }
}