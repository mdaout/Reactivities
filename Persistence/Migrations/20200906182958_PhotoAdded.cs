using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class PhotoAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Bio",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Photos",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Uri = table.Column<string>(nullable: true),
                    IsMain = table.Column<bool>(nullable: false),
                    AppUserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Photos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Photos_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            // migrationBuilder.CreateTable(
            //     name: "UserActivities",
            //     columns: table => new
            //     {
            //         AppUserId = table.Column<string>(nullable: false),
            //         ActivityId = table.Column<Guid>(nullable: false),
            //         DateJoined = table.Column<DateTime>(nullable: false),
            //         IsHost = table.Column<bool>(nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_UserActivities", x => new { x.AppUserId, x.ActivityId });
            //         table.ForeignKey(
            //             name: "FK_UserActivities_Activities_ActivityId",
            //             column: x => x.ActivityId,
            //             principalTable: "Activities",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //         table.ForeignKey(
            //             name: "FK_UserActivities_AspNetUsers_AppUserId",
            //             column: x => x.AppUserId,
            //             principalTable: "AspNetUsers",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //     });

            migrationBuilder.CreateIndex(
                name: "IX_Photos_AppUserId",
                table: "Photos",
                column: "AppUserId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_UserActivities_ActivityId",
            //     table: "UserActivities",
            //     column: "ActivityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Photos");

            migrationBuilder.DropTable(
                name: "UserActivities");

            migrationBuilder.DropColumn(
                name: "Bio",
                table: "AspNetUsers");
        }
    }
}
