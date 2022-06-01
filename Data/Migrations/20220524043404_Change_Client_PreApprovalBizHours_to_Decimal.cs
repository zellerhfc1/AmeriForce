using Microsoft.EntityFrameworkCore.Migrations;

namespace AmeriForce.Data.Migrations
{
    public partial class Change_Client_PreApprovalBizHours_to_Decimal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Pre_Approval_Biz_Hours",
                table: "Clients",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Pre_Approval_Biz_Hours",
                table: "Clients",
                type: "int",
                nullable: true,
                oldClrType: typeof(decimal),
                oldNullable: true);
        }
    }
}
