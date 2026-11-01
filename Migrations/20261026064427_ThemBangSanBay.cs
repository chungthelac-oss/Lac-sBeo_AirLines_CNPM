using Microsoft.EntityFrameworkCore.Migrations;
#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DatVeMayBay.Migrations
{
    /// <inheritdoc />
    public partial class ThemBangSanBay : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DiemDi",
                table: "ChuyenBays",
                type: "nvarchar(3)",
                maxLength: 3,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "DiemDen",
                table: "ChuyenBays",
                type: "nvarchar(3)",
                maxLength: 3,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "SanBays",
                columns: table => new
                {
                    Ma = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    Ten = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ThanhPho = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SanBays", x => x.Ma);
                });

            migrationBuilder.InsertData(
                table: "SanBays",
                columns: new[] { "Ma", "Ten", "ThanhPho" },
                values: new object[,]
                {
                    { "CXR", "Cam Ranh", "Nha Trang" },
                    { "DAD", "Đà Nẵng", "Đà Nẵng" },
                    { "DLI", "Liên Khương", "Đà Lạt" },
                    { "HAN", "Nội Bài", "Hà Nội" },
                    { "HPH", "Cát Bi", "Hải Phòng" },
                    { "HUI", "Phú Bài", "Huế" },
                    { "PQC", "Phú Quốc", "Phú Quốc" },
                    { "SGN", "Tân Sơn Nhất", "TP. Hồ Chí Minh" },
                    { "VCA", "Cần Thơ", "Cần Thơ" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChuyenBays_DiemDen",
                table: "ChuyenBays",
                column: "DiemDen");

            migrationBuilder.CreateIndex(
                name: "IX_ChuyenBays_DiemDi",
                table: "ChuyenBays",
                column: "DiemDi");

            migrationBuilder.AddForeignKey(
                name: "FK_ChuyenBays_SanBays_DiemDen",
                table: "ChuyenBays",
                column: "DiemDen",
                principalTable: "SanBays",
                principalColumn: "Ma",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ChuyenBays_SanBays_DiemDi",
                table: "ChuyenBays",
                column: "DiemDi",
                principalTable: "SanBays",
                principalColumn: "Ma",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChuyenBays_SanBays_DiemDen",
                table: "ChuyenBays");

            migrationBuilder.DropForeignKey(
                name: "FK_ChuyenBays_SanBays_DiemDi",
                table: "ChuyenBays");

            migrationBuilder.DropTable(
                name: "SanBays");

            migrationBuilder.DropIndex(
                name: "IX_ChuyenBays_DiemDen",
                table: "ChuyenBays");

            migrationBuilder.DropIndex(
                name: "IX_ChuyenBays_DiemDi",
                table: "ChuyenBays");

            migrationBuilder.AlterColumn<string>(
                name: "DiemDi",
                table: "ChuyenBays",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(3)",
                oldMaxLength: 3);

            migrationBuilder.AlterColumn<string>(
                name: "DiemDen",
                table: "ChuyenBays",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(3)",
                oldMaxLength: 3);
        }
    }
}
