using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScholarBridge.Migrations
{
    /// <inheritdoc />
    public partial class ScholarshipEdit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequiresDocument",
                table: "Applications");

            migrationBuilder.AddColumn<bool>(
                name: "RequiresDocument",
                table: "Scholarships",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequiresDocument",
                table: "Scholarships");

            migrationBuilder.AddColumn<bool>(
                name: "RequiresDocument",
                table: "Applications",
                type: "bit",
                nullable: true);
        }
    }
}
