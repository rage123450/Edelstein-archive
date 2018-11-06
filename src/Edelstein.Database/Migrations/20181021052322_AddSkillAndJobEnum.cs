using Microsoft.EntityFrameworkCore.Migrations;

namespace Edelstein.Database.Migrations
{
    public partial class AddSkillAndJobEnum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SkillID",
                table: "SkillRecord");

            migrationBuilder.AddColumn<int>(
                name: "Skill",
                table: "SkillRecord",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Skill",
                table: "SkillRecord");

            migrationBuilder.AddColumn<int>(
                name: "SkillID",
                table: "SkillRecord",
                nullable: false,
                defaultValue: 0);
        }
    }
}
