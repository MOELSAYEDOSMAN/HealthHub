using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthHup.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDrug : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Areas_areaId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "ActivePharmaceuticalDrug");

            migrationBuilder.DropTable(
                name: "pharmacyDrugs");

            migrationBuilder.DropTable(
                name: "ActivePharmaceuticals");

            migrationBuilder.DropTable(
                name: "Drugs");

            migrationBuilder.DropTable(
                name: "pharmacies");

            migrationBuilder.RenameColumn(
                name: "areaId",
                table: "AspNetUsers",
                newName: "AreaId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_areaId",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_AreaId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Areas_AreaId",
                table: "AspNetUsers",
                column: "AreaId",
                principalTable: "Areas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Areas_AreaId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "AreaId",
                table: "AspNetUsers",
                newName: "areaId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_AreaId",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_areaId");

            migrationBuilder.CreateTable(
                name: "ActivePharmaceuticals",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    precent = table.Column<double>(type: "float", nullable: false),
                    type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivePharmaceuticals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Drugs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drugs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "pharmacies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    areaId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    xLoc = table.Column<double>(type: "float", nullable: true),
                    yLoc = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pharmacies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_pharmacies_Areas_areaId",
                        column: x => x.areaId,
                        principalTable: "Areas",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ActivePharmaceuticalDrug",
                columns: table => new
                {
                    activePharmaceuticalsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    drugsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivePharmaceuticalDrug", x => new { x.activePharmaceuticalsId, x.drugsId });
                    table.ForeignKey(
                        name: "FK_ActivePharmaceuticalDrug_ActivePharmaceuticals_activePharmaceuticalsId",
                        column: x => x.activePharmaceuticalsId,
                        principalTable: "ActivePharmaceuticals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActivePharmaceuticalDrug_Drugs_drugsId",
                        column: x => x.drugsId,
                        principalTable: "Drugs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "pharmacyDrugs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DrugId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PharmacyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    Expire = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pharmacyDrugs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_pharmacyDrugs_Drugs_DrugId",
                        column: x => x.DrugId,
                        principalTable: "Drugs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_pharmacyDrugs_pharmacies_PharmacyId",
                        column: x => x.PharmacyId,
                        principalTable: "pharmacies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActivePharmaceuticalDrug_drugsId",
                table: "ActivePharmaceuticalDrug",
                column: "drugsId");

            migrationBuilder.CreateIndex(
                name: "IX_pharmacies_areaId",
                table: "pharmacies",
                column: "areaId");

            migrationBuilder.CreateIndex(
                name: "IX_pharmacyDrugs_DrugId",
                table: "pharmacyDrugs",
                column: "DrugId");

            migrationBuilder.CreateIndex(
                name: "IX_pharmacyDrugs_PharmacyId",
                table: "pharmacyDrugs",
                column: "PharmacyId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Areas_areaId",
                table: "AspNetUsers",
                column: "areaId",
                principalTable: "Areas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
