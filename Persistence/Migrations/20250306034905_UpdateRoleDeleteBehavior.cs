using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRoleDeleteBehavior : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServerRole_AspNetUsers_UserId",
                table: "ServerRole");

            migrationBuilder.DropIndex(
                name: "IX_ServerRole_RoleId",
                table: "ServerRole");

            migrationBuilder.DropIndex(
                name: "IX_ServerRole_UserId_RoleId_ServerId",
                table: "ServerRole");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "ServerRole",
                newName: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ServerRole_AppUserId",
                table: "ServerRole",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ServerRole_RoleId_ServerId",
                table: "ServerRole",
                columns: new[] { "RoleId", "ServerId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ServerRole_AspNetUsers_AppUserId",
                table: "ServerRole",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServerRole_AspNetUsers_AppUserId",
                table: "ServerRole");

            migrationBuilder.DropIndex(
                name: "IX_ServerRole_AppUserId",
                table: "ServerRole");

            migrationBuilder.DropIndex(
                name: "IX_ServerRole_RoleId_ServerId",
                table: "ServerRole");

            migrationBuilder.RenameColumn(
                name: "AppUserId",
                table: "ServerRole",
                newName: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ServerRole_RoleId",
                table: "ServerRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_ServerRole_UserId_RoleId_ServerId",
                table: "ServerRole",
                columns: new[] { "UserId", "RoleId", "ServerId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ServerRole_AspNetUsers_UserId",
                table: "ServerRole",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
