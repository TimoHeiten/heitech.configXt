using Microsoft.EntityFrameworkCore.Migrations;
using heitech.configXt.Interface;

#nullable disable

namespace heitech.configXt.Migrationen
{
    public partial class AddKind : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<ConfigKind>(
                name: "Kind",
                table: "ConfigModelEntity",
                type: "INTEGER",
                maxLength: 10,
                nullable: false,
                defaultValueSql: "0");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Kind",
                table: "ConfigModelEntity");
        }
    }
}
