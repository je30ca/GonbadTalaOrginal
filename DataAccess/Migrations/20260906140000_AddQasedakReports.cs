using DataAccess.Data;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations;

[DbContext(typeof(GonbadDbContext))]
[Migration("20260906140000_AddQasedakReports")]
public partial class AddQasedakReports : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "QasedakReports",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                ExecutionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                Shift = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                Subject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ExecutionCount = table.Column<int>(type: "int", nullable: false),
                PresentedCircles = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ExecutionLocations = table.Column<string>(type: "nvarchar(max)", nullable: false),
                AudienceCount = table.Column<int>(type: "int", nullable: false),
                AgeGroup = table.Column<string>(type: "nvarchar(max)", nullable: false),
                CreatedByKhademId = table.Column<int>(type: "int", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_QasedakReports", x => x.Id);
                table.ForeignKey("FK_QasedakReports_Khadems_CreatedByKhademId", x => x.CreatedByKhademId, "Khadems", "Id");
            });

        migrationBuilder.CreateTable(
            name: "QasedakReportKhadems",
            columns: table => new { QasedakReportId = table.Column<int>(type: "int", nullable: false), KhademId = table.Column<int>(type: "int", nullable: false) },
            constraints: table =>
            {
                table.PrimaryKey("PK_QasedakReportKhadems", x => new { x.QasedakReportId, x.KhademId });
                table.ForeignKey("FK_QasedakReportKhadems_QasedakReports_QasedakReportId", x => x.QasedakReportId, "QasedakReports", "Id", onDelete: ReferentialAction.Cascade);
                table.ForeignKey("FK_QasedakReportKhadems_Khadems_KhademId", x => x.KhademId, "Khadems", "Id", onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateIndex(name: "IX_QasedakReports_CreatedByKhademId", table: "QasedakReports", column: "CreatedByKhademId");
        migrationBuilder.CreateIndex(name: "IX_QasedakReports_ExecutionDate_Shift", table: "QasedakReports", columns: new[] { "ExecutionDate", "Shift" }, unique: true);
        migrationBuilder.CreateIndex(name: "IX_QasedakReportKhadems_KhademId", table: "QasedakReportKhadems", column: "KhademId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "QasedakReportKhadems");
        migrationBuilder.DropTable(name: "QasedakReports");
    }
}
