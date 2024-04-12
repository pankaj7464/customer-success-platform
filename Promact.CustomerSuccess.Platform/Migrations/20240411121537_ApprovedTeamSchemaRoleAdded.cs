using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Promact.CustomerSuccess.Platform.Migrations
{
    /// <inheritdoc />
    public partial class ApprovedTeamSchemaRoleAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "ApprovedTeams");

            migrationBuilder.RenameColumn(
                name: "NoOfResouces",
                table: "ApprovedTeams",
                newName: "NoOfResources");

            migrationBuilder.AddColumn<Guid>(
                name: "DeleterId",
                table: "ApprovedTeams",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "ApprovedTeams",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ApprovedTeams",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "RoleId",
                table: "ApprovedTeams",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ApprovedTeams_RoleId",
                table: "ApprovedTeams",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApprovedTeams_AbpRoles_RoleId",
                table: "ApprovedTeams",
                column: "RoleId",
                principalTable: "AbpRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApprovedTeams_AbpRoles_RoleId",
                table: "ApprovedTeams");

            migrationBuilder.DropIndex(
                name: "IX_ApprovedTeams_RoleId",
                table: "ApprovedTeams");

            migrationBuilder.DropColumn(
                name: "DeleterId",
                table: "ApprovedTeams");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "ApprovedTeams");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ApprovedTeams");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "ApprovedTeams");

            migrationBuilder.RenameColumn(
                name: "NoOfResources",
                table: "ApprovedTeams",
                newName: "NoOfResouces");

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "ApprovedTeams",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
