﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RemoraDiscordBot.Data;

#nullable disable

namespace RemoraDiscordBot.Data.Migrations
{
    [DbContext(typeof(RemoraDiscordBotDbContext))]
    [Migration("20221230161905_UserGuildXpTwoPrimaryKeys")]
    partial class UserGuildXpTwoPrimaryKeys
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("RemoraDiscordBot.Data.Domain.Xp.UserGuildXp", b =>
                {
                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.Property<long>("GuildId")
                        .HasColumnType("bigint");

                    b.Property<long>("Level")
                        .HasColumnType("bigint");

                    b.Property<long>("XpAmount")
                        .HasColumnType("bigint");

                    b.Property<long>("XpNeededToLevelUp")
                        .HasColumnType("bigint");

                    b.HasKey("UserId", "GuildId");

                    b.ToTable("UserGuildXps");
                });
#pragma warning restore 612, 618
        }
    }
}
