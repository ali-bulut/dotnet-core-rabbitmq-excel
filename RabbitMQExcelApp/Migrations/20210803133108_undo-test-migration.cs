using Microsoft.EntityFrameworkCore.Migrations;

namespace RabbitMQExcelApp.Migrations
{
    public partial class undotestmigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TestField",
                table: "UserFiles");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TestField",
                table: "UserFiles",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
