namespace App.Data.Domain.EMCS
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("dbo.GoodsReceiveItem")] 
    public class GoodsReceiveItem 
    {
        public long Id { get; set; } 
        public long IdGr { get; set; } 
        public long IdCipl { get; set; } 
        public string DoNo { get; set; } 
        public string DaNo { get; set; } 
        public string FileName { get; set; } 
        public DateTime CreateDate { get; set; } 
        public string CreateBy { get; set; } 
        public DateTime? UpdateDate { get; set; } 
        public string UpdateBy { get; set; } 
        public bool IsDelete { get; set; } 
    } 
} 