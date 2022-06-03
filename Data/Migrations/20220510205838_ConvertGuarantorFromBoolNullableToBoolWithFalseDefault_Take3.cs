using Microsoft.EntityFrameworkCore.Migrations;

namespace AmeriForce.Data.Migrations
{
    public partial class ConvertGuarantorFromBoolNullableToBoolWithFalseDefault_Take3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "Guarantor",
                table: "Contacts",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "Guarantor",
                table: "Contacts",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool));
        }
    }
}
