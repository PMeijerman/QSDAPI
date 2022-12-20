using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Central_API.Migrations
{
    public partial class added_team_property : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "University",
                table: "Team",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "University",
                table: "Team");
        }
    }
}
