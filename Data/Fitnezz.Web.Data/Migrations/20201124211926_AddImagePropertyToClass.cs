using Microsoft.EntityFrameworkCore.Migrations;

namespace Fitnezz.Web.Data.Migrations
{
    public partial class AddImagePropertyToClass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Classes",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Classes");
        }
    }
}
