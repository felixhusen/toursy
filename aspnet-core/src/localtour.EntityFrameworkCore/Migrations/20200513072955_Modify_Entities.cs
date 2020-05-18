using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace localtour.Migrations
{
    public partial class Modify_Entities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ExpMonth",
                table: "Transactions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ExpYear",
                table: "Transactions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "NameOnCard",
                table: "Transactions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Transactions",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Requests",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Requests",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Disputes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Disputes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Bookings",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpMonth",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "ExpYear",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "NameOnCard",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Disputes");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Disputes");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Bookings");
        }
    }
}
