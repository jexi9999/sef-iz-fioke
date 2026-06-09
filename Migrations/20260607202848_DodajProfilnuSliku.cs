using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SefIzFioke.Migrations
{
    /// <inheritdoc />
    public partial class DodajProfilnuSliku : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Korisnici",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    KorisnickoIme = table.Column<string>(type: "TEXT", nullable: false),
                    Ime = table.Column<string>(type: "TEXT", nullable: false),
                    Prezime = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Lozinka = table.Column<string>(type: "TEXT", nullable: false),
                    IsAdmin = table.Column<bool>(type: "INTEGER", nullable: false),
                    JeBlokirn = table.Column<bool>(type: "INTEGER", nullable: false),
                    ProfilnaSlika = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Korisnici", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Recepti",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Naziv = table.Column<string>(type: "TEXT", nullable: false),
                    KratakOpis = table.Column<string>(type: "TEXT", nullable: false),
                    Namirnice = table.Column<string>(type: "TEXT", nullable: false),
                    Sastojci = table.Column<string>(type: "TEXT", nullable: false),
                    NacinPripreme = table.Column<string>(type: "TEXT", nullable: false),
                    DijetetskiRestrikcije = table.Column<string>(type: "TEXT", nullable: false),
                    Napomene = table.Column<string>(type: "TEXT", nullable: false),
                    BrojPorcija = table.Column<int>(type: "INTEGER", nullable: false),
                    VremePripreme = table.Column<string>(type: "TEXT", nullable: false),
                    SlikaPath = table.Column<string>(type: "TEXT", nullable: false),
                    DodatneSlika = table.Column<string>(type: "TEXT", nullable: false),
                    VideoPath = table.Column<string>(type: "TEXT", nullable: false),
                    KorisnikId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recepti", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Recepti_Korisnici_KorisnikId",
                        column: x => x.KorisnikId,
                        principalTable: "Korisnici",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Upozorenja",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Poruka = table.Column<string>(type: "TEXT", nullable: false),
                    DatumSlanja = table.Column<DateTime>(type: "TEXT", nullable: false),
                    KorisnikId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Upozorenja", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Upozorenja_Korisnici_KorisnikId",
                        column: x => x.KorisnikId,
                        principalTable: "Korisnici",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Komentari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Tekst = table.Column<string>(type: "TEXT", nullable: false),
                    MediaPath = table.Column<string>(type: "TEXT", nullable: false),
                    ReceptId = table.Column<int>(type: "INTEGER", nullable: false),
                    KorisnikId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Komentari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Komentari_Korisnici_KorisnikId",
                        column: x => x.KorisnikId,
                        principalTable: "Korisnici",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Komentari_Recepti_ReceptId",
                        column: x => x.ReceptId,
                        principalTable: "Recepti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Komentari_KorisnikId",
                table: "Komentari",
                column: "KorisnikId");

            migrationBuilder.CreateIndex(
                name: "IX_Komentari_ReceptId",
                table: "Komentari",
                column: "ReceptId");

            migrationBuilder.CreateIndex(
                name: "IX_Korisnici_Email",
                table: "Korisnici",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Korisnici_KorisnickoIme",
                table: "Korisnici",
                column: "KorisnickoIme",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Recepti_KorisnikId",
                table: "Recepti",
                column: "KorisnikId");

            migrationBuilder.CreateIndex(
                name: "IX_Upozorenja_KorisnikId",
                table: "Upozorenja",
                column: "KorisnikId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Komentari");

            migrationBuilder.DropTable(
                name: "Upozorenja");

            migrationBuilder.DropTable(
                name: "Recepti");

            migrationBuilder.DropTable(
                name: "Korisnici");
        }
    }
}
