using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddShiftReport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ShiftReports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReportDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Shift = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuranActivity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AhkamActivity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GameActivity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PoemActivity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StoryActivity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CraftActivity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalParticipants = table.Column<int>(type: "int", nullable: false),
                    RegularCount = table.Column<int>(type: "int", nullable: false),
                    TravelerCount = table.Column<int>(type: "int", nullable: false),
                    PresentKhads = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KhademId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShiftReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShiftReports_Khadems_KhademId",
                        column: x => x.KhademId,
                        principalTable: "Khadems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShiftReports_KhademId",
                table: "ShiftReports",
                column: "KhademId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShiftReports");
        }
    }
}
