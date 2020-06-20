using Microsoft.EntityFrameworkCore.Migrations;

namespace BlogWeb.WebUI.Migrations
{
    public partial class BlogUpExplataion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Explanation",
                table: "Blogs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Explanation",
                table: "Blogs");
        }
    }
}
