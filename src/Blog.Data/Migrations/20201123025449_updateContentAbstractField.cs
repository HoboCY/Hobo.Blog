using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Blog.Data.Migrations
{
    public partial class updateContentAbstractField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "application_role",
                keyColumn: "Id",
                keyValue: "ab7bdc62-1b73-408c-b1a2-428508e1bdd3");

            migrationBuilder.DeleteData(
                table: "category",
                keyColumn: "Id",
                keyValue: "4ea6f8e8-112f-4c04-9fcd-f800fefefed7");

            migrationBuilder.DeleteData(
                table: "category",
                keyColumn: "Id",
                keyValue: "707ce769-8a87-4201-abd7-a92b7662980e");

            migrationBuilder.DeleteData(
                table: "category",
                keyColumn: "Id",
                keyValue: "9f79b4c0-3c7b-4bb5-b21d-498b83911eb3");

            migrationBuilder.AlterColumn<string>(
                name: "ContentAbstract",
                table: "post",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255) CHARACTER SET utf8mb4",
                oldMaxLength: 255);

            migrationBuilder.InsertData(
                table: "application_role",
                columns: new[] { "Id", "ConcurrencyStamp", "CreationTime", "CreatorId", "DeleterId", "DeletionTime", "IsDeleted", "LastModificationTime", "LastModifierId", "Name", "NormalizedName" },
                values: new object[] { "42a0a25a-550d-4e30-a2ff-ac4db3db3c86", "fdd4e14c-0b0e-464e-8f2a-39ca73155aba", new DateTime(2020, 11, 23, 2, 54, 49, 76, DateTimeKind.Utc).AddTicks(2167), null, null, null, false, null, null, "administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "category",
                columns: new[] { "Id", "CategoryName", "CreationTime", "CreatorId", "DeleterId", "DeletionTime", "IsDeleted", "LastModificationTime", "LastModifierId", "NormalizedCategoryName" },
                values: new object[,]
                {
                    { "3e9fec2a-8a42-4ef2-bef5-472b36ea5d45", "asp .net core", new DateTime(2020, 11, 23, 2, 54, 49, 83, DateTimeKind.Utc).AddTicks(1350), null, null, null, false, null, null, "ASP .NET CORE" },
                    { "153b6bbd-7cea-4ee4-92be-bdb18774bc10", "c#", new DateTime(2020, 11, 23, 2, 54, 49, 83, DateTimeKind.Utc).AddTicks(1810), null, null, null, false, null, null, "C#" },
                    { "f1a348f2-cf22-4334-89d1-eca34af47735", "asp .net core mvc", new DateTime(2020, 11, 23, 2, 54, 49, 83, DateTimeKind.Utc).AddTicks(1820), null, null, null, false, null, null, "ASP .NET CORE MVC" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "application_role",
                keyColumn: "Id",
                keyValue: "42a0a25a-550d-4e30-a2ff-ac4db3db3c86");

            migrationBuilder.DeleteData(
                table: "category",
                keyColumn: "Id",
                keyValue: "153b6bbd-7cea-4ee4-92be-bdb18774bc10");

            migrationBuilder.DeleteData(
                table: "category",
                keyColumn: "Id",
                keyValue: "3e9fec2a-8a42-4ef2-bef5-472b36ea5d45");

            migrationBuilder.DeleteData(
                table: "category",
                keyColumn: "Id",
                keyValue: "f1a348f2-cf22-4334-89d1-eca34af47735");

            migrationBuilder.AlterColumn<string>(
                name: "ContentAbstract",
                table: "post",
                type: "varchar(255) CHARACTER SET utf8mb4",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.InsertData(
                table: "application_role",
                columns: new[] { "Id", "ConcurrencyStamp", "CreationTime", "CreatorId", "DeleterId", "DeletionTime", "IsDeleted", "LastModificationTime", "LastModifierId", "Name", "NormalizedName" },
                values: new object[] { "ab7bdc62-1b73-408c-b1a2-428508e1bdd3", "5eb526bc-8b9b-4641-b344-8494f06f35ad", new DateTime(2020, 11, 10, 13, 10, 27, 951, DateTimeKind.Local).AddTicks(500), null, null, null, false, null, null, "administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "category",
                columns: new[] { "Id", "CategoryName", "CreationTime", "CreatorId", "DeleterId", "DeletionTime", "IsDeleted", "LastModificationTime", "LastModifierId", "NormalizedCategoryName" },
                values: new object[,]
                {
                    { "4ea6f8e8-112f-4c04-9fcd-f800fefefed7", "asp .net core", new DateTime(2020, 11, 10, 13, 10, 27, 958, DateTimeKind.Local).AddTicks(4962), null, null, null, false, null, null, "ASP .NET CORE" },
                    { "707ce769-8a87-4201-abd7-a92b7662980e", "c#", new DateTime(2020, 11, 10, 13, 10, 27, 958, DateTimeKind.Local).AddTicks(5469), null, null, null, false, null, null, "C#" },
                    { "9f79b4c0-3c7b-4bb5-b21d-498b83911eb3", "asp .net core mvc", new DateTime(2020, 11, 10, 13, 10, 27, 958, DateTimeKind.Local).AddTicks(5482), null, null, null, false, null, null, "ASP .NET CORE MVC" }
                });
        }
    }
}
