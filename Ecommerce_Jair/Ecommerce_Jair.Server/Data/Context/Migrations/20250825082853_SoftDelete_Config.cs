using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce_Jair.Server.Migrations
{
    /// <inheritdoc />
    public partial class SoftDelete_Config : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ProductReviews",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Categories",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_IsActive_Category",
                table: "Products",
                columns: new[] { "IsActive", "CategoryID" });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_IsActive_Parent",
                table: "Categories",
                columns: new[] { "IsActive", "ParentCategoryID" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Products_IsActive_Category",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Categories_IsActive_Parent",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ProductReviews");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Categories");
        }
    }
}
