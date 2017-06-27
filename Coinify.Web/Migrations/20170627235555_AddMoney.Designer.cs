using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Coinify.Web.Models;

namespace Coinify.Web.Migrations
{
    [DbContext(typeof(CoinifyWebContext))]
    [Migration("20170627235555_AddMoney")]
    partial class AddMoney
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

                    b.Property<int>("Size");

                    b.Property<int>("Value");

                    b.HasKey("CoinId");

                    b.ToTable("Coin");
                });

            modelBuilder.Entity("Coinify.Web.Models.Note", b =>
                {
                    b.Property<int>("NoteId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Value");

                    b.HasKey("NoteId");

                    b.ToTable("Note");
                });
        }
    }
}
