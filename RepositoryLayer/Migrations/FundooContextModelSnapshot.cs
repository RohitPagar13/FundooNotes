﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RepositoryLayer.Context;

#nullable disable

namespace RepositoryLayer.Migrations
{
    [DbContext(typeof(FundooContext))]
    partial class FundooContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.20")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("RepositoryLayer.Entities.Collaborator", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("NoteId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("NoteId");

                    b.ToTable("Collaborators");
                });

            modelBuilder.Entity("RepositoryLayer.Entities.Label", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("LabelName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("labels");
                });

            modelBuilder.Entity("RepositoryLayer.Entities.Note", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("isArchieve")
                        .HasColumnType("bit");

                    b.Property<bool>("isTrashed")
                        .HasColumnType("bit");

                    b.Property<int>("userId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("userId");

                    b.ToTable("notes");
                });

            modelBuilder.Entity("RepositoryLayer.Entities.NoteLabel", b =>
                {
                    b.Property<int>("noteId")
                        .HasColumnType("int");

                    b.Property<int>("labelId")
                        .HasColumnType("int");

                    b.HasKey("noteId", "labelId");

                    b.HasIndex("labelId");

                    b.ToTable("NoteLabels");
                });

            modelBuilder.Entity("RepositoryLayer.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("BirthDate")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("users");
                });

            modelBuilder.Entity("RepositoryLayer.Entities.Collaborator", b =>
                {
                    b.HasOne("RepositoryLayer.Entities.Note", null)
                        .WithMany("collaborators")
                        .HasForeignKey("NoteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RepositoryLayer.Entities.Note", b =>
                {
                    b.HasOne("RepositoryLayer.Entities.User", null)
                        .WithMany("UserNotes")
                        .HasForeignKey("userId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RepositoryLayer.Entities.NoteLabel", b =>
                {
                    b.HasOne("RepositoryLayer.Entities.Label", null)
                        .WithMany("labelNotes")
                        .HasForeignKey("labelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RepositoryLayer.Entities.Note", null)
                        .WithMany("noteLables")
                        .HasForeignKey("noteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RepositoryLayer.Entities.Label", b =>
                {
                    b.Navigation("labelNotes");
                });

            modelBuilder.Entity("RepositoryLayer.Entities.Note", b =>
                {
                    b.Navigation("collaborators");

                    b.Navigation("noteLables");
                });

            modelBuilder.Entity("RepositoryLayer.Entities.User", b =>
                {
                    b.Navigation("UserNotes");
                });
#pragma warning restore 612, 618
        }
    }
}
