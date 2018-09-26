using Microsoft.EntityFrameworkCore.Migrations;

namespace Edelstein.Database.Migrations
{
    public partial class AddInventoryType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "ItemInventory",
                nullable: false,
                oldClrType: typeof(byte));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte>(
                name: "Type",
                table: "ItemInventory",
                nullable: false,
                oldClrType: typeof(int));
        }
    }
}
