using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Data
{
    public class GonbadDbContext : DbContext
    {
        public GonbadDbContext(DbContextOptions<GonbadDbContext> options) : base(options)
        {
        }
        public DbSet<InfoChild> InfoChilds  { get; set; }
        public DbSet<InfoKhadem> InfoKhadems {  get; set; }
        public DbSet<TimeShitChild> TimeShitChilds { get; set; }
        //public DbSet<child1> Children { get; set; }
       // public DbSet<Visit> Visits { get; set; }

        public DbSet<kid> Kids { get; set; }
        public DbSet<TimeSheet> TimeSheets { get; set; }
        public DbSet<Khadem> Khadems { get; set; }
        public DbSet<ShiftReport> ShiftReports { get; set; }
        public DbSet<QasedakReport> QasedakReports { get; set; }
        public DbSet<QasedakReportKhadem> QasedakReportKhadems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<QasedakReport>()
                .HasIndex(report => new { report.ExecutionDate, report.Shift })
                .IsUnique();

            modelBuilder.Entity<QasedakReportKhadem>()
                .HasKey(item => new { item.QasedakReportId, item.KhademId });

            modelBuilder.Entity<QasedakReportKhadem>()
                .HasOne(item => item.QasedakReport)
                .WithMany(report => report.Khadems)
                .HasForeignKey(item => item.QasedakReportId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<QasedakReportKhadem>()
                .HasOne(item => item.Khadem)
                .WithMany()
                .HasForeignKey(item => item.KhademId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class MyDbContextFactory : IDesignTimeDbContextFactory<GonbadDbContext>
    {

        GonbadDbContext IDesignTimeDbContextFactory<GonbadDbContext>.CreateDbContext(string[] args)
        {

            var builder = new DbContextOptionsBuilder<GonbadDbContext>();

            builder.UseSqlServer("Server=.;Database=GonbadDB;Trusted_Connection=True;TrustServerCertificate=True");

            return new GonbadDbContext(builder.Options);
        }
    }
}
