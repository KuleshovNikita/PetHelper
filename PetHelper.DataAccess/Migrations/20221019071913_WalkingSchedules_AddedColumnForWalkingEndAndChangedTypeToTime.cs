using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetHelper.DataAccess.Migrations
{
    public partial class WalkingSchedules_AddedColumnForWalkingEndAndChangedTypeToTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ScheduledTime",
                table: "WalkingSchedules");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "ScheduledEnd",
                table: "WalkingSchedules",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "ScheduledStart",
                table: "WalkingSchedules",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ScheduledEnd",
                table: "WalkingSchedules");

            migrationBuilder.DropColumn(
                name: "ScheduledStart",
                table: "WalkingSchedules");

            migrationBuilder.AddColumn<DateTime>(
                name: "ScheduledTime",
                table: "WalkingSchedules",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
