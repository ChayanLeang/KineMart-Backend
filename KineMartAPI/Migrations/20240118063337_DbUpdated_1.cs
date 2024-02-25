using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KineMartAPI.Migrations
{
    /// <inheritdoc />
    public partial class DbUpdated_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "product_id",
                table: "product_properties",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_product_properties_product_id",
                table: "product_properties",
                column: "product_id");

            migrationBuilder.AddForeignKey(
                name: "FK_product_properties_products_product_id",
                table: "product_properties",
                column: "product_id",
                principalTable: "products",
                principalColumn: "product_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_product_properties_products_product_id",
                table: "product_properties");

            migrationBuilder.DropIndex(
                name: "IX_product_properties_product_id",
                table: "product_properties");

            migrationBuilder.DropColumn(
                name: "product_id",
                table: "product_properties");
        }
    }
}
