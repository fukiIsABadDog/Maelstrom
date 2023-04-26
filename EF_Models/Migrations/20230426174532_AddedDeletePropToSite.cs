using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EF_Models.Migrations
{
    public partial class AddedDeletePropToSite : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Deleted",
                table: "Sites",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
