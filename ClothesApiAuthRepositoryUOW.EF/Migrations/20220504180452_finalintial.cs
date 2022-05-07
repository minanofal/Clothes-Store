using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClothesApiAuthRepositoryUOW.EF.Migrations
{
    public partial class finalintial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
               name: "Users",
               columns: table => new
               {
                   Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                   FirstName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                   LastName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                   UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                   NormalizedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                   Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                   NormalizedEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                   EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                   PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                   SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                   ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                   PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                   PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                   TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                   LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                   LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                   AccessFailedCount = table.Column<int>(type: "int", nullable: false)
               },
               constraints: table =>
               {
                   table.PrimaryKey("PK_Users", x => x.Id);
               });
            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaims", x => x.Id);
                    table.ForeignKey(
                      name: "FK_RoleClaims_Roles_RoleId",
                      column: x => x.RoleId,
                      principalTable: "Roles",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Restrict);
               
                });

            

            migrationBuilder.CreateTable(
                name: "UserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                    table.ForeignKey(
                       name: "FK_UserClaims_Users_UserId",
                       column: x => x.UserId,
                       principalTable: "Users",
                       principalColumn: "Id",
                       onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => new { x.ProviderKey, x.LoginProvider });
                    table.ForeignKey(
                        name: "FK_UserLogins_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.RoleId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

           

            migrationBuilder.CreateTable(
                name: "UserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => new { x.LoginProvider, x.Name, x.UserId });
                    table.ForeignKey(
                      name: "FK_AUserTokens_Users_UserId",
                      column: x => x.UserId,
                      principalTable: "Users",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Restrict);
               
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "2c0a7dac-8f82-475f-a448-2f2034f1a879", "042b076e-edcc-4828-a978-42e094be0360", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "5d92a67c-3ec6-4d17-8719-87491dd5af92", "270ddab9-47a5-45a9-9d6c-bf97f43414a2", "User", "USER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoleClaims");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "UserClaims");

            migrationBuilder.DropTable(
                name: "UserLogins");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "UserTokens");
        }
    }
}
