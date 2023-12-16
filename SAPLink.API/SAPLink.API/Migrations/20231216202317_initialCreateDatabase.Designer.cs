﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SAPLink.EF.Data;

#nullable disable

namespace SAPLink.API.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20231216202317_initialCreateDatabase")]
    partial class initialCreateDatabase
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.9");

            modelBuilder.Entity("SAPLink.Core.Models.Schedules", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1L)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Document")
                        .HasColumnType("INTEGER");

                    b.Property<string>("DocumentName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Schedules", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Document = 1,
                            DocumentName = "Items"
                        },
                        new
                        {
                            Id = 2,
                            Document = 2,
                            DocumentName = "GoodsReceiptPos"
                        },
                        new
                        {
                            Id = 3,
                            Document = 3,
                            DocumentName = "SalesInvoices"
                        },
                        new
                        {
                            Id = 4,
                            Document = 4,
                            DocumentName = "ReturnInvoices"
                        },
                        new
                        {
                            Id = 5,
                            Document = 5,
                            DocumentName = "StockTransfers"
                        });
                });

            modelBuilder.Entity("SAPLink.Core.Models.System.Clients", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Active")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Clients", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Active = false,
                            Name = "Al-Kaffary Subsidiary - SAP Live DB (KaffaryDB)"
                        },
                        new
                        {
                            Id = 2,
                            Active = false,
                            Name = "Test Subsidiary - SAP Test DB (TESTDB)"
                        },
                        new
                        {
                            Id = 3,
                            Active = true,
                            Name = "Fakeeh Vision Subsidiary - SAP Local DB (SBODemoGB)"
                        });
                });

            modelBuilder.Entity("SAPLink.Core.Models.System.Credentials", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Active")
                        .HasColumnType("INTEGER");

                    b.Property<string>("AuthPassword")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("AuthSession")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("AuthUserName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Authorization")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("BackOfficeUri")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("BaseUri")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("ClientId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("CommonUri")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("CompanyDb")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Cookie")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("DbPassword")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("DbUserName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("EnvironmentCode")
                        .HasColumnType("INTEGER");

                    b.Property<string>("EnvironmentName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("IntegrationUrl")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Origin")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PrismPassword")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PrismUserName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Referer")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("RestUri")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Server")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("ServerTypes")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ServiceLayerUri")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.ToTable("Credentials", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Active = false,
                            AuthPassword = "",
                            AuthSession = "",
                            AuthUserName = "{{\"UserName\" : \"manager\",\"CompanyDB\" : \"\"}}",
                            Authorization = "",
                            BackOfficeUri = "http://kaffaryretail.alkaffary.com:8080/api/backoffice",
                            BaseUri = "http://kaffaryretail.alkaffary.com:8080",
                            ClientId = 1,
                            CommonUri = "http://kaffaryretail.alkaffary.com:8080/v1/rest",
                            CompanyDb = "",
                            Cookie = "",
                            DbPassword = "sap123456*",
                            DbUserName = "sa",
                            EnvironmentCode = 1,
                            EnvironmentName = "Production Environment",
                            IntegrationUrl = "https://localhost:44326",
                            Origin = "http://kaffaryretail.alkaffary.com:8080",
                            Password = "",
                            PrismPassword = "RetailTec@123",
                            PrismUserName = "SAPLINK",
                            Referer = "http://kaffaryretail.alkaffary.com:8080/prism.shtml",
                            RestUri = "http://kaffaryretail.alkaffary.com:8080/api/common",
                            Server = "SAP-TEST",
                            ServerTypes = 10,
                            ServiceLayerUri = "https://sap-test:50000/b1s/v1/",
                            UserName = "manager"
                        },
                        new
                        {
                            Id = 2,
                            Active = false,
                            AuthPassword = "Ag123456*",
                            AuthSession = "369B7B1BF58F469896B06B804BFBE272",
                            AuthUserName = "{{\"UserName\" : \"manager\",\"CompanyDB\" : \"TESTDB\"}}",
                            Authorization = "Basic eyJVc2VyTmFtZSI6ICJtYW5hZ2VyIiwgIkNvbXBhbnlEQiI6ICJURVNUREIifTpBZzEyMzQ1Nio=",
                            BackOfficeUri = "http://postest.alkaffary.com:8080/api/backoffice",
                            BaseUri = "http://postest.alkaffary.com:8080",
                            ClientId = 2,
                            CommonUri = "http://postest.alkaffary.com:8080/v1/rest",
                            CompanyDb = "TESTDB",
                            Cookie = "",
                            DbPassword = "sap123456*",
                            DbUserName = "sa",
                            EnvironmentCode = 2,
                            EnvironmentName = "Test Environment",
                            IntegrationUrl = "https://localhost:44326",
                            Origin = "http://postest.alkaffary.com:8080",
                            Password = "Ag123456*",
                            PrismPassword = "kaf@admin",
                            PrismUserName = "sysadmin",
                            Referer = "http://postest.alkaffary.com:8080/prism.shtml",
                            RestUri = "http://postest.alkaffary.com:8080/api/common",
                            Server = "SAP-TEST",
                            ServerTypes = 10,
                            ServiceLayerUri = "https://sap-test:50000/b1s/v1/",
                            UserName = "manager"
                        },
                        new
                        {
                            Id = 3,
                            Active = true,
                            AuthPassword = "manager",
                            AuthSession = "F1726B4EC6304D969ED816D844617C02",
                            AuthUserName = "{{\"UserName\" : \"manager\",\"CompanyDB\" : \"SBODemoGB\"}}",
                            Authorization = "Basic eyJVc2VyTmFtZSI6ICJtYW5hZ2VyIiwgIkNvbXBhbnlEQiI6ICJTQk9EZW1vR0IifTptYW5hZ2Vy",
                            BackOfficeUri = "http://194.163.155.105/api/backoffice",
                            BaseUri = "http://194.163.155.105",
                            ClientId = 3,
                            CommonUri = "http://194.163.155.105/v1/rest",
                            CompanyDb = "SBODemoGB",
                            Cookie = "",
                            DbPassword = "P@ssw0rd",
                            DbUserName = "sa",
                            EnvironmentCode = 3,
                            EnvironmentName = "Local Environment",
                            IntegrationUrl = "https://localhost:44326",
                            Origin = "http://194.163.155.105",
                            Password = "manager",
                            PrismPassword = "sysadmin",
                            PrismUserName = "sysadmin",
                            Referer = "http://194.163.155.105/prism.shtml",
                            RestUri = "http://194.163.155.105/api/common",
                            Server = "ABOALIA",
                            ServerTypes = 15,
                            ServiceLayerUri = "https://Localhost:50000/b1s/v1/",
                            UserName = "manager"
                        });
                });

            modelBuilder.Entity("SAPLink.Core.Models.System.Recurrence", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1L)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("DayOfWeek")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Document")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Interval")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Recurring")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Recurrences", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            DayOfWeek = 0,
                            Document = 0,
                            Interval = 2,
                            Recurring = 1
                        },
                        new
                        {
                            Id = 2,
                            DayOfWeek = 0,
                            Document = 1,
                            Interval = 2,
                            Recurring = 1
                        },
                        new
                        {
                            Id = 3,
                            DayOfWeek = 0,
                            Document = 2,
                            Interval = 2,
                            Recurring = 1
                        },
                        new
                        {
                            Id = 4,
                            DayOfWeek = 0,
                            Document = 3,
                            Interval = 2,
                            Recurring = 1
                        });
                });

            modelBuilder.Entity("SAPLink.Core.Models.System.Subsidiaries", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ActivePriceLevelid")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ActiveSeasonSid")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ActiveStoreSid")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ActiveTaxCode")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Clerksid")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("CredentialId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Number")
                        .HasColumnType("INTEGER");

                    b.Property<long>("SID")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("CredentialId");

                    b.ToTable("Subsidiaries", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ActivePriceLevelid = "664651377000135721",
                            ActiveSeasonSid = "664651377000169734",
                            ActiveStoreSid = "664651285000116261",
                            ActiveTaxCode = "664651377000183746",
                            Clerksid = "674955099100039866",
                            CredentialId = 1,
                            Name = "AlKaffary - (Production)",
                            Number = 1,
                            SID = 664651285000113257L
                        },
                        new
                        {
                            Id = 2,
                            ActivePriceLevelid = "663852140000113721",
                            ActiveSeasonSid = "663852140000143734",
                            ActiveStoreSid = "674650601000132347",
                            ActiveTaxCode = "663852140000157746",
                            Clerksid = "674654182000171601",
                            CredentialId = 2,
                            Name = "AlKaffary - (Test)",
                            Number = 1,
                            SID = 663852103000153257L
                        },
                        new
                        {
                            Id = 3,
                            ActivePriceLevelid = "675951940000193772",
                            ActiveSeasonSid = "675951941000138785",
                            ActiveStoreSid = "675951888000150261",
                            ActiveTaxCode = "",
                            Clerksid = "675951888000149260",
                            CredentialId = 3,
                            Name = "Local Environment (Public API)",
                            Number = 1,
                            SID = 675951888000146257L
                        });
                });

            modelBuilder.Entity("SAPLink.Core.Models.System.Sync", b =>
                {
                    b.Property<int>("UpdateType")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Date")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("UpdateType");

                    b.ToTable("Sync", (string)null);

                    b.HasData(
                        new
                        {
                            UpdateType = 0,
                            Date = new DateTime(2023, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "InitialDepartment"
                        },
                        new
                        {
                            UpdateType = 1,
                            Date = new DateTime(2023, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "SyncDepartment"
                        },
                        new
                        {
                            UpdateType = 2,
                            Date = new DateTime(2023, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "InitialVendors"
                        },
                        new
                        {
                            UpdateType = 3,
                            Date = new DateTime(2023, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "SyncVendors"
                        },
                        new
                        {
                            UpdateType = 4,
                            Date = new DateTime(2023, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "InitialItems"
                        },
                        new
                        {
                            UpdateType = 5,
                            Date = new DateTime(2023, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "SyncItems"
                        },
                        new
                        {
                            UpdateType = 6,
                            Date = new DateTime(2023, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "InitialGoodsReceiptPO"
                        },
                        new
                        {
                            UpdateType = 7,
                            Date = new DateTime(2023, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "SyncGoodsReceiptPO"
                        });
                });

            modelBuilder.Entity("SAPLink.Core.Models.Times", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1L)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ScheduleId")
                        .HasColumnType("INTEGER");

                    b.Property<TimeOnly>("Time")
                        .HasColumnType("TEXT");

                    b.Property<int>("TimeId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ScheduleId");

                    b.ToTable("Times", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Active = true,
                            ScheduleId = 1,
                            Time = new TimeOnly(7, 0, 0),
                            TimeId = 1
                        },
                        new
                        {
                            Id = 2,
                            Active = true,
                            ScheduleId = 1,
                            Time = new TimeOnly(12, 0, 0),
                            TimeId = 2
                        },
                        new
                        {
                            Id = 3,
                            Active = true,
                            ScheduleId = 1,
                            Time = new TimeOnly(17, 0, 0),
                            TimeId = 3
                        },
                        new
                        {
                            Id = 4,
                            Active = true,
                            ScheduleId = 2,
                            Time = new TimeOnly(7, 0, 0),
                            TimeId = 1
                        },
                        new
                        {
                            Id = 5,
                            Active = true,
                            ScheduleId = 2,
                            Time = new TimeOnly(12, 0, 0),
                            TimeId = 2
                        },
                        new
                        {
                            Id = 6,
                            Active = true,
                            ScheduleId = 2,
                            Time = new TimeOnly(17, 0, 0),
                            TimeId = 3
                        },
                        new
                        {
                            Id = 7,
                            Active = true,
                            ScheduleId = 3,
                            Time = new TimeOnly(13, 0, 0),
                            TimeId = 1
                        },
                        new
                        {
                            Id = 8,
                            Active = true,
                            ScheduleId = 3,
                            Time = new TimeOnly(18, 0, 0),
                            TimeId = 2
                        },
                        new
                        {
                            Id = 9,
                            Active = true,
                            ScheduleId = 3,
                            Time = new TimeOnly(0, 0, 0),
                            TimeId = 3
                        },
                        new
                        {
                            Id = 10,
                            Active = true,
                            ScheduleId = 4,
                            Time = new TimeOnly(13, 0, 0),
                            TimeId = 1
                        },
                        new
                        {
                            Id = 11,
                            Active = true,
                            ScheduleId = 4,
                            Time = new TimeOnly(18, 0, 0),
                            TimeId = 2
                        },
                        new
                        {
                            Id = 12,
                            Active = true,
                            ScheduleId = 4,
                            Time = new TimeOnly(0, 0, 0),
                            TimeId = 3
                        },
                        new
                        {
                            Id = 13,
                            Active = true,
                            ScheduleId = 5,
                            Time = new TimeOnly(13, 0, 0),
                            TimeId = 1
                        },
                        new
                        {
                            Id = 14,
                            Active = true,
                            ScheduleId = 5,
                            Time = new TimeOnly(18, 0, 0),
                            TimeId = 2
                        },
                        new
                        {
                            Id = 15,
                            Active = true,
                            ScheduleId = 5,
                            Time = new TimeOnly(0, 0, 0),
                            TimeId = 3
                        });
                });

            modelBuilder.Entity("SAPLink.Core.Models.System.Credentials", b =>
                {
                    b.HasOne("SAPLink.Core.Models.System.Clients", "Client")
                        .WithMany("Credentials")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");
                });

            modelBuilder.Entity("SAPLink.Core.Models.System.Subsidiaries", b =>
                {
                    b.HasOne("SAPLink.Core.Models.System.Credentials", "Credential")
                        .WithMany("Subsidiaries")
                        .HasForeignKey("CredentialId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Credential");
                });

            modelBuilder.Entity("SAPLink.Core.Models.Times", b =>
                {
                    b.HasOne("SAPLink.Core.Models.Schedules", "Schedule")
                        .WithMany("Times")
                        .HasForeignKey("ScheduleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Schedule");
                });

            modelBuilder.Entity("SAPLink.Core.Models.Schedules", b =>
                {
                    b.Navigation("Times");
                });

            modelBuilder.Entity("SAPLink.Core.Models.System.Clients", b =>
                {
                    b.Navigation("Credentials");
                });

            modelBuilder.Entity("SAPLink.Core.Models.System.Credentials", b =>
                {
                    b.Navigation("Subsidiaries");
                });
#pragma warning restore 612, 618
        }
    }
}
