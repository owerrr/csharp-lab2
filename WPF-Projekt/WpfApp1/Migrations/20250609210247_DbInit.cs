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
                    Hourly_Paycheck = table.Column<int>(type: "INTEGER", nullable: false)
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
                    Phonenumber = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false)
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
                columns: new[] { "Id", "Hourly_Paycheck", "Name" },
                values: new object[,]
                {
                    { 1, 25, "Novice" },
                    { 2, 35, "Mechanic" },
                    { 3, 50, "Manager" }
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Email", "Firstname", "Lastname", "Phonenumber" },
                values: new object[,]
                {
                    { 1, "testowy@mail.com", "Denis", "Biskup", "123412341" },
                    { 2, "testowy2@mail.com", "Testowy", "Pracownik", "111222333" }
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
                columns: new[] { "Id", "Client_Id", "Employee_Id", "Employee_Title_Id", "Name", "Password" },
                values: new object[,]
                {
                    { 1, null, 1, 3, "admin", "$2a$11$5Q4XJlPQM2r2rwk9qMJdp.yT0IYabz6SPq5gpPHggLQkcNTk5Gs6i" },
                    { 2, null, 2, 2, "pracownik", "$2a$11$5Q4XJlPQM2r2rwk9qMJdp.yT0IYabz6SPq5gpPHggLQkcNTk5Gs6i" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Client_Vehicles_Client_Id",
                table: "Client_Vehicles",
                column: "Client_Id");

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
                name: "Client_Vehicles");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "Employee_Titles");

            migrationBuilder.DropTable(
                name: "Employees");
        }
    }
}
