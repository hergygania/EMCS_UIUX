namespace App.Data.Domain.EMCS
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel;

    [Table("dbo.NpePeb")]
    public class NpePeb
    {
        public long Id { get; set; }
        public long IdCl { get; set; }

        //[MaxLength(29), ErrorMessage = "Invalid Format Input : xxxxxx-xxxxxx-xxxxxxxx-xxxxxx"]
        //[MinLength(29), ErrorMessage = "Invalid Format Input : xxxxxx-xxxxxx-xxxxxxxx-xxxxxx"]
        //[Required]
        //[RegularExpression("[0-9]{6}-[0-9]{6}-[0-9]{8}-[0-9]{6}", ErrorMessage = "Invalid Format Input : xxxxxx-xxxxxx-xxxxxxxx-xxxxxx")]
        //[RegularExpression("([1-9][0-9]*)", ErrorMessage = "Invalid Format Input : xxxxxx-xxxxxx-xxxxxxxx-xxxxxx")]
        //[RegularExpression("[0-9]{6}-[0-9]{6}-[0-9]{8}-[0-9]{6}", ErrorMessage = "Invalid Format Input : xxxxxx-xxxxxx-xxxxxxxx-xxxxxx")]
        //[MaxLength(29)]
        //[MinLength(29)]
        [Required]
        public string AjuNumber { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMM yyyy}")]
        [DataType(DataType.DateTime)]
        [DisplayName("Aju Date")]
        public DateTime? AjuDate { get; set; }

        public string NpeNumber { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMM yyyy}")]
        [DataType(DataType.DateTime)]
        [DisplayName("NPE Date")]
        public DateTime? NpeDate { get; set; }

        public string Npwp { get; set; }
        public string ReceiverName { get; set; }
        public string PassPabeanOffice { get; set; }
        public decimal Dhe { get; set; }
        public decimal PebFob { get; set; }
        public string Valuta { get; set; }
        public string DescriptionPassword { get; set; }
        public bool DocumentComplete { get; set; }
        public string WarehouseLocation { get; set; }
        public decimal Rate { get; set; }
        public decimal FreightPayment { get; set; }
        public decimal InsuranceAmount { get; set; }

        public string CreateBy { get; set; }

        public DateTime CreateDate { get; set; }

        public string UpdateBy { get; set; }

        public DateTime? UpdateDate { get; set; }

        public bool IsDelete { get; set; }

        public bool DraftPeb { get; set; }
        public string RegistrationNumber { get; set; }
        public DateTime? NpeDateSubmitToCustomOffice { get; set; }
    }
}