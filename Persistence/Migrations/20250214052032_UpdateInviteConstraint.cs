using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateInviteConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invites_AspNetUsers_AuthorId",
                table: "Invites");

            migrationBuilder.AddForeignKey(
                name: "FK_Invites_AspNetUsers_AuthorId",
                table: "Invites",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invites_AspNetUsers_AuthorId",
                table: "Invites");

            migrationBuilder.AddForeignKey(
                name: "FK_Invites_AspNetUsers_AuthorId",
                table: "Invites",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
