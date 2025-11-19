using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ST10448420_CMCsystem.Migrations
{
    /// <inheritdoc />
    public partial class AddHourlyRateToLecturer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "HourlyRate",
                table: "Lecturer",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HourlyRate",
                table: "Lecturer");
        }
    }
}
