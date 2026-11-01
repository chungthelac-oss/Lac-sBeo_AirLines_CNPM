using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DatVeMayBay.Migrations
{
    /// <inheritdoc />
    public partial class KhoiTao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChuyenBays",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SoHieu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiemDi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiemDen = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GioCatCanh = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GioHaCanh = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GiaVe = table.Column<decimal>(type: "decimal(18,0)", precision: 18, scale: 0, nullable: false),
                    SoGheTrong = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChuyenBays", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DatChos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaDatCho = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ChuyenBayId = table.Column<int>(type: "int", nullable: false),
                    HoTen = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoDienThoai = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NgayDat = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TongTien = table.Column<decimal>(type: "decimal(18,0)", precision: 18, scale: 0, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DatChos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DatChos_ChuyenBays_ChuyenBayId",
                        column: x => x.ChuyenBayId,
                        principalTable: "ChuyenBays",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ChuyenBays",
                columns: new[] { "Id", "DiemDen", "DiemDi", "GiaVe", "GioCatCanh", "GioHaCanh", "SoGheTrong", "SoHieu" },
                values: new object[,]
                {
                    { 1, "HAN", "SGN", 1290000m, new DateTime(2026, 10, 28, 6, 15, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 28, 8, 25, 0, 0, DateTimeKind.Unspecified), 12, "CD 101" },
                    { 2, "HAN", "SGN", 1490000m, new DateTime(2026, 10, 28, 11, 40, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 28, 13, 50, 0, 0, DateTimeKind.Unspecified), 3, "CD 103" },
                    { 3, "DAD", "SGN", 890000m, new DateTime(2026, 10, 28, 9, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 28, 10, 20, 0, 0, DateTimeKind.Unspecified), 20, "CD 205" },
                    { 4, "PQC", "HAN", 1750000m, new DateTime(2026, 10, 29, 14, 30, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 29, 16, 45, 0, 0, DateTimeKind.Unspecified), 8, "CD 307" },
                    { 5, "SGN", "HAN", 1390000m, new DateTime(2026, 10, 30, 18, 10, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 30, 20, 20, 0, 0, DateTimeKind.Unspecified), 15, "CD 102" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_DatChos_ChuyenBayId",
                table: "DatChos",
                column: "ChuyenBayId");

            migrationBuilder.CreateIndex(
                name: "IX_DatChos_MaDatCho",
                table: "DatChos",
                column: "MaDatCho",
                unique: true,
                filter: "[MaDatCho] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DatChos");

            migrationBuilder.DropTable(
                name: "ChuyenBays");
        }
    }
}
