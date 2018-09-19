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
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Username = table.Column<string>(maxLength: 13, nullable: true),
                    Password = table.Column<string>(maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Characters",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AccountId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(maxLength: 13, nullable: true),
                    Gender = table.Column<byte>(nullable: false),
                    Skin = table.Column<byte>(nullable: false),
                    Face = table.Column<int>(nullable: false),
                    Hair = table.Column<int>(nullable: false),
                    Level = table.Column<byte>(nullable: false),
                    Job = table.Column<short>(nullable: false),
                    Str = table.Column<short>(nullable: false),
                    Dex = table.Column<short>(nullable: false),
                    Int = table.Column<short>(nullable: false),
                    Luk = table.Column<short>(nullable: false),
                    Hp = table.Column<int>(nullable: false),
                    MaxHp = table.Column<int>(nullable: false),
                    Mp = table.Column<int>(nullable: false),
                    MaxMp = table.Column<int>(nullable: false),
                    Ap = table.Column<short>(nullable: false),
                    Sp = table.Column<short>(nullable: false),
                    Exp = table.Column<int>(nullable: false),
                    Pop = table.Column<short>(nullable: false),
                    Money = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Characters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Characters_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Characters_AccountId",
                table: "Characters",
                column: "AccountId");
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
