using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class init2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InfoKhademId",
                table: "TimeShitChilds",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "InfochildId",
                table: "TimeShitChilds",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TimeShitChilds_InfochildId",
                table: "TimeShitChilds",
                column: "InfochildId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeShitChilds_InfoKhademId",
                table: "TimeShitChilds",
                column: "InfoKhademId");

            migrationBuilder.AddForeignKey(
                name: "FK_TimeShitChilds_InfoChilds_InfochildId",
                table: "TimeShitChilds",
                column: "InfochildId",
                principalTable: "InfoChilds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TimeShitChilds_InfoKhadems_InfoKhademId",
                table: "TimeShitChilds",
                column: "InfoKhademId",
                principalTable: "InfoKhadems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeShitChilds_InfoChilds_InfochildId",
                table: "TimeShitChilds");

            migrationBuilder.DropForeignKey(
                name: "FK_TimeShitChilds_InfoKhadems_InfoKhademId",
                table: "TimeShitChilds");

            migrationBuilder.DropIndex(
                name: "IX_TimeShitChilds_InfochildId",
                table: "TimeShitChilds");

            migrationBuilder.DropIndex(
                name: "IX_TimeShitChilds_InfoKhademId",
                table: "TimeShitChilds");

            migrationBuilder.DropColumn(
                name: "InfoKhademId",
                table: "TimeShitChilds");

            migrationBuilder.DropColumn(
                name: "InfochildId",
                table: "TimeShitChilds");
        }
    }
}
