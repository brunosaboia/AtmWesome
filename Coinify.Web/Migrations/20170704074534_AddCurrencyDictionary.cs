using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Coinify.Web.Migrations
{
    public partial class AddCurrencyDictionary : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JsonCoinDictionary",
                table: "AutomatedTellerMachine");

            migrationBuilder.DropColumn(
                name: "JsonNoteDictionary",
                table: "AutomatedTellerMachine");

            migrationBuilder.AddColumn<int>(
                name: "CurrencyDictionaryId",
                table: "AutomatedTellerMachine",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CurrencyDictionary",
                columns: table => new
                {
                    CurrencyDictionaryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    JsonCoinDictionary = table.Column<string>(nullable: true),
                    JsonNoteDictionary = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyDictionary", x => x.CurrencyDictionaryId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AutomatedTellerMachine_CurrencyDictionaryId",
                table: "AutomatedTellerMachine",
                column: "CurrencyDictionaryId");

            migrationBuilder.AddForeignKey(
                name: "FK_AutomatedTellerMachine_CurrencyDictionary_CurrencyDictionaryId",
                table: "AutomatedTellerMachine",
                column: "CurrencyDictionaryId",
                principalTable: "CurrencyDictionary",
                principalColumn: "CurrencyDictionaryId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AutomatedTellerMachine_CurrencyDictionary_CurrencyDictionaryId",
                table: "AutomatedTellerMachine");

            migrationBuilder.DropTable(
                name: "CurrencyDictionary");

            migrationBuilder.DropIndex(
                name: "IX_AutomatedTellerMachine_CurrencyDictionaryId",
                table: "AutomatedTellerMachine");

            migrationBuilder.DropColumn(
                name: "CurrencyDictionaryId",
                table: "AutomatedTellerMachine");

            migrationBuilder.AddColumn<string>(
                name: "JsonCoinDictionary",
                table: "AutomatedTellerMachine",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JsonNoteDictionary",
                table: "AutomatedTellerMachine",
                nullable: true);
        }
    }
}
