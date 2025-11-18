using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ST10448420_CMCsystem.Migrations
{
    /// <inheritdoc />
    public partial class SeedTheHRuserInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "HR",
                columns: new[] { "HRID", "Email", "FirstName", "Password", "Surname", "Username" },
                values: new object[] { "HR000001", "hr@system.com", "System", "#HRadmin123#", "Administrator", "HRadmin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "HR",
                keyColumn: "HRID",
                keyValue: "HR000001");
        }
    }
}
