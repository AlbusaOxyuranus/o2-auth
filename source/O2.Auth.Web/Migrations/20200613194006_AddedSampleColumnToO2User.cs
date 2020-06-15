using Microsoft.EntityFrameworkCore.Migrations;

namespace O2.Auth.Web.Migrations
{
    public partial class AddedSampleColumnToO2User : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Sample",
                schema: "dbo",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sample",
                schema: "dbo",
                table: "AspNetUsers");
        }
    }
}
