using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ModelGenerator.Migrations
{
    public partial class newLinks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Source_ModelPreferences_ModelPreferencesId",
                table: "Source");

            migrationBuilder.DropForeignKey(
                name: "FK_Target_ModelPreferences_ModelPreferencesId",
                table: "Target");

            migrationBuilder.DropIndex(
                name: "IX_Target_ModelPreferencesId",
                table: "Target");

            migrationBuilder.DropIndex(
                name: "IX_Source_ModelPreferencesId",
                table: "Source");

            migrationBuilder.DropColumn(
                name: "ModelPreferencesId",
                table: "Target");

            migrationBuilder.DropColumn(
                name: "ModelPreferencesId",
                table: "Source");

            migrationBuilder.CreateTable(
                name: "ModelPreferencesSource",
                columns: table => new
                {
                    SourceId = table.Column<Guid>(nullable: false),
                    ModelPreferencesId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModelPreferencesSource", x => new { x.SourceId, x.ModelPreferencesId });
                    table.ForeignKey(
                        name: "FK_ModelPreferencesSource_Source_ModelPreferencesId",
                        column: x => x.ModelPreferencesId,
                        principalTable: "Source",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ModelPreferencesSource_ModelPreferences_SourceId",
                        column: x => x.SourceId,
                        principalTable: "ModelPreferences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ModelPreferencesTarget",
                columns: table => new
                {
                    TargetId = table.Column<Guid>(nullable: false),
                    ModelPreferencesId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModelPreferencesTarget", x => new { x.TargetId, x.ModelPreferencesId });
                    table.ForeignKey(
                        name: "FK_ModelPreferencesTarget_Target_ModelPreferencesId",
                        column: x => x.ModelPreferencesId,
                        principalTable: "Target",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ModelPreferencesTarget_ModelPreferences_TargetId",
                        column: x => x.TargetId,
                        principalTable: "ModelPreferences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ModelPreferencesSource_ModelPreferencesId",
                table: "ModelPreferencesSource",
                column: "ModelPreferencesId");

            migrationBuilder.CreateIndex(
                name: "IX_ModelPreferencesTarget_ModelPreferencesId",
                table: "ModelPreferencesTarget",
                column: "ModelPreferencesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ModelPreferencesSource");

            migrationBuilder.DropTable(
                name: "ModelPreferencesTarget");

            migrationBuilder.AddColumn<Guid>(
                name: "ModelPreferencesId",
                table: "Target",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ModelPreferencesId",
                table: "Source",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Target_ModelPreferencesId",
                table: "Target",
                column: "ModelPreferencesId");

            migrationBuilder.CreateIndex(
                name: "IX_Source_ModelPreferencesId",
                table: "Source",
                column: "ModelPreferencesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Source_ModelPreferences_ModelPreferencesId",
                table: "Source",
                column: "ModelPreferencesId",
                principalTable: "ModelPreferences",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Target_ModelPreferences_ModelPreferencesId",
                table: "Target",
                column: "ModelPreferencesId",
                principalTable: "ModelPreferences",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
