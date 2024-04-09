using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Promact.CustomerSuccess.Platform.Migrations
{
    /// <inheritdoc />
    public partial class RoleFixing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovalDate",
                table: "VersionHistories",
                type: "timestamp without time zone",
                nullable: true);
            migrationBuilder.AddColumn<DateTime>(
                name: "RevisionDate",
                table: "VersionHistories",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_AuditHistories_ProjectId",
                table: "AuditHistories",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditHistories_Projects_ProjectId",
                table: "AuditHistories",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditHistories_Projects_ProjectId",
                table: "AuditHistories");

            migrationBuilder.DropIndex(
                name: "IX_AuditHistories_ProjectId",
                table: "AuditHistories");

            migrationBuilder.DropColumn(
                name: "ApprovalDate",
                table: "VersionHistories");

            migrationBuilder.DropColumn(
                name: "RevisionDate",
                table: "VersionHistories");
        }
    }
}
