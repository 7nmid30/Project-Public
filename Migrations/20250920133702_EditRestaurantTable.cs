using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project.Migrations
{
    public partial class EditRestaurantTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "SnsUrl1",
                table: "Restaurants");

            migrationBuilder.RenameColumn(
                name: "WebUrl",
                table: "Restaurants",
                newName: "Url");

            migrationBuilder.RenameColumn(
                name: "SnsUrl3",
                table: "Restaurants",
                newName: "PhoneNumber");

            migrationBuilder.RenameColumn(
                name: "SnsUrl2",
                table: "Restaurants",
                newName: "CurrencyCode");

            migrationBuilder.RenameColumn(
                name: "Genre",
                table: "Restaurants",
                newName: "Category");

            migrationBuilder.AddColumn<int>(
                name: "Apple",
                table: "Restaurants",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EndPrice",
                table: "Restaurants",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StartPrice",
                table: "Restaurants",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Apple",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "EndPrice",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "StartPrice",
                table: "Restaurants");

            migrationBuilder.RenameColumn(
                name: "Url",
                table: "Restaurants",
                newName: "WebUrl");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "Restaurants",
                newName: "SnsUrl3");

            migrationBuilder.RenameColumn(
                name: "CurrencyCode",
                table: "Restaurants",
                newName: "SnsUrl2");

            migrationBuilder.RenameColumn(
                name: "Category",
                table: "Restaurants",
                newName: "Genre");

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Restaurants",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SnsUrl1",
                table: "Restaurants",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
