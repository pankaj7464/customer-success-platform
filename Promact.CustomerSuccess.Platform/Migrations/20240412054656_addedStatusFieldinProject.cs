using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Promact.CustomerSuccess.Platform.Migrations
{
    /// <inheritdoc />
    public partial class addedStatusFieldinProject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Stakeholders");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Stakeholders");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Stakeholders");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "ProjectResources");

            migrationBuilder.RenameColumn(
                name: "Start",
                table: "ProjectResources",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "End",
                table: "ProjectResources",
                newName: "EndDate");

            migrationBuilder.AddColumn<Guid>(
                name: "RoleId",
                table: "Stakeholders",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Stakeholders",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "status",
                table: "Projects",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "RoleId",
                table: "ProjectResources",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Stakeholders_RoleId",
                table: "Stakeholders",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Stakeholders_UserId",
                table: "Stakeholders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectResources_RoleId",
                table: "ProjectResources",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectResources_AbpRoles_RoleId",
                table: "ProjectResources",
                column: "RoleId",
                principalTable: "AbpRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Stakeholders_AbpRoles_RoleId",
                table: "Stakeholders",
                column: "RoleId",
                principalTable: "AbpRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Stakeholders_AbpUsers_UserId",
                table: "Stakeholders",
                column: "UserId",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectResources_AbpRoles_RoleId",
                table: "ProjectResources");

            migrationBuilder.DropForeignKey(
                name: "FK_Stakeholders_AbpRoles_RoleId",
                table: "Stakeholders");

            migrationBuilder.DropForeignKey(
                name: "FK_Stakeholders_AbpUsers_UserId",
                table: "Stakeholders");

            migrationBuilder.DropIndex(
                name: "IX_Stakeholders_RoleId",
                table: "Stakeholders");

            migrationBuilder.DropIndex(
                name: "IX_Stakeholders_UserId",
                table: "Stakeholders");

            migrationBuilder.DropIndex(
                name: "IX_ProjectResources_RoleId",
                table: "ProjectResources");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "Stakeholders");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Stakeholders");

            migrationBuilder.DropColumn(
                name: "status",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "ProjectResources");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "ProjectResources",
                newName: "Start");

            migrationBuilder.RenameColumn(
                name: "EndDate",
                table: "ProjectResources",
                newName: "End");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Stakeholders",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Stakeholders",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Stakeholders",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "ProjectResources",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
