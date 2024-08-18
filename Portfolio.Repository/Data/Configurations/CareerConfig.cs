using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portfolio.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portfolio.Repository.Data.Configurations
{
    public class CareerConfig : IEntityTypeConfiguration<Career>
    {
        public void Configure(EntityTypeBuilder<Career> builder)
        {
            builder.Property(C => C.Date).IsRequired();
            builder.Property(C => C.CompanyName).IsRequired();
            builder.Property(C => C.Description).IsRequired();


        }
    }
}
