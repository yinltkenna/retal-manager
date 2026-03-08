using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace IdentityService.src.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixRolePermissionRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmailVerificationTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false),
                    UsedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailVerificationTokens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Group = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UrlIcon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Permissions_Permissions_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RevokedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsRevoked = table.Column<bool>(type: "bit", nullable: false),
                    ReplacedByToken = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Username = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IsEmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    LastPasswordChangedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ResetPasswordToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResetPasswordExpiry = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: true),
                    LockOutEnd = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RolePermissions",
                columns: table => new
                {
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PermissionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissions", x => new { x.RoleId, x.PermissionId });
                    table.ForeignKey(
                        name: "FK_RolePermissions_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermissions_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "Code", "Description", "Group", "IsActive", "Name", "ParentId", "SortOrder", "Type", "UrlIcon" },
                values: new object[,]
                {
                    { new Guid("1b5aeb2f-e5b6-41c4-9c6a-3a8a9acb0d3f"), "user.view_logs", null, "User", true, "View User Logs", null, 6, "API", null },
                    { new Guid("3f9c82c7-aa6e-4b34-9e35-971f2be1a378"), "user.update", null, "User", true, "Update User", null, 3, "API", null },
                    { new Guid("7c5d8dc0-7169-4b2e-8d44-9a7bb9fcc0e8"), "user.delete", null, "User", true, "Delete User", null, 4, "API", null },
                    { new Guid("a8e96f8f-6ae8-4ffb-bb90-5a90786951d1"), "user.view", null, "User", true, "View User", null, 1, "API", null },
                    { new Guid("c9a3c1a0-2d0a-4a88-bfa2-72ee4178fd3f"), "user.create", null, "User", true, "Create User", null, 2, "API", null },
                    { new Guid("e8c1e1b3-50ac-4361-a2f1-3736ca3d6a92"), "user.lock", null, "User", true, "Lock User", null, 5, "API", null }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Description", "IsActive", "IsDeleted", "Name" },
                values: new object[,]
                {
                    { new Guid("5a1c2e3f-4b6d-4c8e-9f1a-2b3c4d5e6f7a"), "Admin role with full permissions", true, false, "Admin" },
                    { new Guid("6f7a8b9c-1d2e-3f4a-5b6c-7d8e9f0a1b2c"), "User role with limited permissions", true, false, "User" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "CreatedAt", "Email", "IsActive", "IsDeleted", "IsEmailConfirmed", "LastPasswordChangedAt", "LastUpdatedAt", "LockOutEnd", "PasswordHash", "PhoneNumber", "ResetPasswordExpiry", "ResetPasswordToken", "Status", "TenantId", "Username" },
                values: new object[,]
                {
                    { new Guid("1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c6d"), null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "admin@example.com", false, false, true, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "$2a$11$AEB8w8vo4lFGC971/BTZeu1s3eBRZ49bLuz7ZIRB.oMDlvsAHqGwe", "1234567890", null, null, "Active", null, "adminApp" },
                    { new Guid("6f7a8b9c-1d2e-3f4a-5b6c-7d8e9f0a1b2c"), null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "userApp@gmail.com", false, false, true, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "$2a$11$AEB8w8vo4lFGC971/BTZeu1s3eBRZ49bLuz7ZIRB.oMDlvsAHqGwe", "1234567880", null, null, "Active", null, "userApp" }
                });

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "PermissionId", "RoleId" },
                values: new object[,]
                {
                    { new Guid("1b5aeb2f-e5b6-41c4-9c6a-3a8a9acb0d3f"), new Guid("5a1c2e3f-4b6d-4c8e-9f1a-2b3c4d5e6f7a") },
                    { new Guid("3f9c82c7-aa6e-4b34-9e35-971f2be1a378"), new Guid("5a1c2e3f-4b6d-4c8e-9f1a-2b3c4d5e6f7a") },
                    { new Guid("7c5d8dc0-7169-4b2e-8d44-9a7bb9fcc0e8"), new Guid("5a1c2e3f-4b6d-4c8e-9f1a-2b3c4d5e6f7a") },
                    { new Guid("a8e96f8f-6ae8-4ffb-bb90-5a90786951d1"), new Guid("5a1c2e3f-4b6d-4c8e-9f1a-2b3c4d5e6f7a") },
                    { new Guid("c9a3c1a0-2d0a-4a88-bfa2-72ee4178fd3f"), new Guid("5a1c2e3f-4b6d-4c8e-9f1a-2b3c4d5e6f7a") },
                    { new Guid("e8c1e1b3-50ac-4361-a2f1-3736ca3d6a92"), new Guid("5a1c2e3f-4b6d-4c8e-9f1a-2b3c4d5e6f7a") },
                    { new Guid("1b5aeb2f-e5b6-41c4-9c6a-3a8a9acb0d3f"), new Guid("6f7a8b9c-1d2e-3f4a-5b6c-7d8e9f0a1b2c") },
                    { new Guid("a8e96f8f-6ae8-4ffb-bb90-5a90786951d1"), new Guid("6f7a8b9c-1d2e-3f4a-5b6c-7d8e9f0a1b2c") }
                });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { new Guid("5a1c2e3f-4b6d-4c8e-9f1a-2b3c4d5e6f7a"), new Guid("1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c6d") },
                    { new Guid("6f7a8b9c-1d2e-3f4a-5b6c-7d8e9f0a1b2c"), new Guid("6f7a8b9c-1d2e-3f4a-5b6c-7d8e9f0a1b2c") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_Code",
                table: "Permissions",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_Group",
                table: "Permissions",
                column: "Group");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_ParentId",
                table: "Permissions",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_Token",
                table: "RefreshTokens",
                column: "Token");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_PermissionId",
                table: "RolePermissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Name",
                table: "Roles",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_TenantId",
                table: "Users",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailVerificationTokens");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "RolePermissions");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
