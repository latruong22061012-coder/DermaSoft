# 💊 Cập Nhật Form Kê Đơn Thuốc - PhieuKhamForm

## Mô tả thay đổi

Thêm **hiển thị giá thuốc** và **chức năng xuất đơn thuốc** vào PhieuKhamForm.

---

## ✅ Các tính năng cần thêm

### 1. **Hiển thị giá thuốc trong DataGridView**

**Hiện tại:** `dgvDonThuoc` chỉ hiện:
- Tên thuốc
- Số lượng
- Liều dùng
- Cách dùng

**Cần thêm:**
- **Đơn giá** (VD: 15,000đ/viên)
- **Thành tiền** = Số lượng × Đơn giá

**Cột mới trong dgvDonThuoc:**
```csharp
dgvDonThuoc.Columns.Add(new DataGridViewTextBoxColumn 
{ 
    Name = "colDonGia", 
    HeaderText = "Đơn giá", 
    FillWeight = 15F,
    DefaultCellStyle = new DataGridViewCellStyle 
    { 
        Format = "N0", 
        Alignment = DataGridViewContentAlignment.MiddleRight 
    }
});

dgvDonThuoc.Columns.Add(new DataGridViewTextBoxColumn 
{ 
    Name = "colThanhTien", 
    HeaderText = "Thành tiền", 
    FillWeight = 18F,
    ReadOnly = true, // Tính tự động
    DefaultCellStyle = new DataGridViewCellStyle 
    { 
        Format = "N0", 
        Alignment = DataGridViewContentAlignment.MiddleRight,
        Font = new Font("Segoe UI", 9F, FontStyle.Bold),
        ForeColor = Color.FromArgb(184, 138, 40) // Gold
    }
});
```

### 2. **Tự động tính thành tiền khi nhập số lượng**

**Event:** `dgvDonThuoc.CellEndEdit`

```csharp
private void DgvDonThuoc_CellEndEdit(object sender, DataGridViewCellEventArgs e)
{
    if (e.RowIndex < 0) return;

    string colName = dgvDonThuoc.Columns[e.ColumnIndex].Name;

    // Khi sửa số lượng → tính lại thành tiền
    if (colName == "colSoLuong")
    {
        var row = dgvDonThuoc.Rows[e.RowIndex];

        if (int.TryParse(row.Cells["colSoLuong"].Value?.ToString(), out int soLuong) &&
            decimal.TryParse(row.Cells["colDonGia"].Value?.ToString(), out decimal donGia))
        {
            decimal thanhTien = soLuong * donGia;
            row.Cells["colThanhTien"].Value = thanhTien;
        }
    }
}
```

### 3. **Nút "📄 Xuất Đơn Thuốc"**

**Vị trí:** Bên cạnh nút "Hoàn Thành Khám" trong pnlActions

**Chức năng:**
1. Hiển thị dialog chọn định dạng xuất (Text hoặc PDF)
2. Tạo file đơn thuốc với thông tin:
   - Tên phòng khám + logo
   - Thông tin bệnh nhân
   - Thông tin bác sĩ
   - Danh sách thuốc + liều dùng
   - Tổng tiền thuốc
   - Lời dặn (nếu có)

---

## 🛠️ Cấu trúc code mới

### File: `PhieuKhamForm.cs`

#### 1. Thêm controls mới

```csharp
private Guna2Button btnXuatDonThuoc;
private Label lblTongTienThuoc; // Hiển thị tổng tiền thuốc phía dưới grid
```

#### 2. Khởi tạo nút Xuất Đơn

```csharp
private void TaoNutXuatDonThuoc()
{
    btnXuatDonThuoc = new Guna2Button
    {
        Text = "📄  Xuất Đơn Thuốc",
        Font = AppFonts.BodyBold,
        ForeColor = Color.White,
        FillColor = ColorTranslator.FromHtml("#0891B2"), // Cyan
        BorderRadius = 20,
        Size = new Size(180, 42),
        Cursor = Cursors.Hand,
        Enabled = false // Chỉ enable khi có thuốc trong đơn
    };
    btnXuatDonThuoc.Click += BtnXuatDonThuoc_Click;

    pnlActions.Controls.Add(btnXuatDonThuoc);
}
```

#### 3. Logic xuất đơn thuốc

