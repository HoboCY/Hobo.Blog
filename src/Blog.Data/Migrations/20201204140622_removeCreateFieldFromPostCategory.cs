using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Blog.Data.Migrations
{
    public partial class removeCreateFieldFromPostCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_post_category_application_user_CreatorId",
                table: "post_category");

            migrationBuilder.DropIndex(
                name: "IX_post_category_CreatorId",
                table: "post_category");

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

            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "post_category");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "post_category");

            migrationBuilder.DropColumn(
                name: "DeleterId",
                table: "post_category");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "post_category");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "post_category");

            migrationBuilder.InsertData(
                table: "application_role",
                columns: new[] { "Id", "ConcurrencyStamp", "CreationTime", "CreatorId", "DeleterId", "DeletionTime", "IsDeleted", "LastModificationTime", "LastModifierId", "Name", "NormalizedName" },
                values: new object[] { "622d9389-e7f7-4421-bf00-4082e2be9c4e", "27dbafbd-e4b2-458e-85b5-a53451c2c2d5", new DateTime(2020, 12, 4, 14, 6, 22, 305, DateTimeKind.Utc).AddTicks(2456), null, null, null, false, null, null, "administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "category",
                columns: new[] { "Id", "CategoryName", "CreationTime", "CreatorId", "DeleterId", "DeletionTime", "IsDeleted", "LastModificationTime", "LastModifierId", "NormalizedCategoryName" },
                values: new object[,]
                {
                    { "0027247b-86e2-438e-833a-25a4ce9fce3c", "asp .net core", new DateTime(2020, 12, 4, 14, 6, 22, 312, DateTimeKind.Utc).AddTicks(1344), null, null, null, false, null, null, "ASP .NET CORE" },
                    { "b3b14138-4d6b-49a3-a8ed-ffac02fa21b8", "c#", new DateTime(2020, 12, 4, 14, 6, 22, 312, DateTimeKind.Utc).AddTicks(1799), null, null, null, false, null, null, "C#" },
                    { "f6c7a6ad-580f-4441-9a82-190b24ee062a", "asp .net core mvc", new DateTime(2020, 12, 4, 14, 6, 22, 312, DateTimeKind.Utc).AddTicks(1809), null, null, null, false, null, null, "ASP .NET CORE MVC" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "application_role",
                keyColumn: "Id",
                keyValue: "622d9389-e7f7-4421-bf00-4082e2be9c4e");

            migrationBuilder.DeleteData(
                table: "category",
                keyColumn: "Id",
                keyValue: "0027247b-86e2-438e-833a-25a4ce9fce3c");

            migrationBuilder.DeleteData(
                table: "category",
                keyColumn: "Id",
                keyValue: "b3b14138-4d6b-49a3-a8ed-ffac02fa21b8");

            migrationBuilder.DeleteData(
                table: "category",
                keyColumn: "Id",
                keyValue: "f6c7a6ad-580f-4441-9a82-190b24ee062a");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "post_category",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatorId",
                table: "post_category",
                type: "varchar(50)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeleterId",
                table: "post_category",
                type: "varchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "post_category",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "post_category",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

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

            migrationBuilder.CreateIndex(
                name: "IX_post_category_CreatorId",
                table: "post_category",
                column: "CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_post_category_application_user_CreatorId",
                table: "post_category",
                column: "CreatorId",
                principalTable: "application_user",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
