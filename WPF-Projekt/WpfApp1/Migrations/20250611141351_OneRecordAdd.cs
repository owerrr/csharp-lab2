using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WpfApp1.Migrations
{
    /// <inheritdoc />
    public partial class OneRecordAdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "EmployeeWorkOnVehicles",
                columns: new[] { "Id", "ClientVehicle_Id", "Date", "Employee_Id", "IsDone", "WorkOn" },
                values: new object[] { 2, 2, new DateOnly(2025, 6, 11), 1, false, "wymiana żarówek:25.99:1;wymiana opon:49.99:1;wymiana skrzyni biegow:199.99:0" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "EmployeeWorkOnVehicles",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
