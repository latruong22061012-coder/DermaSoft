-- ═══════════════════════════════════════════════════════════════════════
-- 🔒 MIGRATION: Thêm chức năng KHÓA THẺ thành viên
-- ═══════════════════════════════════════════════════════════════════════
-- Thay thế chức năng "Hủy thẻ" (DELETE) bằng "Khóa thẻ" (UPDATE DaKhoa=1)
-- giúp:
--   • Giữ nguyên lịch sử giao dịch & điểm tích lũy
--   • Có thể mở khóa lại nếu cần
--   • Không làm mất dữ liệu khách hàng
-- ═══════════════════════════════════════════════════════════════════════

SET NOCOUNT ON;

PRINT '════════════════════════════════════════════════════════════';
PRINT '  🔒 THÊM CHỨC NĂNG KHÓA THẺ - ThanhVienInfo';
PRINT '════════════════════════════════════════════════════════════';

-- Thêm cột DaKhoa (BIT) vào bảng ThanhVienInfo
IF NOT EXISTS (
    SELECT 1 FROM sys.columns
    WHERE object_id = OBJECT_ID('ThanhVienInfo') AND name = 'DaKhoa')
BEGIN
    ALTER TABLE ThanhVienInfo 
        ADD DaKhoa BIT NOT NULL DEFAULT 0;
    PRINT '  ✅ Đã thêm cột DaKhoa vào bảng ThanhVienInfo.';
END
ELSE
    PRINT '  ℹ️  Cột DaKhoa đã tồn tại.';

-- Thêm cột NgayKhoa để lưu thời điểm khóa (tùy chọn, để audit)
IF NOT EXISTS (
    SELECT 1 FROM sys.columns
    WHERE object_id = OBJECT_ID('ThanhVienInfo') AND name = 'NgayKhoa')
BEGIN
    ALTER TABLE ThanhVienInfo 
        ADD NgayKhoa DATETIME NULL;
    PRINT '  ✅ Đã thêm cột NgayKhoa vào bảng ThanhVienInfo.';
END
ELSE
    PRINT '  ℹ️  Cột NgayKhoa đã tồn tại.';

PRINT '';
PRINT '  Hoàn thành: ' + CONVERT(VARCHAR, GETDATE(), 120);
PRINT '════════════════════════════════════════════════════════════';
