using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Bangazon.Migrations
{
    public partial class ExpirationDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ExpirationDate",
                table: "PaymentType",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-ffff-ffff-ffff-ffffffffffff",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "eda94fd2-de3e-4998-97ed-b8cbcc878be1", "AQAAAAEAACcQAAAAEHtytsxkugFIHG2K8iIQzLNZACIoXFnSOGP8LuZweDCxgW/7I6Yi1dAXMrgX/NAv1w==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpirationDate",
                table: "PaymentType");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-ffff-ffff-ffff-ffffffffffff",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "ea142dae-7e8f-4362-99ca-dc1b4d4f0ef5", "AQAAAAEAACcQAAAAEEQi7QbQJ+4Wf8IKoUDqt6ShqGzulBQpN3T0YX/erYJZMDLPUPeCrh9SVxklRNRVIQ==" });
        }
    }
}