```csharp
private void BtnXuatDonThuoc_Click(object sender, EventArgs e)
{
    if (dgvDonThuoc.Rows.Count == 0)
    {
        MessageBox.Show("Chưa có thuốc trong đơn.", "Thông báo",
            MessageBoxButtons.OK, MessageBoxIcon.Information);
        return;
    }

    // Dialog chọn định dạng
    using (var dlg = new Form
    {
        Text = "Xuất Đơn Thuốc",
        Size = new Size(400, 200),
        StartPosition = FormStartPosition.CenterParent,
        FormBorderStyle = FormBorderStyle.FixedDialog,
        MaximizeBox = false, MinimizeBox = false
    })
    {
        dlg.Controls.Add(new Label
        {
            Text = "Chọn định dạng xuất:",
            Location = new Point(20, 20),
            AutoSize = true
        });

        var btnText = new Button
        {
            Text = "📄 Text (.txt)",
            Location = new Point(20, 60),
            Size = new Size(160, 40)
        };
        btnText.Click += (s2, e2) =>
        {
            XuatDonThuocText();
            dlg.Close();
        };

        var btnPDF = new Button
        {
            Text = "📕 PDF (.pdf)",
            Location = new Point(200, 60),
            Size = new Size(160, 40)
        };
        btnPDF.Click += (s2, e2) =>
        {
            MessageBox.Show("Tính năng xuất PDF đang phát triển.",
                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            dlg.Close();
        };

        dlg.Controls.Add(btnText);
        dlg.Controls.Add(btnPDF);
        dlg.ShowDialog(this);
    }
}

private void XuatDonThuocText()
{
    using (var sfd = new SaveFileDialog())
    {
        sfd.Filter = "Text Files (*.txt)|*.txt";
        sfd.FileName = $"DonThuoc_PK{_maPhieuKham}_{DateTime.Now:yyyyMMdd_HHmm}.txt";
        sfd.Title = "Lưu Đơn Thuốc";

        if (sfd.ShowDialog() != DialogResult.OK) return;

        try
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("═══════════════════════════════════════════════════════");
            sb.AppendLine("           PHÒNG KHÁM DA LIỄU DERMASOFT");
            sb.AppendLine("        Địa chỉ: 123 Đường ABC, Quận XYZ, TP.HCM");
            sb.AppendLine("                 ☎  Hotline: 1900-xxxx");
            sb.AppendLine("═══════════════════════════════════════════════════════");
            sb.AppendLine();
            sb.AppendLine("                    ĐƠN THUỐC");
            sb.AppendLine();
            sb.AppendLine($"Ngày kê đơn: {DateTime.Now:dd/MM/yyyy HH:mm}");
            sb.AppendLine($"Bệnh nhân:   {lblBNHoTen.Text}");
            sb.AppendLine($"Bác sĩ:      {lblBacSiHoTen.Text}");
            sb.AppendLine($"Chẩn đoán:   {txtChanDoan.Text}");
            sb.AppendLine();
            sb.AppendLine("───────────────────────────────────────────────────────");
            sb.AppendLine(" STT | Tên thuốc          | SL  | Đơn giá   | Thành tiền");
            sb.AppendLine("───────────────────────────────────────────────────────");

            decimal tongTien = 0;
            int stt = 1;

            foreach (DataGridViewRow row in dgvDonThuoc.Rows)
            {
                if (row.IsNewRow) continue;

                string tenThuoc = row.Cells["colTenThuoc"].Value?.ToString() ?? "";
                int soLuong = Convert.ToInt32(row.Cells["colSoLuong"].Value ?? 0);
                decimal donGia = Convert.ToDecimal(row.Cells["colDonGia"].Value ?? 0);
                decimal thanhTien = Convert.ToDecimal(row.Cells["colThanhTien"].Value ?? 0);
                string lieuDung = row.Cells["colLieuDung"].Value?.ToString() ?? "";
                string cachDung = row.Cells["colCachDung"].Value?.ToString() ?? "";

                sb.AppendLine($" {stt,3} | {tenThuoc,-20} | {soLuong,3} | {donGia,9:N0} | {thanhTien,11:N0}");
                sb.AppendLine($"      Liều dùng: {lieuDung}");
                sb.AppendLine($"      Cách dùng: {cachDung}");
                sb.AppendLine();

                tongTien += thanhTien;
                stt++;
            }

            sb.AppendLine("───────────────────────────────────────────────────────");
            sb.AppendLine($"                         TỔNG CỘNG: {tongTien,11:N0} đ");
            sb.AppendLine("───────────────────────────────────────────────────────");
            sb.AppendLine();
            sb.AppendLine("LƯU Ý:");
            sb.AppendLine("- Uống thuốc đúng liều, đúng giờ");
            sb.AppendLine("- Tái khám khi có triệu chứng bất thường");
            sb.AppendLine();
            sb.AppendLine($"                         Bác sĩ khám");
            sb.AppendLine($"                       {lblBacSiHoTen.Text}");

            System.IO.File.WriteAllText(sfd.FileName, sb.ToString(), 
                new System.Text.UTF8Encoding(true));

            MessageBox.Show("Đã xuất đơn thuốc thành công!\n" + sfd.FileName,
                "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Mở file vừa tạo
            System.Diagnostics.Process.Start(sfd.FileName);
        }
        catch (Exception ex)
        {
            MessageBox.Show("Lỗi xuất file: " + ex.Message, "Lỗi",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
```

