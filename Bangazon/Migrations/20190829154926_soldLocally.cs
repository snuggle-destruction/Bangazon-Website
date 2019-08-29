using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Bangazon.Migrations
{
    public partial class soldLocally : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Boolean>(
                name: "SoldLocally",
                table: "Product",
                nullable: false,
                defaultValue: false);
        }
    }
}
