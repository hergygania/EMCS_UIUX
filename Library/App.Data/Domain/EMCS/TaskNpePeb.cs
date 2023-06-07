namespace App.Data.Domain.EMCS
{
    using System.ComponentModel.DataAnnotations;

    public class TaskNpePeb
    {
        public long Id { get; set; }
        public long IdCl { get; set; }
        [Required]
        public string AjuNumber { get; set; }
        [Required]
        public System.DateTime AjuDate { get; set; }
        public string PebNumber { get; set; }
        public System.DateTime PebDate { get; set; }
        public string NpeNumber { get; set; }
        public System.DateTime NpeDate { get; set; }
        public string FileName { get; set; }
        public string Npwp { get; set; }
        public string ReceiverName { get; set; }
        public string PassPabeanOffice { get; set; }
        public decimal Dhe { get; set; }
        public decimal PebFob { get; set; }
        public string Valuta { get; set; }
        public string DescriptionPassword { get; set; }
        public string WarehouseLocation { get; set; }
        public bool DocumentComplete { get; set; }
        public string Receipt { get; set; }
        public decimal Rate { get; set; }
        public decimal FreightPayment { get; set; }
        public decimal InsuranceAmount { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public System.DateTime UpdateDate { get; set; }
        public bool IsDelete { get; set; }
        
        [Required]
        public string Description { get; set; }
        [Required]
        public string SpecialInstruction { get; set; }
        public string DocumentRequired { get; set; }

<<<<<<< HEAD
<<<<<<< HEAD
        public System.DateTime NpeDateSubmitToCustomOffice { get; set; }

=======
>>>>>>> 639d8d0 (Intial commit)
=======
        public System.DateTime NpeDateSubmitToCustomOffice { get; set; }

>>>>>>> b773f28 (intial commit for changes from himanshu and vijendra)
    }
}