﻿// <auto-generated />
using System;
using LRTV.ContextModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace LRTV.Migrations.Players
{
    [DbContext(typeof(PlayersContext))]
    [Migration("20240515164629_AGP_NewPlayers_Good")]
    partial class AGP_NewPlayers_Good
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("LRTV.Models.PlayerModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Achievements")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Age")
                        .HasColumnType("int");

                    b.Property<int?>("CurrentTeamId")
                        .HasColumnType("int");

                    b.Property<float>("Headshots")
                        .HasColumnType("real");

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("KD")
                        .HasColumnType("real");

                    b.Property<int>("MapsPlayed")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nickname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("Rating")
                        .HasColumnType("real");

                    b.Property<int>("TeamID")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CurrentTeamId");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("LRTV.Models.TeamModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Ranking")
                        .HasColumnType("int");

                    b.Property<string>("Region")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("LRTV.Models.PlayerModel", b =>
                {
                    b.HasOne("LRTV.Models.TeamModel", "CurrentTeam")
                        .WithMany()
                        .HasForeignKey("CurrentTeamId");

                    b.Navigation("CurrentTeam");
                });
#pragma warning restore 612, 618
        }
    }
}
