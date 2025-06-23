using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sanad.Migrations
{
    /// <inheritdoc />
    public partial class AddMultiLangSupportToProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CategoryAr",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoryAr",
                table: "Products");
        }
    }
}
