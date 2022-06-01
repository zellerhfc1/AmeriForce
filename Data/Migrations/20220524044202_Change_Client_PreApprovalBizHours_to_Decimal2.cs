using Microsoft.EntityFrameworkCore.Migrations;

namespace AmeriForce.Data.Migrations
{
    public partial class Change_Client_PreApprovalBizHours_to_Decimal2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Pre_Approval_Biz_Hours",
                table: "Clients",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Pre_Approval_Biz_Hours",
                table: "Clients",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal));
        }
    }
}
