using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace localtour.Migrations
{
    public partial class Added_Message : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateSent = table.Column<DateTime>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    ReceiverId = table.Column<long>(nullable: true),
                    SenderId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Messages");
        }
    }
}
