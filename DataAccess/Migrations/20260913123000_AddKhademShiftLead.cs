using DataAccess.Data;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations;

[DbContext(typeof(GonbadDbContext))]
[Migration("20260913123000_AddKhademShiftLead")]
public partial class AddKhademShiftLead : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<int>(name: "ShiftLeadId", table: "Khadems", type: "int", nullable: true);
        migrationBuilder.CreateIndex(name: "IX_Khadems_ShiftLeadId", table: "Khadems", column: "ShiftLeadId");
        migrationBuilder.AddForeignKey(name: "FK_Khadems_Khadems_ShiftLeadId", table: "Khadems", column: "ShiftLeadId", principalTable: "Khadems", principalColumn: "Id", onDelete: ReferentialAction.Restrict);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(name: "FK_Khadems_Khadems_ShiftLeadId", table: "Khadems");
        migrationBuilder.DropIndex(name: "IX_Khadems_ShiftLeadId", table: "Khadems");
        migrationBuilder.DropColumn(name: "ShiftLeadId", table: "Khadems");
    }
}
