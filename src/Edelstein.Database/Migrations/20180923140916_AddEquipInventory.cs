using Microsoft.EntityFrameworkCore.Migrations;

namespace Edelstein.Database.Migrations
{
    public partial class AddEquipInventory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InventoryEquipID",
                table: "Characters",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Characters_InventoryEquipID",
                table: "Characters",
                column: "InventoryEquipID");

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_ItemInventory_InventoryEquipID",
                table: "Characters",
                column: "InventoryEquipID",
                principalTable: "ItemInventory",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Characters_ItemInventory_InventoryEquipID",
                table: "Characters");

            migrationBuilder.DropIndex(
                name: "IX_Characters_InventoryEquipID",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "InventoryEquipID",
                table: "Characters");
        }
    }
}
