using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetConnect.Data.Migrations
{
    public partial class AddAprrovePet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Approved",
                table: "Pets",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Approved",
                table: "Pets");
        }
    }
}
