﻿// <auto-generated />
using System;
using InnoGotchi_WebApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace InnoGotchi_WebApi.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20221129131347_UserUpdate")]
    partial class UserUpdate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("InnoGotchi_WebApi.Models.FarmModels.Farm", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Farms");
                });

            modelBuilder.Entity("InnoGotchi_WebApi.Models.FriendFarm", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("FarmId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "FarmId");

                    b.HasIndex("FarmId");

                    b.ToTable("FriendFarms");
                });

            modelBuilder.Entity("InnoGotchi_WebApi.Models.PetModels.Pet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("Age")
                        .HasColumnType("int");

                    b.Property<int>("Body")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("Eyes")
                        .HasColumnType("int");

                    b.Property<int?>("FarmId")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<int>("HappinessDays")
                        .HasColumnType("int");

                    b.Property<DateTime>("HappinessDaysStart")
                        .HasColumnType("datetime2");

                    b.Property<int>("Hunger")
                        .HasColumnType("int");

                    b.Property<bool>("IsAlive")
                        .HasColumnType("bit");

                    b.Property<DateTime>("LastDrank")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("LastFed")
                        .HasColumnType("datetime2");

                    b.Property<int>("Mouth")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<int>("Nose")
                        .HasColumnType("int");

                    b.Property<int>("Thirst")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FarmId");

                    b.ToTable("Pets");
                });

            modelBuilder.Entity("InnoGotchi_WebApi.Models.UserModels.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("PasswordHash")
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<int>("PasswordLength")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("InnoGotchi_WebApi.Models.FarmModels.Farm", b =>
                {
                    b.HasOne("InnoGotchi_WebApi.Models.UserModels.User", "User")
                        .WithOne("Farm")
                        .HasForeignKey("InnoGotchi_WebApi.Models.FarmModels.Farm", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("InnoGotchi_WebApi.Models.FriendFarm", b =>
                {
                    b.HasOne("InnoGotchi_WebApi.Models.FarmModels.Farm", "Farm")
                        .WithMany("Friends")
                        .HasForeignKey("FarmId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("InnoGotchi_WebApi.Models.UserModels.User", "User")
                        .WithMany("FriendsFarms")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Farm");

                    b.Navigation("User");
                });

            modelBuilder.Entity("InnoGotchi_WebApi.Models.PetModels.Pet", b =>
                {
                    b.HasOne("InnoGotchi_WebApi.Models.FarmModels.Farm", "Farm")
                        .WithMany("Pets")
                        .HasForeignKey("FarmId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Farm");
                });

            modelBuilder.Entity("InnoGotchi_WebApi.Models.FarmModels.Farm", b =>
                {
                    b.Navigation("Friends");

                    b.Navigation("Pets");
                });

            modelBuilder.Entity("InnoGotchi_WebApi.Models.UserModels.User", b =>
                {
                    b.Navigation("Farm");

                    b.Navigation("FriendsFarms");
                });
#pragma warning restore 612, 618
        }
    }
}
