using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserSocial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invites_AspNetUsers_CreatedBy",
                table: "Invites");

            migrationBuilder.DropForeignKey(
                name: "FK_Servers_AspNetUsers_OwnerId",
                table: "Servers");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Invites",
                newName: "AuthorId");

            migrationBuilder.RenameIndex(
                name: "IX_Invites_CreatedBy",
                table: "Invites",
                newName: "IX_Invites_AuthorId");

            migrationBuilder.CreateTable(
                name: "Friends",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    FriendId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Friends", x => new { x.UserId, x.FriendId });
                    table.ForeignKey(
                        name: "FK_Friends_AspNetUsers_FriendId",
                        column: x => x.FriendId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Friends_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserBlocks",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    BlockedUserId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBlocks", x => new { x.UserId, x.BlockedUserId });
                    table.ForeignKey(
                        name: "FK_UserBlocks_AspNetUsers_BlockedUserId",
                        column: x => x.BlockedUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserBlocks_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Friends_FriendId",
                table: "Friends",
                column: "FriendId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBlocks_BlockedUserId",
                table: "UserBlocks",
                column: "BlockedUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invites_AspNetUsers_AuthorId",
                table: "Invites",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Servers_AspNetUsers_OwnerId",
                table: "Servers",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invites_AspNetUsers_AuthorId",
                table: "Invites");

            migrationBuilder.DropForeignKey(
                name: "FK_Servers_AspNetUsers_OwnerId",
                table: "Servers");

            migrationBuilder.DropTable(
                name: "Friends");

            migrationBuilder.DropTable(
                name: "UserBlocks");

            migrationBuilder.RenameColumn(
                name: "AuthorId",
                table: "Invites",
                newName: "CreatedBy");

            migrationBuilder.RenameIndex(
                name: "IX_Invites_AuthorId",
                table: "Invites",
                newName: "IX_Invites_CreatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Invites_AspNetUsers_CreatedBy",
                table: "Invites",
                column: "CreatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Servers_AspNetUsers_OwnerId",
                table: "Servers",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
