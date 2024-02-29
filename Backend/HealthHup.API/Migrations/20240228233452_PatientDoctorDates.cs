using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthHup.API.Migrations
{
    /// <inheritdoc />
    public partial class PatientDoctorDates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PatientDates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    patientId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    doctorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FromTime = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientDates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatientDates_AspNetUsers_patientId",
                        column: x => x.patientId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PatientDates_Doctors_doctorId",
                        column: x => x.doctorId,
                        principalTable: "Doctors",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PatientDates_doctorId",
                table: "PatientDates",
                column: "doctorId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientDates_patientId",
                table: "PatientDates",
                column: "patientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PatientDates");
        }
    }
}
