using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ModelGenerator.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ModelPreferences",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AnonymityLevel = table.Column<int>(nullable: false),
                    LocationCharacteristic = table.Column<int>(nullable: false),
                    NetworkCharacteristic = table.Column<int>(nullable: false),
                    OtherDBConnections = table.Column<int>(nullable: false),
                    PersonalDataActionCharacteristics = table.Column<int>(nullable: false),
                    PersonalDataPermissionSplit = table.Column<int>(nullable: false),
                    PersonalDataSharingLevel = table.Column<int>(nullable: false),
                    PrivacyViolationDanger = table.Column<int>(nullable: false),
                    IntegrityViolationDanger = table.Column<int>(nullable: false),
                    AvailabilityViolationDanger = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModelPreferences", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Threat",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ThreatId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Threat", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Login = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Source",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    ModelPreferencesId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Source", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Source_ModelPreferences_ModelPreferencesId",
                        column: x => x.ModelPreferencesId,
                        principalTable: "ModelPreferences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Target",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    ModelPreferencesId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Target", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Target_ModelPreferences_ModelPreferencesId",
                        column: x => x.ModelPreferencesId,
                        principalTable: "ModelPreferences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ThreatPossibility",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ThreatId = table.Column<Guid>(nullable: false),
                    RiskProbability = table.Column<int>(nullable: false),
                    ModelPreferencesId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThreatPossibility", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ThreatPossibility_ModelPreferences_ModelPreferencesId",
                        column: x => x.ModelPreferencesId,
                        principalTable: "ModelPreferences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ThreatPossibility_Threat_ThreatId",
                        column: x => x.ThreatId,
                        principalTable: "Threat",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Model",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    PreferencesId = table.Column<Guid>(nullable: true),
                    UserId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Model", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Model_ModelPreferences_PreferencesId",
                        column: x => x.PreferencesId,
                        principalTable: "ModelPreferences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Model_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ThreatDanger",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ThreatId = table.Column<Guid>(nullable: false),
                    SourceId = table.Column<Guid>(nullable: false),
                    Properties = table.Column<string>(nullable: true),
                    DangerLevel = table.Column<int>(nullable: false),
                    ModelPreferencesId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThreatDanger", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ThreatDanger_ModelPreferences_ModelPreferencesId",
                        column: x => x.ModelPreferencesId,
                        principalTable: "ModelPreferences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ThreatDanger_Source_SourceId",
                        column: x => x.SourceId,
                        principalTable: "Source",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ThreatDanger_Threat_ThreatId",
                        column: x => x.ThreatId,
                        principalTable: "Threat",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ThreatSource",
                columns: table => new
                {
                    ThreatId = table.Column<Guid>(nullable: false),
                    SourceId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThreatSource", x => new { x.ThreatId, x.SourceId });
                    table.ForeignKey(
                        name: "FK_ThreatSource_Threat_SourceId",
                        column: x => x.SourceId,
                        principalTable: "Threat",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ThreatSource_Source_ThreatId",
                        column: x => x.ThreatId,
                        principalTable: "Source",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ThreatTarget",
                columns: table => new
                {
                    ThreatId = table.Column<Guid>(nullable: false),
                    TargetId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThreatTarget", x => new { x.ThreatId, x.TargetId });
                    table.ForeignKey(
                        name: "FK_ThreatTarget_Threat_TargetId",
                        column: x => x.TargetId,
                        principalTable: "Threat",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ThreatTarget_Target_ThreatId",
                        column: x => x.ThreatId,
                        principalTable: "Target",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ModelLine",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ModelId = table.Column<Guid>(nullable: false),
                    LineId = table.Column<int>(nullable: false),
                    ThreatId = table.Column<Guid>(nullable: false),
                    TargetId = table.Column<Guid>(nullable: false),
                    SourceId = table.Column<Guid>(nullable: false),
                    Possibility = table.Column<int>(nullable: false),
                    RealisationCoefficient = table.Column<string>(nullable: true),
                    DangerLevel = table.Column<int>(nullable: false),
                    IsActual = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModelLine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ModelLine_Model_ModelId",
                        column: x => x.ModelId,
                        principalTable: "Model",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ModelLine_Source_SourceId",
                        column: x => x.SourceId,
                        principalTable: "Source",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ModelLine_Target_TargetId",
                        column: x => x.TargetId,
                        principalTable: "Target",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ModelLine_Threat_ThreatId",
                        column: x => x.ThreatId,
                        principalTable: "Threat",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Model_PreferencesId",
                table: "Model",
                column: "PreferencesId");

            migrationBuilder.CreateIndex(
                name: "IX_Model_UserId",
                table: "Model",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ModelLine_ModelId",
                table: "ModelLine",
                column: "ModelId");

            migrationBuilder.CreateIndex(
                name: "IX_ModelLine_SourceId",
                table: "ModelLine",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_ModelLine_TargetId",
                table: "ModelLine",
                column: "TargetId");

            migrationBuilder.CreateIndex(
                name: "IX_ModelLine_ThreatId",
                table: "ModelLine",
                column: "ThreatId");

            migrationBuilder.CreateIndex(
                name: "IX_Source_ModelPreferencesId",
                table: "Source",
                column: "ModelPreferencesId");

            migrationBuilder.CreateIndex(
                name: "IX_Target_ModelPreferencesId",
                table: "Target",
                column: "ModelPreferencesId");

            migrationBuilder.CreateIndex(
                name: "IX_ThreatDanger_ModelPreferencesId",
                table: "ThreatDanger",
                column: "ModelPreferencesId");

            migrationBuilder.CreateIndex(
                name: "IX_ThreatDanger_SourceId",
                table: "ThreatDanger",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_ThreatDanger_ThreatId",
                table: "ThreatDanger",
                column: "ThreatId");

            migrationBuilder.CreateIndex(
                name: "IX_ThreatPossibility_ModelPreferencesId",
                table: "ThreatPossibility",
                column: "ModelPreferencesId");

            migrationBuilder.CreateIndex(
                name: "IX_ThreatPossibility_ThreatId",
                table: "ThreatPossibility",
                column: "ThreatId");

            migrationBuilder.CreateIndex(
                name: "IX_ThreatSource_SourceId",
                table: "ThreatSource",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_ThreatTarget_TargetId",
                table: "ThreatTarget",
                column: "TargetId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ModelLine");

            migrationBuilder.DropTable(
                name: "ThreatDanger");

            migrationBuilder.DropTable(
                name: "ThreatPossibility");

            migrationBuilder.DropTable(
                name: "ThreatSource");

            migrationBuilder.DropTable(
                name: "ThreatTarget");

            migrationBuilder.DropTable(
                name: "Model");

            migrationBuilder.DropTable(
                name: "Source");

            migrationBuilder.DropTable(
                name: "Threat");

            migrationBuilder.DropTable(
                name: "Target");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "ModelPreferences");
        }
    }
}
