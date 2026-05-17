-- ═══════════════════════════════════════════════════════════════════════
-- ⚠️  DEPRECATED - KHÔNG CẦN CHẠY SCRIPT NÀY NỮA
-- ═══════════════════════════════════════════════════════════════════════
-- LÝ DO:
-- STT lịch hẹn hiện được TÍNH ĐỘNG TRỰC TIẾP trong SELECT query 
-- của AppointmentForm.LoadLichHen() bằng ROW_NUMBER() OVER (...).
--
-- Không cần:
--   ❌ Cột SoThuTu trong bảng LichHen
--   ❌ Trigger TRG_LichHen_GanSTT
--   ❌ Backfill dữ liệu
--
-- Ưu điểm cách mới:
--   ✅ STT luôn chính xác (tính realtime khi query)
--   ✅ Không cần maintenance DB / trigger
--   ✅ Tự động cập nhật khi thêm/xác nhận/hủy lịch
--   ✅ Không có vấn đề recursion trigger / concurrency
-- ═══════════════════════════════════════════════════════════════════════

SET NOCOUNT ON;

PRINT '⚠️  Script này đã DEPRECATED - STT giờ được tính động trong code.';
PRINT '   Xem: AppointmentForm.LoadLichHen() - ROW_NUMBER() OVER (...)';
PRINT '';

-- ── ROLLBACK nếu đã chạy script cũ (bỏ comment để thực thi) ──
/*
IF OBJECT_ID('TRG_LichHen_GanSTT', 'TR') IS NOT NULL
BEGIN
    DROP TRIGGER TRG_LichHen_GanSTT;
    PRINT '✅ Đã xóa trigger TRG_LichHen_GanSTT';
END

IF EXISTS (SELECT 1 FROM sys.columns 
           WHERE object_id = OBJECT_ID('LichHen') AND name = 'SoThuTu')
BEGIN
    ALTER TABLE LichHen DROP COLUMN SoThuTu;
    PRINT '✅ Đã xóa cột SoThuTu khỏi bảng LichHen';
END
*/
