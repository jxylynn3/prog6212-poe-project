using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ST10448420_CMCsystem.Migrations
{
    /// <inheritdoc />
    public partial class AddAdditionalNotesColumnToSupportingDocument : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SupportingDocuments_Claim_ClaimID",
                table: "SupportingDocuments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SupportingDocuments",
                table: "SupportingDocuments");

            migrationBuilder.RenameTable(
                name: "SupportingDocuments",
                newName: "SupportingDocument");

            migrationBuilder.RenameIndex(
                name: "IX_SupportingDocuments_ClaimID",
                table: "SupportingDocument",
                newName: "IX_SupportingDocument_ClaimID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SupportingDocument",
                table: "SupportingDocument",
                column: "DocumentID");

            migrationBuilder.AddForeignKey(
                name: "FK_SupportingDocument_Claim_ClaimID",
                table: "SupportingDocument",
                column: "ClaimID",
                principalTable: "Claim",
                principalColumn: "ClaimID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SupportingDocument_Claim_ClaimID",
                table: "SupportingDocument");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SupportingDocument",
                table: "SupportingDocument");

            migrationBuilder.RenameTable(
                name: "SupportingDocument",
                newName: "SupportingDocuments");

            migrationBuilder.RenameIndex(
                name: "IX_SupportingDocument_ClaimID",
                table: "SupportingDocuments",
                newName: "IX_SupportingDocuments_ClaimID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SupportingDocuments",
                table: "SupportingDocuments",
                column: "DocumentID");

            migrationBuilder.AddForeignKey(
                name: "FK_SupportingDocuments_Claim_ClaimID",
                table: "SupportingDocuments",
                column: "ClaimID",
                principalTable: "Claim",
                principalColumn: "ClaimID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
