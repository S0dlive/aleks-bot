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

            modelBuilder.Entity("RemoraDiscordBot.Data.Domain.AutoRoles.AutoRoleChannel", b =>
                {
                    b.Property<long>("MessageId")
                        .HasColumnType("bigint");

                    b.Property<long>("GuildId")
                        .HasColumnType("bigint");

                    b.Property<long>("ChannelId")
                        .HasColumnType("bigint");

                    b.HasKey("MessageId", "GuildId");

                    b.ToTable("AutoRoleChannels");
                });

            modelBuilder.Entity("RemoraDiscordBot.Data.Domain.AutoRoles.AutoRoleReaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Emoji")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<long>("InstigatorGuildId")
                        .HasColumnType("bigint");

                    b.Property<long>("InstigatorMessageId")
                        .HasColumnType("bigint");

                    b.Property<string>("Label")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<ulong>("RoleId")
                        .HasColumnType("bigint unsigned");

                    b.HasKey("Id");

                    b.HasIndex("InstigatorMessageId", "InstigatorGuildId");

                    b.ToTable("AutoRoleReactions");
                });

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

                    b.Property<long>("PossessorGuildId")
                        .HasColumnType("bigint");

                    b.Property<long>("PossessorId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("PossessorId", "PossessorGuildId")
                        .IsUnique();

                    b.ToTable("UserGuildCreatures");
                });

            modelBuilder.Entity("RemoraDiscordBot.Data.Domain.Experience.UserGuildXp", b =>
                {
                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.Property<long>("GuildId")
                        .HasColumnType("bigint");

                    b.Property<int>("CreatureId")
                        .HasColumnType("int");

                    b.Property<long>("Level")
                        .HasColumnType("bigint");

                    b.Property<long>("XpAmount")
                        .HasColumnType("bigint");

                    b.Property<long>("XpNeededToLevelUp")
                        .HasColumnType("bigint");

                    b.HasKey("UserId", "GuildId");

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

            modelBuilder.Entity("RemoraDiscordBot.Data.Domain.AutoRoles.AutoRoleReaction", b =>
                {
                    b.HasOne("RemoraDiscordBot.Data.Domain.AutoRoles.AutoRoleChannel", "AutoRoleChannel")
                        .WithMany("Reactions")
                        .HasForeignKey("InstigatorMessageId", "InstigatorGuildId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AutoRoleChannel");
                });

            modelBuilder.Entity("RemoraDiscordBot.Data.Domain.Experience.UserGuildCreature", b =>
                {
                    b.HasOne("RemoraDiscordBot.Data.Domain.Experience.UserGuildXp", "Possessor")
                        .WithOne("Creature")
                        .HasForeignKey("RemoraDiscordBot.Data.Domain.Experience.UserGuildCreature", "PossessorId", "PossessorGuildId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Possessor");
                });

            modelBuilder.Entity("RemoraDiscordBot.Data.Domain.AutoRoles.AutoRoleChannel", b =>
                {
                    b.Navigation("Reactions");
                });

            modelBuilder.Entity("RemoraDiscordBot.Data.Domain.Experience.UserGuildXp", b =>
                {
                    b.Navigation("Creature")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
