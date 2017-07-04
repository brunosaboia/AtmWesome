using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Coinify.Web.Migrations
{
    public partial class ChangeAtmDispenserDataType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CoinSize_AutomatedTellerMachine_AutomatedTellerMachineId",
                table: "CoinSize");

            migrationBuilder.DropIndex(
                name: "IX_CoinSize_AutomatedTellerMachineId",
                table: "CoinSize");

            migrationBuilder.DropColumn(
                name: "AutomatedTellerMachineId",
                table: "CoinSize");

            migrationBuilder.AddColumn<string>(
                name: "JsonCoinDispensersDictionary",
                table: "AutomatedTellerMachine",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JsonCoinDispensersDictionary",
                table: "AutomatedTellerMachine");

            migrationBuilder.AddColumn<int>(
                name: "AutomatedTellerMachineId",
                table: "CoinSize",
                nullable: true);

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
    }
}
