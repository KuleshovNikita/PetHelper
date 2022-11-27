using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetHelper.DataAccess.Migrations
{
    public partial class Pets_AddedIsWalkingColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsWalking",
                table: "Pets",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsWalking",
                table: "Pets");
        }
    }
}
