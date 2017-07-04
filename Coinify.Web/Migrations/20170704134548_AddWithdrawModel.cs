using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Coinify.Web.Migrations
{
    public partial class AddWithdrawModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Withdraw",
                columns: table => new
                {
                    WithdrawId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AutomatedTellerMachineId = table.Column<int>(nullable: false),
                    CurrencyDictionaryId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    WithdrawDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Withdraw", x => x.WithdrawId);
                    table.ForeignKey(
                        name: "FK_Withdraw_AutomatedTellerMachine_AutomatedTellerMachineId",
                        column: x => x.AutomatedTellerMachineId,
                        principalTable: "AutomatedTellerMachine",
                        principalColumn: "AutomatedTellerMachineId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Withdraw_CurrencyDictionary_CurrencyDictionaryId",
                        column: x => x.CurrencyDictionaryId,
                        principalTable: "CurrencyDictionary",
                        principalColumn: "CurrencyDictionaryId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Withdraw_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Withdraw_AutomatedTellerMachineId",
                table: "Withdraw",
                column: "AutomatedTellerMachineId");

            migrationBuilder.CreateIndex(
                name: "IX_Withdraw_CurrencyDictionaryId",
                table: "Withdraw",
                column: "CurrencyDictionaryId");

            migrationBuilder.CreateIndex(
                name: "IX_Withdraw_UserId",
                table: "Withdraw",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Withdraw");
        }
    }
}
