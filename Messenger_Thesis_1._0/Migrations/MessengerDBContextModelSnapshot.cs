﻿// <auto-generated />
using System;
using Messenger_Thesis_1._0.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Messenger_Thesis_1._0.Migrations
{
    [DbContext(typeof(MessengerDBContext))]
    partial class MessengerDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.0-rtm-35687")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Messenger_Thesis_1._0.Data.Model.Client", b =>
                {
                    b.Property<int>("ClientID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address");

                    b.Property<int>("ClientNumber");

                    b.Property<int>("ContactNumber");

                    b.Property<DateTime>("DateRegistered");

                    b.Property<string>("Email");

                    b.Property<string>("Name");

                    b.Property<string>("Password");

                    b.Property<string>("PaymentTerms");

                    b.Property<string>("Representative");

                    b.Property<string>("Status");

                    b.HasKey("ClientID");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("Messenger_Thesis_1._0.Data.Model.Feedback", b =>
                {
                    b.Property<int>("FeedbackID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateFeedbacked");

                    b.Property<string>("Message");

                    b.Property<string>("Name");

                    b.Property<int>("Rating");

                    b.Property<string>("Subject");

                    b.HasKey("FeedbackID");

                    b.ToTable("Feedbacks");
                });

            modelBuilder.Entity("Messenger_Thesis_1._0.Data.Model.Letter", b =>
                {
                    b.Property<int>("LetterID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateIns");

                    b.Property<DateTime>("DateOfDelivery");

                    b.Property<string>("LocationOfDelivery");

                    b.Property<string>("MessengerName");

                    b.Property<DateTime>("PaymentDate");

                    b.Property<int>("Price");

                    b.Property<int>("ProjectID");

                    b.Property<string>("ReceiverName");

                    b.Property<string>("SenderName");

                    b.HasKey("LetterID");

                    b.ToTable("Letters");
                });

            modelBuilder.Entity("Messenger_Thesis_1._0.Data.Model.Project", b =>
                {
                    b.Property<int>("ProjectID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Area");

                    b.Property<string>("ClientName");

                    b.Property<DateTime>("CurrentDateStart");

                    b.Property<string>("Email");

                    b.Property<string>("ImageName");

                    b.Property<DateTime>("InvoiceDate");

                    b.Property<DateTime>("LastPaymentDate");

                    b.Property<string>("PaymentTerms");

                    b.Property<float>("Price");

                    b.Property<string>("ProjectCode");

                    b.Property<string>("ProjectName");

                    b.Property<int>("Quantity");

                    b.Property<string>("Status");

                    b.Property<int>("TotalLettersPerMonth");

                    b.HasKey("ProjectID");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("Messenger_Thesis_1._0.Data.Model.User", b =>
                {
                    b.Property<int>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address");

                    b.Property<DateTime>("BirthDate");

                    b.Property<string>("CompanyName");

                    b.Property<int>("DeliveryID");

                    b.Property<string>("DeliveryStatus");

                    b.Property<string>("Email");

                    b.Property<string>("FirstName");

                    b.Property<string>("Gender");

                    b.Property<string>("ImageName");

                    b.Property<string>("LastName");

                    b.Property<string>("MiddleName");

                    b.Property<string>("Password");

                    b.Property<string>("Phonenumber");

                    b.Property<string>("Role");

                    b.Property<int>("SOAToDeliver");

                    b.Property<int>("TotalNumberOfLetters");

                    b.HasKey("UserID");

                    b.ToTable("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
