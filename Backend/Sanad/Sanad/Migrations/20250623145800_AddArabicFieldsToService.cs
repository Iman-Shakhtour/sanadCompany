using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sanad.Migrations
{
    /// <inheritdoc />
    public partial class AddArabicFieldsToService : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.AddColumn<string>(
            //     name: "DescriptionAr",
            //     table: "Services",
            //     type: "nvarchar(max)",
            //     nullable: false,
            //     defaultValue: "");

            // migrationBuilder.AddColumn<string>(
            //     name: "DetailsAr",
            //     table: "Services",
            //     type: "nvarchar(max)",
            //     nullable: true);

        //     migrationBuilder.AddColumn<string>(
        //         name: "TitleAr",
        //         table: "Services",
        //         type: "nvarchar(max)",
        //         nullable: false,
        //         defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.DropColumn(
            //     name: "DescriptionAr",
            //     table: "Services");

            // migrationBuilder.DropColumn(
            //     name: "DetailsAr",
            //     table: "Services");

            // migrationBuilder.DropColumn(
            //     name: "TitleAr",
            //     table: "Services");
        }
    }
}
