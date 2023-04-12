using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EF_Models.Migrations
{
    public partial class addedImageDataProp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "ImageData",
                table: "Sites",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
           
        }
    }
}
