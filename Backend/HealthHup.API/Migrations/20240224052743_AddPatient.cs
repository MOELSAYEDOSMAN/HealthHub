using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthHup.API.Migrations
{
    /// <inheritdoc />
    public partial class AddPatient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "priceSession",
                table: "Doctors",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "AspNetUsers_Diseases",
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
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    responsibledDoctorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    persistent = table.Column<bool>(type: "bit", nullable: false),
                    Cured = table.Column<bool>(type: "bit", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "MedicalSessions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiseaseName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DoctorId1 = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PatientId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    DoctorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicalSessions_AspNetUsers_DoctorId1",
                        column: x => x.DoctorId1,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MedicalSessions_AspNetUsers_PatientId",
                        column: x => x.PatientId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MedicalSessions_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctors",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Repentance",
                columns: table => new
                {
                    MedicalSessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    drugId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Repeat = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RepeatCount = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Repentance", x => new { x.MedicalSessionId, x.Id });
                    table.ForeignKey(
                        name: "FK_Repentance_Drugs_drugId",
                        column: x => x.drugId,
                        principalTable: "Drugs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Repentance_MedicalSessions_MedicalSessionId",
                        column: x => x.MedicalSessionId,
                        principalTable: "MedicalSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_Diseases_responsibledDoctorId",
                table: "AspNetUsers_Diseases",
                column: "responsibledDoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalSessions_DoctorId",
                table: "MedicalSessions",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalSessions_DoctorId1",
                table: "MedicalSessions",
                column: "DoctorId1");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalSessions_PatientId",
                table: "MedicalSessions",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Repentance_drugId",
                table: "Repentance",
                column: "drugId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetUsers_Diseases");

            migrationBuilder.DropTable(
                name: "Doctors_Diseases");

            migrationBuilder.DropTable(
                name: "Repentance");

            migrationBuilder.DropTable(
                name: "MedicalSessions");

            migrationBuilder.DropColumn(
                name: "priceSession",
                table: "Doctors");
        }
    }
}
