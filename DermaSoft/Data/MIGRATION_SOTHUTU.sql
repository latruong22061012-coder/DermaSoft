-- ═══════════════════════════════════════════════════════════════════════
-- MIGRATION: Thêm cột SoThuTu vào bảng LichHen
-- ═══════════════════════════════════════════════════════════════════════
-- Chạy script này trên SSMS để thêm cột SoThuTu cho tính năng cấp STT tự động.
-- ═══════════════════════════════════════════════════════════════════════

IF NOT EXISTS (
    SELECT 1 FROM sys.columns
    WHERE object_id = OBJECT_ID('LichHen') AND name = 'SoThuTu'
)
BEGIN
    ALTER TABLE LichHen ADD SoThuTu INT NULL;
    PRINT '✅ Đã thêm cột SoThuTu vào bảng LichHen.';
END
ELSE
BEGIN
    PRINT '⚠️ Cột SoThuTu đã tồn tại trong bảng LichHen.';
END
