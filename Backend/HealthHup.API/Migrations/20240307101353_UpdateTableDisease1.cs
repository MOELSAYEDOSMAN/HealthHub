using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthHup.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableDisease1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Disease",
                table: "Disease");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Disease",
                table: "Disease",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Disease_PatientId",
                table: "Disease",
                column: "PatientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Disease",
                table: "Disease");

            migrationBuilder.DropIndex(
                name: "IX_Disease_PatientId",
                table: "Disease");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Disease",
                table: "Disease",
                columns: new[] { "PatientId", "Id" });
        }
    }
}
