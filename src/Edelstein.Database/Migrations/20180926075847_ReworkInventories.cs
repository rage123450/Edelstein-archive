using Microsoft.EntityFrameworkCore.Migrations;

namespace Edelstein.Database.Migrations
{
    public partial class ReworkInventories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountData_Accounts_AccountID",
                table: "AccountData");

            migrationBuilder.DropForeignKey(
                name: "FK_Characters_Accounts_AccountID",
                table: "Characters");

            migrationBuilder.DropForeignKey(
                name: "FK_Characters_ItemInventory_InventoryCashID",
                table: "Characters");

            migrationBuilder.DropForeignKey(
                name: "FK_Characters_ItemInventory_InventoryConsumeID",
                table: "Characters");

            migrationBuilder.DropForeignKey(
                name: "FK_Characters_ItemInventory_InventoryEquipID",
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
                name: "FK_FunctionKey_Characters_CharacterID",
                table: "FunctionKey");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemSlot_ItemInventory_ItemInventoryID",
                table: "ItemSlot");

            migrationBuilder.DropIndex(
                name: "IX_Characters_InventoryCashID",
                table: "Characters");

            migrationBuilder.DropIndex(
                name: "IX_Characters_InventoryConsumeID",
                table: "Characters");

            migrationBuilder.DropIndex(
                name: "IX_Characters_InventoryEquipID",
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
                name: "InventoryEquipID",
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

            migrationBuilder.AddColumn<int>(
                name: "CharacterID",
                table: "ItemInventory",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "Type",
                table: "ItemInventory",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.CreateIndex(
                name: "IX_ItemInventory_CharacterID",
                table: "ItemInventory",
                column: "CharacterID");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountData_Accounts_AccountID",
                table: "AccountData",
                column: "AccountID",
                principalTable: "Accounts",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_Accounts_AccountID",
                table: "Characters",
                column: "AccountID",
                principalTable: "Accounts",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FunctionKey_Characters_CharacterID",
                table: "FunctionKey",
                column: "CharacterID",
                principalTable: "Characters",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemInventory_Characters_CharacterID",
                table: "ItemInventory",
                column: "CharacterID",
                principalTable: "Characters",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemSlot_ItemInventory_ItemInventoryID",
                table: "ItemSlot",
                column: "ItemInventoryID",
                principalTable: "ItemInventory",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountData_Accounts_AccountID",
                table: "AccountData");

            migrationBuilder.DropForeignKey(
                name: "FK_Characters_Accounts_AccountID",
                table: "Characters");

            migrationBuilder.DropForeignKey(
                name: "FK_FunctionKey_Characters_CharacterID",
                table: "FunctionKey");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemInventory_Characters_CharacterID",
                table: "ItemInventory");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemSlot_ItemInventory_ItemInventoryID",
                table: "ItemSlot");

            migrationBuilder.DropIndex(
                name: "IX_ItemInventory_CharacterID",
                table: "ItemInventory");

            migrationBuilder.DropColumn(
                name: "CharacterID",
                table: "ItemInventory");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "ItemInventory");

            migrationBuilder.AddColumn<int>(
                name: "InventoryCashID",
                table: "Characters",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InventoryConsumeID",
                table: "Characters",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InventoryEquipID",
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

            migrationBuilder.CreateIndex(
                name: "IX_Characters_InventoryCashID",
                table: "Characters",
                column: "InventoryCashID");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_InventoryConsumeID",
                table: "Characters",
                column: "InventoryConsumeID");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_InventoryEquipID",
                table: "Characters",
                column: "InventoryEquipID");

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

            migrationBuilder.AddForeignKey(
                name: "FK_AccountData_Accounts_AccountID",
                table: "AccountData",
                column: "AccountID",
                principalTable: "Accounts",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_Accounts_AccountID",
                table: "Characters",
                column: "AccountID",
                principalTable: "Accounts",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

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
                name: "FK_Characters_ItemInventory_InventoryEquipID",
                table: "Characters",
                column: "InventoryEquipID",
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
                name: "FK_FunctionKey_Characters_CharacterID",
                table: "FunctionKey",
                column: "CharacterID",
                principalTable: "Characters",
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
    }
}
