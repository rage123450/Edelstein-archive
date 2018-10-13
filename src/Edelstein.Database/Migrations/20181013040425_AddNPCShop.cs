using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Edelstein.Database.Migrations
{
    public partial class AddNPCShop : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NPCShops",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TemplateID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NPCShops", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "NPCShopItem",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TemplateID = table.Column<int>(nullable: false),
                    Position = table.Column<int>(nullable: false),
                    Price = table.Column<int>(nullable: false),
                    DiscountRate = table.Column<int>(nullable: false),
                    TokenTemplateID = table.Column<int>(nullable: false),
                    TokenPrice = table.Column<int>(nullable: false),
                    ItemPeriod = table.Column<int>(nullable: false),
                    LevelLimited = table.Column<int>(nullable: false),
                    UnitPrice = table.Column<float>(nullable: false),
                    MaxPerSlot = table.Column<int>(nullable: false),
                    Stock = table.Column<int>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    NPCShopID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NPCShopItem", x => x.ID);
                    table.ForeignKey(
                        name: "FK_NPCShopItem_NPCShops_NPCShopID",
                        column: x => x.NPCShopID,
                        principalTable: "NPCShops",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NPCShopItem_NPCShopID",
                table: "NPCShopItem",
                column: "NPCShopID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NPCShopItem");

            migrationBuilder.DropTable(
                name: "NPCShops");
        }
    }
}
