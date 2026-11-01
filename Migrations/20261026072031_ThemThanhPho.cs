using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DatVeMayBay.Migrations
{
    /// <inheritdoc />
    public partial class ThemThanhPho : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ThanhPho",
                table: "SanBays");

            migrationBuilder.CreateTable(
                name: "ThanhPhos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ten = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThanhPhos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SanBayThanhPhos",
                columns: table => new
                {
                    SanBayMa = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    ThanhPhoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SanBayThanhPhos", x => new { x.SanBayMa, x.ThanhPhoId });
                    table.ForeignKey(
                        name: "FK_SanBayThanhPhos_SanBays_SanBayMa",
                        column: x => x.SanBayMa,
                        principalTable: "SanBays",
                        principalColumn: "Ma",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SanBayThanhPhos_ThanhPhos_ThanhPhoId",
                        column: x => x.ThanhPhoId,
                        principalTable: "ThanhPhos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ThanhPhos",
                columns: new[] { "Id", "Ten" },
                values: new object[,]
                {
                    { 1, "TP. Hồ Chí Minh" },
                    { 2, "Bình Dương" },
                    { 3, "Hà Nội" },
                    { 4, "Bắc Ninh" },
                    { 5, "Đà Nẵng" },
                    { 6, "Hội An" },
                    { 7, "Phú Quốc" },
                    { 8, "Nha Trang" },
                    { 9, "Cam Ranh" },
                    { 10, "Hải Phòng" },
                    { 11, "Huế" },
                    { 12, "Đà Lạt" },
                    { 13, "Cần Thơ" }
                });

            migrationBuilder.InsertData(
                table: "SanBayThanhPhos",
                columns: new[] { "SanBayMa", "ThanhPhoId" },
                values: new object[,]
                {
                    { "CXR", 8 },
                    { "CXR", 9 },
                    { "DAD", 5 },
                    { "DAD", 6 },
                    { "DLI", 12 },
                    { "HAN", 3 },
                    { "HAN", 4 },
                    { "HPH", 10 },
                    { "HUI", 11 },
                    { "PQC", 7 },
                    { "SGN", 1 },
                    { "SGN", 2 },
                    { "VCA", 13 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_SanBayThanhPhos_ThanhPhoId",
                table: "SanBayThanhPhos",
                column: "ThanhPhoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SanBayThanhPhos");

            migrationBuilder.DropTable(
                name: "ThanhPhos");

            migrationBuilder.AddColumn<string>(
                name: "ThanhPho",
                table: "SanBays",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "SanBays",
                keyColumn: "Ma",
                keyValue: "CXR",
                column: "ThanhPho",
                value: "Nha Trang");

            migrationBuilder.UpdateData(
                table: "SanBays",
                keyColumn: "Ma",
                keyValue: "DAD",
                column: "ThanhPho",
                value: "Đà Nẵng");

            migrationBuilder.UpdateData(
                table: "SanBays",
                keyColumn: "Ma",
                keyValue: "DLI",
                column: "ThanhPho",
                value: "Đà Lạt");

            migrationBuilder.UpdateData(
                table: "SanBays",
                keyColumn: "Ma",
                keyValue: "HAN",
                column: "ThanhPho",
                value: "Hà Nội");

            migrationBuilder.UpdateData(
                table: "SanBays",
                keyColumn: "Ma",
                keyValue: "HPH",
                column: "ThanhPho",
                value: "Hải Phòng");

            migrationBuilder.UpdateData(
                table: "SanBays",
                keyColumn: "Ma",
                keyValue: "HUI",
                column: "ThanhPho",
                value: "Huế");

            migrationBuilder.UpdateData(
                table: "SanBays",
                keyColumn: "Ma",
                keyValue: "PQC",
                column: "ThanhPho",
                value: "Phú Quốc");

            migrationBuilder.UpdateData(
                table: "SanBays",
                keyColumn: "Ma",
                keyValue: "SGN",
                column: "ThanhPho",
                value: "TP. Hồ Chí Minh");

            migrationBuilder.UpdateData(
                table: "SanBays",
                keyColumn: "Ma",
                keyValue: "VCA",
                column: "ThanhPho",
                value: "Cần Thơ");
        }
    }
}
