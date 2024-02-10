using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthHup.API.Migrations
{
    /// <inheritdoc />
    public partial class AddCoulmsToDoctor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DepartmentName",
                table: "Doctors");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfJoin",
                table: "Doctors",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfSendRequest",
                table: "Doctors",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfJoin",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "DateOfSendRequest",
                table: "Doctors");

            migrationBuilder.AddColumn<string>(
                name: "DepartmentName",
                table: "Doctors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
