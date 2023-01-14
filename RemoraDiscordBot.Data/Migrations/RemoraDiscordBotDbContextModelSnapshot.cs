﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RemoraDiscordBot.Data;

#nullable disable

namespace RemoraDiscordBot.Data.Migrations
{
    [DbContext(typeof(RemoraDiscordBotDbContext))]
    partial class RemoraDiscordBotDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("RemoraDiscordBot.Data.Domain.Experience.UserGuildCreature", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("CreatureType")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("IsEgg")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("Level")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("UserGuildCreatures");
                });

            modelBuilder.Entity("RemoraDiscordBot.Data.Domain.Experience.UserGuildXp", b =>
                {
                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.Property<long>("GuildId")
                        .HasColumnType("bigint");

                    b.Property<string>("AssociatedCreature")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("CreatureId")
                        .HasColumnType("int");

                    b.Property<long>("Level")
                        .HasColumnType("bigint");

                    b.Property<int?>("UserGuildCreatureId")
                        .HasColumnType("int");

                    b.Property<long>("XpAmount")
                        .HasColumnType("bigint");

                    b.Property<long>("XpNeededToLevelUp")
                        .HasColumnType("bigint");

                    b.HasKey("UserId", "GuildId");

                    b.HasIndex("CreatureId");

                    b.HasIndex("UserGuildCreatureId");

                    b.ToTable("UserGuildXps");
                });

            modelBuilder.Entity("RemoraDiscordBot.Data.Domain.Welcomer.WelcomerGuild", b =>
                {
                    b.Property<long>("GuildId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<bool>("IsWelcomerEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<long?>("WelcomeChannelId")
                        .HasColumnType("bigint");

                    b.Property<string>("WelcomeMessage")
                        .HasColumnType("longtext");

                    b.HasKey("GuildId");

                    b.ToTable("WelcomerGuilds");
                });

            modelBuilder.Entity("RemoraDiscordBot.Data.Domain.Experience.UserGuildXp", b =>
                {
                    b.HasOne("RemoraDiscordBot.Data.Domain.Experience.UserGuildCreature", "Creature")
                        .WithMany()
                        .HasForeignKey("CreatureId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RemoraDiscordBot.Data.Domain.Experience.UserGuildCreature", null)
                        .WithMany()
                        .HasForeignKey("UserGuildCreatureId");

                    b.Navigation("Creature");
                });
#pragma warning restore 612, 618
        }
    }
}
