using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MineServer.Migrations
{
    public partial class addreplacementrule1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TemplateReplaceRules",
                table: "TemplateReplaceRules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReplacementRules",
                table: "ReplacementRules");

            migrationBuilder.RenameTable(
                name: "TemplateReplaceRules",
                newName: "db_templatereplacementrule");

            migrationBuilder.RenameTable(
                name: "ReplacementRules",
                newName: "db_replacementrule");

            migrationBuilder.AddPrimaryKey(
                name: "PK_db_templatereplacementrule",
                table: "db_templatereplacementrule",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_db_replacementrule",
                table: "db_replacementrule",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_db_templatereplacementrule",
                table: "db_templatereplacementrule");

            migrationBuilder.DropPrimaryKey(
                name: "PK_db_replacementrule",
                table: "db_replacementrule");

            migrationBuilder.RenameTable(
                name: "db_templatereplacementrule",
                newName: "TemplateReplaceRules");

            migrationBuilder.RenameTable(
                name: "db_replacementrule",
                newName: "ReplacementRules");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TemplateReplaceRules",
                table: "TemplateReplaceRules",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReplacementRules",
                table: "ReplacementRules",
                column: "Id");
        }
    }
}
