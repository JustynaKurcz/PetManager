﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using PetManager.Infrastructure.EF.DbContext;

#nullable disable

namespace PetManager.Infrastructure.EF.Migrations
{
    [DbContext(typeof(PetManagerDbContext))]
    [Migration("20250111192737_AddIsNotificationSentPropertyToAppointment")]
    partial class AddIsNotificationSentPropertyToAppointment
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0-rc.2.24474.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("PetManager.Core.HealthRecords.Entities.Appointment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("AppointmentDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Diagnosis")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("HealthRecordId")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsNotificationSent")
                        .HasColumnType("boolean");

                    b.Property<string>("Notes")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("HealthRecordId");

                    b.ToTable("Appointments", (string)null);
                });

            modelBuilder.Entity("PetManager.Core.HealthRecords.Entities.HealthRecord", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Notes")
                        .HasColumnType("text");

                    b.Property<Guid>("PetId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("PetId")
                        .IsUnique();

                    b.ToTable("HealthRecords", (string)null);
                });

            modelBuilder.Entity("PetManager.Core.HealthRecords.Entities.Vaccination", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("HealthRecordId")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsNotificationSent")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset>("NextVaccinationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset>("VaccinationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("VaccinationName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("HealthRecordId");

                    b.ToTable("Vaccinations", (string)null);
                });

            modelBuilder.Entity("PetManager.Core.Pets.Entities.Image", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("BlobUrl")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("PetId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("UploadedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("PetId")
                        .IsUnique();

                    b.ToTable("Images", (string)null);
                });

            modelBuilder.Entity("PetManager.Core.Pets.Entities.Pet", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("BirthDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Breed")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<short>("Gender")
                        .HasColumnType("smallint");

                    b.Property<Guid>("HealthRecordId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("ImageId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<short>("Species")
                        .HasColumnType("smallint");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Pets", (string)null);
                });

            modelBuilder.Entity("PetManager.Core.Users.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset?>("LastChangePasswordDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LastName")
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<short>("Role")
                        .HasColumnType("smallint");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("PetManager.Core.HealthRecords.Entities.Appointment", b =>
                {
                    b.HasOne("PetManager.Core.HealthRecords.Entities.HealthRecord", "HealthRecord")
                        .WithMany("Appointments")
                        .HasForeignKey("HealthRecordId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("HealthRecord");
                });

            modelBuilder.Entity("PetManager.Core.HealthRecords.Entities.HealthRecord", b =>
                {
                    b.HasOne("PetManager.Core.Pets.Entities.Pet", "Pet")
                        .WithOne("HealthRecord")
                        .HasForeignKey("PetManager.Core.HealthRecords.Entities.HealthRecord", "PetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Pet");
                });

            modelBuilder.Entity("PetManager.Core.HealthRecords.Entities.Vaccination", b =>
                {
                    b.HasOne("PetManager.Core.HealthRecords.Entities.HealthRecord", "HealthRecord")
                        .WithMany("Vaccinations")
                        .HasForeignKey("HealthRecordId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("HealthRecord");
                });

            modelBuilder.Entity("PetManager.Core.Pets.Entities.Image", b =>
                {
                    b.HasOne("PetManager.Core.Pets.Entities.Pet", "Pet")
                        .WithOne("Image")
                        .HasForeignKey("PetManager.Core.Pets.Entities.Image", "PetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Pet");
                });

            modelBuilder.Entity("PetManager.Core.Pets.Entities.Pet", b =>
                {
                    b.HasOne("PetManager.Core.Users.Entities.User", "User")
                        .WithMany("Pets")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("PetManager.Core.HealthRecords.Entities.HealthRecord", b =>
                {
                    b.Navigation("Appointments");

                    b.Navigation("Vaccinations");
                });

            modelBuilder.Entity("PetManager.Core.Pets.Entities.Pet", b =>
                {
                    b.Navigation("HealthRecord");

                    b.Navigation("Image");
                });

            modelBuilder.Entity("PetManager.Core.Users.Entities.User", b =>
                {
                    b.Navigation("Pets");
                });
#pragma warning restore 612, 618
        }
    }
}
