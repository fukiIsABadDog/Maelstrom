﻿// <auto-generated />
using System;
using EF_Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EF_Models.Migrations
{
    [DbContext(typeof(MaelstromContext))]
    [Migration("20230421211558_DateTimeTestResultsProp")]
    partial class DateTimeTestResultsProp
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.15")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("EF_Models.Models.AppUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("EF_Models.Models.Fish", b =>
                {
                    b.Property<int>("FishID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FishID"), 1L, 1);

                    b.Property<DateTime?>("DateAdded")
                        .HasColumnType("datetime2");

                    b.Property<int>("FishTypeID")
                        .HasColumnType("int");

                    b.Property<byte?>("Image")
                        .HasColumnType("tinyint");

                    b.Property<string>("NameOrTag")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SiteID")
                        .HasColumnType("int");

                    b.HasKey("FishID");

                    b.HasIndex("FishTypeID");

                    b.HasIndex("SiteID");

                    b.ToTable("Fishs");
                });

            modelBuilder.Entity("EF_Models.Models.FishType", b =>
                {
                    b.Property<int>("FishTypeID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FishTypeID"), 1L, 1);

                    b.Property<string>("CommonName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("MaxSize")
                        .HasColumnType("float");

                    b.Property<double?>("MaxTemp")
                        .HasColumnType("float");

                    b.Property<double?>("MinTemp")
                        .HasColumnType("float");

                    b.Property<double?>("PhMax")
                        .HasColumnType("float");

                    b.Property<double?>("PhMin")
                        .HasColumnType("float");

                    b.Property<int?>("RecommendedTankSize")
                        .HasColumnType("int");

                    b.Property<string>("ScientificName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("FishTypeID");

                    b.ToTable("FishTypes");
                });

            modelBuilder.Entity("EF_Models.Models.Site", b =>
                {
                    b.Property<int>("SiteID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SiteID"), 1L, 1);

                    b.Property<int?>("Capacity")
                        .HasColumnType("int");

                    b.Property<byte[]>("ImageData")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Location")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SiteTypeID")
                        .HasColumnType("int");

                    b.HasKey("SiteID");

                    b.HasIndex("SiteTypeID");

                    b.ToTable("Sites");
                });

            modelBuilder.Entity("EF_Models.Models.SiteType", b =>
                {
                    b.Property<int>("SiteTypeID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SiteTypeID"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SiteTypeID");

                    b.ToTable("SiteTypes");
                });

            modelBuilder.Entity("EF_Models.Models.SiteUser", b =>
                {
                    b.Property<int>("SiteUserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SiteUserID"), 1L, 1);

                    b.Property<string>("AppUserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("SiteID")
                        .HasColumnType("int");

                    b.HasKey("SiteUserID");

                    b.HasIndex("AppUserId");

                    b.HasIndex("SiteID");

                    b.ToTable("SiteUsers");
                });

            modelBuilder.Entity("EF_Models.Models.TestResult", b =>
                {
                    b.Property<int>("TestResultID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TestResultID"), 1L, 1);

                    b.Property<decimal?>("Alkalinty")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("Ammonia")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("Calcium")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal?>("Magnesium")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("Nitrate")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("Nitrite")
                        .HasColumnType("decimal(18,2)");

                    b.Property<float?>("Ph")
                        .HasColumnType("real");

                    b.Property<decimal?>("Phosphate")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("Sality")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("SiteUserID")
                        .HasColumnType("int");

                    b.Property<float?>("Temperature")
                        .HasColumnType("real");

                    b.HasKey("TestResultID");

                    b.HasIndex("SiteUserID");

                    b.ToTable("TestResults");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("EF_Models.Models.Fish", b =>
                {
                    b.HasOne("EF_Models.Models.FishType", "FishType")
                        .WithMany("Fishs")
                        .HasForeignKey("FishTypeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EF_Models.Models.Site", "Site")
                        .WithMany("Fishs")
                        .HasForeignKey("SiteID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FishType");

                    b.Navigation("Site");
                });

            modelBuilder.Entity("EF_Models.Models.Site", b =>
                {
                    b.HasOne("EF_Models.Models.SiteType", "SiteType")
                        .WithMany("Sites")
                        .HasForeignKey("SiteTypeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SiteType");
                });

            modelBuilder.Entity("EF_Models.Models.SiteUser", b =>
                {
                    b.HasOne("EF_Models.Models.AppUser", "AppUser")
                        .WithMany("SiteUsers")
                        .HasForeignKey("AppUserId");

                    b.HasOne("EF_Models.Models.Site", "Site")
                        .WithMany("SiteUsers")
                        .HasForeignKey("SiteID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AppUser");

                    b.Navigation("Site");
                });

            modelBuilder.Entity("EF_Models.Models.TestResult", b =>
                {
                    b.HasOne("EF_Models.Models.SiteUser", "SiteUser")
                        .WithMany()
                        .HasForeignKey("SiteUserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SiteUser");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("EF_Models.Models.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("EF_Models.Models.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EF_Models.Models.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("EF_Models.Models.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("EF_Models.Models.AppUser", b =>
                {
                    b.Navigation("SiteUsers");
                });

            modelBuilder.Entity("EF_Models.Models.FishType", b =>
                {
                    b.Navigation("Fishs");
                });

            modelBuilder.Entity("EF_Models.Models.Site", b =>
                {
                    b.Navigation("Fishs");

                    b.Navigation("SiteUsers");
                });

            modelBuilder.Entity("EF_Models.Models.SiteType", b =>
                {
                    b.Navigation("Sites");
                });
#pragma warning restore 612, 618
        }
    }
}
