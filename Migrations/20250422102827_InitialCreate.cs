using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MenadzerRoslin.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Gatunki",
                columns: table => new
                {
                    GatunekId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NazwaGatunku = table.Column<string>(type: "text", nullable: false),
                    WymagaNawadnianiaCoIleDni = table.Column<int>(type: "integer", nullable: false),
                    WymagaNawozeniaCoIleDni = table.Column<int>(type: "integer", nullable: false),
                    Swiatlo = table.Column<string>(type: "text", nullable: false),
                    TemperaturaMin = table.Column<double>(type: "double precision", nullable: false),
                    TemperaturaMax = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gatunki", x => x.GatunekId);
                });

            migrationBuilder.CreateTable(
                name: "Rosliny",
                columns: table => new
                {
                    RoslinaId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nazwa = table.Column<string>(type: "text", nullable: false),
                    DataZakupu = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Miejsce = table.Column<string>(type: "text", nullable: false),
                    GatunekId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rosliny", x => x.RoslinaId);
                    table.ForeignKey(
                        name: "FK_Rosliny_Gatunki_GatunekId",
                        column: x => x.GatunekId,
                        principalTable: "Gatunki",
                        principalColumn: "GatunekId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Przypomnienia",
                columns: table => new
                {
                    PrzypomnienieId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoslinaId = table.Column<int>(type: "integer", nullable: false),
                    TypZabiegu = table.Column<string>(type: "text", nullable: false),
                    DataPlanowana = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CzyWykonane = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Przypomnienia", x => x.PrzypomnienieId);
                    table.ForeignKey(
                        name: "FK_Przypomnienia_Rosliny_RoslinaId",
                        column: x => x.RoslinaId,
                        principalTable: "Rosliny",
                        principalColumn: "RoslinaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Zabiegi",
                columns: table => new
                {
                    ZabiegId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoslinaId = table.Column<int>(type: "integer", nullable: false),
                    TypZabiegu = table.Column<string>(type: "text", nullable: false),
                    DataWykonania = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Opis = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zabiegi", x => x.ZabiegId);
                    table.ForeignKey(
                        name: "FK_Zabiegi_Rosliny_RoslinaId",
                        column: x => x.RoslinaId,
                        principalTable: "Rosliny",
                        principalColumn: "RoslinaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Przypomnienia_RoslinaId",
                table: "Przypomnienia",
                column: "RoslinaId");

            migrationBuilder.CreateIndex(
                name: "IX_Rosliny_GatunekId",
                table: "Rosliny",
                column: "GatunekId");

            migrationBuilder.CreateIndex(
                name: "IX_Zabiegi_RoslinaId",
                table: "Zabiegi",
                column: "RoslinaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Przypomnienia");

            migrationBuilder.DropTable(
                name: "Zabiegi");

            migrationBuilder.DropTable(
                name: "Rosliny");

            migrationBuilder.DropTable(
                name: "Gatunki");
        }
    }
}
