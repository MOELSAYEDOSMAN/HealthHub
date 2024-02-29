using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthHup.API.Migrations
{
    /// <inheritdoc />
    public partial class ChatMessages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SendMessage = table.Column<bool>(type: "bit", nullable: false),
                    AccseptedMessages = table.Column<bool>(type: "bit", nullable: false),
                    UserSendId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UserReciveId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Groups_AspNetUsers_UserReciveId",
                        column: x => x.UserReciveId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Groups_AspNetUsers_UserSendId",
                        column: x => x.UserSendId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Message",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserSendId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UserReciveId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    dateTiemSendMessage = table.Column<DateTime>(type: "datetime2", nullable: false),
                    See = table.Column<bool>(type: "bit", nullable: false),
                    GroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Message", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Message_AspNetUsers_UserReciveId",
                        column: x => x.UserReciveId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Message_AspNetUsers_UserSendId",
                        column: x => x.UserSendId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Message_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Groups_UserReciveId",
                table: "Groups",
                column: "UserReciveId");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_UserSendId",
                table: "Groups",
                column: "UserSendId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_GroupId",
                table: "Message",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_UserReciveId",
                table: "Message",
                column: "UserReciveId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_UserSendId",
                table: "Message",
                column: "UserSendId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Message");

            migrationBuilder.DropTable(
                name: "Groups");
        }
    }
}
