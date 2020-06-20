using Microsoft.EntityFrameworkCore.Migrations;

namespace BlogWeb.WebUI.Migrations
{
    public partial class SettingCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    SettingId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SiteName = table.Column<string>(nullable: true),
                    SiteTitle = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Keywords = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    SMTPServerHost = table.Column<string>(nullable: true),
                    SMTPServerUsername = table.Column<string>(nullable: true),
                    SMTPServerPassword = table.Column<string>(nullable: true),
                    SMTPServerPort = table.Column<string>(nullable: true),
                    SMTPServerFrom = table.Column<string>(nullable: true),
                    SMTPServerFromName = table.Column<string>(nullable: true),
                    AboutPage = table.Column<string>(nullable: true),
                    ContactPage = table.Column<string>(nullable: true),
                    Facebook = table.Column<string>(nullable: true),
                    Instagram = table.Column<string>(nullable: true),
                    Twitter = table.Column<string>(nullable: true),
                    Linkedin = table.Column<string>(nullable: true),
                    Youtube = table.Column<string>(nullable: true),
                    SocialOther1 = table.Column<string>(nullable: true),
                    SocialOther2 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.SettingId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Settings");
        }
    }
}
