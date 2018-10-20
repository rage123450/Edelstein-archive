using Microsoft.EntityFrameworkCore.Migrations;

namespace Edelstein.Database.Migrations
{
    public partial class RemoveRedundantColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountData_Accounts_AccountID1",
                table: "AccountData");

            migrationBuilder.DropIndex(
                name: "IX_AccountData_AccountID1",
                table: "AccountData");

            migrationBuilder.DropColumn(
                name: "AccountID1",
                table: "AccountData");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AccountID1",
                table: "AccountData",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccountData_AccountID1",
                table: "AccountData",
                column: "AccountID1");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountData_Accounts_AccountID1",
                table: "AccountData",
                column: "AccountID1",
                principalTable: "Accounts",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
