﻿// <auto-generated />
using Kuiper.Clustering.ServiceApi.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Kuiper.Clustering.ServiceApi.Migrations
{
    [DbContext(typeof(KvStoreDbContext))]
    [Migration("20240617164012_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.6");

            modelBuilder.Entity("Kuiper.Clustering.ServiceApi.Storage.Models.InternalStoreObject", b =>
                {
                    b.Property<string>("Key")
                        .HasColumnType("TEXT");

                    b.Property<long>("Timestamp")
                        .HasColumnType("INTEGER");

                    b.Property<byte[]>("Value")
                        .HasColumnType("BLOB");

                    b.HasKey("Key");

                    b.ToTable("StoreObjects");
                });
#pragma warning restore 612, 618
        }
    }
}