#### 4. Cập nhật tổng tiền thuốc

```csharp
private void CapNhatTongTienThuoc()
{
    decimal tongTien = 0;
    foreach (DataGridViewRow row in dgvDonThuoc.Rows)
    {
        if (row.IsNewRow) continue;
        tongTien += Convert.ToDecimal(row.Cells["colThanhTien"].Value ?? 0);
    }

    lblTongTienThuoc.Text = $"Tổng tiền thuốc: {tongTien:N0} đ";

    // Enable nút xuất đơn nếu có thuốc
    btnXuatDonThuoc.Enabled = dgvDonThuoc.Rows.Count > 0;
}
```

---

## 📝 Checklist triển khai

### Cơ sở dữ liệu
- [ ] Chạy migration `MIGRATION_SOTHUTU_LICHHEN.sql`
- [ ] Kiểm tra trigger `TRG_LichHen_GanSTT` hoạt động

### AppointmentForm
- [ ] Thêm cột `SoThuTu` vào query `LoadLichHen()`
- [ ] Hiển thị STT trong `dgvLichHen`

### PhieuKhamForm
- [ ] Thêm cột `colDonGia` và `colThanhTien` vào `dgvDonThuoc`
- [ ] Load giá thuốc từ bảng `Thuoc` khi chọn thuốc
- [ ] Tự động tính thành tiền khi nhập số lượng
- [ ] Thêm nút "📄 Xuất Đơn Thuốc"
- [ ] Implement `XuatDonThuocText()`
- [ ] Thêm label tổng tiền thuốc

---

## 🧪 Test Cases

### TC1: Số thứ tự lịch hẹn
**Bước:**
1. Tạo 3 lịch hẹn cho bác sĩ A: 8:00, 8:30, 9:00
2. Lễ tân xác nhận theo thứ tự: 9:00, 8:00, 8:30

**Kết quả:**
- Lịch 8:00 → STT = 1
- Lịch 8:30 → STT = 2
- Lịch 9:00 → STT = 3

### TC2: Giá thuốc và xuất đơn
**Bước:**
1. Bác sĩ kê 3 loại thuốc
2. Nhập số lượng: 10, 5, 20
3. Kiểm tra thành tiền tự động tính
4. Nhấn "Xuất Đơn Thuốc" → chọn Text

**Kết quả:**
- Thành tiền = SL × Đơn giá
- Tổng tiền thuốc đúng
- File .txt xuất ra đầy đủ thông tin

---

## 🎨 UI/UX Preview

### DataGridView Đơn Thuốc (sau khi cập nhật):

| Tên thuốc | SL | Đơn giá | Thành tiền | Liều dùng | Cách dùng | Xóa |
|-----------|----|---------:|----------:|-----------|-----------|-----|
| Vitamin C | 10 | 15,000 | **150,000** | 1 viên/lần | Sau ăn | ❌ |
| Aspirin   |  5 | 8,000  | **40,000**  | 2 viên/lần | Trước ăn | ❌ |

**Tổng tiền thuốc: 190,000 đ**

[📄 Xuất Đơn Thuốc] [💾 Lưu Nháp] [✅ Hoàn Thành Khám]

---

## 📚 Tham khảo

- `Thuoc.DonGia` → Lấy giá từ bảng `Thuoc`
- `InvoiceForm` → Tham khảo cách tính tổng tiền
- `HoaDonPrinter` → Tham khảo format in ấn

---

## ⚠️ Lưu ý

- Giá thuốc có thể thay đổi theo thời gian → cần lưu `DonGia` vào `ChiTietDonThuoc` khi kê đơn
- Xuất PDF yêu cầu thêm library `iTextSharp` hoặc `QuestPDF`
- Cần quyền ghi file vào thư mục người dùng chọn
