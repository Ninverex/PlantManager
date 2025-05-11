using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MenadzerRoslin.Migrations
{
    /// <inheritdoc />
    public partial class AddZdjecieToRoslina : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Zdjecie",
                table: "Rosliny");

            migrationBuilder.AddColumn<string>(
                name: "ZdjeciePath",
                table: "Rosliny",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ZdjeciePath",
                table: "Rosliny");

            migrationBuilder.AddColumn<byte[]>(
                name: "Zdjecie",
                table: "Rosliny",
                type: "bytea",
                nullable: true);
        }
    }
}
