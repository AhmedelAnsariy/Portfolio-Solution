using Microsoft.EntityFrameworkCore;
using Portfolio.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Portfolio.Repository.Data
{
    public class DataDbContext : DbContext
    {


        public DataDbContext(DbContextOptions<DataDbContext> options) : base(options)
        {
            
        }













        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }





        public DbSet<Career> Careers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ClientReview> ClientReviews { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Design> Designs { get; set; }
        public DbSet<EvaluationMonthly> Evaluations { get; set; }








       



    }
}
