using Microsoft.EntityFrameworkCore.Migrations;

namespace BlogWeb.WebUI.Migrations
{
    public partial class SettingOthers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SocialOther1",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "SocialOther2",
                table: "Settings");

            migrationBuilder.AddColumn<string>(
                name: "FacebookSharing",
                table: "Settings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LinkedInSharing",
                table: "Settings",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TakeAnswerCount",
                table: "Settings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TakeBlogCount",
                table: "Settings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TakeCommentCount",
                table: "Settings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TakeSideLastPublishedCount",
                table: "Settings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TakeSideMostCommentCount",
                table: "Settings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TakeSideMostReadCount",
                table: "Settings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TakeSideRandomBlogCount",
                table: "Settings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TwitterSharing",
                table: "Settings",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FacebookSharing",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "LinkedInSharing",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "TakeAnswerCount",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "TakeBlogCount",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "TakeCommentCount",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "TakeSideLastPublishedCount",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "TakeSideMostCommentCount",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "TakeSideMostReadCount",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "TakeSideRandomBlogCount",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "TwitterSharing",
                table: "Settings");

            migrationBuilder.AddColumn<string>(
                name: "SocialOther1",
                table: "Settings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SocialOther2",
                table: "Settings",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
