using Microsoft.EntityFrameworkCore.Migrations;

namespace AmeriForce.Data.Migrations
{
    public partial class UpdateTestCompany_ChangeZipToInt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "MailingPostalCode",
                table: "TestCompany",
                maxLength: 5,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "MailingPostalCode",
                table: "TestCompany",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldMaxLength: 5);
        }
    }
}
