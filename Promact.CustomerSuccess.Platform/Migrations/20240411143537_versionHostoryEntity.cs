using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Promact.CustomerSuccess.Platform.Migrations
{
    /// <inheritdoc />
    public partial class versionHostoryEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VersionHistories_IdentityUser_UserId",
                table: "VersionHistories");

            migrationBuilder.DropTable(
                name: "IdentityUser");

            migrationBuilder.DropIndex(
                name: "IX_VersionHistories_UserId",
                table: "VersionHistories");

            migrationBuilder.DropColumn(
                name: "ApprovedBy",
                table: "VersionHistories");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "VersionHistories");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "VersionHistories",
                newName: "CreatedById");

            migrationBuilder.AlterColumn<float>(
                name: "Version",
                table: "VersionHistories",
                type: "real",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<Guid>(
                name: "ApprovedById",
                table: "VersionHistories",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_VersionHistories_ApprovedById",
                table: "VersionHistories",
                column: "ApprovedById");

            migrationBuilder.CreateIndex(
                name: "IX_VersionHistories_CreatedById",
                table: "VersionHistories",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_VersionHistories_AbpUsers_ApprovedById",
                table: "VersionHistories",
                column: "ApprovedById",
                principalTable: "AbpUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VersionHistories_AbpUsers_CreatedById",
                table: "VersionHistories",
                column: "CreatedById",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VersionHistories_AbpUsers_ApprovedById",
                table: "VersionHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_VersionHistories_AbpUsers_CreatedById",
                table: "VersionHistories");

            migrationBuilder.DropIndex(
                name: "IX_VersionHistories_ApprovedById",
                table: "VersionHistories");

            migrationBuilder.DropIndex(
                name: "IX_VersionHistories_CreatedById",
                table: "VersionHistories");

            migrationBuilder.DropColumn(
                name: "ApprovedById",
                table: "VersionHistories");

            migrationBuilder.RenameColumn(
                name: "CreatedById",
                table: "VersionHistories",
                newName: "CreatedBy");

            migrationBuilder.AlterColumn<int>(
                name: "Version",
                table: "VersionHistories",
                type: "integer",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AddColumn<string>(
                name: "ApprovedBy",
                table: "VersionHistories",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "VersionHistories",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "IdentityUser",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "text", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "text", nullable: true),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    UserName = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityUser", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VersionHistories_UserId",
                table: "VersionHistories",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_VersionHistories_IdentityUser_UserId",
                table: "VersionHistories",
                column: "UserId",
                principalTable: "IdentityUser",
                principalColumn: "Id");
        }
    }
}
