using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Bangazon.Migrations
{
    public partial class orderHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-ffff-ffff-ffff-ffffffffffff",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "2bbb9667-0b02-4bee-b55c-32829c24f691", "AQAAAAEAACcQAAAAEKvEwUmjdz/BGQHKboIhbIHK3M1eyrxD3xwufHgwjwx3TbPpVmsnZDT2YlzbhmAhwg==" });

            migrationBuilder.InsertData(
                table: "Order",
                columns: new[] { "OrderId", "DateCompleted", "PaymentTypeId", "UserId" },
                values: new object[] { 2, new DateTime(2019, 8, 17, 0, 0, 0, 0, DateTimeKind.Local), 2, "00000000-ffff-ffff-ffff-ffffffffffff" });

            migrationBuilder.UpdateData(
                table: "OrderProduct",
                keyColumn: "OrderProductId",
                keyValue: 2,
                column: "OrderId",
                value: 2);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Order",
                keyColumn: "OrderId",
                keyValue: 2);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-ffff-ffff-ffff-ffffffffffff",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "ea142dae-7e8f-4362-99ca-dc1b4d4f0ef5", "AQAAAAEAACcQAAAAEEQi7QbQJ+4Wf8IKoUDqt6ShqGzulBQpN3T0YX/erYJZMDLPUPeCrh9SVxklRNRVIQ==" });

            migrationBuilder.UpdateData(
                table: "OrderProduct",
                keyColumn: "OrderProductId",
                keyValue: 2,
                column: "OrderId",
                value: 1);
        }
    }
}
