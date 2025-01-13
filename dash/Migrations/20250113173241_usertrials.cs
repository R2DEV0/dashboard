using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dash.Migrations
{
    /// <inheritdoc />
    public partial class usertrials : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "TrialCompleted",
                table: "UserAccounts",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "TrialDaysLength",
                table: "UserAccounts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "TrialStart",
                table: "UserAccounts",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TrialCompleted",
                table: "UserAccounts");

            migrationBuilder.DropColumn(
                name: "TrialDaysLength",
                table: "UserAccounts");

            migrationBuilder.DropColumn(
                name: "TrialStart",
                table: "UserAccounts");
        }
    }
}
