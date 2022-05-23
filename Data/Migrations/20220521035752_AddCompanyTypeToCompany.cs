using Microsoft.EntityFrameworkCore.Migrations;

namespace AmeriForce.Data.Migrations
{
    public partial class AddCompanyTypeToCompany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CompanyType",
                table: "Companies",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyType",
                table: "Companies");
        }
    }
}
