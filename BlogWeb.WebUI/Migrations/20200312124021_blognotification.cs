using Microsoft.EntityFrameworkCore.Migrations;

namespace BlogWeb.WebUI.Migrations
{
    public partial class blognotification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsMailSend",
                table: "Blogs",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsMailSend",
                table: "Blogs");
        }
    }
}
