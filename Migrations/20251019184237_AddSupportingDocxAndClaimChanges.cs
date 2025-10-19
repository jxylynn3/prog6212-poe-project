using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ST10448420_CMCsystem.Migrations
{
    /// <inheritdoc />
    public partial class AddSupportingDocxAndClaimChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdditionalNotes",
                table: "SupportingDocuments",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdditionalNotes",
                table: "SupportingDocuments");
        }
    }
}
