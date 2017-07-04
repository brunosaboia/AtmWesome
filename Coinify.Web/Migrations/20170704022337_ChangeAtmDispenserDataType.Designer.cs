using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Coinify.Web.Models;

namespace Coinify.Web.Migrations
{
    [DbContext(typeof(CoinifyWebContext))]
    [Migration("20170704022337_ChangeAtmDispenserDataType")]
    partial class ChangeAtmDispenserDataType
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Coinify.Web.Models.AutomatedTellerMachine", b =>
                {
                    b.Property<int>("AutomatedTellerMachineId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Alias")
                        .IsRequired();

                    b.Property<bool>("HasNoteDispenser");

                    b.Property<string>("JsonCoinDictionary");

                    b.Property<string>("JsonCoinDispensersDictionary");

                    b.Property<string>("JsonNoteDictionary");

                    b.HasKey("AutomatedTellerMachineId");

                    b.ToTable("AutomatedTellerMachine");
                });

            modelBuilder.Entity("Coinify.Web.Models.Coin", b =>
                {
                    b.Property<int>("CoinId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("SizeCoinSizeId");

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

                    b.Property<int>("Balance");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("UserId");

                    b.ToTable("User");
                });

            modelBuilder.Entity("Coinify.Web.Models.Coin", b =>
                {
                    b.HasOne("Coinify.Web.Models.CoinSize", "Size")
                        .WithMany()
                        .HasForeignKey("SizeCoinSizeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
