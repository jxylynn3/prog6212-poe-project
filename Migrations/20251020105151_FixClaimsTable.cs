using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ST10448420_CMCsystem.Migrations
{
    /// <inheritdoc />
    public partial class FixClaimsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Approvals_AcademicManagers_AcademicManagerID",
                table: "Approvals");

            migrationBuilder.DropForeignKey(
                name: "FK_Approvals_Claims_ClaimID",
                table: "Approvals");

            migrationBuilder.DropForeignKey(
                name: "FK_Approvals_ProgrammeCoordinators_ProgrammeCoordinatorID",
                table: "Approvals");

            migrationBuilder.DropForeignKey(
                name: "FK_Claims_Lecturers_LecturerID",
                table: "Claims");

            migrationBuilder.DropForeignKey(
                name: "FK_SupportingDocuments_Claims_ClaimID",
                table: "SupportingDocuments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProgrammeCoordinators",
                table: "ProgrammeCoordinators");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Lecturers",
                table: "Lecturers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Claims",
                table: "Claims");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AcademicManagers",
                table: "AcademicManagers");

            migrationBuilder.RenameTable(
                name: "ProgrammeCoordinators",
                newName: "ProgrammeCoordinator");

            migrationBuilder.RenameTable(
                name: "Lecturers",
                newName: "Lecturer");

            migrationBuilder.RenameTable(
                name: "Claims",
                newName: "Claim");

            migrationBuilder.RenameTable(
                name: "AcademicManagers",
                newName: "AcademicManager");

            migrationBuilder.RenameIndex(
                name: "IX_Claims_LecturerID",
                table: "Claim",
                newName: "IX_Claim_LecturerID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProgrammeCoordinator",
                table: "ProgrammeCoordinator",
                column: "CoordinatorID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Lecturer",
                table: "Lecturer",
                column: "LecturerID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Claim",
                table: "Claim",
                column: "ClaimID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AcademicManager",
                table: "AcademicManager",
                column: "AcademicManagerID");

            migrationBuilder.AddForeignKey(
                name: "FK_Approvals_AcademicManager_AcademicManagerID",
                table: "Approvals",
                column: "AcademicManagerID",
                principalTable: "AcademicManager",
                principalColumn: "AcademicManagerID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Approvals_Claim_ClaimID",
                table: "Approvals",
                column: "ClaimID",
                principalTable: "Claim",
                principalColumn: "ClaimID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Approvals_ProgrammeCoordinator_ProgrammeCoordinatorID",
                table: "Approvals",
                column: "ProgrammeCoordinatorID",
                principalTable: "ProgrammeCoordinator",
                principalColumn: "CoordinatorID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Claim_Lecturer_LecturerID",
                table: "Claim",
                column: "LecturerID",
                principalTable: "Lecturer",
                principalColumn: "LecturerID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SupportingDocuments_Claim_ClaimID",
                table: "SupportingDocuments",
                column: "ClaimID",
                principalTable: "Claim",
                principalColumn: "ClaimID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Approvals_AcademicManager_AcademicManagerID",
                table: "Approvals");

            migrationBuilder.DropForeignKey(
                name: "FK_Approvals_Claim_ClaimID",
                table: "Approvals");

            migrationBuilder.DropForeignKey(
                name: "FK_Approvals_ProgrammeCoordinator_ProgrammeCoordinatorID",
                table: "Approvals");

            migrationBuilder.DropForeignKey(
                name: "FK_Claim_Lecturer_LecturerID",
                table: "Claim");

            migrationBuilder.DropForeignKey(
                name: "FK_SupportingDocuments_Claim_ClaimID",
                table: "SupportingDocuments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProgrammeCoordinator",
                table: "ProgrammeCoordinator");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Lecturer",
                table: "Lecturer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Claim",
                table: "Claim");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AcademicManager",
                table: "AcademicManager");

            migrationBuilder.RenameTable(
                name: "ProgrammeCoordinator",
                newName: "ProgrammeCoordinators");

            migrationBuilder.RenameTable(
                name: "Lecturer",
                newName: "Lecturers");

            migrationBuilder.RenameTable(
                name: "Claim",
                newName: "Claims");

            migrationBuilder.RenameTable(
                name: "AcademicManager",
                newName: "AcademicManagers");

            migrationBuilder.RenameIndex(
                name: "IX_Claim_LecturerID",
                table: "Claims",
                newName: "IX_Claims_LecturerID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProgrammeCoordinators",
                table: "ProgrammeCoordinators",
                column: "CoordinatorID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Lecturers",
                table: "Lecturers",
                column: "LecturerID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Claims",
                table: "Claims",
                column: "ClaimID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AcademicManagers",
                table: "AcademicManagers",
                column: "AcademicManagerID");

            migrationBuilder.AddForeignKey(
                name: "FK_Approvals_AcademicManagers_AcademicManagerID",
                table: "Approvals",
                column: "AcademicManagerID",
                principalTable: "AcademicManagers",
                principalColumn: "AcademicManagerID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Approvals_Claims_ClaimID",
                table: "Approvals",
                column: "ClaimID",
                principalTable: "Claims",
                principalColumn: "ClaimID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Approvals_ProgrammeCoordinators_ProgrammeCoordinatorID",
                table: "Approvals",
                column: "ProgrammeCoordinatorID",
                principalTable: "ProgrammeCoordinators",
                principalColumn: "CoordinatorID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Claims_Lecturers_LecturerID",
                table: "Claims",
                column: "LecturerID",
                principalTable: "Lecturers",
                principalColumn: "LecturerID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SupportingDocuments_Claims_ClaimID",
                table: "SupportingDocuments",
                column: "ClaimID",
                principalTable: "Claims",
                principalColumn: "ClaimID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
