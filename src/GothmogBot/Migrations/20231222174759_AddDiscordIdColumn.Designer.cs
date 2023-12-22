﻿// <auto-generated />
using System;
using GothmogBot.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GothmogBot.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20231222174759_AddDiscordIdColumn")]
    partial class AddDiscordIdColumn
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.14");

            modelBuilder.Entity("DotaMatchUser", b =>
                {
                    b.Property<long>("DotaMatchesId")
                        .HasColumnType("INTEGER");

                    b.Property<long>("UsersSteamAccountId")
                        .HasColumnType("INTEGER");

                    b.HasKey("DotaMatchesId", "UsersSteamAccountId");

                    b.HasIndex("UsersSteamAccountId");

                    b.ToTable("DotaMatchUser");
                });

            modelBuilder.Entity("GothmogBot.Database.DiscordUser", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("DiscordId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("DiscordUsername")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<long>("Points")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("DiscordUsers");
                });

            modelBuilder.Entity("GothmogBot.Database.DotaMatch", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("EndDateTime")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("StartDateTime")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("DotaMatches");
                });

            modelBuilder.Entity("GothmogBot.Database.User", b =>
                {
                    b.Property<long>("SteamAccountId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("DiscordUsername")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("TwitchUsername")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("SteamAccountId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("DotaMatchUser", b =>
                {
                    b.HasOne("GothmogBot.Database.DotaMatch", null)
                        .WithMany()
                        .HasForeignKey("DotaMatchesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GothmogBot.Database.User", null)
                        .WithMany()
                        .HasForeignKey("UsersSteamAccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
