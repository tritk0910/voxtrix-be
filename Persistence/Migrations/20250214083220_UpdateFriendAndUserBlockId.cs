using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFriendAndUserBlockId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserBlocks",
                table: "UserBlocks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Friends",
                table: "Friends");

            migrationBuilder.AlterColumn<string>(
                name: "BlockedUserId",
                table: "UserBlocks",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UserBlocks",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "UserBlockId",
                table: "UserBlocks",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "FriendId",
                table: "Friends",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Friends",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "FriendshipRelationId",
                table: "Friends",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserBlocks",
                table: "UserBlocks",
                column: "UserBlockId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Friends",
                table: "Friends",
                column: "FriendshipRelationId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBlocks_UserId",
                table: "UserBlocks",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Friends_UserId",
                table: "Friends",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserBlocks",
                table: "UserBlocks");

            migrationBuilder.DropIndex(
                name: "IX_UserBlocks_UserId",
                table: "UserBlocks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Friends",
                table: "Friends");

            migrationBuilder.DropIndex(
                name: "IX_Friends_UserId",
                table: "Friends");

            migrationBuilder.DropColumn(
                name: "UserBlockId",
                table: "UserBlocks");

            migrationBuilder.DropColumn(
                name: "FriendshipRelationId",
                table: "Friends");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UserBlocks",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BlockedUserId",
                table: "UserBlocks",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Friends",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FriendId",
                table: "Friends",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserBlocks",
                table: "UserBlocks",
                columns: new[] { "UserId", "BlockedUserId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Friends",
                table: "Friends",
                columns: new[] { "UserId", "FriendId" });
        }
    }
}
