using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ModelGenerator.Migrations
{
    public partial class AddSecurityTestResultTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SecurityTestResultId",
                table: "Model",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SecurityTestResult",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Data = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SecurityTestResult", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Model_SecurityTestResultId",
                table: "Model",
                column: "SecurityTestResultId");

            migrationBuilder.AddForeignKey(
                name: "FK_Model_SecurityTestResult_SecurityTestResultId",
                table: "Model",
                column: "SecurityTestResultId",
                principalTable: "SecurityTestResult",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Model_SecurityTestResult_SecurityTestResultId",
                table: "Model");

            migrationBuilder.DropTable(
                name: "SecurityTestResult");

            migrationBuilder.DropIndex(
                name: "IX_Model_SecurityTestResultId",
                table: "Model");

            migrationBuilder.DropColumn(
                name: "SecurityTestResultId",
                table: "Model");
        }
    }
}
