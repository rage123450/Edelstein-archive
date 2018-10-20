using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Edelstein.Database.Migrations
{
    public partial class AddItemSlotPet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(
                name: "ItemSlotPet_Attribute",
                table: "ItemSlot",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateDead",
                table: "ItemSlot",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "ItemSlotPet_Level",
                table: "ItemSlot",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "PetAttribute",
                table: "ItemSlot",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PetName",
                table: "ItemSlot",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "PetSkill",
                table: "ItemSlot",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RemainLife",
                table: "ItemSlot",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "Repleteness",
                table: "ItemSlot",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "Tameness",
                table: "ItemSlot",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ItemSlotPet_Attribute",
                table: "ItemSlot");

            migrationBuilder.DropColumn(
                name: "DateDead",
                table: "ItemSlot");

            migrationBuilder.DropColumn(
                name: "ItemSlotPet_Level",
                table: "ItemSlot");

            migrationBuilder.DropColumn(
                name: "PetAttribute",
                table: "ItemSlot");

            migrationBuilder.DropColumn(
                name: "PetName",
                table: "ItemSlot");

            migrationBuilder.DropColumn(
                name: "PetSkill",
                table: "ItemSlot");

            migrationBuilder.DropColumn(
                name: "RemainLife",
                table: "ItemSlot");

            migrationBuilder.DropColumn(
                name: "Repleteness",
                table: "ItemSlot");

            migrationBuilder.DropColumn(
                name: "Tameness",
                table: "ItemSlot");
        }
    }
}
