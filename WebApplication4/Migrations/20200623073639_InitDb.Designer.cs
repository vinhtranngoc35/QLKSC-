﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApplication4.Models;

namespace WebApplication4.Migrations
{
    [DbContext(typeof(ProductContext))]
    [Migration("20200623073639_InitDb")]
    partial class InitDb
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("WebApplication4.Models.BillDetail", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BookingDetailId")
                        .HasColumnType("int");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<int?>("Total")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("BookingDetailId");

                    b.HasIndex("ProductId");

                    b.ToTable("BillDetails");
                });

            modelBuilder.Entity("WebApplication4.Models.Booking", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CheckIn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CheckOut")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime2");

                    b.Property<int?>("CustomerId")
                        .HasColumnType("int");

                    b.Property<int?>("ExtraPrice")
                        .HasColumnType("int");

                    b.Property<bool?>("IsDelete")
                        .HasColumnType("bit");

                    b.Property<int>("NumberRoom")
                        .HasColumnType("int");

                    b.Property<int?>("PriceRoom")
                        .HasColumnType("int");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.Property<int?>("Total")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("CustomerId");

                    b.ToTable("Bookings");
                });

            modelBuilder.Entity("WebApplication4.Models.BookingDetail", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BookingId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("CheckIn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CheckInExpected")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("CheckOut")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CheckOutExpected")
                        .HasColumnType("datetime2");

                    b.Property<int?>("Discount")
                        .HasColumnType("int");

                    b.Property<bool?>("IsDelete")
                        .HasColumnType("bit");

                    b.Property<int?>("NumberDate")
                        .HasColumnType("int");

                    b.Property<int?>("NumberDateDiscount")
                        .HasColumnType("int");

                    b.Property<int?>("Price")
                        .HasColumnType("int");

                    b.Property<int?>("PriceDiscount")
                        .HasColumnType("int");

                    b.Property<int?>("PriceExtra")
                        .HasColumnType("int");

                    b.Property<int?>("PriceRoom")
                        .HasColumnType("int");

                    b.Property<int>("RoomId")
                        .HasColumnType("int");

                    b.Property<int?>("Total")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("BookingId");

                    b.HasIndex("RoomId");

                    b.ToTable("BookingDetails");
                });

            modelBuilder.Entity("WebApplication4.Models.Customer", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("BookingDetailId")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IdentityCard")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.HasIndex("BookingDetailId");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("WebApplication4.Models.Discount", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("NumberDiscount")
                        .HasColumnType("int");

                    b.Property<DateTime>("TimeStart")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("TimeStop")
                        .HasColumnType("datetime2");

                    b.Property<int>("TypeRoomId")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("TypeRoomId");

                    b.ToTable("Discounts");
                });

            modelBuilder.Entity("WebApplication4.Models.EnterProduct", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("bit");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<int>("RemainingAmount")
                        .HasColumnType("int");

                    b.Property<int>("UId")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("ProductId");

                    b.ToTable("EnterProducts");
                });

            modelBuilder.Entity("WebApplication4.Models.Product", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Company")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.Property<int>("PriceProduct")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<int>("TypeId")
                        .HasColumnType("int");

                    b.Property<string>("Unit")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UrlImage")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.HasIndex("TypeId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("WebApplication4.Models.Room", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsDelete")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.Property<int>("TypeRoomId")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("TypeRoomId");

                    b.ToTable("Rooms");
                });

            modelBuilder.Entity("WebApplication4.Models.Type", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsDelete")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Types");
                });

            modelBuilder.Entity("WebApplication4.Models.TypeRoom", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsDelete")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.ToTable("TypeRooms");
                });

            modelBuilder.Entity("WebApplication4.Models.BillDetail", b =>
                {
                    b.HasOne("WebApplication4.Models.BookingDetail", "BookingDetail")
                        .WithMany("BillDetails")
                        .HasForeignKey("BookingDetailId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebApplication4.Models.Product", "Product")
                        .WithMany("BillDetails")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WebApplication4.Models.Booking", b =>
                {
                    b.HasOne("WebApplication4.Models.Customer", "Customer")
                        .WithMany("Bookings")
                        .HasForeignKey("CustomerId");
                });

            modelBuilder.Entity("WebApplication4.Models.BookingDetail", b =>
                {
                    b.HasOne("WebApplication4.Models.Booking", "Booking")
                        .WithMany("BookingDetails")
                        .HasForeignKey("BookingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebApplication4.Models.Room", "Room")
                        .WithMany("BookingDetails")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WebApplication4.Models.Customer", b =>
                {
                    b.HasOne("WebApplication4.Models.BookingDetail", "BookingDetail")
                        .WithMany("Customers")
                        .HasForeignKey("BookingDetailId");
                });

            modelBuilder.Entity("WebApplication4.Models.Discount", b =>
                {
                    b.HasOne("WebApplication4.Models.TypeRoom", "TypeRoom")
                        .WithMany("Discounts")
                        .HasForeignKey("TypeRoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WebApplication4.Models.EnterProduct", b =>
                {
                    b.HasOne("WebApplication4.Models.Product", "Product")
                        .WithMany("EnterProducts")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WebApplication4.Models.Product", b =>
                {
                    b.HasOne("WebApplication4.Models.Type", "Type")
                        .WithMany("Products")
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WebApplication4.Models.Room", b =>
                {
                    b.HasOne("WebApplication4.Models.TypeRoom", "TypeRoom")
                        .WithMany("Rooms")
                        .HasForeignKey("TypeRoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
