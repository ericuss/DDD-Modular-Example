using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace DDD.Customer.Data.Context.Migrations
{
    public partial class Add_Some_Properties_to_Customer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Customers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Surname",
                table: "Customers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "Surname",
                table: "Customers");
        }
    }
}
