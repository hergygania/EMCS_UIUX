namespace App.Data.Domain.EMCS
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("dbo.Type")]
    public class Type
    {
        public int Id { get; set; }      
        public string Name { get; set; }
        
    }
}