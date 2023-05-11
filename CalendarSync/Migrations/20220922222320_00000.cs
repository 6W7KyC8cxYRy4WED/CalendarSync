using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalendarSync.Migrations
{
    public partial class _00000 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MicrosoftUsers",
                columns: table => new
                {
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AccessToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastAccessTokenStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MicrosoftUsers", x => x.Email);
                });

            migrationBuilder.CreateTable(
                name: "Calendars",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MicrosoftUserEmail = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    WriteOnly = table.Column<bool>(type: "bit", nullable: false),
                    Purge = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Calendars", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Calendars_MicrosoftUsers_MicrosoftUserEmail",
                        column: x => x.MicrosoftUserEmail,
                        principalTable: "MicrosoftUsers",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RootCalendarId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Events_Calendars_RootCalendarId",
                        column: x => x.RootCalendarId,
                        principalTable: "Calendars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CloneEvents",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RootCalendarId = table.Column<string>(type: "nvarchar(450)", nullable: false),
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
                name: "IX_Calendars_MicrosoftUserEmail",
                table: "Calendars",
                column: "MicrosoftUserEmail");

            migrationBuilder.CreateIndex(
                name: "IX_CloneEvents_EventId",
                table: "CloneEvents",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_CloneEvents_RootCalendarId",
                table: "CloneEvents",
                column: "RootCalendarId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_RootCalendarId",
                table: "Events",
                column: "RootCalendarId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CloneEvents");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Calendars");

            migrationBuilder.DropTable(
                name: "MicrosoftUsers");
        }
    }
}
