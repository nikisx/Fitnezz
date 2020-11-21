using Microsoft.EntityFrameworkCore.Migrations;

namespace Fitnezz.Web.Data.Migrations
{
    public partial class AddPublicProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "Workouts",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "MealPlans",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "Workouts");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "MealPlans");
        }
    }
}
