using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MineServer.Migrations
{
    public partial class update3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LinkedSpiderId",
                table: "db_template",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LinkedSpiderId",
                table: "db_template");
        }
    }
}
