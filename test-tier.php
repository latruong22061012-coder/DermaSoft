<?php
/**
 * Test Kết Quả Khám Bệnh - Giả lập dữ liệu từ Windows App
 * Tạo PhieuKham + DichVu + Thuoc + ChiTietDichVu + ChiTietDonThuoc + HoaDon
 * Truy cập: http://localhost:3000/test-tier.php
 */
require_once __DIR__ . '/app/Core/Database.php';
use App\Core\Database;

session_start();
$pdo = Database::connect();

$sessionUser = $_SESSION['authenticated_user'] ?? null;
$phone = $sessionUser['SoDienThoai'] ?? '';
$message = '';
$messageType = '';

// Tìm BenhNhan
$benhNhan = null;
if ($phone) {
    $stmt = $pdo->prepare("SELECT * FROM BenhNhan WHERE SoDienThoai = ? AND IsDeleted = 0");
    $stmt->execute([$phone]);
    $benhNhan = $stmt->fetch(PDO::FETCH_ASSOC);
}

// ─── Dữ liệu mẫu da liễu ───────────────────────────────────
$sampleDichVu = [
    ['TenDichVu' => 'Khám da liễu tổng quát',         'DonGia' => 200000],
    ['TenDichVu' => 'Điều trị mụn trứng cá',          'DonGia' => 350000],
    ['TenDichVu' => 'Laser trị nám',                   'DonGia' => 1500000],
    ['TenDichVu' => 'Peel da hóa học',                 'DonGia' => 800000],
    ['TenDichVu' => 'Sinh thiết da',                   'DonGia' => 500000],
    ['TenDichVu' => 'Điều trị viêm da cơ địa',        'DonGia' => 300000],
    ['TenDichVu' => 'Đốt điện u mềm lây',             'DonGia' => 250000],
    ['TenDichVu' => 'Chăm sóc da chuyên sâu',         'DonGia' => 600000],
];

$sampleThuoc = [
    ['TenThuoc' => 'Tretinoin Cream 0.05%',   'DonViTinh' => 'Tuýp',  'DonGia' => 85000,  'SoLuongTon' => 200],
    ['TenThuoc' => 'Adapalene Gel 0.1%',      'DonViTinh' => 'Tuýp',  'DonGia' => 120000, 'SoLuongTon' => 150],
    ['TenThuoc' => 'Clindamycin Gel 1%',      'DonViTinh' => 'Tuýp',  'DonGia' => 65000,  'SoLuongTon' => 300],
    ['TenThuoc' => 'Hydrocortisone Cream 1%', 'DonViTinh' => 'Tuýp',  'DonGia' => 45000,  'SoLuongTon' => 250],
    ['TenThuoc' => 'Cetirizine 10mg',         'DonViTinh' => 'Viên',  'DonGia' => 3000,   'SoLuongTon' => 500],
    ['TenThuoc' => 'Doxycycline 100mg',       'DonViTinh' => 'Viên',  'DonGia' => 5000,   'SoLuongTon' => 400],
    ['TenThuoc' => 'Ketoconazole Cream 2%',   'DonViTinh' => 'Tuýp',  'DonGia' => 55000,  'SoLuongTon' => 180],
    ['TenThuoc' => 'Sunscreen SPF50+',        'DonViTinh' => 'Chai',  'DonGia' => 350000, 'SoLuongTon' => 100],
];

