using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KineMartAPI.Migrations
{
    /// <inheritdoc />
    public partial class DbUpdated_8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "user_id",
                table: "imports",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "user_id",
                table: "exports",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "refresh_tokens",
                columns: table => new
                {
                    re_token_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    token = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    jwt_id = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    isRevoked = table.Column<bool>(type: "bit", nullable: false),
                    date_add = table.Column<DateTime>(type: "datetime2", nullable: false),
                    date_expire = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_refresh_tokens", x => x.re_token_id);
                    table.ForeignKey(
                        name: "FK_refresh_tokens_AspNetUsers_user_id",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_imports_user_id",
                table: "imports",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_exports_user_id",
                table: "exports",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_refresh_tokens_user_id",
                table: "refresh_tokens",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_exports_AspNetUsers_user_id",
                table: "exports",
                column: "user_id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_imports_AspNetUsers_user_id",
                table: "imports",
                column: "user_id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_exports_AspNetUsers_user_id",
                table: "exports");

            migrationBuilder.DropForeignKey(
                name: "FK_imports_AspNetUsers_user_id",
                table: "imports");

            migrationBuilder.DropTable(
                name: "refresh_tokens");

            migrationBuilder.DropIndex(
                name: "IX_imports_user_id",
                table: "imports");

            migrationBuilder.DropIndex(
                name: "IX_exports_user_id",
                table: "exports");

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "imports");

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "exports");
        }
    }
}
