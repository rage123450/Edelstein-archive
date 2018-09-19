using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Edelstein.Database.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Username = table.Column<string>(maxLength: 13, nullable: true),
                    Password = table.Column<string>(maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Characters",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AccountID = table.Column<int>(nullable: true),
                    Name = table.Column<string>(maxLength: 13, nullable: true),
                    Gender = table.Column<byte>(nullable: false),
                    Skin = table.Column<byte>(nullable: false),
                    Face = table.Column<int>(nullable: false),
                    Hair = table.Column<int>(nullable: false),
                    Level = table.Column<byte>(nullable: false),
                    Job = table.Column<short>(nullable: false),
                    STR = table.Column<short>(nullable: false),
                    DEX = table.Column<short>(nullable: false),
                    INT = table.Column<short>(nullable: false),
                    LUK = table.Column<short>(nullable: false),
                    HP = table.Column<int>(nullable: false),
                    MaxHP = table.Column<int>(nullable: false),
                    MP = table.Column<int>(nullable: false),
                    MaxMP = table.Column<int>(nullable: false),
                    AP = table.Column<short>(nullable: false),
                    SP = table.Column<short>(nullable: false),
                    EXP = table.Column<int>(nullable: false),
                    POP = table.Column<short>(nullable: false),
                    Money = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Characters", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Characters_Accounts_AccountID",
                        column: x => x.AccountID,
                        principalTable: "Accounts",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Characters_AccountID",
                table: "Characters",
                column: "AccountID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Characters");

            migrationBuilder.DropTable(
                name: "Accounts");
        }
    }
}
