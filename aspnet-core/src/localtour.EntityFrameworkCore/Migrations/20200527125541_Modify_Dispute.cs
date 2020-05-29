using Microsoft.EntityFrameworkCore.Migrations;

namespace localtour.Migrations
{
    public partial class Modify_Dispute : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "Disputes",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Disputes_UserId",
                table: "Disputes",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Disputes_AbpUsers_UserId",
                table: "Disputes",
                column: "UserId",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Disputes_AbpUsers_UserId",
                table: "Disputes");

            migrationBuilder.DropIndex(
                name: "IX_Disputes_UserId",
                table: "Disputes");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Disputes");
        }
    }
}
