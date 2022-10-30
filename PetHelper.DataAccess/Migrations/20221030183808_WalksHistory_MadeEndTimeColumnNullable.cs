using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetHelper.DataAccess.Migrations
{
    public partial class WalksHistory_MadeEndTimeColumnNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsGeneralData",
                table: "IdlePetStatistic",
                newName: "IsUnifiedAnimalData");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndTime",
                table: "WalksHistory",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsUnifiedAnimalData",
                table: "IdlePetStatistic",
                newName: "IsGeneralData");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndTime",
                table: "WalksHistory",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}
