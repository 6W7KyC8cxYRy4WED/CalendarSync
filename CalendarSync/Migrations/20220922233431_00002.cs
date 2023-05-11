using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalendarSync.Migrations
{
    public partial class _00002 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LinkedCalendars",
                columns: table => new
                {
                    RootCalendarId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LinkedCalendarId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LinkedCalendars", x => new { x.RootCalendarId, x.LinkedCalendarId });
                    table.ForeignKey(
                        name: "FK_LinkedCalendars_Calendars_RootCalendarId",
                        column: x => x.RootCalendarId,
                        principalTable: "Calendars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LinkedCalendars");
        }
    }
}
