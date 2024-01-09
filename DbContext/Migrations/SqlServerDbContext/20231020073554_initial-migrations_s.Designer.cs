﻿// <auto-generated />
using System;
using DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DbContext.Migrations.SqlServerDbContext
{
    [DbContext(typeof(csMainDbContext.SqlServerDbContext))]
    [Migration("20231020073554_initial-migrations_s")]
    partial class initialmigrations_s
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DbModels.csAttractionDbM", b =>
                {
                    b.Property<Guid>("AttractionID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AttractionName")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)");

                    b.Property<bool>("Seeded")
                        .HasColumnType("bit");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("AttractionID");

                    b.HasIndex("AttractionName");

                    b.ToTable("Attractions", "supusr");
                });

            modelBuilder.Entity("DbModels.csCityDbM", b =>
                {
                    b.Property<Guid>("CityID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CityName")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)");

                    b.Property<bool>("Seeded")
                        .HasColumnType("bit");

                    b.HasKey("CityID");

                    b.ToTable("Cities", "supusr");
                });

            modelBuilder.Entity("DbModels.csCommentDbM", b =>
                {
                    b.Property<Guid>("CommentID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AttractionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)");

                    b.Property<bool>("Seeded")
                        .HasColumnType("bit");

                    b.HasKey("CommentID");

                    b.ToTable("Comments", "supusr");
                });

            modelBuilder.Entity("DbModels.csUserDbM", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Role")
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Models.DTO.gstusrInfoAttractionsDto", b =>
                {
                    b.Property<string>("City")
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("NrAttractions")
                        .HasColumnType("int");

                    b.ToTable((string)null);

                    b.ToView("vwInfoAttractions", "gstusr");
                });

            modelBuilder.Entity("Models.DTO.gstusrInfoCitiesDto", b =>
                {
                    b.Property<string>("City")
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("NrCities")
                        .HasColumnType("int");

                    b.ToTable((string)null);

                    b.ToView("vwInfoCities", "gstusr");
                });

            modelBuilder.Entity("Models.DTO.gstusrInfoCommentsDto", b =>
                {
                    b.Property<string>("Comment")
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("NrComments")
                        .HasColumnType("int");

                    b.ToTable((string)null);

                    b.ToView("vwInfoComments", "gstusr");
                });

            modelBuilder.Entity("Models.DTO.gstusrInfoDbDto", b =>
                {
                    b.Property<int>("nrAttractionsWithComment")
                        .HasColumnType("int");

                    b.Property<int>("nrSeededAttractions")
                        .HasColumnType("int");

                    b.Property<int>("nrSeededCities")
                        .HasColumnType("int");

                    b.Property<int>("nrSeededComments")
                        .HasColumnType("int");

                    b.Property<int>("nrSeededDescriptions")
                        .HasColumnType("int");

                    b.Property<int>("nrSeededTitles")
                        .HasColumnType("int");

                    b.Property<int>("nrUnseededAttractions")
                        .HasColumnType("int");

                    b.Property<int>("nrUnseededCities")
                        .HasColumnType("int");

                    b.Property<int>("nrUnseededComments")
                        .HasColumnType("int");

                    b.Property<int>("nrUnseededDescriptions")
                        .HasColumnType("int");

                    b.Property<int>("nrUnseededTitles")
                        .HasColumnType("int");

                    b.ToTable((string)null);

                    b.ToView("vwInfoDb", "gstusr");
                });

            modelBuilder.Entity("csAttractionDbMcsCommentDbM", b =>
                {
                    b.Property<Guid>("AttractionsDbMAttractionID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CommentsDbMCommentID")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("AttractionsDbMAttractionID", "CommentsDbMCommentID");

                    b.HasIndex("CommentsDbMCommentID");

                    b.ToTable("csAttractionDbMcsCommentDbM", "supusr");
                });

            modelBuilder.Entity("csAttractionDbMcsCommentDbM", b =>
                {
                    b.HasOne("DbModels.csAttractionDbM", null)
                        .WithMany()
                        .HasForeignKey("AttractionsDbMAttractionID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DbModels.csCommentDbM", null)
                        .WithMany()
                        .HasForeignKey("CommentsDbMCommentID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
