﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TruyenChu.Data;

#nullable disable

namespace TruyenChu.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20241029085915_IniCreate")]
    partial class IniCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TruyenChu.Data.Author", b =>
                {
                    b.Property<int>("AuthorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AuthorId"));

                    b.Property<string>("Bio")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AuthorId");

                    b.ToTable("Authors");
                });

            modelBuilder.Entity("TruyenChu.Data.Category", b =>
                {
                    b.Property<int>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CategoryId"));

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CategoryId");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("TruyenChu.Data.Story", b =>
                {
                    b.Property<int>("StoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("StoryId"));

                    b.Property<int>("AuthorId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StoryTitle")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ViewCount")
                        .HasColumnType("int");

                    b.HasKey("StoryId");

                    b.HasIndex("AuthorId");

                    b.ToTable("Stories");
                });

            modelBuilder.Entity("TruyenChu.Data.StoryCategory", b =>
                {
                    b.Property<int>("StoryId")
                        .HasColumnType("int");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.HasKey("StoryId", "CategoryId");

                    b.HasIndex("CategoryId");

                    b.ToTable("StoryCategories");
                });

            modelBuilder.Entity("TruyenChu.Data.Story", b =>
                {
                    b.HasOne("TruyenChu.Data.Author", "Author")
                        .WithMany("Stories")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");
                });

            modelBuilder.Entity("TruyenChu.Data.StoryCategory", b =>
                {
                    b.HasOne("TruyenChu.Data.Category", "Category")
                        .WithMany("StoryCategories")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TruyenChu.Data.Story", "Story")
                        .WithMany("StoryCategories")
                        .HasForeignKey("StoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("Story");
                });

            modelBuilder.Entity("TruyenChu.Data.Author", b =>
                {
                    b.Navigation("Stories");
                });

            modelBuilder.Entity("TruyenChu.Data.Category", b =>
                {
                    b.Navigation("StoryCategories");
                });

            modelBuilder.Entity("TruyenChu.Data.Story", b =>
                {
                    b.Navigation("StoryCategories");
                });
#pragma warning restore 612, 618
        }
    }
}
