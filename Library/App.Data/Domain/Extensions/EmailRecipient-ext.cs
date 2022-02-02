using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Data.Domain
{
    public partial class EmailRecipient
    {

        public IEnumerable<string> EmailList { get; set; }
    }
}