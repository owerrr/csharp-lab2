using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WpfApp1.Migrations
{
    /// <inheritdoc />
    public partial class DbInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Firstname = table.Column<string>(type: "TEXT", nullable: false),
                    Lastname = table.Column<string>(type: "TEXT", nullable: false),
                    Phonenumber = table.Column<string>(type: "TEXT", nullable: false),
                    Postalcode = table.Column<string>(type: "TEXT", nullable: false),
                    City = table.Column<string>(type: "TEXT", nullable: false),
                    Street = table.Column<string>(type: "TEXT", nullable: false),
                    Building_No = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employee_Titles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Hourly_Paycheck = table.Column<int>(type: "INTEGER", nullable: false),
                    Modify_Employees_Permissions = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee_Titles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Firstname = table.Column<string>(type: "TEXT", nullable: false),
                    Lastname = table.Column<string>(type: "TEXT", nullable: false),
                    Phonenumber = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Client_Vehicles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Client_Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Car_Model = table.Column<string>(type: "TEXT", nullable: false),
                    Car_Vin = table.Column<string>(type: "TEXT", nullable: false),
                    Car_RegNo = table.Column<string>(type: "TEXT", nullable: false),
                    Car_Year = table.Column<int>(type: "INTEGER", nullable: false),
                    IsMaintenanced = table.Column<bool>(type: "INTEGER", nullable: false),
                    EstimatedMaintenanceEnd = table.Column<DateOnly>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Client_Vehicles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Client_Vehicles_Clients_Client_Id",
                        column: x => x.Client_Id,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    Employee_Title_Id = table.Column<int>(type: "INTEGER", nullable: true),
                    Client_Id = table.Column<int>(type: "INTEGER", nullable: true),
                    Employee_Id = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Clients_Client_Id",
                        column: x => x.Client_Id,
                        principalTable: "Clients",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Users_Employee_Titles_Employee_Title_Id",
                        column: x => x.Employee_Title_Id,
                        principalTable: "Employee_Titles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Users_Employees_Employee_Id",
                        column: x => x.Employee_Id,
                        principalTable: "Employees",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EmployeeWorkOnVehicles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ClientVehicle_Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Employee_Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Date = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    WorkOn = table.Column<string>(type: "TEXT", nullable: false),
                    IsDone = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeWorkOnVehicles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeWorkOnVehicles_Client_Vehicles_ClientVehicle_Id",
                        column: x => x.ClientVehicle_Id,
                        principalTable: "Client_Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeWorkOnVehicles_Employees_Employee_Id",
                        column: x => x.Employee_Id,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "Id", "Building_No", "City", "Firstname", "Lastname", "Phonenumber", "Postalcode", "Street" },
                values: new object[,]
                {
                    { 1, "12a", "Wieliczka", "Denis", "Biskup", "123456789", "32-020", "Fajna" },
                    { 2, "1", "Testowe", "Test", "Tester", "111222333", "12-011", "Testowa" }
                });

            migrationBuilder.InsertData(
                table: "Employee_Titles",
                columns: new[] { "Id", "Hourly_Paycheck", "Modify_Employees_Permissions", "Name" },
                values: new object[,]
                {
                    { 1, 25, 0, "Novice" },
                    { 2, 35, 0, "Mechanic" },
                    { 3, 50, 1, "Manager" },
                    { 4, 0, 2, "Admin" }
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Firstname", "Lastname", "Phonenumber" },
                values: new object[,]
                {
                    { 1, "Denis", "Biskup", "123412341" },
                    { 2, "Testowy", "Manager", "111222333" },
                    { 3, "Testowy", "Pracownik", "111222333" }
                });

            migrationBuilder.InsertData(
                table: "Client_Vehicles",
                columns: new[] { "Id", "Car_Model", "Car_RegNo", "Car_Vin", "Car_Year", "Client_Id", "EstimatedMaintenanceEnd", "IsMaintenanced" },
                values: new object[,]
                {
                    { 1, "Seat Ibiza 3", "KWI9123", "3VWSB81H8WM210368", 2003, 1, null, false },
                    { 2, "Dodge Ram Pickup", "KRTEST", "1D7HA16D94J171206", 2016, 1, null, false }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Client_Id", "Email", "Employee_Id", "Employee_Title_Id", "Name", "Password" },
                values: new object[,]
                {
                    { 1, null, "testowy@mail.com", 1, 4, "admin", "$2a$11$5Q4XJlPQM2r2rwk9qMJdp.yT0IYabz6SPq5gpPHggLQkcNTk5Gs6i" },
                    { 2, null, "testowy2@mail.com", 2, 3, "manager", "$2a$11$5Q4XJlPQM2r2rwk9qMJdp.yT0IYabz6SPq5gpPHggLQkcNTk5Gs6i" },
                    { 3, null, "testowy3@mail.com", 3, 2, "pracownik", "$2a$11$5Q4XJlPQM2r2rwk9qMJdp.yT0IYabz6SPq5gpPHggLQkcNTk5Gs6i" },
                    { 4, 1, "user@mail.com", null, null, "user", "$2a$11$5Q4XJlPQM2r2rwk9qMJdp.yT0IYabz6SPq5gpPHggLQkcNTk5Gs6i" }
                });

            migrationBuilder.InsertData(
                table: "EmployeeWorkOnVehicles",
                columns: new[] { "Id", "ClientVehicle_Id", "Date", "Employee_Id", "IsDone", "WorkOn" },
                values: new object[] { 1, 1, new DateOnly(2025, 6, 11), 1, false, "wymiana żarówek:25.99:1;wymiana opon:49.99:1;wymiana skrzyni biegow:199.99:0" });

            migrationBuilder.CreateIndex(
                name: "IX_Client_Vehicles_Client_Id",
                table: "Client_Vehicles",
                column: "Client_Id");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeWorkOnVehicles_ClientVehicle_Id",
                table: "EmployeeWorkOnVehicles",
                column: "ClientVehicle_Id");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeWorkOnVehicles_Employee_Id",
                table: "EmployeeWorkOnVehicles",
                column: "Employee_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Client_Id",
                table: "Users",
                column: "Client_Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Employee_Id",
                table: "Users",
                column: "Employee_Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Employee_Title_Id",
                table: "Users",
                column: "Employee_Title_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeWorkOnVehicles");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Client_Vehicles");

            migrationBuilder.DropTable(
                name: "Employee_Titles");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Clients");
        }
    }
}
