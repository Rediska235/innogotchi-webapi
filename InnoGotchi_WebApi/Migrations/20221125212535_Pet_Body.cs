using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InnoGotchi_WebApi.Migrations
{
    public partial class Pet_Body : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Ears",
                table: "Pets",
                newName: "Body");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Body",
                table: "Pets",
                newName: "Ears");
        }
    }
}
