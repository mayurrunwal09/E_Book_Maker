using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Book.Data.Migrations
{
    public partial class ini : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Format",
                table: "books",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Author",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                column: "SecurityStamp",
                value: "3eb941e9-fe06-43d4-809e-a3241a2c9c88");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                column: "SecurityStamp",
                value: "84f21554-97b3-4796-941c-3ad342850ace");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Format",
                table: "books");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Author");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                column: "SecurityStamp",
                value: "54ce8cbe-4ed9-499b-aeee-2bbf1378fb3e");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                column: "SecurityStamp",
                value: "f08f91d3-a7d4-45b2-bfa2-36e22312ecea");
        }
    }
}
