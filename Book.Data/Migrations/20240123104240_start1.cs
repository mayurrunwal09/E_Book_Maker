using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Book.Data.Migrations
{
    public partial class start1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "Gender", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "Phoneno", "Role", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "1", 0, "1", "admin@example.com", false, null, false, null, "AdminFirstName", null, "Admin", null, null, false, null, "Admin", "54ce8cbe-4ed9-499b-aeee-2bbf1378fb3e", false, "admin@example.com" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "Gender", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "Phoneno", "Role", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "2", 0, "2", "author@example.com", false, null, false, null, "AuthorFirstName", null, "Author", null, null, false, null, "Author", "f08f91d3-a7d4-45b2-bfa2-36e22312ecea", false, "author@example.com" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "AspNetUsers");
        }
    }
}
