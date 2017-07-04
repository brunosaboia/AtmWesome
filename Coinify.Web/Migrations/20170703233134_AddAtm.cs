using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Coinify.Web.Migrations
{
    public partial class AddAtm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AutomatedTellerMachineId",
                table: "CoinSize",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AutomatedTellerMachine",
                columns: table => new
                {
                    AutomatedTellerMachineId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Alias = table.Column<string>(nullable: false),
                    HasNoteDispenser = table.Column<bool>(nullable: false),
                    JsonCoinDictionary = table.Column<string>(nullable: true),
                    JsonNoteDictionary = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AutomatedTellerMachine", x => x.AutomatedTellerMachineId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CoinSize_AutomatedTellerMachineId",
                table: "CoinSize",
                column: "AutomatedTellerMachineId");

            migrationBuilder.AddForeignKey(
                name: "FK_CoinSize_AutomatedTellerMachine_AutomatedTellerMachineId",
                table: "CoinSize",
                column: "AutomatedTellerMachineId",
                principalTable: "AutomatedTellerMachine",
                principalColumn: "AutomatedTellerMachineId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CoinSize_AutomatedTellerMachine_AutomatedTellerMachineId",
                table: "CoinSize");

            migrationBuilder.DropTable(
                name: "AutomatedTellerMachine");

            migrationBuilder.DropIndex(
                name: "IX_CoinSize_AutomatedTellerMachineId",
                table: "CoinSize");

            migrationBuilder.DropColumn(
                name: "AutomatedTellerMachineId",
                table: "CoinSize");
        }
    }
}
