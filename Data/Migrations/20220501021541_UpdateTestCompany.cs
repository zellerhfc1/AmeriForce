using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AmeriForce.Data.Migrations
{
    public partial class UpdateTestCompany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CharterState",
                table: "TestCompany",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "TestCompany",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "TestCompany",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedDate",
                table: "TestCompany",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "MailingAddress",
                table: "TestCompany",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MailingCity",
                table: "TestCompany",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MailingPostalCode",
                table: "TestCompany",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MailingState",
                table: "TestCompany",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SICCode",
                table: "TestCompany",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CharterState",
                table: "TestCompany");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "TestCompany");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "TestCompany");

            migrationBuilder.DropColumn(
                name: "LastModifiedDate",
                table: "TestCompany");

            migrationBuilder.DropColumn(
                name: "MailingAddress",
                table: "TestCompany");

            migrationBuilder.DropColumn(
                name: "MailingCity",
                table: "TestCompany");

            migrationBuilder.DropColumn(
                name: "MailingPostalCode",
                table: "TestCompany");

            migrationBuilder.DropColumn(
                name: "MailingState",
                table: "TestCompany");

            migrationBuilder.DropColumn(
                name: "SICCode",
                table: "TestCompany");
        }
    }
}
