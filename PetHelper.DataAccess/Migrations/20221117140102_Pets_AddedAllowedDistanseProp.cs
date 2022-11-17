using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetHelper.DataAccess.Migrations
{
    public partial class Pets_AddedAllowedDistanseProp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "AllowedDistance",
                table: "Pets",
                type: "float",
                nullable: false,
                defaultValue: 15.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllowedDistance",
                table: "Pets");
        }
    }
}
