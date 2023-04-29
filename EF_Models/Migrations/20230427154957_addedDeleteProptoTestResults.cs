using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EF_Models.Migrations
{
    public partial class addedDeleteProptoTestResults : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Deleted",
                table: "TestResults",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
