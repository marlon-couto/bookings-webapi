﻿// <auto-generated />
using System;
using BookingsWebApi.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BookingsWebApi.Migrations
{
    [DbContext(typeof(BookingsDbContext))]
    partial class BookingsDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.0");

            modelBuilder.Entity("BookingsWebApi.Models.Booking", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CheckIn")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CheckOut")
                        .HasColumnType("TEXT");

                    b.Property<int>("GuestQuantity")
                        .HasColumnType("INTEGER");

                    b.Property<string>("RoomId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("RoomId");

                    b.HasIndex("UserId");

                    b.ToTable("Bookings");

                    b.HasData(
                        new
                        {
                            Id = "1",
                            CheckIn = new DateTime(2023, 11, 7, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CheckOut = new DateTime(2023, 11, 8, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            GuestQuantity = 1,
                            RoomId = "1",
                            UserId = "1"
                        },
                        new
                        {
                            Id = "2",
                            CheckIn = new DateTime(2023, 11, 7, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CheckOut = new DateTime(2023, 11, 8, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            GuestQuantity = 1,
                            RoomId = "2",
                            UserId = "1"
                        },
                        new
                        {
                            Id = "3",
                            CheckIn = new DateTime(2023, 11, 7, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CheckOut = new DateTime(2023, 11, 8, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            GuestQuantity = 1,
                            RoomId = "3",
                            UserId = "2"
                        },
                        new
                        {
                            Id = "4",
                            CheckIn = new DateTime(2023, 11, 7, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CheckOut = new DateTime(2023, 11, 8, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            GuestQuantity = 1,
                            RoomId = "4",
                            UserId = "2"
                        });
                });

            modelBuilder.Entity("BookingsWebApi.Models.City", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Cities");

                    b.HasData(
                        new
                        {
                            Id = "1",
                            Name = "City 1",
                            State = "State 1"
                        },
                        new
                        {
                            Id = "2",
                            Name = "City 2",
                            State = "State 2"
                        });
                });

            modelBuilder.Entity("BookingsWebApi.Models.Hotel", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("CityId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CityId");

                    b.ToTable("Hotels");

                    b.HasData(
                        new
                        {
                            Id = "1",
                            Address = "Address 1",
                            CityId = "1",
                            Name = "Hotel 1"
                        },
                        new
                        {
                            Id = "2",
                            Address = "Address 2",
                            CityId = "2",
                            Name = "Hotel 2"
                        });
                });

            modelBuilder.Entity("BookingsWebApi.Models.Room", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<int>("Capacity")
                        .HasColumnType("INTEGER");

                    b.Property<string>("HotelId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("HotelId");

                    b.ToTable("Rooms");

                    b.HasData(
                        new
                        {
                            Id = "1",
                            Capacity = 2,
                            HotelId = "1",
                            Image = "Image 1",
                            Name = "Room 1"
                        },
                        new
                        {
                            Id = "2",
                            Capacity = 1,
                            HotelId = "1",
                            Image = "Image 2",
                            Name = "Room 2"
                        },
                        new
                        {
                            Id = "3",
                            Capacity = 3,
                            HotelId = "1",
                            Image = "Image 3",
                            Name = "Room 3"
                        },
                        new
                        {
                            Id = "4",
                            Capacity = 2,
                            HotelId = "2",
                            Image = "Image 4",
                            Name = "Room 4"
                        },
                        new
                        {
                            Id = "5",
                            Capacity = 1,
                            HotelId = "2",
                            Image = "Image 5",
                            Name = "Room 5"
                        },
                        new
                        {
                            Id = "6",
                            Capacity = 3,
                            HotelId = "2",
                            Image = "Image 6",
                            Name = "Room 6"
                        });
                });

            modelBuilder.Entity("BookingsWebApi.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("TEXT");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = "1",
                            Email = "user1@mail.com",
                            Name = "User 1",
                            Password = "Pass1",
                            Role = "Client"
                        },
                        new
                        {
                            Id = "2",
                            Email = "user2@mail.com",
                            Name = "User 2",
                            Password = "Pass2",
                            Role = "Client"
                        },
                        new
                        {
                            Id = "3",
                            Email = "user3@mail.com",
                            Name = "User 3",
                            Password = "Pass3",
                            Role = "Admin"
                        });
                });

            modelBuilder.Entity("BookingsWebApi.Models.Booking", b =>
                {
                    b.HasOne("BookingsWebApi.Models.Room", "Room")
                        .WithMany("Bookings")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BookingsWebApi.Models.User", "User")
                        .WithMany("Bookings")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Room");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BookingsWebApi.Models.Hotel", b =>
                {
                    b.HasOne("BookingsWebApi.Models.City", "City")
                        .WithMany("Hotels")
                        .HasForeignKey("CityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("City");
                });

            modelBuilder.Entity("BookingsWebApi.Models.Room", b =>
                {
                    b.HasOne("BookingsWebApi.Models.Hotel", "Hotel")
                        .WithMany("Rooms")
                        .HasForeignKey("HotelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Hotel");
                });

            modelBuilder.Entity("BookingsWebApi.Models.City", b =>
                {
                    b.Navigation("Hotels");
                });

            modelBuilder.Entity("BookingsWebApi.Models.Hotel", b =>
                {
                    b.Navigation("Rooms");
                });

            modelBuilder.Entity("BookingsWebApi.Models.Room", b =>
                {
                    b.Navigation("Bookings");
                });

            modelBuilder.Entity("BookingsWebApi.Models.User", b =>
                {
                    b.Navigation("Bookings");
                });
#pragma warning restore 612, 618
        }
    }
}
