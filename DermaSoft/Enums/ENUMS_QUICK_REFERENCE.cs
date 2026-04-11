/*
 * ============================================================
 * DERMASOFT ENUMS - TÀI LIỆU TỔNG HỢP
 * ============================================================
 * Tất cả các enum được định nghĩa trong thư mục DermaSoft.Enums
 * và được đồng bộ 100% với SQL Server database
 * 
 * Ngày tạo: 2026-03-23
 * Phiên bản: 1.0
 * ============================================================
 */

namespace DermaSoft.Enums
{
    /// <summary>
    /// Tài liệu tổng hợp - Quick Reference
    /// 
    /// IMPORT:
    ///   using DermaSoft.Enums;
    /// 
    /// DANH SÁCH ENUMS:
    /// 
    /// 1. VaiTro (1-3)
    ///    - Admin = 1
    ///    - BacSi = 2
    ///    - LeTan = 3
    /// 
    /// 2. TrangThaiPhieuKham (0-4)
    ///    - Moi = 0
    ///    - DangKham = 1
    ///    - HoanThanh = 2
    ///    - DaThanhToan = 3
    ///    - DaHuy = 4
    /// 
    /// 3. TrangThaiLichHen (0-3)
    ///    - ChoXacNhan = 0
    ///    - DaXacNhan = 1
    ///    - HoanThanh = 2
    ///    - DaHuy = 3
    /// 
    /// 4. TrangThaiTaiKhoan (0-1)
    ///    - Khoa = 0
    ///    - HoatDong = 1
    /// 
    /// 5. TrangThaiOTP (0-2)
    ///    - ChuaXacThuc = 0
    ///    - DaXacThuc = 1
    ///    - HetHan = 2
    /// 
    /// 6. DiemDanhGia (1-5)
    ///    - RatTe = 1
    ///    - Te = 2
    ///    - BinhThuong = 3
    ///    - Tot = 4
    ///    - RatTot = 5
    /// 
    /// 7. LoaiThongBao (1-3)
    ///    - Email = 1
    ///    - SMS = 2
    ///    - TrongUngDung = 3
    /// 
    /// ============================================================
    /// VÍ DỤ SỬ DỤNG:
    /// ============================================================
    /// 
    /// // So sánh trạng thái
    /// if (phieuKham.TrangThai == TrangThaiPhieuKham.DaThanhToan)
    /// {
    ///     Console.WriteLine("Đã thanh toán");
    /// }
    /// 
    /// // Cast sang int
    /// int statusId = (int)TrangThaiPhieuKham.HoanThanh; // 2
    /// 
    /// // Lấy tên enum
    /// string name = TrangThaiPhieuKham.HoanThanh.ToString(); // "HoanThanh"
    /// 
    /// // Chuyển từ int sang enum
    /// TrangThaiPhieuKham status = (TrangThaiPhieuKham)2; // HoanThanh
    /// 
    /// ============================================================
    /// ĐỒ THỐNG SQL:
    /// ============================================================
    /// 
    /// SQL File: 01_Tables_Data.sql
    /// - Bảng VaiTro: INSERT 3 vai trò (Admin, BacSi, LeTan)
    /// 
    /// SQL File: 05_Constraints.sql
    /// - CHK_TrangThaiLich: BETWEEN 0 AND 3
    /// - CHK_TrangThaiPhieuKham: BETWEEN 0 AND 4
    /// - CHK_TrangThaiTK: IN (0, 1)
    /// - CHK_DiemDanh: BETWEEN 1 AND 5
    /// - CHK_LoaiThongBao: IN (1, 2, 3)
    /// 
    /// ============================================================
    /// MODELS SỬ DỤNG ENUMS:
    /// ============================================================
    /// 
    /// - VaiTroModel.cs
    ///   public VaiTro MaVaiTro { get; set; }
    /// 
    /// - AppointmentModel.cs
    ///   public TrangThaiLichHen TrangThai { get; set; }
    /// 
    /// - PhieuKhamModel.cs
    ///   public TrangThaiPhieuKham TrangThai { get; set; }
    /// 
    /// ============================================================
    /// TÀI LIỆU THÊM:
    /// ============================================================
    /// 
    /// - DermaSoft\DermaSoft\Enums\README.md
    ///   Chi tiết cách sử dụng từng enum
    /// 
    /// - DermaSoft\ENUM_SUMMARY_VI.md
    ///   Tóm tắt tiếng Việt
    /// 
    /// - DermaSoft\ENUM_COMPLETION_REPORT.txt
    ///   Báo cáo hoàn thành
    /// 
    /// ============================================================
    /// </summary>
    public static class EnumsDocumentation
    {
        // Lớp này chỉ dùng cho documentation, không có code logic
    }
}
