using Microsoft.EntityFrameworkCore.Migrations;

namespace BlogWeb.WebUI.Migrations
{
    public partial class UpdateSettingFooter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FrequentlyAskedPage",
                table: "Settings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrivacyPolicyPage",
                table: "Settings",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FrequentlyAskedPage",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "PrivacyPolicyPage",
                table: "Settings");
        }
    }
}
