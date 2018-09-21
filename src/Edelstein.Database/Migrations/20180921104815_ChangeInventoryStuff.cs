using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Edelstein.Database.Migrations
{
    public partial class ChangeInventoryStuff : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Characters_ItemInventory<ItemSlotBundle>_InventoryCashID",
                table: "Characters");

            migrationBuilder.DropForeignKey(
                name: "FK_Characters_ItemInventory<ItemSlotBundle>_InventoryConsumeID",
                table: "Characters");

            migrationBuilder.DropForeignKey(
                name: "FK_Characters_ItemInventory<ItemSlotEquip>_InventoryEquippedCas~",
                table: "Characters");

            migrationBuilder.DropForeignKey(
                name: "FK_Characters_ItemInventory<ItemSlotEquip>_InventoryEquippedID",
                table: "Characters");

            migrationBuilder.DropForeignKey(
                name: "FK_Characters_ItemInventory<ItemSlotBundle>_InventoryEtcID",
                table: "Characters");

            migrationBuilder.DropForeignKey(
                name: "FK_Characters_ItemInventory<ItemSlotBundle>_InventoryInstallID",
                table: "Characters");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemSlot_ItemInventory<ItemSlotBundle>_ItemInventory<ItemSlo~",
                table: "ItemSlot");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemSlot_ItemInventory<ItemSlotEquip>_ItemInventory<ItemSlot~",
                table: "ItemSlot");

            migrationBuilder.DropTable(
                name: "ItemInventory<ItemSlotBundle>");

            migrationBuilder.DropTable(
                name: "ItemInventory<ItemSlotEquip>");

            migrationBuilder.DropIndex(
                name: "IX_ItemSlot_ItemInventory<ItemSlotBundle>ID",
                table: "ItemSlot");

            migrationBuilder.DropColumn(
                name: "ItemInventory<ItemSlotBundle>ID",
                table: "ItemSlot");

            migrationBuilder.RenameColumn(
                name: "ItemInventory<ItemSlotEquip>ID",
                table: "ItemSlot",
                newName: "ItemInventoryID");

            migrationBuilder.RenameIndex(
                name: "IX_ItemSlot_ItemInventory<ItemSlotEquip>ID",
                table: "ItemSlot",
                newName: "IX_ItemSlot_ItemInventoryID");

            migrationBuilder.CreateTable(
                name: "ItemInventory",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SlotMax = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemInventory", x => x.ID);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_ItemInventory_InventoryCashID",
                table: "Characters",
                column: "InventoryCashID",
                principalTable: "ItemInventory",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_ItemInventory_InventoryConsumeID",
                table: "Characters",
                column: "InventoryConsumeID",
                principalTable: "ItemInventory",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_ItemInventory_InventoryEquippedCashID",
                table: "Characters",
                column: "InventoryEquippedCashID",
                principalTable: "ItemInventory",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_ItemInventory_InventoryEquippedID",
                table: "Characters",
                column: "InventoryEquippedID",
                principalTable: "ItemInventory",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_ItemInventory_InventoryEtcID",
                table: "Characters",
                column: "InventoryEtcID",
                principalTable: "ItemInventory",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_ItemInventory_InventoryInstallID",
                table: "Characters",
                column: "InventoryInstallID",
                principalTable: "ItemInventory",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemSlot_ItemInventory_ItemInventoryID",
                table: "ItemSlot",
                column: "ItemInventoryID",
                principalTable: "ItemInventory",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Characters_ItemInventory_InventoryCashID",
                table: "Characters");

            migrationBuilder.DropForeignKey(
                name: "FK_Characters_ItemInventory_InventoryConsumeID",
                table: "Characters");

            migrationBuilder.DropForeignKey(
                name: "FK_Characters_ItemInventory_InventoryEquippedCashID",
                table: "Characters");

            migrationBuilder.DropForeignKey(
                name: "FK_Characters_ItemInventory_InventoryEquippedID",
                table: "Characters");

            migrationBuilder.DropForeignKey(
                name: "FK_Characters_ItemInventory_InventoryEtcID",
                table: "Characters");

            migrationBuilder.DropForeignKey(
                name: "FK_Characters_ItemInventory_InventoryInstallID",
                table: "Characters");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemSlot_ItemInventory_ItemInventoryID",
                table: "ItemSlot");

            migrationBuilder.DropTable(
                name: "ItemInventory");

            migrationBuilder.RenameColumn(
                name: "ItemInventoryID",
                table: "ItemSlot",
                newName: "ItemInventory<ItemSlotEquip>ID");

            migrationBuilder.RenameIndex(
                name: "IX_ItemSlot_ItemInventoryID",
                table: "ItemSlot",
                newName: "IX_ItemSlot_ItemInventory<ItemSlotEquip>ID");

            migrationBuilder.AddColumn<int>(
                name: "ItemInventory<ItemSlotBundle>ID",
                table: "ItemSlot",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ItemInventory<ItemSlotBundle>",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SlotMax = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemInventory<ItemSlotBundle>", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ItemInventory<ItemSlotEquip>",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SlotMax = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemInventory<ItemSlotEquip>", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItemSlot_ItemInventory<ItemSlotBundle>ID",
                table: "ItemSlot",
                column: "ItemInventory<ItemSlotBundle>ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_ItemInventory<ItemSlotBundle>_InventoryCashID",
                table: "Characters",
                column: "InventoryCashID",
                principalTable: "ItemInventory<ItemSlotBundle>",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_ItemInventory<ItemSlotBundle>_InventoryConsumeID",
                table: "Characters",
                column: "InventoryConsumeID",
                principalTable: "ItemInventory<ItemSlotBundle>",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_ItemInventory<ItemSlotEquip>_InventoryEquippedCas~",
                table: "Characters",
                column: "InventoryEquippedCashID",
                principalTable: "ItemInventory<ItemSlotEquip>",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_ItemInventory<ItemSlotEquip>_InventoryEquippedID",
                table: "Characters",
                column: "InventoryEquippedID",
                principalTable: "ItemInventory<ItemSlotEquip>",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_ItemInventory<ItemSlotBundle>_InventoryEtcID",
                table: "Characters",
                column: "InventoryEtcID",
                principalTable: "ItemInventory<ItemSlotBundle>",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_ItemInventory<ItemSlotBundle>_InventoryInstallID",
                table: "Characters",
                column: "InventoryInstallID",
                principalTable: "ItemInventory<ItemSlotBundle>",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemSlot_ItemInventory<ItemSlotBundle>_ItemInventory<ItemSlo~",
                table: "ItemSlot",
                column: "ItemInventory<ItemSlotBundle>ID",
                principalTable: "ItemInventory<ItemSlotBundle>",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemSlot_ItemInventory<ItemSlotEquip>_ItemInventory<ItemSlot~",
                table: "ItemSlot",
                column: "ItemInventory<ItemSlotEquip>ID",
                principalTable: "ItemInventory<ItemSlotEquip>",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
