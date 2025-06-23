using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sanad.Migrations
{
    /// <inheritdoc />
    public partial class AddArabicFieldsToProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           
            // migrationBuilder.AddColumn<string>(
            //     name: "DescriptionAr",
            //     table: "Products",
            //     type: "nvarchar(max)",
            //     nullable: false,
            //     defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LongDescriptionAr",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TitleAr",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
           
            // migrationBuilder.DropColumn(
            //     name: "DescriptionAr",
            //     table: "Products");

            migrationBuilder.DropColumn(
                name: "LongDescriptionAr",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "TitleAr",
                table: "Products");
        }
    }
}
