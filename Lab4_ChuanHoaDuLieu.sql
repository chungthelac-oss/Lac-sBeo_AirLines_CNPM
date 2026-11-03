/* =====================================================================
   LAB 4 - DỮ LIỆU MẪU HỆ THỐNG ĐẶT VÉ MÁY BAY
   Database: DatVeMayBay (theo code repo DatVeMayBay_CNPM)

   File này chạy SAU KHI đã import xong 4 file .sql tải từ Mockaroo:
   ChuyenBays -> ChangDungs -> HanhKhachs -> DatChos.
   Mockaroo sinh ngẫu nhiên nên có chỗ chưa hợp lý (giờ hạ cánh trước
   giờ cất cánh, điểm đi trùng điểm đến...). Script này sửa lại cho đúng
   nghiệp vụ. Chỉ đụng tới dữ liệu Mockaroo (Id từ 1001 trở lên),
   không đụng dữ liệu mẫu có sẵn trong code (HasData).

   CHỈ CHẠY 1 LẦN (phần 3.2 trừ ghế trống, chạy 2 lần sẽ trừ 2 lần).
   ===================================================================== */

-- ---------------------------------------------------------------------
-- PHẦN 0 (CHỈ DÙNG KHI MUỐN LÀM LẠI TỪ ĐẦU): xóa dữ liệu Mockaroo đã import.
-- Bôi đen 4 dòng DELETE dưới đây, bấm Ctrl+K rồi Ctrl+U để bỏ dấu --,
-- bôi đen lại và bấm Execute.
-- ---------------------------------------------------------------------
-- DELETE FROM DatChos    WHERE Id >= 1001;
-- DELETE FROM HanhKhachs WHERE Id >= 1001;
-- DELETE FROM ChangDungs WHERE Id >= 1001;
-- DELETE FROM ChuyenBays WHERE Id >= 1001;

/* ---------------------------------------------------------------------
   PHẦN 1: CHUYẾN BAY (ChuyenBays)
   --------------------------------------------------------------------- */

-- 1.1 Điểm đến phải khác điểm đi (đề bài ý 6)
UPDATE c
SET c.DiemDen = (SELECT TOP 1 s.Ma FROM SanBays s WHERE s.Ma <> c.DiemDi ORDER BY NEWID())
FROM ChuyenBays c
WHERE c.Id >= 1001 AND c.DiemDen = c.DiemDi;

-- 1.2 Số hiệu = mã hãng + 3 số, giống dữ liệu có sẵn (VD: "CD 345")
UPDATE c
SET c.SoHieu = h.Ma + ' ' + CAST(100 + ABS(CHECKSUM(NEWID())) % 900 AS nvarchar(3))
FROM ChuyenBays c
JOIN HangHangKhongs h ON h.Id = c.HangHangKhongId
WHERE c.Id >= 1001;

-- 1.3 Giờ hạ cánh = giờ cất cánh + 60 đến 180 phút (đề bài ý 7)
UPDATE ChuyenBays
SET GioHaCanh = DATEADD(MINUTE, 60 + ABS(CHECKSUM(NEWID())) % 121, GioCatCanh)
WHERE Id >= 1001;

-- 1.4 Chuyến có chặng dừng thì bay lâu hơn thêm 60 phút
UPDATE c
SET c.GioHaCanh = DATEADD(MINUTE, 60, c.GioHaCanh)
FROM ChuyenBays c
WHERE c.Id >= 1001
  AND EXISTS (SELECT 1 FROM ChangDungs d WHERE d.ChuyenBayId = c.Id);

/* ---------------------------------------------------------------------
   PHẦN 2: CHẶNG DỪNG (ChangDungs) - đề bài ý 8, 9
   --------------------------------------------------------------------- */

-- 2.1 Sân bay dừng không được trùng sân bay đi / sân bay đến
UPDATE d
SET d.SanBayMa = (SELECT TOP 1 s.Ma
                  FROM SanBays s
                  WHERE s.Ma NOT IN (c.DiemDi, c.DiemDen)
                  ORDER BY NEWID())
FROM ChangDungs d
JOIN ChuyenBays c ON c.Id = d.ChuyenBayId
WHERE d.Id >= 1001 AND d.SanBayMa IN (c.DiemDi, c.DiemDen);

