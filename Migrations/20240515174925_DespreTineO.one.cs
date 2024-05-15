using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LRTV.Migrations.Players
{
    /// <inheritdoc />
    public partial class DespreTineOone : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Players_TeamModel_CurrentTeamId",
                table: "Players");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TeamModel",
                table: "TeamModel");

            migrationBuilder.RenameTable(
                name: "TeamModel",
                newName: "Teams");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Teams",
                table: "Teams",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Teams_CurrentTeamId",
                table: "Players",
                column: "CurrentTeamId",
                principalTable: "Teams",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Players_Teams_CurrentTeamId",
                table: "Players");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Teams",
                table: "Teams");

            migrationBuilder.RenameTable(
                name: "Teams",
                newName: "TeamModel");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TeamModel",
                table: "TeamModel",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Players_TeamModel_CurrentTeamId",
                table: "Players",
                column: "CurrentTeamId",
                principalTable: "TeamModel",
                principalColumn: "Id");
        }
    }
}
