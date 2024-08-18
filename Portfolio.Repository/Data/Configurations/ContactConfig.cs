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
    public class ContactConfig : IEntityTypeConfiguration<Contact>
    {
        public void Configure(EntityTypeBuilder<Contact> builder)
        {
            builder.Property(cl => cl.PersonName).IsRequired();

            builder.Property(cl => cl.Company).IsRequired();

            builder.Property(cl => cl.Phone).IsRequired();

            builder.Property(cl => cl.Description).IsRequired();


            builder.Property(cl => cl.Country).IsRequired();
        }
    }
}
