using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScholarBridge.Migrations
{
    /// <inheritdoc />
    public partial class DonorNameAndSurname : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DonorName",
                table: "DonorDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DonorSurname",
                table: "DonorDetails",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DonorName",
                table: "DonorDetails");

            migrationBuilder.DropColumn(
                name: "DonorSurname",
                table: "DonorDetails");
        }
    }
}
