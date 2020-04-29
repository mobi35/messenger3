using Microsoft.EntityFrameworkCore.Migrations;

namespace Messenger_Thesis_1._0.Migrations
{
    public partial class archivecreatedbyuser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "UserArchive",
                table: "Projects",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "UserArchive",
                table: "Contracts",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserArchive",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "UserArchive",
                table: "Contracts");
        }
    }
}
