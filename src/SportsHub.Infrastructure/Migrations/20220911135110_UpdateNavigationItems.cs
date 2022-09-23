using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportsHub.Infrastructure.Migrations
{
    public partial class UpdateNavigationItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence<int>(
                name: "CategoryOrderIndexes");

            migrationBuilder.CreateSequence<int>(
                name: "SubcategoryOrderIndexes");

            migrationBuilder.CreateSequence<int>(
                name: "TeamOrderIndexes");

            migrationBuilder.AddColumn<bool>(
                name: "IsHidden",
                table: "Teams",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "OrderIndex",
                table: "Teams",
                type: "int",
                nullable: false,
                defaultValueSql: "NEXT VALUE FOR TeamOrderIndexes");

            migrationBuilder.AddColumn<bool>(
                name: "IsHidden",
                table: "Subcategories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "OrderIndex",
                table: "Subcategories",
                type: "int",
                nullable: false,
                defaultValueSql: "NEXT VALUE FOR SubcategoryOrderIndexes");

            migrationBuilder.AddColumn<bool>(
                name: "IsHidden",
                table: "Categories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "OrderIndex",
                table: "Categories",
                type: "int",
                nullable: false,
                defaultValueSql: "NEXT VALUE FOR CategoryOrderIndexes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropSequence(
                name: "CategoryOrderIndexes");

            migrationBuilder.DropSequence(
                name: "SubcategoryOrderIndexes");

            migrationBuilder.DropSequence(
                name: "TeamOrderIndexes");

            migrationBuilder.DropColumn(
                name: "IsHidden",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "OrderIndex",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "IsHidden",
                table: "Subcategories");

            migrationBuilder.DropColumn(
                name: "OrderIndex",
                table: "Subcategories");

            migrationBuilder.DropColumn(
                name: "IsHidden",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "OrderIndex",
                table: "Categories");
        }
    }
}
