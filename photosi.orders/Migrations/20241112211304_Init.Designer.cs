﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PhotoSi.Orders.Data;

#nullable disable

namespace photosi.orders.Migrations
{
    [DbContext(typeof(OrdersDbContext))]
    [Migration("20241112211304_Init")]
    partial class Init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("PhotoSi.Orders.Models.Ordine", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Data")
                        .HasColumnType("datetime2");

                    b.Property<string>("NrOrdine")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("PickupPointId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Stato")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("Ordini", "dbo");
                });

            modelBuilder.Entity("PhotoSi.Orders.Models.RigaOrdine", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Articolo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Descrizione")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("NrRiga")
                        .HasColumnType("int");

                    b.Property<Guid>("OrdineId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Prezzo")
                        .HasColumnType("decimal(18,2)");

                    b.Property<Guid>("ProdottoId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Quantita")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("OrdineId");

                    b.ToTable("OrdineRighe", "dbo");
                });

            modelBuilder.Entity("PhotoSi.Orders.Models.RigaOrdine", b =>
                {
                    b.HasOne("PhotoSi.Orders.Models.Ordine", "Ordine")
                        .WithMany("RigheOrdine")
                        .HasForeignKey("OrdineId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Ordine");
                });

            modelBuilder.Entity("PhotoSi.Orders.Models.Ordine", b =>
                {
                    b.Navigation("RigheOrdine");
                });
#pragma warning restore 612, 618
        }
    }
}
