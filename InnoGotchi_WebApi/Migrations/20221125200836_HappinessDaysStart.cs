using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InnoGotchi_WebApi.Migrations
{
    public partial class HappinessDaysStart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pets_Farms_FarmId",
                table: "Pets");

            migrationBuilder.AlterColumn<int>(
                name: "FarmId",
                table: "Pets",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "HappinessDaysStart",
                table: "Pets",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_Pets_Farms_FarmId",
                table: "Pets",
                column: "FarmId",
                principalTable: "Farms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pets_Farms_FarmId",
                table: "Pets");

            migrationBuilder.DropColumn(
                name: "HappinessDaysStart",
                table: "Pets");

            migrationBuilder.AlterColumn<int>(
                name: "FarmId",
                table: "Pets",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Pets_Farms_FarmId",
                table: "Pets",
                column: "FarmId",
                principalTable: "Farms",
                principalColumn: "Id");
        }
    }
}
