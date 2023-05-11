﻿// <auto-generated />
using CalendarSync.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CalendarSync.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20220922222320_00000")]
    partial class _00000
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("CalendarSync.Entities.Calendar", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("MicrosoftUserEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("Purge")
                        .HasColumnType("bit");

                    b.Property<bool>("WriteOnly")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("MicrosoftUserEmail");

                    b.ToTable("Calendars");
                });

            modelBuilder.Entity("CalendarSync.Entities.CloneEvent", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("EventId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RootCalendarId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("EventId");

                    b.HasIndex("RootCalendarId");

                    b.ToTable("CloneEvents");
                });

            modelBuilder.Entity("CalendarSync.Entities.Event", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RootCalendarId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RootCalendarId");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("CalendarSync.Entities.MicrosoftUser", b =>
                {
                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AccessToken")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("LastAccessTokenStatus")
                        .HasColumnType("int");

                    b.Property<string>("RefreshToken")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TenantId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Email");

                    b.ToTable("MicrosoftUsers");
                });

            modelBuilder.Entity("CalendarSync.Entities.Calendar", b =>
                {
                    b.HasOne("CalendarSync.Entities.MicrosoftUser", "MicrosoftUser")
                        .WithMany("Calendars")
                        .HasForeignKey("MicrosoftUserEmail")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MicrosoftUser");
                });

            modelBuilder.Entity("CalendarSync.Entities.CloneEvent", b =>
                {
                    b.HasOne("CalendarSync.Entities.Event", null)
                        .WithMany("CloneEvents")
                        .HasForeignKey("EventId");

                    b.HasOne("CalendarSync.Entities.Calendar", "RootCalendar")
                        .WithMany()
                        .HasForeignKey("RootCalendarId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RootCalendar");
                });

            modelBuilder.Entity("CalendarSync.Entities.Event", b =>
                {
                    b.HasOne("CalendarSync.Entities.Calendar", "RootCalendar")
                        .WithMany()
                        .HasForeignKey("RootCalendarId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RootCalendar");
                });

            modelBuilder.Entity("CalendarSync.Entities.Event", b =>
                {
                    b.Navigation("CloneEvents");
                });

            modelBuilder.Entity("CalendarSync.Entities.MicrosoftUser", b =>
                {
                    b.Navigation("Calendars");
                });
#pragma warning restore 612, 618
        }
    }
}
