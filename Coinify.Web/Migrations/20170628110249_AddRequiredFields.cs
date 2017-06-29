using Microsoft.EntityFrameworkCore.Migrations;

namespace Coinify.Web.Migrations
{
    public partial class AddRequiredFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Coin_CoinSize_SizeCoinSizeId",
                table: "Coin");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "User",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SizeCoinSizeId",
                table: "Coin",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Coin_CoinSize_SizeCoinSizeId",
                table: "Coin",
                column: "SizeCoinSizeId",
                principalTable: "CoinSize",
                principalColumn: "CoinSizeId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Coin_CoinSize_SizeCoinSizeId",
                table: "Coin");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "User",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<int>(
                name: "SizeCoinSizeId",
                table: "Coin",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Coin_CoinSize_SizeCoinSizeId",
                table: "Coin",
                column: "SizeCoinSizeId",
                principalTable: "CoinSize",
                principalColumn: "CoinSizeId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
