USE DERMASOFT;
GO

-- ═══════════════════════════════════════
-- TẠO BẢNG CaiDatHeThong (key-value settings)
-- ═══════════════════════════════════════

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'CaiDatHeThong')
BEGIN
    CREATE TABLE CaiDatHeThong (
        Khoa   VARCHAR(50)    PRIMARY KEY,
        GiaTri NVARCHAR(500)  NOT NULL,
        MoTa   NVARCHAR(200)  NULL
    );

    -- Seed giá trị mặc định
    INSERT INTO CaiDatHeThong (Khoa, GiaTri, MoTa) VALUES
    ('NGUONG_THAP',       '10',         N'Ngưỡng tồn kho mức Thấp'),
    ('NGUONG_NGUY_HIEM',  '3',          N'Ngưỡng tồn kho mức Nguy hiểm'),
    ('MK_MAC_DINH',       'Temp@2026',  N'Mật khẩu mặc định khi tạo/reset nhân viên');

    PRINT N'✓ Bảng CaiDatHeThong đã tạo và seed xong.'
END
ELSE
    PRINT N'✓ Bảng CaiDatHeThong đã tồn tại.'
GO

-- ═══════════════════════════════════════
-- KHÔI PHỤC cột MoTa trong ThongTinPhongKham
-- (nếu bị ghi đè bởi settings version cũ)
-- ═══════════════════════════════════════

UPDATE ThongTinPhongKham
SET MoTa = NULL
WHERE MoTa LIKE 'NGUONG_THAP=%'
   OR MoTa LIKE 'MK_MAC_DINH=%';

PRINT N'✓ Đã khôi phục cột MoTa trong ThongTinPhongKham (nếu bị ghi đè).'
GO
