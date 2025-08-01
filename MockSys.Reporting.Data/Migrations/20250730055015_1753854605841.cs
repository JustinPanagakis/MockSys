using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MockSys.Reporting.Data.Migrations
{
    /// <inheritdoc />
    public partial class _1753854605841 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ErrorMessage",
                table: "SyncLogs",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "SyncLogs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SyncedRecordsCount",
                table: "SyncLogs",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ErrorMessage",
                table: "SyncLogs");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "SyncLogs");

            migrationBuilder.DropColumn(
                name: "SyncedRecordsCount",
                table: "SyncLogs");
        }
    }
}
