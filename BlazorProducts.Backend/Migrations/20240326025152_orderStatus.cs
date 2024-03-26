using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BlazorProducts.Backend.Migrations
{
    /// <inheritdoc />
    public partial class orderStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2abbc012-3567-48cf-b76e-5ab86f6b30f8");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4eacf2f2-1545-49b4-a8c0-85afee6d528a");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "b4bd856b-0739-4447-b436-b8dceb428478", null, "Administrator", "ADMINISTRATOR" },
                    { "df3fda87-15a0-444a-8cc9-ec6434a197ec", null, "Viewer", "VIEWER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b4bd856b-0739-4447-b436-b8dceb428478");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "df3fda87-15a0-444a-8cc9-ec6434a197ec");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Orders");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2abbc012-3567-48cf-b76e-5ab86f6b30f8", null, "Viewer", "VIEWER" },
                    { "4eacf2f2-1545-49b4-a8c0-85afee6d528a", null, "Administrator", "ADMINISTRATOR" }
                });
        }
    }
}