$samplePhieuKham = [
    [
        'TrieuChung' => 'Mụn trứng cá vùng trán và má, da nhờn, có mụn viêm đỏ',
        'ChanDoan'   => 'Acne Vulgaris - Mụn trứng cá mức độ trung bình',
        'TrangThai'  => 1,
        'GhiChu'     => 'Tái khám sau 2 tuần',
        'NgayOffset' => -30,
        'dichVuIdx'  => [0, 1],
        'thuocIdx'   => [[0,1,'Thoa 1 lần/tối'],[2,2,'Thoa 2 lần/ngày'],[5,20,'Uống 1 viên/ngày x 20 ngày']],
    ],
    [
        'TrieuChung' => 'Nám vùng gò má hai bên, xuất hiện sau sinh 6 tháng',
        'ChanDoan'   => 'Melasma - Nám da hỗn hợp Fitzpatrick IV',
        'TrangThai'  => 1,
        'GhiChu'     => 'Liệu trình 3 buổi laser, cách nhau 4 tuần',
        'NgayOffset' => -20,
        'dichVuIdx'  => [0, 2],
        'thuocIdx'   => [[0,1,'Thoa 1 lần/tối vùng nám'],[7,1,'Thoa sáng trước ra nắng']],
    ],
    [
        'TrieuChung' => 'Ngứa đỏ vùng khuỷu tay và đầu gối, tái phát theo mùa',
        'ChanDoan'   => 'Atopic Dermatitis - Viêm da cơ địa giai đoạn bán cấp',
        'TrangThai'  => 1,
        'GhiChu'     => 'Tránh tiếp xúc chất tẩy rửa mạnh, dưỡng ẩm thường xuyên',
        'NgayOffset' => -10,
        'dichVuIdx'  => [0, 5],
        'thuocIdx'   => [[3,2,'Thoa 2 lần/ngày vùng tổn thương'],[4,30,'Uống 1 viên/tối']],
    ],
    [
        'TrieuChung' => 'Nhiều nốt sùi nhỏ vùng cổ và nách',
        'ChanDoan'   => 'Molluscum Contagiosum - U mềm lây',
        'TrangThai'  => 1,
        'GhiChu'     => 'Đã đốt điện 12 nốt, tái khám sau 1 tuần',
        'NgayOffset' => -5,
        'dichVuIdx'  => [0, 6],
        'thuocIdx'   => [[2,1,'Thoa vùng đốt 2 lần/ngày x 5 ngày']],
    ],
    [
        'TrieuChung' => 'Da xỉn màu, lỗ chân lông to vùng mũi và má',
        'ChanDoan'   => 'Tư vấn chăm sóc da - Peel + chăm sóc chuyên sâu',
        'TrangThai'  => 0,
        'GhiChu'     => 'Liệu trình 4 buổi, cách nhau 2 tuần',
        'NgayOffset' => -2,
        'dichVuIdx'  => [0, 3, 7],
        'thuocIdx'   => [[1,1,'Thoa 1 lần/tối'],[7,1,'Thoa sáng hàng ngày']],
    ],
];

