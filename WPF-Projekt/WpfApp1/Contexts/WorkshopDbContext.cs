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
        public DbSet<Employees> Employees { get; set; }
        public DbSet<EmployeeWorkOnVehicles> EmployeeWorkOnVehicles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=Workshop.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>().HasOne(u => u.EmployeeTitle).WithMany(e => e.Users).HasForeignKey(u => u.Employee_Title_Id).IsRequired(false);
            modelBuilder.Entity<Users>().HasOne(u => u.Client).WithOne(c => c.User).HasForeignKey<Users>(u => u.Client_Id).IsRequired(false);
            modelBuilder.Entity<Users>().HasOne(u => u.Employee).WithOne(e => e.User).HasForeignKey<Users>(u => u.Employee_Id).IsRequired(false);
            modelBuilder.Entity<Users>().HasIndex(u => u.Client_Id).IsUnique();
            modelBuilder.Entity<Users>().HasIndex(u => u.Employee_Id).IsUnique();
            modelBuilder.Entity<ClientVehicles>().HasOne(v => v.Client).WithMany(c => c.Vehicles).HasForeignKey(v => v.Client_Id);
            modelBuilder.Entity<EmployeeWorkOnVehicles>().HasOne(e => e.ClientVehicle).WithMany(v => v.EmployeeWorkOnVehicles).HasForeignKey(e => e.ClientVehicle_Id);
            modelBuilder.Entity<EmployeeWorkOnVehicles>().HasOne(e => e.Employee).WithMany(v => v.EmployeeWorkOnVehicles).HasForeignKey(e => e.Employee_Id);

            modelBuilder.Entity<EmployeeTitles>().HasData(
                new EmployeeTitles { Id=1, Name="Novice", Hourly_Paycheck=25, Modify_Employees_Permissions = 0 },
                new EmployeeTitles { Id=2, Name="Mechanic", Hourly_Paycheck=35, Modify_Employees_Permissions = 0 },
                new EmployeeTitles { Id=3, Name="Manager", Hourly_Paycheck=50, Modify_Employees_Permissions = 1 },
                new EmployeeTitles { Id=4, Name="Admin", Hourly_Paycheck=0, Modify_Employees_Permissions = 2 }
            );

            modelBuilder.Entity<Employees>().HasData(
                new Employees { Id = 1, Firstname = "Denis", Lastname = "Biskup", Phonenumber = "123412341" },
                new Employees { Id = 2, Firstname = "Testowy", Lastname = "Manager", Phonenumber = "111222333" },
                new Employees { Id = 3, Firstname = "Testowy", Lastname = "Pracownik", Phonenumber = "111222333" }
            );

            modelBuilder.Entity<Clients>().HasData(
                new Clients { Id = 1, Firstname = "Denis", Lastname = "Biskup", Phonenumber = "123456789", Postalcode = "32-020", City = "Wieliczka", Street = "Fajna", Building_No = "12a" },
                new Clients { Id = 2, Firstname = "Test", Lastname = "Tester", Phonenumber = "111222333", Postalcode = "12-011", City = "Testowe", Street = "Testowa", Building_No = "1" }
            );

            modelBuilder.Entity<Users>().HasData(
                new Users { Id = 1, Name = "admin", Password = "$2a$11$5Q4XJlPQM2r2rwk9qMJdp.yT0IYabz6SPq5gpPHggLQkcNTk5Gs6i", Email = "testowy@mail.com", Employee_Title_Id = 4, Employee_Id = 1 },
                new Users { Id = 2, Name = "manager", Password = "$2a$11$5Q4XJlPQM2r2rwk9qMJdp.yT0IYabz6SPq5gpPHggLQkcNTk5Gs6i", Email = "testowy2@mail.com", Employee_Title_Id = 3, Employee_Id = 2 },
                new Users { Id = 3, Name = "pracownik", Password = "$2a$11$5Q4XJlPQM2r2rwk9qMJdp.yT0IYabz6SPq5gpPHggLQkcNTk5Gs6i", Email = "testowy3@mail.com", Employee_Title_Id = 2, Employee_Id = 3 },
                new Users { Id = 4, Name = "user", Password = "$2a$11$5Q4XJlPQM2r2rwk9qMJdp.yT0IYabz6SPq5gpPHggLQkcNTk5Gs6i", Email="user@mail.com", Client_Id=1 }
            );

            modelBuilder.Entity<ClientVehicles>().HasData(
                new ClientVehicles { Id=1, Client_Id=1, Car_Model="Seat Ibiza 3", Car_Vin="3VWSB81H8WM210368", Car_RegNo="KWI9123", Car_Year=2003},
                new ClientVehicles { Id=2, Client_Id=1, Car_Model="Dodge Ram Pickup", Car_Vin= "1D7HA16D94J171206", Car_RegNo="KRTEST", Car_Year=2016}
            );

            modelBuilder.Entity<EmployeeWorkOnVehicles>().HasData(
                new EmployeeWorkOnVehicles { Id = 1, ClientVehicle_Id = 1, Employee_Id = 1, Date = DateOnly.FromDateTime(DateTime.Today), WorkOn = "wymiana żarówek:25.99:1;wymiana opon:49.99:1;wymiana skrzyni biegow:199.99:0" },
                new EmployeeWorkOnVehicles { Id = 2, ClientVehicle_Id = 2, Employee_Id = 1, Date = DateOnly.FromDateTime(DateTime.Today), WorkOn = "wymiana żarówek:25.99:1;wymiana opon:49.99:1;wymiana skrzyni biegow:199.99:0" }
            );
        }
    }
}
