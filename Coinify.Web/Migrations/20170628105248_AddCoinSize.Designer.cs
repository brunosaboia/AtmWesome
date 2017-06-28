using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Coinify.Web.Models;

namespace Coinify.Web.Migrations
{
    [DbContext(typeof(CoinifyWebContext))]
    [Migration("20170628105248_AddCoinSize")]
    partial class AddCoinSize
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Coinify.Web.Models.Coin", b =>
                {
                    b.Property<int>("CoinId")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("SizeCoinSizeId");

                    b.Property<int>("Value");

                    b.HasKey("CoinId");

                    b.HasIndex("SizeCoinSizeId");

                    b.ToTable("Coin");
                });

            modelBuilder.Entity("Coinify.Web.Models.CoinSize", b =>
                {
                    b.Property<int>("CoinSizeId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Size");

                    b.HasKey("CoinSizeId");

                    b.ToTable("CoinSize");
                });

            modelBuilder.Entity("Coinify.Web.Models.Note", b =>
                {
                    b.Property<int>("NoteId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Value");

                    b.HasKey("NoteId");

                    b.ToTable("Note");
                });

            modelBuilder.Entity("Coinify.Web.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("UserId");

                    b.ToTable("User");
                });

            modelBuilder.Entity("Coinify.Web.Models.Coin", b =>
                {
                    b.HasOne("Coinify.Web.Models.CoinSize", "Size")
                        .WithMany()
                        .HasForeignKey("SizeCoinSizeId");
                });
        }
    }
}