// ─── Xử lý tạo dữ liệu ─────────────────────────────────────
if ($_SERVER['REQUEST_METHOD'] === 'POST' && isset($_POST['action'])) {
    $action = $_POST['action'];

    if ($action === 'seed' && $benhNhan) {
        try {
            $pdo->beginTransaction();

            // 1) Đảm bảo có bác sĩ (NguoiDung với vai trò bác sĩ = MaVaiTro 2)
            $doctor = $pdo->query("SELECT TOP 1 MaNguoiDung FROM NguoiDung WHERE MaVaiTro = 2")->fetch(PDO::FETCH_ASSOC);
            if (!$doctor) {
                // Tạo bác sĩ mẫu — commit tạm để tránh FK lỗi khi INSERT PhieuKham
                $pdo->commit();
                $stmtDoc = $pdo->prepare(
                    'INSERT INTO NguoiDung (HoTen, SoDienThoai, Email, TenDangNhap, MatKhau, MaVaiTro, TrangThaiTK)
                     VALUES (N\'BS. Nguyễn Thị Thanh Hương\', \'0901234567\', \'huong.bs@darmaclinic.vn\', \'bshuong\',
                             \'$2y$10$p6N.Q6L7Q0a5K8M9d2Z7.OxXz9e2r5q8k7j3n4m5l6b2c9d8e7f6g5\', 2, 1)'
                );
                $stmtDoc->execute();
                $doctor = $pdo->query("SELECT TOP 1 MaNguoiDung FROM NguoiDung WHERE MaVaiTro = 2")->fetch(PDO::FETCH_ASSOC);
                $doctorId = (int)$doctor['MaNguoiDung'];
                $pdo->beginTransaction();
            } else {
                $doctorId = (int)$doctor['MaNguoiDung'];
            }

            // 2) Seed DichVu nếu chưa có
            foreach ($sampleDichVu as $dv) {
                $check = $pdo->prepare("SELECT MaDichVu FROM DichVu WHERE TenDichVu = ?");
                $check->execute([$dv['TenDichVu']]);
                if (!$check->fetch()) {
                    $ins = $pdo->prepare("INSERT INTO DichVu (TenDichVu, DonGia) VALUES (?, ?)");
                    $ins->execute([$dv['TenDichVu'], $dv['DonGia']]);
                }
            }
            $dichVuRows = $pdo->query("SELECT MaDichVu, TenDichVu, DonGia FROM DichVu ORDER BY MaDichVu ASC")->fetchAll(PDO::FETCH_ASSOC);

            // 3) Seed Thuoc nếu chưa có
            foreach ($sampleThuoc as $t) {
                $check = $pdo->prepare("SELECT MaThuoc FROM Thuoc WHERE TenThuoc = ?");
                $check->execute([$t['TenThuoc']]);
                if (!$check->fetch()) {
                    $ins = $pdo->prepare("INSERT INTO Thuoc (TenThuoc, DonViTinh, DonGia, SoLuongTon) VALUES (?, ?, ?, ?)");
                    $ins->execute([$t['TenThuoc'], $t['DonViTinh'], $t['DonGia'], $t['SoLuongTon']]);
                }
            }
            $thuocRows = $pdo->query("SELECT MaThuoc, TenThuoc, DonGia FROM Thuoc ORDER BY MaThuoc ASC")->fetchAll(PDO::FETCH_ASSOC);

            $dichVuMap = array_values($dichVuRows);
            $thuocMap  = array_values($thuocRows);

            $maBN = (int)$benhNhan['MaBenhNhan'];
            $soPhieu = 0;

            // 4) Tạo PhieuKham + chi tiết
            foreach ($samplePhieuKham as $pk) {
                $ngayKham = date('Y-m-d H:i:s', strtotime($pk['NgayOffset'] . ' days'));

                $stmtPK = $pdo->prepare("INSERT INTO PhieuKham (MaBenhNhan, MaNguoiDung, NgayKham, TrieuChung, ChanDoan, TrangThai, GhiChu, IsDeleted) VALUES (?, ?, ?, ?, ?, ?, ?, 0)");
                $stmtPK->execute([$maBN, $doctorId, $ngayKham, $pk['TrieuChung'], $pk['ChanDoan'], $pk['TrangThai'], $pk['GhiChu']]);
                $maPhieuKham = (int)$pdo->lastInsertId();

                // Chi tiết dịch vụ
                $tongDichVu = 0;
                foreach ($pk['dichVuIdx'] as $dvIdx) {
                    if (isset($dichVuMap[$dvIdx])) {
                        $dv = $dichVuMap[$dvIdx];
                        $thanhTien = (float)$dv['DonGia'];
                        $tongDichVu += $thanhTien;
                        $stmtDV = $pdo->prepare("INSERT INTO ChiTietDichVu (MaPhieuKham, MaDichVu, SoLuong, ThanhTien) VALUES (?, ?, 1, ?)");
                        $stmtDV->execute([$maPhieuKham, $dv['MaDichVu'], $thanhTien]);
                    }
                }

                // Chi tiết đơn thuốc
                $tongThuoc = 0;
                foreach ($pk['thuocIdx'] as [$tIdx, $soLuong, $lieuDung]) {
                    if (isset($thuocMap[$tIdx])) {
                        $t = $thuocMap[$tIdx];
                        $tongThuoc += (float)$t['DonGia'] * $soLuong;
                        $stmtT = $pdo->prepare("INSERT INTO ChiTietDonThuoc (MaPhieuKham, MaThuoc, SoLuong, LieuDung) VALUES (?, ?, ?, ?)");
                        $stmtT->execute([$maPhieuKham, $t['MaThuoc'], $soLuong, $lieuDung]);
                    }
                }

                // Hóa đơn (chỉ tạo cho phiếu đã hoàn thành)
                if ($pk['TrangThai'] === 1) {
                    $tongTien = $tongDichVu + $tongThuoc;
                    $stmtHD = $pdo->prepare("INSERT INTO HoaDon (MaPhieuKham, TongTien, TongTienDichVu, TongThuoc, GiamGia, TienKhachTra, TienThua, PhuongThucThanhToan, NgayThanhToan, TrangThai) VALUES (?, ?, ?, ?, 0, ?, 0, N'Tiền mặt', ?, 1)");
                    $stmtHD->execute([$maPhieuKham, $tongTien, $tongDichVu, $tongThuoc, $tongTien, $ngayKham]);
                }

                $soPhieu++;
            }

            $pdo->commit();
            $message = "Đã tạo thành công $soPhieu phiếu khám mẫu với đầy đủ dịch vụ, đơn thuốc và hóa đơn!";
            $messageType = 'success';
        } catch (Exception $e) {
            if ($pdo->inTransaction()) $pdo->rollBack();
            $message = "Lỗi: " . $e->getMessage();
            $messageType = 'danger';
        }
    }

    if ($action === 'clear' && $benhNhan) {
        try {
            $maBN = (int)$benhNhan['MaBenhNhan'];
            $pdo->exec("DELETE hd FROM HoaDon hd INNER JOIN PhieuKham pk ON hd.MaPhieuKham = pk.MaPhieuKham WHERE pk.MaBenhNhan = $maBN");
            $pdo->exec("DELETE ct FROM ChiTietDonThuoc ct INNER JOIN PhieuKham pk ON ct.MaPhieuKham = pk.MaPhieuKham WHERE pk.MaBenhNhan = $maBN");
            $pdo->exec("DELETE ct FROM ChiTietDichVu ct INNER JOIN PhieuKham pk ON ct.MaPhieuKham = pk.MaPhieuKham WHERE pk.MaBenhNhan = $maBN");
            $pdo->exec("DELETE ha FROM HinhAnhBenhLy ha INNER JOIN PhieuKham pk ON ha.MaPhieuKham = pk.MaPhieuKham WHERE pk.MaBenhNhan = $maBN");
            $pdo->exec("DELETE FROM PhieuKham WHERE MaBenhNhan = $maBN");
            $message = "Đã xóa toàn bộ phiếu khám mẫu!";
            $messageType = 'warning';
        } catch (Exception $e) {
            $message = "Lỗi xóa: " . $e->getMessage();
            $messageType = 'danger';
        }
    }
}

// ─── Lấy phiếu khám hiện tại ────────────────────────────────
$phieuKhams = [];
if ($benhNhan) {
    $stmt = $pdo->prepare(
        "SELECT pk.*, nd.HoTen AS TenBacSi
         FROM PhieuKham pk
         LEFT JOIN NguoiDung nd ON pk.MaNguoiDung = nd.MaNguoiDung
         WHERE pk.MaBenhNhan = ? AND pk.IsDeleted = 0
         ORDER BY pk.NgayKham DESC"
    );
    $stmt->execute([$benhNhan['MaBenhNhan']]);
    $phieuKhams = $stmt->fetchAll(PDO::FETCH_ASSOC);
}

$pkStatusMap = [
    0 => ['label' => 'Chờ xử lý',     'class' => 'warning'],
    1 => ['label' => 'Đã hoàn thành',  'class' => 'success'],
    2 => ['label' => 'Đã hủy',         'class' => 'secondary'],
];
?>
<!DOCTYPE html>
<html lang="vi">
<head>
    <meta charset="UTF-8">
    <title>Test Kết Quả Khám Bệnh</title>
    <link href="public/assets/vendor/bootstrap/css/bootstrap.min.css" rel="stylesheet">
    <link rel="stylesheet" href="public/assets/vendor/bootstrap-icons/font/bootstrap-icons.css">
</head>
<body class="bg-light">
<div class="container py-5" style="max-width:900px">
    <h2 class="mb-2"><i class="bi bi-clipboard2-pulse text-primary me-2"></i>Test Kết Quả Khám Bệnh</h2>
    <p class="text-muted mb-4">Giả lập dữ liệu PhieuKham được đẩy từ Windows App → hiển thị trên Website Profile</p>

    <?php if (!$sessionUser): ?>
    <div class="alert alert-warning"><i class="bi bi-exclamation-triangle me-2"></i>Bạn chưa đăng nhập. Hãy đăng nhập trước.</div>
    <?php elseif (!$benhNhan): ?>
    <div class="alert alert-warning"><i class="bi bi-exclamation-triangle me-2"></i>Không tìm thấy hồ sơ BenhNhan cho SĐT: <?= htmlspecialchars($phone, ENT_QUOTES, 'UTF-8') ?>. Hãy đặt lịch khám trước để tạo hồ sơ.</div>
    <?php else: ?>

    <?php if ($message): ?>
    <div class="alert alert-<?= $messageType ?> alert-dismissible fade show">
        <?= htmlspecialchars($message, ENT_QUOTES, 'UTF-8') ?>
        <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
    </div>
    <?php endif; ?>

    <div class="card shadow-sm mb-4">
        <div class="card-body">
            <div class="row align-items-center">
                <div class="col-md-6">
                    <p class="mb-1"><strong>Bệnh nhân:</strong> <?= htmlspecialchars($benhNhan['HoTen'] ?? '', ENT_QUOTES, 'UTF-8') ?></p>
                    <p class="mb-1"><strong>SĐT:</strong> <?= htmlspecialchars($phone, ENT_QUOTES, 'UTF-8') ?></p>
                    <p class="mb-0"><strong>Số phiếu khám:</strong> <?= count($phieuKhams) ?></p>
                </div>
                <div class="col-md-6 text-md-end mt-3 mt-md-0">
                    <form method="POST" class="d-inline">
                        <input type="hidden" name="action" value="seed">
                        <button type="submit" class="btn btn-primary rounded-pill px-4 me-2" <?= count($phieuKhams) > 0 ? 'disabled title="Đã có dữ liệu, xóa trước rồi tạo lại"' : '' ?>>
                            <i class="bi bi-database-add me-1"></i>Tạo 5 phiếu khám mẫu
                        </button>
                    </form>
                    <form method="POST" class="d-inline">
                        <input type="hidden" name="action" value="clear">
                        <button type="submit" class="btn btn-outline-danger rounded-pill px-4" <?= count($phieuKhams) === 0 ? 'disabled' : '' ?>
                                onclick="return confirm('Xóa toàn bộ phiếu khám mẫu?')">
                            <i class="bi bi-trash me-1"></i>Xóa tất cả
                        </button>
                    </form>
                </div>
            </div>
        </div>
    </div>

    <?php if (!empty($phieuKhams)): ?>
    <h5 class="fw-bold mb-3">Danh sách phiếu khám (<?= count($phieuKhams) ?>)</h5>

    <?php foreach ($phieuKhams as $pk):
        $maPhieuKham = (int)$pk['MaPhieuKham'];
        $tt = (int)($pk['TrangThai'] ?? 0);
        $badge = $pkStatusMap[$tt] ?? ['label' => 'Không rõ', 'class' => 'dark'];
        $ngayKham = new DateTime($pk['NgayKham']);

        $stDV = $pdo->prepare("SELECT ctdv.*, dv.TenDichVu FROM ChiTietDichVu ctdv JOIN DichVu dv ON ctdv.MaDichVu = dv.MaDichVu WHERE ctdv.MaPhieuKham = ?");
        $stDV->execute([$maPhieuKham]);
        $dichVus = $stDV->fetchAll(PDO::FETCH_ASSOC);

        $stT = $pdo->prepare("SELECT ctdt.*, t.TenThuoc, t.DonViTinh, t.DonGia FROM ChiTietDonThuoc ctdt JOIN Thuoc t ON ctdt.MaThuoc = t.MaThuoc WHERE ctdt.MaPhieuKham = ?");
        $stT->execute([$maPhieuKham]);
        $thuocs = $stT->fetchAll(PDO::FETCH_ASSOC);

        $stHD = $pdo->prepare("SELECT * FROM HoaDon WHERE MaPhieuKham = ?");
        $stHD->execute([$maPhieuKham]);
        $hoaDon = $stHD->fetch(PDO::FETCH_ASSOC);
    ?>
    <div class="card shadow-sm mb-3">
        <div class="card-header bg-white d-flex justify-content-between align-items-center py-3">
            <div>
                <strong class="text-primary">Phiếu #<?= $maPhieuKham ?></strong>
                <span class="text-muted ms-3"><i class="bi bi-calendar3 me-1"></i><?= $ngayKham->format('d/m/Y') ?></span>
                <?php if (!empty($pk['TenBacSi'])): ?>
                <span class="text-muted ms-3"><i class="bi bi-person-badge me-1"></i><?= htmlspecialchars($pk['TenBacSi'], ENT_QUOTES, 'UTF-8') ?></span>
                <?php endif; ?>
            </div>
            <span class="badge bg-<?= $badge['class'] ?>"><?= $badge['label'] ?></span>
        </div>
        <div class="card-body">
            <div class="row mb-3">
                <div class="col-md-6">
                    <p class="mb-1"><small class="text-muted">Triệu chứng:</small></p>
                    <p class="fw-medium"><?= htmlspecialchars($pk['TrieuChung'] ?? '', ENT_QUOTES, 'UTF-8') ?></p>
                </div>
                <div class="col-md-6">
                    <p class="mb-1"><small class="text-muted">Chẩn đoán:</small></p>
                    <p class="fw-bold text-primary"><?= htmlspecialchars($pk['ChanDoan'] ?? '', ENT_QUOTES, 'UTF-8') ?></p>
                </div>
            </div>
            <?php if (!empty($pk['GhiChu'])): ?>
            <p class="mb-3"><small class="text-muted">Ghi chú:</small> <em><?= htmlspecialchars($pk['GhiChu'], ENT_QUOTES, 'UTF-8') ?></em></p>
            <?php endif; ?>

            <div class="row g-3">
                <?php if (!empty($dichVus)): ?>
                <div class="col-md-6">
                    <h6 class="fw-bold"><i class="bi bi-heart-pulse text-danger me-1"></i>Dịch vụ</h6>
                    <table class="table table-sm table-bordered mb-0">
                        <thead class="table-light"><tr><th>Dịch vụ</th><th class="text-end">Thành tiền</th></tr></thead>
                        <tbody>
                        <?php foreach ($dichVus as $dv): ?>
                        <tr>
                            <td><?= htmlspecialchars($dv['TenDichVu'], ENT_QUOTES, 'UTF-8') ?></td>
                            <td class="text-end"><?= number_format((float)$dv['ThanhTien']) ?>đ</td>
                        </tr>
                        <?php endforeach; ?>
                        </tbody>
                    </table>
                </div>
                <?php endif; ?>

                <?php if (!empty($thuocs)): ?>
                <div class="col-md-6">
                    <h6 class="fw-bold"><i class="bi bi-capsule text-success me-1"></i>Đơn thuốc</h6>
                    <table class="table table-sm table-bordered mb-0">
                        <thead class="table-light"><tr><th>Thuốc</th><th>SL</th><th>Liều dùng</th></tr></thead>
                        <tbody>
                        <?php foreach ($thuocs as $t): ?>
                        <tr>
                            <td><?= htmlspecialchars($t['TenThuoc'], ENT_QUOTES, 'UTF-8') ?></td>
                            <td class="text-center"><?= (int)$t['SoLuong'] ?> <?= htmlspecialchars($t['DonViTinh'], ENT_QUOTES, 'UTF-8') ?></td>
                            <td><small><?= htmlspecialchars($t['LieuDung'] ?? '', ENT_QUOTES, 'UTF-8') ?></small></td>
                        </tr>
                        <?php endforeach; ?>
                        </tbody>
                    </table>
                </div>
                <?php endif; ?>
            </div>

            <?php if ($hoaDon): ?>
            <div class="mt-3 p-3 bg-light rounded-3">
                <div class="d-flex justify-content-between align-items-center flex-wrap gap-2">
                    <div>
                        <small class="text-muted">Hóa đơn #<?= (int)$hoaDon['MaHoaDon'] ?></small>
                        <span class="badge bg-<?= $hoaDon['TrangThai'] ? 'success' : 'warning' ?> ms-2"><?= $hoaDon['TrangThai'] ? 'Đã thanh toán' : 'Chưa thanh toán' ?></span>
                    </div>
                    <div class="text-end">
                        <small class="text-muted">DV: <?= number_format((float)$hoaDon['TongTienDichVu']) ?>đ + Thuốc: <?= number_format((float)$hoaDon['TongThuoc']) ?>đ</small>
                        <strong class="d-block text-primary fs-5"><?= number_format((float)$hoaDon['TongTien']) ?>đ</strong>
                    </div>
                </div>
            </div>
            <?php endif; ?>
        </div>
    </div>
    <?php endforeach; ?>

    <div class="text-center mt-4">
        <a href="index.php?route=profile" class="btn btn-primary rounded-pill px-5" target="_blank">
            <i class="bi bi-person-vcard me-2"></i>Xem trang Profile → Lịch Sử Khám
        </a>
    </div>
    <?php endif; ?>

    <?php endif; ?>
</div>
<script src="public/assets/vendor/bootstrap/js/bootstrap.bundle.min.js"></script>
</body>
</html>
