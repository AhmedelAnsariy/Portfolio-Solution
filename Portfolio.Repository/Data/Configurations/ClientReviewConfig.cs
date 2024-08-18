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
    public class ClientReviewConfig : IEntityTypeConfiguration<ClientReview>
    {
        public void Configure(EntityTypeBuilder<ClientReview> builder)
        {
            builder.Property(cl => cl.Name).IsRequired();
            builder.Property(cl => cl.Company).IsRequired();
            builder.Property(cl => cl.Description).IsRequired();
            builder.Property(cl => cl.PictureUrl).IsRequired();

        }
    }
}
