﻿// <auto-generated />
using System;
using HealthHup.API.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace HealthHup.API.Migrations
{
    [DbContext(typeof(ApplicatoinDataBaseContext))]
    [Migration("20240223004626_AddNewDrugsModle")]
    partial class AddNewDrugsModle
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.15")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("HealthHup.API.Model.Models.AdressModle.Area", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("governorateId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("key")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("governorateId");

                    b.ToTable("Areas");
                });

            modelBuilder.Entity("HealthHup.API.Model.Models.AdressModle.Governorate", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("key")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Governorates");
                });

            modelBuilder.Entity("HealthHup.API.Model.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<Guid>("AreaId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Brdate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("Gender")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

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

                    b.Property<string>("src")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AreaId");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("HealthHup.API.Model.Models.DrugModel.Drug", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("smiles")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Drugs");
                });

            modelBuilder.Entity("HealthHup.API.Model.Models.Hospital.Doctor", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Accept")
                        .HasColumnType("bit");

                    b.Property<string>("AddressDescrption")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CollegeName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateOfJoin")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateOfSendRequest")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("GraduationYear")
                        .HasColumnType("datetime2");

                    b.Property<string>("SummaryCareer")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("areaId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("doctorId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<Guid>("drSpecialtieId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("areaId");

                    b.HasIndex("doctorId");

                    b.HasIndex("drSpecialtieId");

                    b.ToTable("Doctors");
                });

            modelBuilder.Entity("HealthHup.API.Model.Models.Hospital.Review", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PatientId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("SendTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("doctorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("rate")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("PatientId");

                    b.HasIndex("doctorId");

                    b.ToTable("Reviews");
                });

            modelBuilder.Entity("HealthHup.API.Model.Models.Hospital.Specialtie", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Specialties");
                });

            modelBuilder.Entity("HealthHup.API.Model.Models.PharmacyModel.Pharmacy", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("areaId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<double?>("xLoc")
                        .HasColumnType("float");

                    b.Property<double?>("yLoc")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("areaId");

                    b.ToTable("pharmacies");
                });

            modelBuilder.Entity("HealthHup.API.Model.Models.PharmacyModel.PharmacyDrug", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Count")
                        .HasColumnType("int");

                    b.Property<string>("DrugId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("Expire")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("PharmacyId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("DrugId");

                    b.HasIndex("PharmacyId");

                    b.ToTable("pharmacyDrugs");
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

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

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

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

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
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

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
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("HealthHup.API.Model.Models.AdressModle.Area", b =>
                {
                    b.HasOne("HealthHup.API.Model.Models.AdressModle.Governorate", "governorate")
                        .WithMany("areas")
                        .HasForeignKey("governorateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("governorate");
                });

            modelBuilder.Entity("HealthHup.API.Model.Models.ApplicationUser", b =>
                {
                    b.HasOne("HealthHup.API.Model.Models.AdressModle.Area", "area")
                        .WithMany()
                        .HasForeignKey("AreaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("area");
                });

            modelBuilder.Entity("HealthHup.API.Model.Models.Hospital.Doctor", b =>
                {
                    b.HasOne("HealthHup.API.Model.Models.AdressModle.Area", "area")
                        .WithMany("doctors")
                        .HasForeignKey("areaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HealthHup.API.Model.Models.ApplicationUser", "doctor")
                        .WithMany()
                        .HasForeignKey("doctorId");

                    b.HasOne("HealthHup.API.Model.Models.Hospital.Specialtie", "drSpecialtie")
                        .WithMany("Doctors")
                        .HasForeignKey("drSpecialtieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsMany("HealthHup.API.Model.Models.Hospital.DoctorCertificate", "Certificates", b1 =>
                        {
                            b1.Property<Guid>("DoctorId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int");

                            SqlServerPropertyBuilderExtensions.UseIdentityColumn(b1.Property<int>("Id"));

                            b1.Property<string>("src")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("DoctorId", "Id");

                            b1.ToTable("DoctorCertificate");

                            b1.WithOwner()
                                .HasForeignKey("DoctorId");
                        });

                    b.OwnsMany("HealthHup.API.Model.Models.Hospital.DoctorDate", "Dates", b1 =>
                        {
                            b1.Property<Guid>("DoctorId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int");

                            SqlServerPropertyBuilderExtensions.UseIdentityColumn(b1.Property<int>("Id"));

                            b1.Property<string>("DayName")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("From")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("To")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("DoctorId", "Id");

                            b1.ToTable("DoctorDate");

                            b1.WithOwner()
                                .HasForeignKey("DoctorId");
                        });

                    b.Navigation("Certificates");

                    b.Navigation("Dates");

                    b.Navigation("area");

                    b.Navigation("doctor");

                    b.Navigation("drSpecialtie");
                });

            modelBuilder.Entity("HealthHup.API.Model.Models.Hospital.Review", b =>
                {
                    b.HasOne("HealthHup.API.Model.Models.ApplicationUser", "Patient")
                        .WithMany()
                        .HasForeignKey("PatientId");

                    b.HasOne("HealthHup.API.Model.Models.Hospital.Doctor", "doctor")
                        .WithMany("reviews")
                        .HasForeignKey("doctorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Patient");

                    b.Navigation("doctor");
                });

            modelBuilder.Entity("HealthHup.API.Model.Models.PharmacyModel.Pharmacy", b =>
                {
                    b.HasOne("HealthHup.API.Model.Models.AdressModle.Area", "area")
                        .WithMany("pharmacys")
                        .HasForeignKey("areaId");

                    b.Navigation("area");
                });

            modelBuilder.Entity("HealthHup.API.Model.Models.PharmacyModel.PharmacyDrug", b =>
                {
                    b.HasOne("HealthHup.API.Model.Models.DrugModel.Drug", "Drug")
                        .WithMany()
                        .HasForeignKey("DrugId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HealthHup.API.Model.Models.PharmacyModel.Pharmacy", "Pharmacy")
                        .WithMany("pharmacyDrugs")
                        .HasForeignKey("PharmacyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Drug");

                    b.Navigation("Pharmacy");
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
                    b.HasOne("HealthHup.API.Model.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("HealthHup.API.Model.Models.ApplicationUser", null)
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

                    b.HasOne("HealthHup.API.Model.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("HealthHup.API.Model.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HealthHup.API.Model.Models.AdressModle.Area", b =>
                {
                    b.Navigation("doctors");

                    b.Navigation("pharmacys");
                });

            modelBuilder.Entity("HealthHup.API.Model.Models.AdressModle.Governorate", b =>
                {
                    b.Navigation("areas");
                });

            modelBuilder.Entity("HealthHup.API.Model.Models.Hospital.Doctor", b =>
                {
                    b.Navigation("reviews");
                });

            modelBuilder.Entity("HealthHup.API.Model.Models.Hospital.Specialtie", b =>
                {
                    b.Navigation("Doctors");
                });

            modelBuilder.Entity("HealthHup.API.Model.Models.PharmacyModel.Pharmacy", b =>
                {
                    b.Navigation("pharmacyDrugs");
                });
#pragma warning restore 612, 618
        }
    }
}
