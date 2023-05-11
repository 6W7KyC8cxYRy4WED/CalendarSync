using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalendarSync.Migrations
{
    public partial class _00003 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ChangeKey",
                table: "CloneEvents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChangeKey",
                table: "CloneEvents");
        }
    }
}
