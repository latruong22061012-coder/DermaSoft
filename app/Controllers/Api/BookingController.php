<?php
/**
 * Booking API Controller
 * Xử lý đặt lịch hẹn từ website (cả khách vãng lai lẫn user đã login)
 */

namespace App\Controllers\Api;

use App\Controllers\ApiController;
use App\Core\Auth;
use App\Core\Database;

class BookingController extends ApiController
{
    /**
     * POST /api/booking/create
     * Body: { hoTen, soDienThoai, thoiGianHen (Y-m-d H:i), ghiChu? }
     */
    public function create(): void
    {
        Auth::startSession();

        $data = $this->getJSON();

        $errors = $this->validate($data, [
            'hoTen'      => 'required|minlen:3',
            'soDienThoai'=> 'required',
            'thoiGianHen'=> 'required',
        ]);

        if (!empty($errors)) {
            $this->error('Dữ liệu không hợp lệ', $errors, 400);
            return;
        }

        // Validate phone
        $phone = trim($data['soDienThoai']);
        if (!preg_match('/^(0)(3[2-9]|5[6-9]|7[06-9]|8[0-9]|9[0-9])[0-9]{7}$/', $phone)) {
            $this->error('Số điện thoại không đúng định dạng', null, 400);
            return;
        }

        // Validate & parse datetime (format: Y-m-d H:i)
        $thoiGianHenStr = trim($data['thoiGianHen']);
        $dt = \DateTime::createFromFormat('Y-m-d H:i', $thoiGianHenStr);
        if (!$dt || $dt->format('Y-m-d H:i') !== $thoiGianHenStr) {
            $this->error('Thời gian hẹn không đúng định dạng (YYYY-MM-DD HH:MM)', null, 400);
            return;
        }

        // Phải là ngày trong tương lai
        if ($dt <= new \DateTime()) {
            $this->error('Thời gian hẹn phải sau thời điểm hiện tại', null, 400);
            return;
        }

        // Không đặt quá 60 ngày từ hôm nay
        $maxDate = new \DateTime('+60 days');
        if ($dt > $maxDate) {
            $this->error('Không thể đặt lịch quá 60 ngày từ hôm nay', null, 400);
            return;
        }

        $ghiChu = isset($data['ghiChu']) ? trim($data['ghiChu']) : null;
        if ($ghiChu === '') $ghiChu = null;

        // Kiểm tra trùng lịch ở PHP trước khi gọi SP
        // (tránh vấn đề encoding với thông báo lỗi tiếng Việt từ pdo_sqlsrv trên Windows)
        try {
            $duplicate = Database::fetchOne(
                "SELECT TOP 1 lh.MaLichHen
                 FROM LichHen lh
                 JOIN BenhNhan bn ON lh.MaBenhNhan = bn.MaBenhNhan
                 WHERE bn.SoDienThoai = ?
                   AND CAST(lh.ThoiGianHen AS DATE) = CAST(? AS DATE)
                   AND lh.TrangThai IN (1, 2)",
                [$phone, $dt->format('Y-m-d')]
            );
            if ($duplicate) {
                $this->error('Số điện thoại này đã có lịch hẹn trong ngày đã chọn. Vui lòng chọn ngày khác.', null, 400);
                return;
            }
        } catch (\Exception $e) {
            error_log('Lỗi kiểm tra trùng lịch: ' . $e->getMessage());
        }

        try {
            $result = Database::fetchOne(
                "EXEC SP_DatLichHen @HoTen = ?, @SoDienThoai = ?, @ThoiGianHen = ?, @GhiChu = ?",
                [
                    trim($data['hoTen']),
                    $phone,
                    $dt->format('Y-m-d H:i:s'),
                    $ghiChu,
                ]
            );

            $this->success([
                'maLichHen'  => $result['MaLichHen']  ?? null,
                'maBenhNhan' => $result['MaBenhNhan'] ?? null,
                'thoiGianHen'=> $dt->format('d/m/Y H:i'),
            ], 'Đặt lịch thành công! Chúng tôi sẽ liên hệ xác nhận trong vòng 30 phút.');

        } catch (\Exception $e) {
            error_log('Lỗi đặt lịch: ' . $e->getMessage());
            $this->error('Lỗi hệ thống khi đặt lịch. Vui lòng thử lại sau.', null, 500);
        }
    }
}
