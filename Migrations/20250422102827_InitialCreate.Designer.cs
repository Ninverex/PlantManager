﻿// <auto-generated />
using System;
using MenadzerRoslin.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MenadzerRoslin.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250422102827_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("MenadzerRoslin.Models.Gatunek", b =>
                {
                    b.Property<int>("GatunekId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("GatunekId"));

                    b.Property<string>("NazwaGatunku")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Swiatlo")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<double>("TemperaturaMax")
                        .HasColumnType("double precision");

                    b.Property<double>("TemperaturaMin")
                        .HasColumnType("double precision");

                    b.Property<int>("WymagaNawadnianiaCoIleDni")
                        .HasColumnType("integer");

                    b.Property<int>("WymagaNawozeniaCoIleDni")
                        .HasColumnType("integer");

                    b.HasKey("GatunekId");

                    b.ToTable("Gatunki");
                });

            modelBuilder.Entity("MenadzerRoslin.Models.Przypomnienie", b =>
                {
                    b.Property<int>("PrzypomnienieId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("PrzypomnienieId"));

                    b.Property<bool>("CzyWykonane")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("DataPlanowana")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("RoslinaId")
                        .HasColumnType("integer");

                    b.Property<string>("TypZabiegu")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("PrzypomnienieId");

                    b.HasIndex("RoslinaId");

                    b.ToTable("Przypomnienia");
                });

            modelBuilder.Entity("MenadzerRoslin.Models.Roslina", b =>
                {
                    b.Property<int>("RoslinaId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("RoslinaId"));

                    b.Property<DateTime>("DataZakupu")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("GatunekId")
                        .HasColumnType("integer");

                    b.Property<string>("Miejsce")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Nazwa")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("RoslinaId");

                    b.HasIndex("GatunekId");

                    b.ToTable("Rosliny");
                });

            modelBuilder.Entity("MenadzerRoslin.Models.Zabieg", b =>
                {
                    b.Property<int>("ZabiegId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ZabiegId"));

                    b.Property<DateTime>("DataWykonania")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Opis")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("RoslinaId")
                        .HasColumnType("integer");

                    b.Property<string>("TypZabiegu")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("ZabiegId");

                    b.HasIndex("RoslinaId");

                    b.ToTable("Zabiegi");
                });

            modelBuilder.Entity("MenadzerRoslin.Models.Przypomnienie", b =>
                {
                    b.HasOne("MenadzerRoslin.Models.Roslina", "Roslina")
                        .WithMany("Przypomnienia")
                        .HasForeignKey("RoslinaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Roslina");
                });

            modelBuilder.Entity("MenadzerRoslin.Models.Roslina", b =>
                {
                    b.HasOne("MenadzerRoslin.Models.Gatunek", "Gatunek")
                        .WithMany("Rosliny")
                        .HasForeignKey("GatunekId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Gatunek");
                });

            modelBuilder.Entity("MenadzerRoslin.Models.Zabieg", b =>
                {
                    b.HasOne("MenadzerRoslin.Models.Roslina", "Roslina")
                        .WithMany("Zabiegi")
                        .HasForeignKey("RoslinaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Roslina");
                });

            modelBuilder.Entity("MenadzerRoslin.Models.Gatunek", b =>
                {
                    b.Navigation("Rosliny");
                });

            modelBuilder.Entity("MenadzerRoslin.Models.Roslina", b =>
                {
                    b.Navigation("Przypomnienia");

                    b.Navigation("Zabiegi");
                });
#pragma warning restore 612, 618
        }
    }
}
