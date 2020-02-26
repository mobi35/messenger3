using Microsoft.EntityFrameworkCore.Migrations;

namespace Messenger_Thesis_1._0.Migrations
{
    public partial class addedfourrates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Feedbacks");

            migrationBuilder.AddColumn<float>(
                name: "Behaviour",
                table: "Feedbacks",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Overall",
                table: "Feedbacks",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Promptness",
                table: "Feedbacks",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Quality",
                table: "Feedbacks",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Responsiveness",
                table: "Feedbacks",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Behaviour",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "Overall",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "Promptness",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "Quality",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "Responsiveness",
                table: "Feedbacks");

            migrationBuilder.AddColumn<int>(
                name: "Rating",
                table: "Feedbacks",
                nullable: false,
                defaultValue: 0);
        }
    }
}
