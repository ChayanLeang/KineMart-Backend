using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KineMartAPI.Migrations
{
    /// <inheritdoc />
    public partial class DbUpdated_3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "logs",
                columns: table => new
                {
                    log_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    message_template = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    level = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    date = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    exception = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    log_properties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    log_event = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_logs", x => x.log_id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "logs");
        }
    }
}
