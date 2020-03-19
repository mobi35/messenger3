using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Messenger_Thesis_1._0.Migrations
{
    public partial class addeds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    ClientID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClientNumber = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Representative = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    ContactNumber = table.Column<int>(nullable: false),
                    Address = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    DateRegistered = table.Column<DateTime>(nullable: false),
                    Password = table.Column<string>(nullable: true),
                    PaymentTerms = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.ClientID);
                });

            migrationBuilder.CreateTable(
                name: "Feedbacks",
                columns: table => new
                {
                    FeedbackID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Subject = table.Column<string>(nullable: true),
                    Message = table.Column<string>(nullable: true),
                    Rating = table.Column<int>(nullable: false),
                    DateFeedbacked = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feedbacks", x => x.FeedbackID);
                });

            migrationBuilder.CreateTable(
                name: "Letters",
                columns: table => new
                {
                    LetterID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProjectID = table.Column<int>(nullable: false),
                    ReceiverName = table.Column<string>(nullable: true),
                    SenderName = table.Column<string>(nullable: true),
                    LocationOfDelivery = table.Column<string>(nullable: true),
                    Price = table.Column<int>(nullable: false),
                    MessengerName = table.Column<string>(nullable: true),
                    DateOfDelivery = table.Column<DateTime>(nullable: false),
                    PaymentDate = table.Column<DateTime>(nullable: false),
                    DateIns = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Letters", x => x.LetterID);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    ProjectID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProjectCode = table.Column<string>(nullable: true),
                    ClientName = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    Area = table.Column<string>(nullable: true),
                    Quantity = table.Column<int>(nullable: false),
                    Price = table.Column<float>(nullable: false),
                    ProjectName = table.Column<string>(nullable: true),
                    ImageName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    InvoiceDate = table.Column<DateTime>(nullable: false),
                    LastPaymentDate = table.Column<DateTime>(nullable: false),
                    CurrentDateStart = table.Column<DateTime>(nullable: false),
                    PaymentTerms = table.Column<string>(nullable: true),
                    TotalLettersPerMonth = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.ProjectID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Email = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    ImageName = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    MiddleName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Gender = table.Column<string>(nullable: true),
                    BirthDate = table.Column<DateTime>(nullable: false),
                    Role = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    Phonenumber = table.Column<string>(nullable: true),
                    CompanyName = table.Column<string>(nullable: true),
                    DeliveryID = table.Column<int>(nullable: false),
                    DeliveryStatus = table.Column<string>(nullable: true),
                    SOAToDeliver = table.Column<int>(nullable: false),
                    TotalNumberOfLetters = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "Feedbacks");

            migrationBuilder.DropTable(
                name: "Letters");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
