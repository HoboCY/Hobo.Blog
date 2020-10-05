using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Blog.Data.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "app_user",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(100)", nullable: false),
                    username = table.Column<string>(maxLength: 50, nullable: false),
                    normalized_username = table.Column<string>(maxLength: 50, nullable: false),
                    password = table.Column<string>(maxLength: 100, nullable: false),
                    email = table.Column<string>(maxLength: 50, nullable: false),
                    normalized_email = table.Column<string>(maxLength: 50, nullable: false),
                    mobile = table.Column<string>(maxLength: 15, nullable: false),
                    creation_time = table.Column<DateTime>(nullable: false),
                    last_modification_time = table.Column<DateTime>(nullable: true),
                    last_modifier_id = table.Column<string>(type: "varchar(100)", nullable: true),
                    last_login_time = table.Column<DateTime>(nullable: true),
                    status = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_app_user", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "category",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(100)", nullable: false),
                    category_name = table.Column<string>(maxLength: 50, nullable: false),
                    normalized_category_name = table.Column<string>(maxLength: 50, nullable: false),
                    creation_time = table.Column<DateTime>(nullable: false),
                    creator_id = table.Column<string>(type: "varchar(100)", nullable: false),
                    last_modification_time = table.Column<DateTime>(nullable: true),
                    last_modifier_id = table.Column<string>(type: "varchar(100)", nullable: true),
                    is_deleted = table.Column<bool>(nullable: false),
                    deleter_id = table.Column<string>(type: "varchar(100)", nullable: true),
                    deletion_time = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_category", x => x.id);
                    table.ForeignKey(
                        name: "FK_category_app_user_creator_id",
                        column: x => x.creator_id,
                        principalTable: "app_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "comment",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(100)", nullable: false),
                    post_id = table.Column<string>(type: "varchar(100)", nullable: false),
                    comment_content = table.Column<string>(maxLength: 250, nullable: false),
                    creator_id = table.Column<string>(type: "varchar(100)", nullable: false),
                    creation_time = table.Column<DateTime>(nullable: false),
                    is_deleted = table.Column<bool>(nullable: false),
                    deletion_time = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_comment", x => x.id);
                    table.ForeignKey(
                        name: "FK_comment_app_user_creator_id",
                        column: x => x.creator_id,
                        principalTable: "app_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "post",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(100)", nullable: false),
                    title = table.Column<string>(maxLength: 220, nullable: false),
                    content = table.Column<string>(type: "longtext", nullable: false),
                    content_abstract = table.Column<string>(type: "varchar(255)", nullable: false),
                    creator_id = table.Column<string>(type: "varchar(100)", nullable: false),
                    creation_time = table.Column<DateTime>(nullable: false),
                    last_modification_time = table.Column<DateTime>(nullable: true),
                    is_deleted = table.Column<bool>(nullable: false),
                    deletion_time = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_post", x => x.id);
                    table.ForeignKey(
                        name: "FK_post_app_user_creator_id",
                        column: x => x.creator_id,
                        principalTable: "app_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "role",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(100)", nullable: false),
                    name = table.Column<string>(nullable: false),
                    normalized_name = table.Column<string>(nullable: false),
                    creation_time = table.Column<DateTime>(nullable: false),
                    creator_id = table.Column<string>(type: "varchar(100)", nullable: false),
                    last_modification_time = table.Column<DateTime>(nullable: true),
                    last_modifier_id = table.Column<string>(type: "varchar(100)", nullable: true),
                    is_deleted = table.Column<bool>(nullable: false),
                    deleter_id = table.Column<string>(type: "varchar(100)", nullable: true),
                    deletion_time = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_role", x => x.id);
                    table.ForeignKey(
                        name: "FK_role_app_user_creator_id",
                        column: x => x.creator_id,
                        principalTable: "app_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "comment_reply",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(100)", nullable: false),
                    comment_id = table.Column<string>(type: "varchar(100)", nullable: false),
                    reply_content = table.Column<string>(maxLength: 250, nullable: false),
                    creator_id = table.Column<string>(type: "varchar(100)", nullable: false),
                    creation_time = table.Column<DateTime>(nullable: false),
                    is_deleted = table.Column<bool>(nullable: false),
                    deletion_time = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_comment_reply", x => x.id);
                    table.ForeignKey(
                        name: "FK_comment_reply_comment_comment_id",
                        column: x => x.comment_id,
                        principalTable: "comment",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_comment_reply_app_user_creator_id",
                        column: x => x.creator_id,
                        principalTable: "app_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "post_category",
                columns: table => new
                {
                    category_id = table.Column<string>(type: "varchar(100)", nullable: false),
                    post_id = table.Column<string>(type: "varchar(100)", nullable: false),
                    creation_time = table.Column<DateTime>(nullable: false),
                    creator_id = table.Column<string>(type: "varchar(100)", nullable: false),
                    is_deleted = table.Column<bool>(nullable: false),
                    deleter_id = table.Column<string>(type: "varchar(100)", nullable: true),
                    deletion_time = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_post_category", x => new { x.post_id, x.category_id });
                    table.ForeignKey(
                        name: "FK_post_category_category_category_id",
                        column: x => x.category_id,
                        principalTable: "category",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_post_category_app_user_creator_id",
                        column: x => x.creator_id,
                        principalTable: "app_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_post_category_post_post_id",
                        column: x => x.post_id,
                        principalTable: "post",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_role",
                columns: table => new
                {
                    user_id = table.Column<string>(type: "varchar(100)", nullable: false),
                    RoleId = table.Column<string>(nullable: false),
                    creation_time = table.Column<DateTime>(nullable: false),
                    creator_id = table.Column<string>(type: "varchar(100)", nullable: false),
                    is_deleted = table.Column<bool>(nullable: false),
                    deleter_id = table.Column<string>(type: "varchar(100)", nullable: true),
                    deletion_time = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_role", x => new { x.user_id, x.RoleId });
                    table.ForeignKey(
                        name: "FK_user_role_app_user_creator_id",
                        column: x => x.creator_id,
                        principalTable: "app_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_role_role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_role_app_user_user_id",
                        column: x => x.user_id,
                        principalTable: "app_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_category_creator_id",
                table: "category",
                column: "creator_id");

            migrationBuilder.CreateIndex(
                name: "IX_comment_creator_id",
                table: "comment",
                column: "creator_id");

            migrationBuilder.CreateIndex(
                name: "IX_comment_reply_comment_id",
                table: "comment_reply",
                column: "comment_id");

            migrationBuilder.CreateIndex(
                name: "IX_comment_reply_creator_id",
                table: "comment_reply",
                column: "creator_id");

            migrationBuilder.CreateIndex(
                name: "IX_post_creator_id",
                table: "post",
                column: "creator_id");

            migrationBuilder.CreateIndex(
                name: "IX_post_category_category_id",
                table: "post_category",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_post_category_creator_id",
                table: "post_category",
                column: "creator_id");

            migrationBuilder.CreateIndex(
                name: "IX_role_creator_id",
                table: "role",
                column: "creator_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_role_creator_id",
                table: "user_role",
                column: "creator_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_role_RoleId",
                table: "user_role",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "comment_reply");

            migrationBuilder.DropTable(
                name: "post_category");

            migrationBuilder.DropTable(
                name: "user_role");

            migrationBuilder.DropTable(
                name: "comment");

            migrationBuilder.DropTable(
                name: "category");

            migrationBuilder.DropTable(
                name: "post");

            migrationBuilder.DropTable(
                name: "role");

            migrationBuilder.DropTable(
                name: "app_user");
        }
    }
}
