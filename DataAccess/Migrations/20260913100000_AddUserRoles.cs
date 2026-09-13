using DataAccess.Data;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
#nullable disable
namespace DataAccess.Migrations;
[DbContext(typeof(GonbadDbContext))]
[Migration("20260913100000_AddUserRoles")]
public partial class AddUserRoles : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder) => migrationBuilder.AddColumn<string>(name: "Role", table: "Khadems", type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "خادم");
    protected override void Down(MigrationBuilder migrationBuilder) => migrationBuilder.DropColumn(name: "Role", table: "Khadems");
}
