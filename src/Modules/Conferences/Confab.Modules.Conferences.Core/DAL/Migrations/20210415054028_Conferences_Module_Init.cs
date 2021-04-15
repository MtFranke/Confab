using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Confab.Modules.Conferences.Core.DAL.Migrations
{
    public partial class Conferences_Module_Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "conferences");

            migrationBuilder.CreateTable(
                name: "Host",
                schema: "conferences",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Host", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Conference",
                schema: "conferences",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    HostId = table.Column<Guid>(type: "uuid", nullable: false),
                    Host = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Location = table.Column<string>(type: "text", nullable: true),
                    LogoUrl = table.Column<string>(type: "text", nullable: true),
                    ParticipantLimit = table.Column<int>(type: "integer", nullable: true),
                    From = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    To = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conference", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Conference_Host_HostId",
                        column: x => x.HostId,
                        principalSchema: "conferences",
                        principalTable: "Host",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Conference_HostId",
                schema: "conferences",
                table: "Conference",
                column: "HostId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Conference",
                schema: "conferences");

            migrationBuilder.DropTable(
                name: "Host",
                schema: "conferences");
        }
    }
}
