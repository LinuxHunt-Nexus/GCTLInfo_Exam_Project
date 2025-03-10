using GCTLInfo_Exam_Project.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GCTLInfo_Exam_Project.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<RosterSchedule> RosterSchedules { get; set; }
        public DbSet<Shift> Shifts { get; set; }
        public DbSet<Designation> Designations { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<DeliveryInfo> DeliveryAddresses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DeliveryInfo>().ToTable("DeliveryInfo");

            modelBuilder.Entity<Employee>()
                .HasKey(e => e.EmployeeID);

            modelBuilder.Entity<RosterSchedule>()
                .HasKey(r => r.AI_ID);

            modelBuilder.Entity<RosterSchedule>()
                .HasOne(r => r.Employee)
                .WithMany()
                .HasForeignKey(r => r.EmployeeID)
                .HasPrincipalKey(e => e.EmployeeID)
                .IsRequired(false);

            modelBuilder.Entity<RosterSchedule>()
                .HasOne(r => r.Shift)
                .WithMany()
                .HasForeignKey(r => r.ShiftCode)
                .HasPrincipalKey(s => s.ShiftCode)
                .IsRequired(false);

            modelBuilder.Entity<Designation>()
                .HasKey(d => d.DesignationCode);

            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Designation)
                .WithMany()
                .HasForeignKey(e => e.DesignationID)
                .HasPrincipalKey(d => d.DesignationCode)
                .IsRequired(false);
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Designation)
                .WithMany()
                .HasForeignKey(e => e.DesignationCode) 
                .HasPrincipalKey(d => d.DesignationCode)
                .IsRequired(false);

            modelBuilder.Entity<RosterSchedule>()
                .HasOne(r => r.Shift)
                .WithMany()
                .HasForeignKey(r => r.ShiftCode)
                .HasPrincipalKey(s => s.ShiftCode)
                .IsRequired(false);


            // Map to Database Tables
            modelBuilder.Entity<Employee>().ToTable("HRM_Employee");
            modelBuilder.Entity<RosterSchedule>().ToTable("HRM_ATD_RosterScheduleEntry");
            modelBuilder.Entity<Shift>().ToTable("HRM_ATD_Shift");
            modelBuilder.Entity<Designation>().ToTable("HRM_Def_Designation");
        }

    }
}
