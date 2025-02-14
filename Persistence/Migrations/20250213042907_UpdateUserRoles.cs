using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserRole");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "ServerMembers");

            migrationBuilder.AddColumn<bool>(
                name: "IsOwner",
                table: "ServerMembers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "ServerRole",
                columns: table => new
                {
                    ServerRoleId = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: true),
                    RoleId = table.Column<string>(type: "text", nullable: true),
                    ServerId = table.Column<string>(type: "text", nullable: true),
                    ServerMemberId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServerRole", x => x.ServerRoleId);
                    table.ForeignKey(
                        name: "FK_ServerRole_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ServerRole_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "RoleId");
                    table.ForeignKey(
                        name: "FK_ServerRole_ServerMembers_ServerMemberId",
                        column: x => x.ServerMemberId,
                        principalTable: "ServerMembers",
                        principalColumn: "ServerMemberId");
                    table.ForeignKey(
                        name: "FK_ServerRole_Servers_ServerId",
                        column: x => x.ServerId,
                        principalTable: "Servers",
                        principalColumn: "ServerId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ServerRole_RoleId",
                table: "ServerRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_ServerRole_ServerId",
                table: "ServerRole",
                column: "ServerId");

            migrationBuilder.CreateIndex(
                name: "IX_ServerRole_ServerMemberId",
                table: "ServerRole",
                column: "ServerMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_ServerRole_UserId_RoleId",
                table: "ServerRole",
                columns: new[] { "UserId", "RoleId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServerRole");

            migrationBuilder.DropColumn(
                name: "IsOwner",
                table: "ServerMembers");

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "ServerMembers",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    UserRoleId = table.Column<string>(type: "text", nullable: false),
                    RoleId = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => x.UserRoleId);
                    table.ForeignKey(
                        name: "FK_UserRole_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserRole_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "RoleId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_RoleId",
                table: "UserRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_UserId_RoleId",
                table: "UserRole",
                columns: new[] { "UserId", "RoleId" },
                unique: true);
        }
    }
}
