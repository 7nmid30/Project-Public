using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project.Migrations
{
    public partial class AddRestaurantNameColumnToUserVisitedRestaurantsTableAndUserFavoriteRestaurantsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RestaurantName",
                table: "UserVisitedRestaurants",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RestaurantName",
                table: "UserFavoriteRestaurants",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RestaurantName",
                table: "UserVisitedRestaurants");

            migrationBuilder.DropColumn(
                name: "RestaurantName",
                table: "UserFavoriteRestaurants");
        }
    }
}
