using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetConnect.Data.Migrations
{
    public partial class LocationPet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Locatiom",
                table: "Pets",
                newName: "Location");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Location",
                table: "Pets",
                newName: "Locatiom");
        }
    }
}
