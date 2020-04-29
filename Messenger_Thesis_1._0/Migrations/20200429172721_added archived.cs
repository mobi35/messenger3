using Microsoft.EntityFrameworkCore.Migrations;

namespace Messenger_Thesis_1._0.Migrations
{
    public partial class addedarchived : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Archived",
                table: "Contracts",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Archived",
                table: "Contracts");
        }
    }
}
