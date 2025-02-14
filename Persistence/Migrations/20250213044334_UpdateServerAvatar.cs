using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateServerAvatar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invite_AspNetUsers_CreatedBy",
                table: "Invite");

            migrationBuilder.DropForeignKey(
                name: "FK_Invite_Channels_ChannelId",
                table: "Invite");

            migrationBuilder.DropForeignKey(
                name: "FK_Invite_Servers_ServerId",
                table: "Invite");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Invite",
                table: "Invite");

            migrationBuilder.RenameTable(
                name: "Invite",
                newName: "Invites");

            migrationBuilder.RenameColumn(
                name: "Image",
                table: "Servers",
                newName: "Avatar");

            migrationBuilder.RenameIndex(
                name: "IX_Invite_ServerId",
                table: "Invites",
                newName: "IX_Invites_ServerId");

            migrationBuilder.RenameIndex(
                name: "IX_Invite_InviteCode",
                table: "Invites",
                newName: "IX_Invites_InviteCode");

            migrationBuilder.RenameIndex(
                name: "IX_Invite_CreatedBy",
                table: "Invites",
                newName: "IX_Invites_CreatedBy");

            migrationBuilder.RenameIndex(
                name: "IX_Invite_ChannelId",
                table: "Invites",
                newName: "IX_Invites_ChannelId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Invites",
                table: "Invites",
                column: "InviteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invites_AspNetUsers_CreatedBy",
                table: "Invites",
                column: "CreatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Invites_Channels_ChannelId",
                table: "Invites",
                column: "ChannelId",
                principalTable: "Channels",
                principalColumn: "ChannelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invites_Servers_ServerId",
                table: "Invites",
                column: "ServerId",
                principalTable: "Servers",
                principalColumn: "ServerId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invites_AspNetUsers_CreatedBy",
                table: "Invites");

            migrationBuilder.DropForeignKey(
                name: "FK_Invites_Channels_ChannelId",
                table: "Invites");

            migrationBuilder.DropForeignKey(
                name: "FK_Invites_Servers_ServerId",
                table: "Invites");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Invites",
                table: "Invites");

            migrationBuilder.RenameTable(
                name: "Invites",
                newName: "Invite");

            migrationBuilder.RenameColumn(
                name: "Avatar",
                table: "Servers",
                newName: "Image");

            migrationBuilder.RenameIndex(
                name: "IX_Invites_ServerId",
                table: "Invite",
                newName: "IX_Invite_ServerId");

            migrationBuilder.RenameIndex(
                name: "IX_Invites_InviteCode",
                table: "Invite",
                newName: "IX_Invite_InviteCode");

            migrationBuilder.RenameIndex(
                name: "IX_Invites_CreatedBy",
                table: "Invite",
                newName: "IX_Invite_CreatedBy");

            migrationBuilder.RenameIndex(
                name: "IX_Invites_ChannelId",
                table: "Invite",
                newName: "IX_Invite_ChannelId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Invite",
                table: "Invite",
                column: "InviteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invite_AspNetUsers_CreatedBy",
                table: "Invite",
                column: "CreatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Invite_Channels_ChannelId",
                table: "Invite",
                column: "ChannelId",
                principalTable: "Channels",
                principalColumn: "ChannelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invite_Servers_ServerId",
                table: "Invite",
                column: "ServerId",
                principalTable: "Servers",
                principalColumn: "ServerId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
