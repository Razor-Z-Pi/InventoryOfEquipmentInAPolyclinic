using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace InventoryOfEquipmentInAPolyclinic.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Username = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Action = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Details = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Timestamp = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "dolzhnosti",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nazvanie = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dolzhnosti", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "podrazdeleniya",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nazvanie = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_podrazdeleniya", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "statusy_oborudovaniya",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nazvanie = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_statusy_oborudovaniya", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Username = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PasswordHash = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Role = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "zdaniya",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    adres = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_zdaniya", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "sotrudniki",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    fio = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    dolzhnost_id = table.Column<int>(type: "int", nullable: false),
                    podrazdelenie_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sotrudniki", x => x.id);
                    table.ForeignKey(
                        name: "FK_sotrudniki_dolzhnosti_dolzhnost_id",
                        column: x => x.dolzhnost_id,
                        principalTable: "dolzhnosti",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_sotrudniki_podrazdeleniya_podrazdelenie_id",
                        column: x => x.podrazdelenie_id,
                        principalTable: "podrazdeleniya",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "mesta",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nazvanie_kabineta = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    etazh = table.Column<int>(type: "int", nullable: false),
                    nomer_kabineta = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    zdanie_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mesta", x => x.id);
                    table.ForeignKey(
                        name: "FK_mesta_zdaniya_zdanie_id",
                        column: x => x.zdanie_id,
                        principalTable: "zdaniya",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "oborudovanie",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nazvanie = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    model = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    serijnyj_nomer = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    data_vypuska = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    data_vvoda = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    srok_sluzhby_let = table.Column<int>(type: "int", nullable: false),
                    status_id = table.Column<int>(type: "int", nullable: false),
                    mesto_id = table.Column<int>(type: "int", nullable: false),
                    otvetstvennyj_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_oborudovanie", x => x.id);
                    table.ForeignKey(
                        name: "FK_oborudovanie_mesta_mesto_id",
                        column: x => x.mesto_id,
                        principalTable: "mesta",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_oborudovanie_sotrudniki_otvetstvennyj_id",
                        column: x => x.otvetstvennyj_id,
                        principalTable: "sotrudniki",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_oborudovanie_statusy_oborudovaniya_status_id",
                        column: x => x.status_id,
                        principalTable: "statusy_oborudovaniya",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "to_obsluzhivanie",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    oborudovanie_id = table.Column<int>(type: "int", nullable: false),
                    data_to = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    data_sleduyushchego = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    tip = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    kommentarij = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_to_obsluzhivanie", x => x.id);
                    table.ForeignKey(
                        name: "FK_to_obsluzhivanie_oborudovanie_oborudovanie_id",
                        column: x => x.oborudovanie_id,
                        principalTable: "oborudovanie",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "PasswordHash", "Role", "Username" },
                values: new object[,]
                {
                    { 1, "admin123", "Admin", "admin" },
                    { 2, "user123", "User", "user" }
                });

            migrationBuilder.InsertData(
                table: "dolzhnosti",
                columns: new[] { "id", "nazvanie" },
                values: new object[,]
                {
                    { 1, "Старшая медсестра" },
                    { 2, "Врач-терапевт" },
                    { 3, "Врач-гинеколог" },
                    { 4, "Главврач" }
                });

            migrationBuilder.InsertData(
                table: "podrazdeleniya",
                columns: new[] { "id", "nazvanie" },
                values: new object[,]
                {
                    { 1, "Отделение ультразвуковой диагностики" },
                    { 2, "Отделение функциональной диагностики" },
                    { 3, "Мужская консультация" },
                    { 4, "Административно-управленческий персонал (АУП)" }
                });

            migrationBuilder.InsertData(
                table: "statusy_oborudovaniya",
                columns: new[] { "id", "nazvanie" },
                values: new object[,]
                {
                    { 1, "В эксплуатации" },
                    { 2, "На ремонте" },
                    { 3, "списано" }
                });

            migrationBuilder.InsertData(
                table: "zdaniya",
                columns: new[] { "id", "adres" },
                values: new object[] { 1, "Г. Абакан, ул. Джугашвили, 67" });

            migrationBuilder.InsertData(
                table: "mesta",
                columns: new[] { "id", "zdanie_id", "nomer_kabineta", "etazh", "nazvanie_kabineta" },
                values: new object[,]
                {
                    { 1, 1, "301", 3, "Кабинет терапевта" },
                    { 2, 1, "314", 3, "Кабинет гинеколога" },
                    { 3, 1, "108", 1, "Лаборатория" },
                    { 4, 1, "111", 1, "Стерилизационная" }
                });

            migrationBuilder.InsertData(
                table: "sotrudniki",
                columns: new[] { "id", "podrazdelenie_id", "fio", "dolzhnost_id" },
                values: new object[,]
                {
                    { 1, 1, "Иванова Мария Петровна", 1 },
                    { 2, 2, "Козлов Иван Валерьевич", 2 },
                    { 3, 3, "Тимошенко Пётр Игоревич", 3 },
                    { 4, 4, "Глухарев Акакий Иоанович", 4 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_mesta_zdanie_id",
                table: "mesta",
                column: "zdanie_id");

            migrationBuilder.CreateIndex(
                name: "IX_oborudovanie_mesto_id",
                table: "oborudovanie",
                column: "mesto_id");

            migrationBuilder.CreateIndex(
                name: "IX_oborudovanie_otvetstvennyj_id",
                table: "oborudovanie",
                column: "otvetstvennyj_id");

            migrationBuilder.CreateIndex(
                name: "IX_oborudovanie_status_id",
                table: "oborudovanie",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "IX_sotrudniki_dolzhnost_id",
                table: "sotrudniki",
                column: "dolzhnost_id");

            migrationBuilder.CreateIndex(
                name: "IX_sotrudniki_podrazdelenie_id",
                table: "sotrudniki",
                column: "podrazdelenie_id");

            migrationBuilder.CreateIndex(
                name: "IX_to_obsluzhivanie_oborudovanie_id",
                table: "to_obsluzhivanie",
                column: "oborudovanie_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditLogs");

            migrationBuilder.DropTable(
                name: "to_obsluzhivanie");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "oborudovanie");

            migrationBuilder.DropTable(
                name: "mesta");

            migrationBuilder.DropTable(
                name: "sotrudniki");

            migrationBuilder.DropTable(
                name: "statusy_oborudovaniya");

            migrationBuilder.DropTable(
                name: "zdaniya");

            migrationBuilder.DropTable(
                name: "dolzhnosti");

            migrationBuilder.DropTable(
                name: "podrazdeleniya");
        }
    }
}
