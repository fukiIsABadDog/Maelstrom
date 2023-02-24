using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFcoreTesting.Models
{
    internal class SeedAccountTypes : IEntityTypeConfiguration<AccountType>
    {
        public void Configure(EntityTypeBuilder<AccountType> entity)
        {
            entity.HasData(
                new AccountType { AccountTypeID = 1, Name = "PremiumMonthly", TermLengthDays = 30, Cost = ((decimal)(12.99)) },
                new AccountType { AccountTypeID = 2, Name = "PremiumYearly", TermLengthDays = 365, Cost = ((decimal)(129.99)) },
                new AccountType { AccountTypeID = 3, Name = "Trail", TermLengthDays = 14,  Cost = ((decimal)(0.00)) }
                ) ;
        }
    }
}