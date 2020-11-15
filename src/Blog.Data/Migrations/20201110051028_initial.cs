using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Blog.Data.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "application_user",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(50)", nullable: false),
                    UserName = table.Column<string>(maxLength: 50, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 50, nullable: true),
                    Email = table.Column<string>(maxLength: 50, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 50, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(maxLength: 100, nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    NickName = table.Column<string>(maxLength: 50, nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastLoginTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletionTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_application_user", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "application_role",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(50)", nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<string>(type: "varchar(50)", nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<string>(type: "varchar(50)", nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterId = table.Column<string>(type: "varchar(50)", nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_application_role", x => x.Id);
                    table.ForeignKey(
                        name: "FK_application_role_application_user_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "application_user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "application_user_claim",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(type: "varchar(50)", nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_application_user_claim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_application_user_claim_application_user_UserId",
                        column: x => x.UserId,
                        principalTable: "application_user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "application_user_login",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(50)", nullable: false),
                    LoginProvider = table.Column<string>(nullable: true),
                    ProviderKey = table.Column<string>(nullable: true),
                    ProviderDisplayName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_application_user_login", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_application_user_login_application_user_UserId",
                        column: x => x.UserId,
                        principalTable: "application_user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "application_user_token",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(50)", nullable: false),
                    LoginProvider = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_application_user_token", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_application_user_token_application_user_UserId",
                        column: x => x.UserId,
                        principalTable: "application_user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "category",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(50)", nullable: false),
                    CategoryName = table.Column<string>(maxLength: 50, nullable: false),
                    NormalizedCategoryName = table.Column<string>(maxLength: 50, nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<string>(type: "varchar(50)", nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<string>(type: "varchar(50)", nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterId = table.Column<string>(type: "varchar(50)", nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_category", x => x.Id);
                    table.ForeignKey(
                        name: "FK_category_application_user_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "application_user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "comment",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(50)", nullable: false),
                    PostId = table.Column<string>(type: "varchar(50)", nullable: false),
                    CommentContent = table.Column<string>(maxLength: 250, nullable: false),
                    CreatorId = table.Column<string>(type: "varchar(50)", nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterId = table.Column<string>(type: "varchar(50)", nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_comment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_comment_application_user_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "application_user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "post",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(50)", nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Content = table.Column<string>(type: "longtext", nullable: false),
                    ContentAbstract = table.Column<string>(maxLength: 255, nullable: false),
                    CreatorId = table.Column<string>(type: "varchar(50)", nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterId = table.Column<string>(type: "varchar(50)", nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_post", x => x.Id);
                    table.ForeignKey(
                        name: "FK_post_application_user_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "application_user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "application_role_claim",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<string>(type: "varchar(50)", nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_application_role_claim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_application_role_claim_application_role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "application_role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "application_user_role",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(50)", nullable: false),
                    RoleId = table.Column<string>(type: "varchar(50)", nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<string>(type: "varchar(50)", nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterId = table.Column<string>(type: "varchar(50)", nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_application_user_role", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_application_user_role_application_user_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "application_user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_application_user_role_application_role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "application_role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_application_user_role_application_user_UserId",
                        column: x => x.UserId,
                        principalTable: "application_user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "comment_reply",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(50)", nullable: false),
                    CommentId = table.Column<string>(type: "varchar(50)", nullable: false),
                    ReplyContent = table.Column<string>(maxLength: 250, nullable: false),
                    CreatorId = table.Column<string>(type: "varchar(50)", nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterId = table.Column<string>(type: "varchar(50)", nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_comment_reply", x => x.Id);
                    table.ForeignKey(
                        name: "FK_comment_reply_comment_CommentId",
                        column: x => x.CommentId,
                        principalTable: "comment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_comment_reply_application_user_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "application_user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "post_category",
                columns: table => new
                {
                    CategoryId = table.Column<string>(type: "varchar(50)", nullable: false),
                    PostId = table.Column<string>(type: "varchar(50)", nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<string>(type: "varchar(50)", nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterId = table.Column<string>(type: "varchar(50)", nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_post_category", x => new { x.PostId, x.CategoryId });
                    table.ForeignKey(
                        name: "FK_post_category_category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_post_category_application_user_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "application_user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_post_category_post_PostId",
                        column: x => x.PostId,
                        principalTable: "post",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_application_role_CreatorId",
                table: "application_role",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "application_role",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_application_role_claim_RoleId",
                table: "application_role_claim",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "application_user",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "application_user",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_application_user_claim_UserId",
                table: "application_user_claim",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_application_user_role_CreatorId",
                table: "application_user_role",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_application_user_role_RoleId",
                table: "application_user_role",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_category_CreatorId",
                table: "category",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_comment_CreatorId",
                table: "comment",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_comment_reply_CommentId",
                table: "comment_reply",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_comment_reply_CreatorId",
                table: "comment_reply",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_post_CreatorId",
                table: "post",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_post_category_CategoryId",
                table: "post_category",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_post_category_CreatorId",
                table: "post_category",
                column: "CreatorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "application_role_claim");

            migrationBuilder.DropTable(
                name: "application_user_claim");

            migrationBuilder.DropTable(
                name: "application_user_login");

            migrationBuilder.DropTable(
                name: "application_user_role");

            migrationBuilder.DropTable(
                name: "application_user_token");

            migrationBuilder.DropTable(
                name: "comment_reply");

            migrationBuilder.DropTable(
                name: "post_category");

            migrationBuilder.DropTable(
                name: "application_role");

            migrationBuilder.DropTable(
                name: "comment");

            migrationBuilder.DropTable(
                name: "category");

            migrationBuilder.DropTable(
                name: "post");

            migrationBuilder.DropTable(
                name: "application_user");
        }
    }
}
