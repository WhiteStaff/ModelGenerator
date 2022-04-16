using Microsoft.EntityFrameworkCore.Migrations;

namespace ModelGenerator.Migrations
{
    public partial class fix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Model_SecurityTestResult_SecurityTestResultId",
                table: "Model");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SecurityTestResult",
                table: "SecurityTestResult");

            migrationBuilder.RenameTable(
                name: "SecurityTestResult",
                newName: "SecurityTestResults");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SecurityTestResults",
                table: "SecurityTestResults",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Model_SecurityTestResults_SecurityTestResultId",
                table: "Model",
                column: "SecurityTestResultId",
                principalTable: "SecurityTestResults",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Model_SecurityTestResults_SecurityTestResultId",
                table: "Model");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SecurityTestResults",
                table: "SecurityTestResults");

            migrationBuilder.RenameTable(
                name: "SecurityTestResults",
                newName: "SecurityTestResult");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SecurityTestResult",
                table: "SecurityTestResult",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Model_SecurityTestResult_SecurityTestResultId",
                table: "Model",
                column: "SecurityTestResultId",
                principalTable: "SecurityTestResult",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
