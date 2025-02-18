using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveInviteChannel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invites_Channels_ChannelId",
                table: "Invites");

            migrationBuilder.DropIndex(
                name: "IX_Invites_ChannelId",
                table: "Invites");

            migrationBuilder.DropColumn(
                name: "ChannelId",
                table: "Invites");

            migrationBuilder.AlterColumn<int>(
                name: "MaxUses",
                table: "Invites",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "MaxUses",
                table: "Invites",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChannelId",
                table: "Invites",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Invites_ChannelId",
                table: "Invites",
                column: "ChannelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invites_Channels_ChannelId",
                table: "Invites",
                column: "ChannelId",
                principalTable: "Channels",
                principalColumn: "ChannelId");
        }
    }
}
