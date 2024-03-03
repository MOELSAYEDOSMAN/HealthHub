using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthHup.API.Migrations
{
    /// <inheritdoc />
    public partial class EditDiss : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientDates_Doctors_doctorId",
                table: "PatientDates");

            migrationBuilder.DropTable(
                name: "AspNetUsers_Diseases");

            migrationBuilder.DropTable(
                name: "Doctors_Diseases");

            migrationBuilder.AlterColumn<Guid>(
                name: "doctorId",
                table: "PatientDates",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Disease",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    persistent = table.Column<bool>(type: "bit", nullable: false),
                    Cured = table.Column<bool>(type: "bit", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    responsibledDoctorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Disease", x => new { x.ApplicationUserId, x.Id });
                    table.ForeignKey(
                        name: "FK_Disease_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Disease_Doctors_responsibledDoctorId",
                        column: x => x.responsibledDoctorId,
                        principalTable: "Doctors",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Disease_responsibledDoctorId",
                table: "Disease",
                column: "responsibledDoctorId");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientDates_Doctors_doctorId",
                table: "PatientDates",
                column: "doctorId",
                principalTable: "Doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientDates_Doctors_doctorId",
                table: "PatientDates");

            migrationBuilder.DropTable(
                name: "Disease");

            migrationBuilder.AlterColumn<Guid>(
                name: "doctorId",
                table: "PatientDates",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateTable(
                name: "AspNetUsers_Diseases",
                columns: table => new
                {
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    responsibledDoctorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Cured = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    persistent = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers_Diseases", x => new { x.ApplicationUserId, x.Id });
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Diseases_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Diseases_Doctors_responsibledDoctorId",
                        column: x => x.responsibledDoctorId,
                        principalTable: "Doctors",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Doctors_Diseases",
                columns: table => new
                {
                    responsibledDoctorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Cured = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    persistent = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doctors_Diseases", x => new { x.responsibledDoctorId, x.Id });
                    table.ForeignKey(
                        name: "FK_Doctors_Diseases_Doctors_responsibledDoctorId",
                        column: x => x.responsibledDoctorId,
                        principalTable: "Doctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_Diseases_responsibledDoctorId",
                table: "AspNetUsers_Diseases",
                column: "responsibledDoctorId");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientDates_Doctors_doctorId",
                table: "PatientDates",
                column: "doctorId",
                principalTable: "Doctors",
                principalColumn: "Id");
        }
    }
}
