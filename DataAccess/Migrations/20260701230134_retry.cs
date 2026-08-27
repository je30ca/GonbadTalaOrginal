using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class retry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeSheets_Khadem_RegisteredByKhademId",
                table: "TimeSheets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Khadem",
                table: "Khadem");

            migrationBuilder.RenameTable(
                name: "Khadem",
                newName: "Khadems");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Khadems",
                table: "Khadems",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TimeSheets_Khadems_RegisteredByKhademId",
                table: "TimeSheets",
                column: "RegisteredByKhademId",
                principalTable: "Khadems",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeSheets_Khadems_RegisteredByKhademId",
                table: "TimeSheets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Khadems",
                table: "Khadems");

            migrationBuilder.RenameTable(
                name: "Khadems",
                newName: "Khadem");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Khadem",
                table: "Khadem",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TimeSheets_Khadem_RegisteredByKhademId",
                table: "TimeSheets",
                column: "RegisteredByKhademId",
                principalTable: "Khadem",
                principalColumn: "Id");
        }
    }
}
