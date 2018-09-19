using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Edelstein.Database.Migrations
{
    public partial class AddInventories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InventoryCashID",
                table: "Characters",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InventoryConsumeID",
                table: "Characters",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InventoryEquippedCashID",
                table: "Characters",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InventoryEquippedID",
                table: "Characters",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InventoryEtcID",
                table: "Characters",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InventoryInstallID",
                table: "Characters",
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

            migrationBuilder.CreateTable(
                name: "ItemSlot",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Slot = table.Column<short>(nullable: false),
                    TemplateID = table.Column<int>(nullable: false),
                    DateExpire = table.Column<DateTime>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    Number = table.Column<short>(nullable: true),
                    Attribute = table.Column<short>(nullable: true),
                    Title = table.Column<string>(maxLength: 13, nullable: true),
                    ItemInventoryItemSlotBundleID = table.Column<int>(name: "ItemInventory<ItemSlotBundle>ID", nullable: true),
                    RUC = table.Column<byte>(nullable: true),
                    CUC = table.Column<byte>(nullable: true),
                    STR = table.Column<short>(nullable: true),
                    DEX = table.Column<short>(nullable: true),
                    INT = table.Column<short>(nullable: true),
                    LUK = table.Column<short>(nullable: true),
                    MaxHP = table.Column<short>(nullable: true),
                    MaxMP = table.Column<short>(nullable: true),
                    PAD = table.Column<short>(nullable: true),
                    MAD = table.Column<short>(nullable: true),
                    PDD = table.Column<short>(nullable: true),
                    MDD = table.Column<short>(nullable: true),
                    ACC = table.Column<short>(nullable: true),
                    EVA = table.Column<short>(nullable: true),
                    Craft = table.Column<short>(nullable: true),
                    Speed = table.Column<short>(nullable: true),
                    Jump = table.Column<short>(nullable: true),
                    ItemSlotEquip_Attribute = table.Column<short>(nullable: true),
                    LevelUpType = table.Column<byte>(nullable: true),
                    Level = table.Column<byte>(nullable: true),
                    EXP = table.Column<int>(nullable: true),
                    Durability = table.Column<int>(nullable: true),
                    IUC = table.Column<int>(nullable: true),
                    Grade = table.Column<byte>(nullable: true),
                    CHUC = table.Column<byte>(nullable: true),
                    Option1 = table.Column<short>(nullable: true),
                    Option2 = table.Column<short>(nullable: true),
                    Option3 = table.Column<short>(nullable: true),
                    Socket1 = table.Column<short>(nullable: true),
                    Socket2 = table.Column<short>(nullable: true),
                    ItemInventoryItemSlotEquipID = table.Column<int>(name: "ItemInventory<ItemSlotEquip>ID", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemSlot", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ItemSlot_ItemInventory<ItemSlotBundle>_ItemInventory<ItemSlo~",
                        column: x => x.ItemInventoryItemSlotBundleID,
                        principalTable: "ItemInventory<ItemSlotBundle>",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ItemSlot_ItemInventory<ItemSlotEquip>_ItemInventory<ItemSlot~",
                        column: x => x.ItemInventoryItemSlotEquipID,
                        principalTable: "ItemInventory<ItemSlotEquip>",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Characters_InventoryCashID",
                table: "Characters",
                column: "InventoryCashID");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_InventoryConsumeID",
                table: "Characters",
                column: "InventoryConsumeID");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_InventoryEquippedCashID",
                table: "Characters",
                column: "InventoryEquippedCashID");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_InventoryEquippedID",
                table: "Characters",
                column: "InventoryEquippedID");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_InventoryEtcID",
                table: "Characters",
                column: "InventoryEtcID");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_InventoryInstallID",
                table: "Characters",
                column: "InventoryInstallID");

            migrationBuilder.CreateIndex(
                name: "IX_ItemSlot_ItemInventory<ItemSlotBundle>ID",
                table: "ItemSlot",
                column: "ItemInventory<ItemSlotBundle>ID");

            migrationBuilder.CreateIndex(
                name: "IX_ItemSlot_ItemInventory<ItemSlotEquip>ID",
                table: "ItemSlot",
                column: "ItemInventory<ItemSlotEquip>ID");

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropTable(
                name: "ItemSlot");

            migrationBuilder.DropTable(
                name: "ItemInventory<ItemSlotBundle>");

            migrationBuilder.DropTable(
                name: "ItemInventory<ItemSlotEquip>");

            migrationBuilder.DropIndex(
                name: "IX_Characters_InventoryCashID",
                table: "Characters");

            migrationBuilder.DropIndex(
                name: "IX_Characters_InventoryConsumeID",
                table: "Characters");

            migrationBuilder.DropIndex(
                name: "IX_Characters_InventoryEquippedCashID",
                table: "Characters");

            migrationBuilder.DropIndex(
                name: "IX_Characters_InventoryEquippedID",
                table: "Characters");

            migrationBuilder.DropIndex(
                name: "IX_Characters_InventoryEtcID",
                table: "Characters");

            migrationBuilder.DropIndex(
                name: "IX_Characters_InventoryInstallID",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "InventoryCashID",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "InventoryConsumeID",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "InventoryEquippedCashID",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "InventoryEquippedID",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "InventoryEtcID",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "InventoryInstallID",
                table: "Characters");
        }
    }
}
