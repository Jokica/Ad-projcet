using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Announcements.Web.Migrations
{
    public partial class fileName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyFile_Companies_CompanyId",
                table: "CompanyFile");

            migrationBuilder.AlterColumn<long>(
                name: "CompanyId",
                table: "CompanyFile",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "CompanyFile",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "IndexAdViewModel",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CompanyName = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ImageUrl = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    IsApproved = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IndexAdViewModel", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyFile_Companies_CompanyId",
                table: "CompanyFile",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyFile_Companies_CompanyId",
                table: "CompanyFile");

            migrationBuilder.DropTable(
                name: "IndexAdViewModel");

            migrationBuilder.DropColumn(
                name: "FileName",
                table: "CompanyFile");

            migrationBuilder.AlterColumn<long>(
                name: "CompanyId",
                table: "CompanyFile",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyFile_Companies_CompanyId",
                table: "CompanyFile",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
