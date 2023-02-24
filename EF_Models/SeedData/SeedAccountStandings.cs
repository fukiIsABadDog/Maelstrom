using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFcoreTesting.Models
{
    internal class SeedAccountStandings : IEntityTypeConfiguration<AccountStanding>
    {
        public void Configure(EntityTypeBuilder<AccountStanding> entity)
        {
            entity.HasData(
                new AccountStanding { AccountStandingID = 1, Name = "Current" },
                new AccountStanding { AccountStandingID = 2, Name = "NotCurrent" }
                );
        }
    }
}
