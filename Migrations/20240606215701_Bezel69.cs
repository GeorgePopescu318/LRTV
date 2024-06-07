using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LRTV.Migrations
{
    /// <inheritdoc />
    public partial class Bezel69 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_News_newsId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_newsId",
                table: "Comments");

            migrationBuilder.AlterColumn<int>(
                name: "userId",
                table: "Comments",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "NewsModelId",
                table: "Comments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_NewsModelId",
                table: "Comments",
                column: "NewsModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_News_NewsModelId",
                table: "Comments",
                column: "NewsModelId",
                principalTable: "News",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_News_NewsModelId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_NewsModelId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "NewsModelId",
                table: "Comments");

            migrationBuilder.AlterColumn<string>(
                name: "userId",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_newsId",
                table: "Comments",
                column: "newsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_News_newsId",
                table: "Comments",
                column: "newsId",
                principalTable: "News",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
