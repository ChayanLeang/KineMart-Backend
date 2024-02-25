using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KineMartAPI.Migrations
{
    /// <inheritdoc />
    public partial class DbUpdated_5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_logs",
                table: "logs");

            migrationBuilder.RenameTable(
                name: "logs",
                newName: "Logs");

            migrationBuilder.RenameColumn(
                name: "properties",
                table: "Logs",
                newName: "Properties");

            migrationBuilder.RenameColumn(
                name: "message",
                table: "Logs",
                newName: "Message");

            migrationBuilder.RenameColumn(
                name: "level",
                table: "Logs",
                newName: "Level");

            migrationBuilder.RenameColumn(
                name: "exception",
                table: "Logs",
                newName: "Exception");

            migrationBuilder.RenameColumn(
                name: "message_template",
                table: "Logs",
                newName: "MessageTemplate");

            migrationBuilder.RenameColumn(
                name: "log_event",
                table: "Logs",
                newName: "LogEvent");

            migrationBuilder.RenameColumn(
                name: "date",
                table: "Logs",
                newName: "TimeStamp");

            migrationBuilder.RenameColumn(
                name: "log_id",
                table: "Logs",
                newName: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Logs",
                table: "Logs",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Logs",
                table: "Logs");

            migrationBuilder.RenameTable(
                name: "Logs",
                newName: "logs");

            migrationBuilder.RenameColumn(
                name: "Properties",
                table: "logs",
                newName: "properties");

            migrationBuilder.RenameColumn(
                name: "Message",
                table: "logs",
                newName: "message");

            migrationBuilder.RenameColumn(
                name: "Level",
                table: "logs",
                newName: "level");

            migrationBuilder.RenameColumn(
                name: "Exception",
                table: "logs",
                newName: "exception");

            migrationBuilder.RenameColumn(
                name: "TimeStamp",
                table: "logs",
                newName: "date");

            migrationBuilder.RenameColumn(
                name: "MessageTemplate",
                table: "logs",
                newName: "message_template");

            migrationBuilder.RenameColumn(
                name: "LogEvent",
                table: "logs",
                newName: "log_event");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "logs",
                newName: "log_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_logs",
                table: "logs",
                column: "log_id");
        }
    }
}
