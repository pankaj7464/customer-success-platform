using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Promact.CustomerSuccess.Platform.Migrations
{
    /// <inheritdoc />
    public partial class AuditHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditHistories_IdentityUser_UserId",
                table: "AuditHistories");

            migrationBuilder.DropIndex(
                name: "IX_AuditHistories_UserId",
                table: "AuditHistories");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AuditHistories");

            migrationBuilder.RenameColumn(
                name: "ReviewedBy",
                table: "AuditHistories",
                newName: "ReviewerId");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "AuditHistories",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "AuditHistories",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "AuditHistories",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LastModifierId",
                table: "AuditHistories",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuditHistories_ReviewerId",
                table: "AuditHistories",
                column: "ReviewerId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditHistories_AbpUsers_ReviewerId",
                table: "AuditHistories",
                column: "ReviewerId",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditHistories_AbpUsers_ReviewerId",
                table: "AuditHistories");

            migrationBuilder.DropIndex(
                name: "IX_AuditHistories_ReviewerId",
                table: "AuditHistories");

            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "AuditHistories");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "AuditHistories");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "AuditHistories");

            migrationBuilder.DropColumn(
                name: "LastModifierId",
                table: "AuditHistories");

            migrationBuilder.RenameColumn(
                name: "ReviewerId",
                table: "AuditHistories",
                newName: "ReviewedBy");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "AuditHistories",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuditHistories_UserId",
                table: "AuditHistories",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditHistories_IdentityUser_UserId",
                table: "AuditHistories",
                column: "UserId",
                principalTable: "IdentityUser",
                principalColumn: "Id");
        }
    }
}
