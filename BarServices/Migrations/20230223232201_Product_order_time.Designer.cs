﻿// <auto-generated />
using System;
using BarServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BarServices.Migrations
{
    [DbContext(typeof(ApplicationDBContext))]
    [Migration("20230223232201_Product_order_time")]
    partial class Productordertime
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BarServices.Models.Elaboration", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Elaborations");
                });

            modelBuilder.Entity("BarServices.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ElaborationId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Offer")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("OrderTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Picture")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasPrecision(8, 2)
                        .HasColumnType("decimal(8,2)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("TableId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ElaborationId");

                    b.HasIndex("TableId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("BarServices.Models.Table", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("BarId")
                        .HasColumnType("int");

                    b.Property<int?>("KitchenId")
                        .HasColumnType("int");

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BarId");

                    b.HasIndex("KitchenId");

                    b.ToTable("Tables");
                });

            modelBuilder.Entity("BarServices.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Roles")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("BarServices.Models.Product", b =>
                {
                    b.HasOne("BarServices.Models.Elaboration", "Elaboration")
                        .WithMany("Products")
                        .HasForeignKey("ElaborationId");

                    b.HasOne("BarServices.Models.Table", "Table")
                        .WithMany("Products")
                        .HasForeignKey("TableId");

                    b.Navigation("Elaboration");

                    b.Navigation("Table");
                });

            modelBuilder.Entity("BarServices.Models.Table", b =>
                {
                    b.HasOne("BarServices.Models.Elaboration", "Bar")
                        .WithMany()
                        .HasForeignKey("BarId");

                    b.HasOne("BarServices.Models.Elaboration", "Kitchen")
                        .WithMany()
                        .HasForeignKey("KitchenId");

                    b.Navigation("Bar");

                    b.Navigation("Kitchen");
                });

            modelBuilder.Entity("BarServices.Models.Elaboration", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("BarServices.Models.Table", b =>
                {
                    b.Navigation("Products");
                });
#pragma warning restore 612, 618
        }
    }
}
