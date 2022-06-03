using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AmeriForce.Data.Migrations
{
    public partial class AddLOVs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LOV_MailingLists",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdateDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOV_MailingLists", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "LOV_RelationshipStatuses",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<string>(nullable: true),
                    DateUpdated = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOV_RelationshipStatuses", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "LOV_TagGradeSorts",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Grade = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    DateUpdated = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOV_TagGradeSorts", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "LOV_UpdateNeededs",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UpdateField = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOV_UpdateNeededs", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LOV_MailingLists");

            migrationBuilder.DropTable(
                name: "LOV_RelationshipStatuses");

            migrationBuilder.DropTable(
                name: "LOV_TagGradeSorts");

            migrationBuilder.DropTable(
                name: "LOV_UpdateNeededs");
        }
    }
}
