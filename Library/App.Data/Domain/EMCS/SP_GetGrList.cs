namespace App.Data.Domain.EMCS
{
    using System; 
    using System.ComponentModel.DataAnnotations; 

    public class SpGetGrList 
    {
        [Key] 
        public long Id { get; set; } 
        public string GrNo { get; set; } 
        public string PicName { get; set; } 
        public string PhoneNumber { get; set; } 
        public string KtpNumber { get; set; } 
        public string SimNumber { get; set; } 
        public string StnkNumber { get; set; } 
        public string NopolNumber { get; set; } 
        public DateTime? EstimationTimePickup { get; set; } 
        public string Notes { get; set; } 
        public string Step { get; set; } 
        public string Status { get; set; } 
        public string StatusViewByUser { get; set; } 
    } 
} 