using Microsoft.EntityFrameworkCore.Migrations;

namespace Edelstein.Database.Migrations
{
    public partial class MoveCharacterRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Characters_Accounts_AccountID",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "WorldID",
                table: "Characters");

            migrationBuilder.RenameColumn(
                name: "AccountID",
                table: "Characters",
                newName: "DataID");

            migrationBuilder.RenameIndex(
                name: "IX_Characters_AccountID",
                table: "Characters",
                newName: "IX_Characters_DataID");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_AccountData_DataID",
                table: "Characters",
                column: "DataID",
                principalTable: "AccountData",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountData_Accounts_AccountID1",
                table: "AccountData");

            migrationBuilder.DropForeignKey(
                name: "FK_Characters_AccountData_DataID",
                table: "Characters");

            migrationBuilder.DropIndex(
                name: "IX_AccountData_AccountID1",
                table: "AccountData");

            migrationBuilder.DropColumn(
                name: "AccountID1",
                table: "AccountData");

            migrationBuilder.RenameColumn(
                name: "DataID",
                table: "Characters",
                newName: "AccountID");

            migrationBuilder.RenameIndex(
                name: "IX_Characters_DataID",
                table: "Characters",
                newName: "IX_Characters_AccountID");

            migrationBuilder.AddColumn<byte>(
                name: "WorldID",
                table: "Characters",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_Accounts_AccountID",
                table: "Characters",
                column: "AccountID",
                principalTable: "Accounts",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
