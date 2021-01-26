using Microsoft.EntityFrameworkCore.Migrations;

namespace RequestsService.Migrations
{
    public partial class fixNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Fields)",
                table: "RequestsTypes",
                newName: "Fields");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Fields",
                table: "RequestsTypes",
                newName: "Fields)");
        }
    }
}
