﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ModelGenerator.DataBase;

namespace ModelGenerator.Migrations
{
    [DbContext(typeof(ThreatsDbContext))]
    [Migration("20200430083713_Init")]
    partial class Init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ModelGenerator.DataBase.Models.Model", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("PreferencesId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("PreferencesId");

                    b.HasIndex("UserId");

                    b.ToTable("Model");
                });

            modelBuilder.Entity("ModelGenerator.DataBase.Models.ModelLine", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("DangerLevel")
                        .HasColumnType("int");

                    b.Property<bool>("IsActual")
                        .HasColumnType("bit");

                    b.Property<int>("LineId")
                        .HasColumnType("int");

                    b.Property<Guid>("ModelId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Possibility")
                        .HasColumnType("int");

                    b.Property<string>("RealisationCoefficient")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("SourceId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("TargetId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ThreatId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ModelId");

                    b.HasIndex("SourceId");

                    b.HasIndex("TargetId");

                    b.HasIndex("ThreatId");

                    b.ToTable("ModelLine");
                });

            modelBuilder.Entity("ModelGenerator.DataBase.Models.ModelPreferences", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("AnonymityLevel")
                        .HasColumnType("int");

                    b.Property<int>("AvailabilityViolationDanger")
                        .HasColumnType("int");

                    b.Property<int>("IntegrityViolationDanger")
                        .HasColumnType("int");

                    b.Property<int>("LocationCharacteristic")
                        .HasColumnType("int");

                    b.Property<int>("NetworkCharacteristic")
                        .HasColumnType("int");

                    b.Property<int>("OtherDBConnections")
                        .HasColumnType("int");

                    b.Property<int>("PersonalDataActionCharacteristics")
                        .HasColumnType("int");

                    b.Property<int>("PersonalDataPermissionSplit")
                        .HasColumnType("int");

                    b.Property<int>("PersonalDataSharingLevel")
                        .HasColumnType("int");

                    b.Property<int>("PrivacyViolationDanger")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("ModelPreferences");
                });

            modelBuilder.Entity("ModelGenerator.DataBase.Models.Source", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ModelPreferencesId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ModelPreferencesId");

                    b.ToTable("Source");
                });

            modelBuilder.Entity("ModelGenerator.DataBase.Models.Target", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ModelPreferencesId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ModelPreferencesId");

                    b.ToTable("Target");
                });

            modelBuilder.Entity("ModelGenerator.DataBase.Models.Threat", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ThreatId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Threat");
                });

            modelBuilder.Entity("ModelGenerator.DataBase.Models.ThreatDanger", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("DangerLevel")
                        .HasColumnType("int");

                    b.Property<Guid?>("ModelPreferencesId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Properties")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("SourceId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ThreatId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ModelPreferencesId");

                    b.HasIndex("SourceId");

                    b.HasIndex("ThreatId");

                    b.ToTable("ThreatDanger");
                });

            modelBuilder.Entity("ModelGenerator.DataBase.Models.ThreatPossibility", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ModelPreferencesId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("RiskProbability")
                        .HasColumnType("int");

                    b.Property<Guid>("ThreatId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ModelPreferencesId");

                    b.HasIndex("ThreatId");

                    b.ToTable("ThreatPossibility");
                });

            modelBuilder.Entity("ModelGenerator.DataBase.Models.ThreatSource", b =>
                {
                    b.Property<Guid>("ThreatId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("SourceId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ThreatId", "SourceId");

                    b.HasIndex("SourceId");

                    b.ToTable("ThreatSource");
                });

            modelBuilder.Entity("ModelGenerator.DataBase.Models.ThreatTarget", b =>
                {
                    b.Property<Guid>("ThreatId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("TargetId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ThreatId", "TargetId");

                    b.HasIndex("TargetId");

                    b.ToTable("ThreatTarget");
                });

            modelBuilder.Entity("ModelGenerator.DataBase.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Login")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("ModelGenerator.DataBase.Models.Model", b =>
                {
                    b.HasOne("ModelGenerator.DataBase.Models.ModelPreferences", "Preferences")
                        .WithMany()
                        .HasForeignKey("PreferencesId");

                    b.HasOne("ModelGenerator.DataBase.Models.User", null)
                        .WithMany("Model")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("ModelGenerator.DataBase.Models.ModelLine", b =>
                {
                    b.HasOne("ModelGenerator.DataBase.Models.Model", "Model")
                        .WithMany("Lines")
                        .HasForeignKey("ModelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ModelGenerator.DataBase.Models.Source", "Source")
                        .WithMany()
                        .HasForeignKey("SourceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ModelGenerator.DataBase.Models.Target", "Target")
                        .WithMany()
                        .HasForeignKey("TargetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ModelGenerator.DataBase.Models.Threat", "Threat")
                        .WithMany()
                        .HasForeignKey("ThreatId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ModelGenerator.DataBase.Models.Source", b =>
                {
                    b.HasOne("ModelGenerator.DataBase.Models.ModelPreferences", null)
                        .WithMany("Source")
                        .HasForeignKey("ModelPreferencesId");
                });

            modelBuilder.Entity("ModelGenerator.DataBase.Models.Target", b =>
                {
                    b.HasOne("ModelGenerator.DataBase.Models.ModelPreferences", null)
                        .WithMany("Target")
                        .HasForeignKey("ModelPreferencesId");
                });

            modelBuilder.Entity("ModelGenerator.DataBase.Models.ThreatDanger", b =>
                {
                    b.HasOne("ModelGenerator.DataBase.Models.ModelPreferences", null)
                        .WithMany("ThreatDangers")
                        .HasForeignKey("ModelPreferencesId");

                    b.HasOne("ModelGenerator.DataBase.Models.Source", "Source")
                        .WithMany()
                        .HasForeignKey("SourceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ModelGenerator.DataBase.Models.Threat", "Threat")
                        .WithMany()
                        .HasForeignKey("ThreatId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ModelGenerator.DataBase.Models.ThreatPossibility", b =>
                {
                    b.HasOne("ModelGenerator.DataBase.Models.ModelPreferences", null)
                        .WithMany("ThreatPossibilities")
                        .HasForeignKey("ModelPreferencesId");

                    b.HasOne("ModelGenerator.DataBase.Models.Threat", "Threat")
                        .WithMany()
                        .HasForeignKey("ThreatId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ModelGenerator.DataBase.Models.ThreatSource", b =>
                {
                    b.HasOne("ModelGenerator.DataBase.Models.Threat", "Threat")
                        .WithMany("ThreatSource")
                        .HasForeignKey("SourceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ModelGenerator.DataBase.Models.Source", "Source")
                        .WithMany("ThreatSource")
                        .HasForeignKey("ThreatId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ModelGenerator.DataBase.Models.ThreatTarget", b =>
                {
                    b.HasOne("ModelGenerator.DataBase.Models.Threat", "Threat")
                        .WithMany("ThreatTarget")
                        .HasForeignKey("TargetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ModelGenerator.DataBase.Models.Target", "Target")
                        .WithMany("ThreatTarget")
                        .HasForeignKey("ThreatId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
