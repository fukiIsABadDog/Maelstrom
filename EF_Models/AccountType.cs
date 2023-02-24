using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFcoreTesting.Models
{
    public partial class AccountType 
    {
        public int AccountTypeID { get; set; }
        public string? Name { get; set; }
        public int TermLengthDays { get; set; }

        public decimal Cost { get; set; } // new prop for use in payments
    }
}
