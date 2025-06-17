using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IssueTracker.Migrations
{
    /// <inheritdoc />
    public partial class Sixth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InnerException",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "Message",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "Source",
                table: "Logs");

            migrationBuilder.RenameColumn(
                name: "StackTrace",
                table: "Logs",
                newName: "JsonLog");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "JsonLog",
                table: "Logs",
                newName: "StackTrace");

            migrationBuilder.AddColumn<string>(
                name: "InnerException",
                table: "Logs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "Logs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Source",
                table: "Logs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
