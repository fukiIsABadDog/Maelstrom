using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFcoreTesting.Models
{
    internal class SeedAccounts : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> entity)
        {
            //    entity.HasData(
            //        new Account
            //        {
            //            AccountID = 1,
            //            AccountStandingID = 1,
            //            AccountTypeID = 1,
            //            City = "Richmond",
            //            Country = "US",
            //            Email = "jba123@gmail.com",
            //            HolderName = "Justin",
            //            StateOrProvince = "VA",
            //            StreetAdress = "123 way drive",
            //            ZipCode = "23225",
            //            // we will deal with account payments later
            //        });
            //}

        }
    }
}
