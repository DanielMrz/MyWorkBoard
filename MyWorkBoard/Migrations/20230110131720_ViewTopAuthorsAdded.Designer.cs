﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MyWorkBoard.Entities.Model;

#nullable disable

namespace MyWorkBoard.Migrations
{
    [DbContext(typeof(MyBoardsContext))]
    [Migration("20230110131720_ViewTopAuthorsAdded")]
    partial class ViewTopAuthorsAdded
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MyWorkBoard.Entities.Model.TopAuthor", b =>
                {
                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("WorkItemsCreated")
                        .HasColumnType("int");

                    b.ToTable((string)null);

                    b.ToView("View_TopAuthors", (string)null);
                });

            modelBuilder.Entity("MyWorkBoard.Entities.Tables.Address", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PostalCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Street")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("MyWorkBoard.Entities.Tables.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<Guid>("AuthorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getutcdate()");

                    b.Property<string>("Message")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedDate")
                        .ValueGeneratedOnUpdate()
                        .HasColumnType("datetime2");

                    b.Property<int>("WorkItemId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("WorkItemId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("MyWorkBoard.Entities.Tables.Linktable.WorkItemTag", b =>
                {
                    b.Property<int>("TagId")
                        .HasColumnType("int");

                    b.Property<int>("WorkItemId")
                        .HasColumnType("int");

                    b.Property<DateTime>("PublicationDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getutcdate()");

                    b.HasKey("TagId", "WorkItemId");

                    b.HasIndex("WorkItemId");

                    b.ToTable("WorkItemTag");
                });

            modelBuilder.Entity("MyWorkBoard.Entities.Tables.State", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("StateValue")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.HasKey("Id");

                    b.ToTable("States");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            StateValue = "To Do"
                        },
                        new
                        {
                            Id = 2,
                            StateValue = "Doing"
                        },
                        new
                        {
                            Id = 3,
                            StateValue = "Done"
                        },
                        new
                        {
                            Id = 4,
                            StateValue = "On Hold"
                        },
                        new
                        {
                            Id = 5,
                            StateValue = "Rejected"
                        });
                });

            modelBuilder.Entity("MyWorkBoard.Entities.Tables.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("TagValue")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Tags");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            TagValue = "Web"
                        },
                        new
                        {
                            Id = 2,
                            TagValue = "UI"
                        },
                        new
                        {
                            Id = 3,
                            TagValue = "Desktop"
                        },
                        new
                        {
                            Id = 4,
                            TagValue = "API"
                        },
                        new
                        {
                            Id = 5,
                            TagValue = "Service"
                        });
                });

            modelBuilder.Entity("MyWorkBoard.Entities.Tables.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("MyWorkBoard.Entities.Tables.WorkItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Area")
                        .HasColumnType("varchar(200)");

                    b.Property<Guid>("AuthorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IterationPath")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Iteration_Path");

                    b.Property<int>("Priority")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(1);

                    b.Property<int>("StateId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("StateId");

                    b.ToTable("WorkItems");

                    b.HasDiscriminator<string>("Discriminator").HasValue("WorkItem");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("MyWorkBoard.Entities.Tables.Epic", b =>
                {
                    b.HasBaseType("MyWorkBoard.Entities.Tables.WorkItem");

                    b.Property<DateTime?>("EndDate")
                        .HasPrecision(3)
                        .HasColumnType("datetime2(3)");

                    b.Property<DateTime?>("StartDate")
                        .HasColumnType("datetime2");

                    b.HasDiscriminator().HasValue("Epic");
                });

            modelBuilder.Entity("MyWorkBoard.Entities.Tables.Issue", b =>
                {
                    b.HasBaseType("MyWorkBoard.Entities.Tables.WorkItem");

                    b.Property<decimal>("Efford")
                        .HasColumnType("decimal(5, 2)");

                    b.HasDiscriminator().HasValue("Issue");
                });

            modelBuilder.Entity("MyWorkBoard.Entities.Tables.Task", b =>
                {
                    b.HasBaseType("MyWorkBoard.Entities.Tables.WorkItem");

                    b.Property<string>("Activity")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<decimal>("RemainingWork")
                        .HasPrecision(14, 2)
                        .HasColumnType("decimal(14,2)");

                    b.HasDiscriminator().HasValue("Task");
                });

            modelBuilder.Entity("MyWorkBoard.Entities.Tables.Address", b =>
                {
                    b.HasOne("MyWorkBoard.Entities.Tables.User", "User")
                        .WithOne("Address")
                        .HasForeignKey("MyWorkBoard.Entities.Tables.Address", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("MyWorkBoard.Entities.Tables.Comment", b =>
                {
                    b.HasOne("MyWorkBoard.Entities.Tables.User", "Author")
                        .WithMany("Comments")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("MyWorkBoard.Entities.Tables.WorkItem", "WorkItem")
                        .WithMany("Comments")
                        .HasForeignKey("WorkItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");

                    b.Navigation("WorkItem");
                });

            modelBuilder.Entity("MyWorkBoard.Entities.Tables.Linktable.WorkItemTag", b =>
                {
                    b.HasOne("MyWorkBoard.Entities.Tables.Tag", "Tag")
                        .WithMany()
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MyWorkBoard.Entities.Tables.WorkItem", "WorkItem")
                        .WithMany()
                        .HasForeignKey("WorkItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Tag");

                    b.Navigation("WorkItem");
                });

            modelBuilder.Entity("MyWorkBoard.Entities.Tables.WorkItem", b =>
                {
                    b.HasOne("MyWorkBoard.Entities.Tables.User", "Author")
                        .WithMany("WorkItems")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MyWorkBoard.Entities.Tables.State", "State")
                        .WithMany()
                        .HasForeignKey("StateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");

                    b.Navigation("State");
                });

            modelBuilder.Entity("MyWorkBoard.Entities.Tables.User", b =>
                {
                    b.Navigation("Address");

                    b.Navigation("Comments");

                    b.Navigation("WorkItems");
                });

            modelBuilder.Entity("MyWorkBoard.Entities.Tables.WorkItem", b =>
                {
                    b.Navigation("Comments");
                });
#pragma warning restore 612, 618
        }
    }
}
