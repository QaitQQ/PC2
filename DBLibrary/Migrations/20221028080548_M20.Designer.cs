﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Server;

#nullable disable

namespace ServerCore.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20221028080548_M20")]
    partial class M20
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-rc.2.22472.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("CRMLibs.City", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("City");
                });

            modelBuilder.Entity("CRMLibs.Directory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int>("ParentId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Directory");
                });

            modelBuilder.Entity("CRMLibs.Event", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DatePlanned")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("DateОccurred")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("PartnerID")
                        .HasColumnType("integer");

                    b.Property<int>("TypeID")
                        .HasColumnType("integer");

                    b.Property<int>("UserID")
                        .HasColumnType("integer");

                    b.Property<int>("СontactPersonID")
                        .HasColumnType("integer");

                    b.Property<string>("Сontent")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("PartnerID");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("CRMLibs.EventType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("EventType");
                });

            modelBuilder.Entity("CRMLibs.Partner", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("CityId")
                        .HasColumnType("integer");

                    b.Property<string>("Contact_1")
                        .HasColumnType("text");

                    b.Property<string>("Contact_2")
                        .HasColumnType("text");

                    b.Property<int>("DirectoryID")
                        .HasColumnType("integer");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<int>("EmployeeID")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Phone")
                        .HasColumnType("text");

                    b.Property<int?>("SphereOfActivityId")
                        .HasColumnType("integer");

                    b.Property<int>("StatusID")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CityId");

                    b.HasIndex("SphereOfActivityId");

                    b.ToTable("Partner");
                });

            modelBuilder.Entity("CRMLibs.SphereOfActivity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("SphereOfActivity");
                });

            modelBuilder.Entity("CRMLibs.Status", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Status");
                });

            modelBuilder.Entity("CRMLibs.СontactPerson", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int>("PartnerID")
                        .HasColumnType("integer");

                    b.Property<string>("Phone")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("PartnerID");

                    b.ToTable("СontactPerson");
                });

            modelBuilder.Entity("Object_Description.DB_Access_Struct+User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Pass")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("StructLibs.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("ItemDBStructId")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ItemDBStructId");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("StructLibs.DetailValue", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("DetailsID")
                        .HasColumnType("integer");

                    b.Property<int?>("ItemDBStructId")
                        .HasColumnType("integer");

                    b.Property<int>("ItemID")
                        .HasColumnType("integer");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ItemDBStructId");

                    b.ToTable("DetailValue");
                });

            modelBuilder.Entity("StructLibs.Details", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Details");
                });

            modelBuilder.Entity("StructLibs.ItemDBStruct", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Currency")
                        .HasColumnType("text");

                    b.Property<DateTime>("DateСhange")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("DescriptionSeparator")
                        .HasColumnType("text");

                    b.Property<string>("Image")
                        .HasColumnType("text");

                    b.Property<int>("ManufactorID")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<double>("PriceDC")
                        .HasColumnType("double precision");

                    b.Property<string>("PriceListName")
                        .HasColumnType("text");

                    b.Property<double>("PriceRC")
                        .HasColumnType("double precision");

                    b.Property<bool>("SiteFlag")
                        .HasColumnType("boolean");

                    b.Property<int>("SiteId")
                        .HasColumnType("integer");

                    b.Property<string>("Sku")
                        .HasColumnType("text");

                    b.Property<string>("SourceName")
                        .HasColumnType("text");

                    b.Property<List<int>>("StorageID")
                        .HasColumnType("integer[]");

                    b.Property<List<string>>("Tags")
                        .HasColumnType("text[]");

                    b.Property<string[]>("СomparisonName")
                        .HasColumnType("text[]");

                    b.HasKey("Id");

                    b.ToTable("Item");
                });

            modelBuilder.Entity("StructLibs.Manufactor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Manufactor");
                });

            modelBuilder.Entity("StructLibs.ManufactorSite", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("ManufactorId")
                        .HasColumnType("integer");

                    b.Property<string>("SearchLink")
                        .HasColumnType("text");

                    b.Property<string>("SiteLink")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("ManufactorSite");
                });

            modelBuilder.Entity("StructLibs.PriceСhangeHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DateСhange")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("ItemID")
                        .HasColumnType("integer");

                    b.Property<int?>("PartnerIDId")
                        .HasColumnType("integer");

                    b.Property<double>("PriceDC")
                        .HasColumnType("double precision");

                    b.Property<double>("PriceRC")
                        .HasColumnType("double precision");

                    b.Property<string>("SourceName")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("PartnerIDId");

                    b.ToTable("PriceСhangeHistory");
                });

            modelBuilder.Entity("StructLibs.Storage", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ID"));

                    b.Property<int>("Count")
                        .HasColumnType("integer");

                    b.Property<DateTime>("DateСhange")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("ItemID")
                        .HasColumnType("integer");

                    b.Property<string>("SourceName")
                        .HasColumnType("text");

                    b.Property<int>("WarehouseID")
                        .HasColumnType("integer");

                    b.HasKey("ID");

                    b.HasIndex("WarehouseID");

                    b.ToTable("Storage");
                });

            modelBuilder.Entity("StructLibs.Warehouse", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int?>("PartnerIDId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("PartnerIDId");

                    b.ToTable("Warehouse");
                });

            modelBuilder.Entity("CRMLibs.Event", b =>
                {
                    b.HasOne("CRMLibs.Partner", null)
                        .WithMany("Events")
                        .HasForeignKey("PartnerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CRMLibs.Partner", b =>
                {
                    b.HasOne("CRMLibs.City", "City")
                        .WithMany()
                        .HasForeignKey("CityId");

                    b.HasOne("CRMLibs.SphereOfActivity", "SphereOfActivity")
                        .WithMany()
                        .HasForeignKey("SphereOfActivityId");

                    b.Navigation("City");

                    b.Navigation("SphereOfActivity");
                });

            modelBuilder.Entity("CRMLibs.СontactPerson", b =>
                {
                    b.HasOne("CRMLibs.Partner", null)
                        .WithMany("СontacPersons")
                        .HasForeignKey("PartnerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("StructLibs.Category", b =>
                {
                    b.HasOne("StructLibs.ItemDBStruct", null)
                        .WithMany("Categories")
                        .HasForeignKey("ItemDBStructId");
                });

            modelBuilder.Entity("StructLibs.DetailValue", b =>
                {
                    b.HasOne("StructLibs.ItemDBStruct", null)
                        .WithMany("Details")
                        .HasForeignKey("ItemDBStructId");
                });

            modelBuilder.Entity("StructLibs.PriceСhangeHistory", b =>
                {
                    b.HasOne("CRMLibs.Partner", "PartnerID")
                        .WithMany()
                        .HasForeignKey("PartnerIDId");

                    b.Navigation("PartnerID");
                });

            modelBuilder.Entity("StructLibs.Storage", b =>
                {
                    b.HasOne("StructLibs.Warehouse", "Warehouse")
                        .WithMany()
                        .HasForeignKey("WarehouseID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Warehouse");
                });

            modelBuilder.Entity("StructLibs.Warehouse", b =>
                {
                    b.HasOne("CRMLibs.Partner", "PartnerID")
                        .WithMany()
                        .HasForeignKey("PartnerIDId");

                    b.Navigation("PartnerID");
                });

            modelBuilder.Entity("CRMLibs.Partner", b =>
                {
                    b.Navigation("Events");

                    b.Navigation("СontacPersons");
                });

            modelBuilder.Entity("StructLibs.ItemDBStruct", b =>
                {
                    b.Navigation("Categories");

                    b.Navigation("Details");
                });
#pragma warning restore 612, 618
        }
    }
}
