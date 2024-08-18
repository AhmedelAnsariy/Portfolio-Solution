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
    public class DesignConfig : IEntityTypeConfiguration<Design>
    {
        public void Configure(EntityTypeBuilder<Design> builder)
        {
           builder.Property(d=>d.PictureUrl).IsRequired();


            builder.HasOne(d => d.Category)
           .WithMany(c => c.Designs)
           .HasForeignKey(d => d.CategoryId)
           .IsRequired();

        }
    }
}
