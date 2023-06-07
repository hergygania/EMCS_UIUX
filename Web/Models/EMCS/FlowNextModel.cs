using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using App.Data.Domain.EMCS;

namespace App.Web.Models.EMCS
{
    public class FlowNextModel
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public long IdStatus { get; set; }
        [Required]
        public long IdStep { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateBy { get; set; }
        public bool IsDelete { get; set; }

        public List<MasterFlow> ListFlow { get; set; }
        public List<MasterFlowStep> ListStep { get; set; }

    }
}