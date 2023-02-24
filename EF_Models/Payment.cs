using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFcoreTesting.Models
{
    public partial class Payment
    {
        public int PaymentID { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal PaymentAmount { get; set; }
        public string? Note { get; set; }

        public int AccountID { get; set; }
        public Account? Account { get; set; }

        
        
    }
}