-- 2.2 Giờ đến / giờ đi của chặng dừng nằm giữa hành trình, dừng 40 phút
UPDATE d
SET d.GioDen = DATEADD(MINUTE, DATEDIFF(MINUTE, c.GioCatCanh, c.GioHaCanh) / 2 - 20, c.GioCatCanh),
    d.GioDi  = DATEADD(MINUTE, DATEDIFF(MINUTE, c.GioCatCanh, c.GioHaCanh) / 2 + 20, c.GioCatCanh)
FROM ChangDungs d
JOIN ChuyenBays c ON c.Id = d.ChuyenBayId
WHERE d.Id >= 1001;

/* ---------------------------------------------------------------------
   PHẦN 3: VÉ (DatChos) - đề bài ý 3, 4, 5
   --------------------------------------------------------------------- */

-- 3.1 Tổng tiền = giá vé của chuyến bay; ngày đặt trong 30 ngày gần đây
UPDATE d
SET d.TongTien = c.GiaVe,
    d.NgayDat  = DATEADD(MINUTE, -(ABS(CHECKSUM(NEWID())) % 43200), GETDATE())
FROM DatChos d
JOIN ChuyenBays c ON c.Id = d.ChuyenBayId
WHERE d.Id >= 1001;

-- 3.2 Trừ số ghế trống theo số vé chưa hủy (giống code DatChoController)
UPDATE c
SET c.SoGheTrong = CASE WHEN c.SoGheTrong - x.SoVe < 0 THEN 0 ELSE c.SoGheTrong - x.SoVe END
FROM ChuyenBays c
JOIN (SELECT ChuyenBayId, COUNT(*) AS SoVe
      FROM DatChos
      WHERE Id >= 1001 AND TrangThai <> 'DaHuy'
      GROUP BY ChuyenBayId) x ON x.ChuyenBayId = c.Id;

/* ---------------------------------------------------------------------
   PHẦN 4: KIỂM TRA KẾT QUẢ (chụp màn hình phần này để nộp)
   --------------------------------------------------------------------- */

-- 4.1 Số dòng mỗi bảng
SELECT 'HangHangKhongs' AS Bang, COUNT(*) AS SoDong FROM HangHangKhongs
UNION ALL SELECT 'ThanhPhos', COUNT(*) FROM ThanhPhos
UNION ALL SELECT 'SanBays', COUNT(*) FROM SanBays
UNION ALL SELECT 'SanBayThanhPhos', COUNT(*) FROM SanBayThanhPhos
UNION ALL SELECT 'ChuyenBays', COUNT(*) FROM ChuyenBays
UNION ALL SELECT 'ChangDungs', COUNT(*) FROM ChangDungs
UNION ALL SELECT 'HanhKhachs', COUNT(*) FROM HanhKhachs
UNION ALL SELECT 'DatChos', COUNT(*) FROM DatChos;

-- 4.2 Các lỗi logic: tất cả phải bằng 0
SELECT
    (SELECT COUNT(*) FROM ChuyenBays WHERE GioHaCanh <= GioCatCanh)  AS HaCanhTruocCatCanh,
    (SELECT COUNT(*) FROM ChuyenBays WHERE DiemDi = DiemDen)          AS TrungSanBay,
    (SELECT COUNT(*) FROM ChangDungs d JOIN ChuyenBays c ON c.Id = d.ChuyenBayId
       WHERE d.GioDen <= c.GioCatCanh OR d.GioDi >= c.GioHaCanh)     AS ChangDungSaiGio,
    (SELECT COUNT(*) FROM DatChos d JOIN ChuyenBays c ON c.Id = d.ChuyenBayId
       WHERE d.NgayDat >= c.GioCatCanh)                               AS DatVeSauGioBay,
    (SELECT COUNT(*) FROM ChuyenBays WHERE SoGheTrong < 0)            AS GheTrongAm;

-- 4.3 Xem thử 10 chuyến bay kèm hãng và sân bay
SELECT TOP 10 c.SoHieu, h.Ten AS HangBay, c.DiemDi, c.DiemDen,
       c.GioCatCanh, c.GioHaCanh, c.GiaVe, c.SoGheTrong, c.MoDatVe
FROM ChuyenBays c
JOIN HangHangKhongs h ON h.Id = c.HangHangKhongId
WHERE c.Id >= 1001
ORDER BY c.GioCatCanh;
