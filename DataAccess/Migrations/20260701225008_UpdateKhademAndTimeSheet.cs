using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateKhademAndTimeSheet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RegisteredByKhademId",
                table: "TimeSheets",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Khadem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Specialization = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WorkingDay = table.Column<int>(type: "int", nullable: false),
                    Shift = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Khadem", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TimeSheets_RegisteredByKhademId",
                table: "TimeSheets",
                column: "RegisteredByKhademId");

            migrationBuilder.AddForeignKey(
                name: "FK_TimeSheets_Khadem_RegisteredByKhademId",
                table: "TimeSheets",
                column: "RegisteredByKhademId",
                principalTable: "Khadem",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeSheets_Khadem_RegisteredByKhademId",
                table: "TimeSheets");

            migrationBuilder.DropTable(
                name: "Khadem");

            migrationBuilder.DropIndex(
                name: "IX_TimeSheets_RegisteredByKhademId",
                table: "TimeSheets");

            migrationBuilder.DropColumn(
                name: "RegisteredByKhademId",
                table: "TimeSheets");
        }
    }
}
