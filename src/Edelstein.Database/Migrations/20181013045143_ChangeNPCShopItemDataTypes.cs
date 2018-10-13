using Microsoft.EntityFrameworkCore.Migrations;

namespace Edelstein.Database.Migrations
{
    public partial class ChangeNPCShopItemDataTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "UnitPrice",
                table: "NPCShopItem",
                nullable: false,
                oldClrType: typeof(float));

            migrationBuilder.AlterColumn<short>(
                name: "Quantity",
                table: "NPCShopItem",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<short>(
                name: "MaxPerSlot",
                table: "NPCShopItem",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<byte>(
                name: "DiscountRate",
                table: "NPCShopItem",
                nullable: false,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "UnitPrice",
                table: "NPCShopItem",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "NPCShopItem",
                nullable: false,
                oldClrType: typeof(short));

            migrationBuilder.AlterColumn<int>(
                name: "MaxPerSlot",
                table: "NPCShopItem",
                nullable: false,
                oldClrType: typeof(short));

            migrationBuilder.AlterColumn<int>(
                name: "DiscountRate",
                table: "NPCShopItem",
                nullable: false,
                oldClrType: typeof(byte));
        }
    }
}
