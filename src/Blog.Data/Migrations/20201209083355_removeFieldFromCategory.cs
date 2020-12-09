using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Blog.Data.Migrations
{
    public partial class removeFieldFromCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NormalizedCategoryName",
                table: "category");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NormalizedCategoryName",
                table: "category",
                type: "varchar(50) CHARACTER SET utf8mb4",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }
    }
}
