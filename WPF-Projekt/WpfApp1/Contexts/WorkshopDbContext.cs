using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Models;

namespace WpfApp1.Contexts
{
    public class WorkshopDbContext: DbContext
    {
        public DbSet<EmployeeTitles> Employee_Titles { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Clients> Clients { get; set; }
        public DbSet<ClientVehicles> Client_Vehicles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=Workshop.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>().HasOne(u => u.EmployeeTitle).WithMany(e => e.Users).HasForeignKey(u => u.Employee_Title_Id).IsRequired(false);
            modelBuilder.Entity<ClientVehicles>().HasOne(v => v.Client).WithMany(c => c.Vehicles).HasForeignKey(v => v.Client_Id);

            modelBuilder.Entity<EmployeeTitles>().HasData(
                new EmployeeTitles { Id=1, Name="Novice", Hourly_Paycheck=25 },
                new EmployeeTitles { Id=2, Name="Mechanic", Hourly_Paycheck=35 },
                new EmployeeTitles { Id=3, Name="Manager", Hourly_Paycheck=50 }
            );

            modelBuilder.Entity<Users>().HasData(
                new Users { Id=1, Name="admin", Password="$2a$11$5Q4XJlPQM2r2rwk9qMJdp.yT0IYabz6SPq5gpPHggLQkcNTk5Gs6i", Employee_Title_Id=3 }
            );

            modelBuilder.Entity<Clients>().HasData(
                new Clients { Id=1, Firstname="Denis", Lastname="Biskup", Phonenumber="123456789", Postalcode="32-020", City="Wieliczka", Street="Fajna", Building_No="12a" },
                new Clients { Id=2, Firstname="Test", Lastname="Tester", Phonenumber="111222333", Postalcode="12-011", City="Testowe", Street="Testowa", Building_No="1" }
            );

            modelBuilder.Entity<ClientVehicles>().HasData(
                new ClientVehicles { Id=1, Client_Id=1, Car_Model="Seat Ibiza 3", Car_Vin="3VWSB81H8WM210368", Car_RegNo="KWI9123", Car_Year=2003},
                new ClientVehicles { Id=2, Client_Id=2, Car_Model="Dodge Ram Pickup", Car_Vin= "1D7HA16D94J171206", Car_RegNo="KRTEST", Car_Year=2016}
            );
        }
    }
}
