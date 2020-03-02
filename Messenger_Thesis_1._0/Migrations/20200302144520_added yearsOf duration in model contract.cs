using Microsoft.EntityFrameworkCore.Migrations;

namespace Messenger_Thesis_1._0.Migrations
{
    public partial class addedyearsOfdurationinmodelcontract : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "YearsOfDuration",
                table: "Contracts",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "YearsOfDuration",
                table: "Contracts");
        }
    }
}
