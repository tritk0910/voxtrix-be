using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRoleStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServerMemberRole_ServerMembers_ServerMemberId",
                table: "ServerMemberRole");

            migrationBuilder.DropForeignKey(
                name: "FK_ServerMemberRole_ServerRole_ServerRoleId",
                table: "ServerMemberRole");

            migrationBuilder.DropTable(
                name: "ServerRole");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ServerMemberRole",
                table: "ServerMemberRole");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Roles",
                table: "Roles");

            migrationBuilder.RenameTable(
                name: "ServerMemberRole",
                newName: "ServerMemberRoles");

            migrationBuilder.RenameTable(
                name: "Roles",
                newName: "ServerRoles");

            migrationBuilder.RenameColumn(
                name: "ServerRoleId",
                table: "ServerMemberRoles",
                newName: "RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_ServerMemberRole_ServerRoleId",
                table: "ServerMemberRoles",
                newName: "IX_ServerMemberRoles_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_ServerMemberRole_ServerMemberId_ServerRoleId",
                table: "ServerMemberRoles",
                newName: "IX_ServerMemberRoles_ServerMemberId_RoleId");

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "ServerRoles",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ServerId",
                table: "ServerRoles",
                type: "text",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ServerMemberRoles",
                table: "ServerMemberRoles",
                column: "ServerMemberRoleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ServerRoles",
                table: "ServerRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_ServerRoles_AppUserId",
                table: "ServerRoles",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ServerRoles_ServerId",
                table: "ServerRoles",
                column: "ServerId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ServerMemberRoles_ServerMembers_ServerMemberId",
                table: "ServerMemberRoles",
                column: "ServerMemberId",
                principalTable: "ServerMembers",
                principalColumn: "ServerMemberId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServerMemberRoles_ServerRoles_RoleId",
                table: "ServerMemberRoles",
                column: "RoleId",
                principalTable: "ServerRoles",
                principalColumn: "RoleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServerRoles_AspNetUsers_AppUserId",
                table: "ServerRoles",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ServerRoles_Servers_ServerId",
                table: "ServerRoles",
                column: "ServerId",
                principalTable: "Servers",
                principalColumn: "ServerId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServerMemberRoles_ServerMembers_ServerMemberId",
                table: "ServerMemberRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_ServerMemberRoles_ServerRoles_RoleId",
                table: "ServerMemberRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_ServerRoles_AspNetUsers_AppUserId",
                table: "ServerRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_ServerRoles_Servers_ServerId",
                table: "ServerRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ServerRoles",
                table: "ServerRoles");

            migrationBuilder.DropIndex(
                name: "IX_ServerRoles_AppUserId",
                table: "ServerRoles");

            migrationBuilder.DropIndex(
                name: "IX_ServerRoles_ServerId",
                table: "ServerRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ServerMemberRoles",
                table: "ServerMemberRoles");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "ServerRoles");

            migrationBuilder.DropColumn(
                name: "ServerId",
                table: "ServerRoles");

            migrationBuilder.RenameTable(
                name: "ServerRoles",
                newName: "Roles");

            migrationBuilder.RenameTable(
                name: "ServerMemberRoles",
                newName: "ServerMemberRole");

            migrationBuilder.RenameColumn(
                name: "RoleId",
                table: "ServerMemberRole",
                newName: "ServerRoleId");

            migrationBuilder.RenameIndex(
                name: "IX_ServerMemberRoles_ServerMemberId_RoleId",
                table: "ServerMemberRole",
                newName: "IX_ServerMemberRole_ServerMemberId_ServerRoleId");

            migrationBuilder.RenameIndex(
                name: "IX_ServerMemberRoles_RoleId",
                table: "ServerMemberRole",
                newName: "IX_ServerMemberRole_ServerRoleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Roles",
                table: "Roles",
                column: "RoleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ServerMemberRole",
                table: "ServerMemberRole",
                column: "ServerMemberRoleId");

            migrationBuilder.CreateTable(
                name: "ServerRole",
                columns: table => new
                {
                    ServerRoleId = table.Column<string>(type: "text", nullable: false),
                    RoleId = table.Column<string>(type: "text", nullable: true),
                    ServerId = table.Column<string>(type: "text", nullable: true),
                    AppUserId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServerRole", x => x.ServerRoleId);
                    table.ForeignKey(
                        name: "FK_ServerRole_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ServerRole_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServerRole_Servers_ServerId",
                        column: x => x.ServerId,
                        principalTable: "Servers",
                        principalColumn: "ServerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ServerRole_AppUserId",
                table: "ServerRole",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ServerRole_RoleId_ServerId",
                table: "ServerRole",
                columns: new[] { "RoleId", "ServerId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ServerRole_ServerId",
                table: "ServerRole",
                column: "ServerId");

            migrationBuilder.AddForeignKey(
                name: "FK_ServerMemberRole_ServerMembers_ServerMemberId",
                table: "ServerMemberRole",
                column: "ServerMemberId",
                principalTable: "ServerMembers",
                principalColumn: "ServerMemberId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServerMemberRole_ServerRole_ServerRoleId",
                table: "ServerMemberRole",
                column: "ServerRoleId",
                principalTable: "ServerRole",
                principalColumn: "ServerRoleId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
