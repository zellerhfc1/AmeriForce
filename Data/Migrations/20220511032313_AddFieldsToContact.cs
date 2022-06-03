using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AmeriForce.Data.Migrations
{
    public partial class AddFieldsToContact : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EmailOptOutDate",
                table: "Contacts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MailingSuite",
                table: "Contacts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OtherSuite",
                table: "Contacts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailOptOutDate",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "MailingSuite",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "OtherSuite",
                table: "Contacts");
        }
    }
}
