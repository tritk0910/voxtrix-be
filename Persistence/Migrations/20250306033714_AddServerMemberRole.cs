using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddServerMemberRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServerRole_ServerMembers_ServerMemberId",
                table: "ServerRole");

            migrationBuilder.DropIndex(
                name: "IX_ServerRole_ServerMemberId",
                table: "ServerRole");

            migrationBuilder.DropColumn(
                name: "ServerMemberId",
                table: "ServerRole");

            migrationBuilder.CreateTable(
                name: "ServerMemberRole",
                columns: table => new
                {
                    ServerMemberRoleId = table.Column<string>(type: "text", nullable: false),
                    ServerMemberId = table.Column<string>(type: "text", nullable: true),
                    ServerRoleId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServerMemberRole", x => x.ServerMemberRoleId);
                    table.ForeignKey(
                        name: "FK_ServerMemberRole_ServerMembers_ServerMemberId",
                        column: x => x.ServerMemberId,
                        principalTable: "ServerMembers",
                        principalColumn: "ServerMemberId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServerMemberRole_ServerRole_ServerRoleId",
                        column: x => x.ServerRoleId,
                        principalTable: "ServerRole",
                        principalColumn: "ServerRoleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ServerMemberRole_ServerMemberId",
                table: "ServerMemberRole",
                column: "ServerMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_ServerMemberRole_ServerRoleId",
                table: "ServerMemberRole",
                column: "ServerRoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServerMemberRole");

            migrationBuilder.AddColumn<string>(
                name: "ServerMemberId",
                table: "ServerRole",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ServerRole_ServerMemberId",
                table: "ServerRole",
                column: "ServerMemberId");

            migrationBuilder.AddForeignKey(
                name: "FK_ServerRole_ServerMembers_ServerMemberId",
                table: "ServerRole",
                column: "ServerMemberId",
                principalTable: "ServerMembers",
                principalColumn: "ServerMemberId");
        }
    }
}
