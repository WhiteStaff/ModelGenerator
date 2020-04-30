using Microsoft.EntityFrameworkCore.Migrations;

namespace ModelGenerator.Migrations
{
    public partial class changeThreatModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsHasAvailabilityViolation",
                table: "Threat",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsHasIntegrityViolation",
                table: "Threat",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsHasPrivacyViolation",
                table: "Threat",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsHasAvailabilityViolation",
                table: "Threat");

            migrationBuilder.DropColumn(
                name: "IsHasIntegrityViolation",
                table: "Threat");

            migrationBuilder.DropColumn(
                name: "IsHasPrivacyViolation",
                table: "Threat");
        }
    }
}
