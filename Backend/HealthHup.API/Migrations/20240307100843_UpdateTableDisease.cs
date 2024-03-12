using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthHup.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableDisease : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Disease_AspNetUsers_ApplicationUserId",
                table: "Disease");

            migrationBuilder.DropForeignKey(
                name: "FK_Repentance_Drugs_drugId1",
                table: "Repentance");

            migrationBuilder.DropIndex(
                name: "IX_Repentance_drugId1",
                table: "Repentance");

            migrationBuilder.DropColumn(
                name: "drugId1",
                table: "Repentance");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId",
                table: "Disease",
                newName: "PatientId");

            migrationBuilder.AlterColumn<string>(
                name: "drugId",
                table: "Repentance",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateIndex(
                name: "IX_Repentance_drugId",
                table: "Repentance",
                column: "drugId");

            migrationBuilder.AddForeignKey(
                name: "FK_Disease_AspNetUsers_PatientId",
                table: "Disease",
                column: "PatientId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Repentance_Drugs_drugId",
                table: "Repentance",
                column: "drugId",
                principalTable: "Drugs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Disease_AspNetUsers_PatientId",
                table: "Disease");

            migrationBuilder.DropForeignKey(
                name: "FK_Repentance_Drugs_drugId",
                table: "Repentance");

            migrationBuilder.DropIndex(
                name: "IX_Repentance_drugId",
                table: "Repentance");

            migrationBuilder.RenameColumn(
                name: "PatientId",
                table: "Disease",
                newName: "ApplicationUserId");

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

            migrationBuilder.CreateIndex(
                name: "IX_Repentance_drugId1",
                table: "Repentance",
                column: "drugId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Disease_AspNetUsers_ApplicationUserId",
                table: "Disease",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Repentance_Drugs_drugId1",
                table: "Repentance",
                column: "drugId1",
                principalTable: "Drugs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
