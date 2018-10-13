using Microsoft.EntityFrameworkCore.Migrations;

namespace Edelstein.Database.Migrations
{
    public partial class AddNPCShopCascadeDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NPCShopItem_NPCShops_NPCShopID",
                table: "NPCShopItem");

            migrationBuilder.AddForeignKey(
                name: "FK_NPCShopItem_NPCShops_NPCShopID",
                table: "NPCShopItem",
                column: "NPCShopID",
                principalTable: "NPCShops",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NPCShopItem_NPCShops_NPCShopID",
                table: "NPCShopItem");

            migrationBuilder.AddForeignKey(
                name: "FK_NPCShopItem_NPCShops_NPCShopID",
                table: "NPCShopItem",
                column: "NPCShopID",
                principalTable: "NPCShops",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
