using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthHup.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDesignMedicalSession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicalSessions_AspNetUsers_DoctorId1",
                table: "MedicalSessions");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicalSessions_Doctors_DoctorId",
                table: "MedicalSessions");

            migrationBuilder.DropForeignKey(
                name: "FK_Repentance_Drugs_drugId",
                table: "Repentance");

            migrationBuilder.DropIndex(
                name: "IX_Repentance_drugId",
                table: "Repentance");

            migrationBuilder.DropIndex(
                name: "IX_MedicalSessions_DoctorId1",
                table: "MedicalSessions");

            migrationBuilder.DropColumn(
                name: "DoctorId1",
                table: "MedicalSessions");

            migrationBuilder.AlterColumn<Guid>(
                name: "drugId",
                table: "Repentance",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "drugId1",
                table: "Repentance",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "DoctorId",
                table: "MedicalSessions",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Repentance_drugId1",
                table: "Repentance",
                column: "drugId1");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalSessions_AspNetUsers_DoctorId",
                table: "MedicalSessions",
                column: "DoctorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Repentance_Drugs_drugId1",
                table: "Repentance",
                column: "drugId1",
                principalTable: "Drugs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicalSessions_AspNetUsers_DoctorId",
                table: "MedicalSessions");

            migrationBuilder.DropForeignKey(
                name: "FK_Repentance_Drugs_drugId1",
                table: "Repentance");

            migrationBuilder.DropIndex(
                name: "IX_Repentance_drugId1",
                table: "Repentance");

            migrationBuilder.DropColumn(
                name: "drugId1",
                table: "Repentance");

            migrationBuilder.AlterColumn<string>(
                name: "drugId",
                table: "Repentance",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "DoctorId",
                table: "MedicalSessions",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DoctorId1",
                table: "MedicalSessions",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Repentance_drugId",
                table: "Repentance",
                column: "drugId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalSessions_DoctorId1",
                table: "MedicalSessions",
                column: "DoctorId1");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalSessions_AspNetUsers_DoctorId1",
                table: "MedicalSessions",
                column: "DoctorId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalSessions_Doctors_DoctorId",
                table: "MedicalSessions",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Repentance_Drugs_drugId",
                table: "Repentance",
                column: "drugId",
                principalTable: "Drugs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
