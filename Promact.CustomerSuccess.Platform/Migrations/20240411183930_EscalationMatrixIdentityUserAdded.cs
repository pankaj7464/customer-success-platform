using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Promact.CustomerSuccess.Platform.Migrations
{
    /// <inheritdoc />
    public partial class EscalationMatrixIdentityUserAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResponsiblePerson",
                table: "EscalationMatrices");

            migrationBuilder.AddColumn<Guid>(
                name: "ResponsiblePersonId",
                table: "EscalationMatrices",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_EscalationMatrices_ResponsiblePersonId",
                table: "EscalationMatrices",
                column: "ResponsiblePersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_EscalationMatrices_AbpUsers_ResponsiblePersonId",
                table: "EscalationMatrices",
                column: "ResponsiblePersonId",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EscalationMatrices_AbpUsers_ResponsiblePersonId",
                table: "EscalationMatrices");

            migrationBuilder.DropIndex(
                name: "IX_EscalationMatrices_ResponsiblePersonId",
                table: "EscalationMatrices");

            migrationBuilder.DropColumn(
                name: "ResponsiblePersonId",
                table: "EscalationMatrices");

            migrationBuilder.AddColumn<string>(
                name: "ResponsiblePerson",
                table: "EscalationMatrices",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
