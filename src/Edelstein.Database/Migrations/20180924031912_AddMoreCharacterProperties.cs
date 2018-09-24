using Microsoft.EntityFrameworkCore.Migrations;

namespace Edelstein.Database.Migrations
{
    public partial class AddMoreCharacterProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FieldID",
                table: "Characters",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<byte>(
                name: "FieldPortal",
                table: "Characters",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<int>(
                name: "PlayTime",
                table: "Characters",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<short>(
                name: "SubJob",
                table: "Characters",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<int>(
                name: "TempEXP",
                table: "Characters",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FieldID",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "FieldPortal",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "PlayTime",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "SubJob",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "TempEXP",
                table: "Characters");
        }
    }
}
