using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetConnect.Data.Migrations
{
    public partial class adoptionRequestsMig2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdoptionRequests_AspNetUsers_UserId",
                table: "AdoptionRequests");

            migrationBuilder.DropIndex(
                name: "IX_AdoptionRequests_UserId",
                table: "AdoptionRequests");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AdoptionRequests");

            migrationBuilder.AlterColumn<string>(
                name: "AdopterId",
                table: "AdoptionRequests",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AdoptionRequests_AdopterId",
                table: "AdoptionRequests",
                column: "AdopterId");

            migrationBuilder.AddForeignKey(
                name: "FK_AdoptionRequests_AspNetUsers_AdopterId",
                table: "AdoptionRequests",
                column: "AdopterId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdoptionRequests_AspNetUsers_AdopterId",
                table: "AdoptionRequests");

            migrationBuilder.DropIndex(
                name: "IX_AdoptionRequests_AdopterId",
                table: "AdoptionRequests");

            migrationBuilder.AlterColumn<string>(
                name: "AdopterId",
                table: "AdoptionRequests",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "AdoptionRequests",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AdoptionRequests_UserId",
                table: "AdoptionRequests",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AdoptionRequests_AspNetUsers_UserId",
                table: "AdoptionRequests",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
