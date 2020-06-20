using Microsoft.EntityFrameworkCore.Migrations;

namespace BlogWeb.WebUI.Migrations
{
    public partial class CatUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Keywords",
                table: "Categories");

            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "Categories",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "Categories");

            migrationBuilder.AddColumn<string>(
                name: "Keywords",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
