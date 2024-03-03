using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthHup.API.Migrations
{
    /// <inheritdoc />
    public partial class EditDesignDoctor_MedicalSession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicalSessions_AspNetUsers_DoctorId",
                table: "MedicalSessions");

            migrationBuilder.AlterColumn<Guid>(
                name: "DoctorId",
                table: "MedicalSessions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalSessions_Doctors_DoctorId",
                table: "MedicalSessions",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicalSessions_Doctors_DoctorId",
                table: "MedicalSessions");

            migrationBuilder.AlterColumn<string>(
                name: "DoctorId",
                table: "MedicalSessions",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalSessions_AspNetUsers_DoctorId",
                table: "MedicalSessions",
                column: "DoctorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
