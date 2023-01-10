using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Central_API.Migrations
{
    public partial class AddTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LocationData_Team_TeamId",
                table: "LocationData");

            migrationBuilder.DropIndex(
                name: "IX_LocationData_TeamId",
                table: "LocationData");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "KartLocationData",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "KartLocationData");

            migrationBuilder.CreateIndex(
                name: "IX_LocationData_TeamId",
                table: "LocationData",
                column: "TeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_LocationData_Team_TeamId",
                table: "LocationData",
                column: "TeamId",
                principalTable: "Team",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
