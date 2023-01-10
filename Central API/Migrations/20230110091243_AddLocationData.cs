using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Central_API.Migrations
{
    public partial class AddLocationData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KartLocationData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KartLongitude = table.Column<double>(type: "float", nullable: false),
                    KartLatitude = table.Column<double>(type: "float", nullable: false),
                    CenterlineLongitude = table.Column<double>(type: "float", nullable: false),
                    CenterlineLatitude = table.Column<double>(type: "float", nullable: false),
                    DistanceTraveled = table.Column<double>(type: "float", nullable: false),
                    TeamId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KartLocationData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KartLocationData_Team_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Team",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PassedPoint",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Point = table.Column<int>(type: "int", nullable: false),
                    TeamId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PassedPoint", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PassedPoint_Team_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Team",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_KartLocationData_TeamId",
                table: "KartLocationData",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_PassedPoint_TeamId",
                table: "PassedPoint",
                column: "TeamId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KartLocationData");

            migrationBuilder.DropTable(
                name: "PassedPoint");
        }
    }
}
