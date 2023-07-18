using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class addparentthread : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "Threads",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Threads_ParentId",
                table: "Threads",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Threads_Threads_ParentId",
                table: "Threads",
                column: "ParentId",
                principalTable: "Threads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Threads_Threads_ParentId",
                table: "Threads");

            migrationBuilder.DropIndex(
                name: "IX_Threads_ParentId",
                table: "Threads");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "Threads");
        }
    }
}
