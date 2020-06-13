using Microsoft.EntityFrameworkCore.Migrations;

namespace O2.Auth.Web.Migrations
{
    public partial class RemovedSampleColumnToO2User : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sample",
                schema: "dbo",
                table: "AspNetUsers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Sample",
                schema: "dbo",
                table: "AspNetUsers",
                nullable: true);
        }
    }
}
