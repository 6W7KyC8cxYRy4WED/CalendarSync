using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalendarSync.Migrations
{
    public partial class _00004 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CloneEvents");

            migrationBuilder.CreateTable(
                name: "ClonedEvents",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RootCalendarId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ChangeKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EventId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClonedEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClonedEvents_Calendars_RootCalendarId",
                        column: x => x.RootCalendarId,
                        principalTable: "Calendars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClonedEvents_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClonedEvents_EventId",
                table: "ClonedEvents",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_ClonedEvents_RootCalendarId",
                table: "ClonedEvents",
                column: "RootCalendarId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClonedEvents");

            migrationBuilder.CreateTable(
                name: "CloneEvents",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RootCalendarId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ChangeKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EventId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CloneEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CloneEvents_Calendars_RootCalendarId",
                        column: x => x.RootCalendarId,
                        principalTable: "Calendars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CloneEvents_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CloneEvents_EventId",
                table: "CloneEvents",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_CloneEvents_RootCalendarId",
                table: "CloneEvents",
                column: "RootCalendarId");
        }
    }
}
