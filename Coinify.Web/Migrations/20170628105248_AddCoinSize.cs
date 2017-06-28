using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Coinify.Web.Migrations
{
    public partial class AddCoinSize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Size",
                table: "Coin");

            migrationBuilder.AddColumn<int>(
                name: "SizeCoinSizeId",
                table: "Coin",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CoinSize",
                columns: table => new
                {
                    CoinSizeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Size = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoinSize", x => x.CoinSizeId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Coin_SizeCoinSizeId",
                table: "Coin",
                column: "SizeCoinSizeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Coin_CoinSize_SizeCoinSizeId",
                table: "Coin",
                column: "SizeCoinSizeId",
                principalTable: "CoinSize",
                principalColumn: "CoinSizeId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Coin_CoinSize_SizeCoinSizeId",
                table: "Coin");

            migrationBuilder.DropTable(
                name: "CoinSize");

            migrationBuilder.DropIndex(
                name: "IX_Coin_SizeCoinSizeId",
                table: "Coin");

            migrationBuilder.DropColumn(
                name: "SizeCoinSizeId",
                table: "Coin");

            migrationBuilder.AddColumn<int>(
                name: "Size",
                table: "Coin",
                nullable: false,
                defaultValue: 0);
        }
    }
}
