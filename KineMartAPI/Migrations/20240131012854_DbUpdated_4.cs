using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KineMartAPI.Migrations
{
    /// <inheritdoc />
    public partial class DbUpdated_4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "log_properties",
                table: "logs",
                newName: "properties");

            migrationBuilder.AlterColumn<string>(
                name: "level",
                table: "logs",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<DateTime>(
                name: "date",
                table: "logs",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "properties",
                table: "logs",
                newName: "log_properties");

            migrationBuilder.AlterColumn<string>(
                name: "level",
                table: "logs",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "date",
                table: "logs",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }
    }
}
