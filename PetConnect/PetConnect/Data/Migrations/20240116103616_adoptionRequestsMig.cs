using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetConnect.Data.Migrations
{
    public partial class adoptionRequestsMig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdopterId",
                table: "AdoptionRequests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "AdoptionRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdopterId",
                table: "AdoptionRequests");

            migrationBuilder.DropColumn(
                name: "Message",
                table: "AdoptionRequests");
        }
    }
}
