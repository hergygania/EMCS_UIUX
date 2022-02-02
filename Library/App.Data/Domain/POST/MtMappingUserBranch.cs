using System.ComponentModel.DataAnnotations.Schema;

namespace App.Data.Domain.POST
{
    [Table("dbo.MtMappingUserBranch")]
    public class MtMappingUserBranch
    {
        public long ID { get; set; }
        public string UserID { get; set; }
        public string FullName { get; set; }
        public string NPWP { get; set; }
        public string NamaCabang { get; set; }
    }
}
