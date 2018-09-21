using Microsoft.EntityFrameworkCore.Migrations;

namespace Edelstein.Database.Migrations
{
    public partial class AddAccountSPW : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SPW",
                table: "Accounts",
                maxLength: 128,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SPW",
                table: "Accounts");
        }
    }
}
