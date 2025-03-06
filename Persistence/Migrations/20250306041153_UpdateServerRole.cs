using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateServerRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ServerMemberRole_ServerMemberId",
                table: "ServerMemberRole");

            migrationBuilder.CreateIndex(
                name: "IX_ServerMemberRole_ServerMemberId_ServerRoleId",
                table: "ServerMemberRole",
                columns: new[] { "ServerMemberId", "ServerRoleId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ServerMemberRole_ServerMemberId_ServerRoleId",
                table: "ServerMemberRole");

            migrationBuilder.CreateIndex(
                name: "IX_ServerMemberRole_ServerMemberId",
                table: "ServerMemberRole",
                column: "ServerMemberId");
        }
    }
}
