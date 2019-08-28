using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Bangazon.Migrations
{
    public partial class soldLocally : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "SoldLocally",
                table: "Product",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-ffff-ffff-ffff-ffffffffffff",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "d5f1082f-1a5f-4fcd-bbc4-f7a390950216", "AQAAAAEAACcQAAAAEHMcfBZ8Brstlx0myQYD6lP1Jg7h9Km94JLrO2k234yp5BZCqoziehJsYM4p/9oLFw==" });

            migrationBuilder.UpdateData(
                table: "Order",
                keyColumn: "OrderId",
                keyValue: 2,
                column: "DateCompleted",
                value: new DateTime(2019, 8, 18, 0, 0, 0, 0, DateTimeKind.Local));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SoldLocally",
                table: "Product");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-ffff-ffff-ffff-ffffffffffff",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "2bbb9667-0b02-4bee-b55c-32829c24f691", "AQAAAAEAACcQAAAAEKvEwUmjdz/BGQHKboIhbIHK3M1eyrxD3xwufHgwjwx3TbPpVmsnZDT2YlzbhmAhwg==" });

            migrationBuilder.UpdateData(
                table: "Order",
                keyColumn: "OrderId",
                keyValue: 2,
                column: "DateCompleted",
                value: new DateTime(2019, 8, 17, 0, 0, 0, 0, DateTimeKind.Local));
        }
    }
}
