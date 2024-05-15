using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LRTV.Migrations.Players
{
    /// <inheritdoc />
    public partial class AGP_NewPlayers_Good : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TeamID",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TeamID",
                table: "Players");
        }
    }
}
