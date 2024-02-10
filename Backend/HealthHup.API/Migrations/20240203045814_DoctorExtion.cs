using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthHup.API.Migrations
{
    /// <inheritdoc />
    public partial class DoctorExtion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "rate",
                table: "Reviews",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<bool>(
                name: "Accept",
                table: "Doctors",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "AddressDescrption",
                table: "Doctors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "areaId",
                table: "Doctors",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "Gender",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "DoctorCertificate",
                columns: table => new
                {
                    DoctorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    src = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorCertificate", x => new { x.DoctorId, x.Id });
                    table.ForeignKey(
                        name: "FK_DoctorCertificate_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DoctorDate",
                columns: table => new
                {
                    DoctorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DayName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    From = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    To = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorDate", x => new { x.DoctorId, x.Id });
                    table.ForeignKey(
                        name: "FK_DoctorDate_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_areaId",
                table: "Doctors",
                column: "areaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_Areas_areaId",
                table: "Doctors",
                column: "areaId",
                principalTable: "Areas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_Areas_areaId",
                table: "Doctors");

            migrationBuilder.DropTable(
                name: "DoctorCertificate");

            migrationBuilder.DropTable(
                name: "DoctorDate");

            migrationBuilder.DropIndex(
                name: "IX_Doctors_areaId",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "rate",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "Accept",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "AddressDescrption",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "areaId",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "AspNetUsers");
        }
    }
}
