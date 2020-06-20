using Microsoft.EntityFrameworkCore.Migrations;

namespace BlogWeb.WebUI.Migrations
{
    public partial class UpBlogAndSetting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Keywords",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "Keywords",
                table: "Blogs");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Keywords",
                table: "Settings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Keywords",
                table: "Blogs",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
