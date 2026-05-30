-- =========================================================
-- 1. XÓA CÁC BẢNG CŨ (DROP TABLES)
-- =========================================================
IF OBJECT_ID('ChiTietHoaDon', 'U') IS NOT NULL DROP TABLE ChiTietHoaDon;
IF OBJECT_ID('HoaDon', 'U') IS NOT NULL DROP TABLE HoaDon;
IF OBJECT_ID('SanPham', 'U') IS NOT NULL DROP TABLE SanPham;
IF OBJECT_ID('KhachHang', 'U') IS NOT NULL DROP TABLE KhachHang;
IF OBJECT_ID('NhanVien', 'U') IS NOT NULL DROP TABLE NhanVien;
IF OBJECT_ID('NhaCungCap', 'U') IS NOT NULL DROP TABLE NhaCungCap;
IF OBJECT_ID('TaiKhoan', 'U') IS NOT NULL DROP TABLE TaiKhoan;

-- =========================================================
-- 2. TẠO LẠI CÁC BẢNG MỚI (CREATE TABLES)
-- =========================================================

-- Bảng Tài Khoản
CREATE TABLE TaiKhoan (
    TenDangNhap VARCHAR(50) PRIMARY KEY,
    MatKhau VARCHAR(255) NOT NULL,
    Quyen NVARCHAR(50) DEFAULT N'Nhân viên',
    MaNV VARCHAR(20) -- Lưu mã nhân viên, KHÔNG dùng khóa ngoại để tự do tháo gỡ
);

-- Bảng Nhân Viên
CREATE TABLE NhanVien (
    MaNV VARCHAR(20) PRIMARY KEY,
    TenNV NVARCHAR(100) NOT NULL,
    GioiTinh NVARCHAR(10),
    NgaySinh DATE,
    SoDienThoai VARCHAR(15),
    DiaChi NVARCHAR(200),
    TrangThai INT DEFAULT 1 -- 1: Đang làm, 0: Đã nghỉ (Xóa mềm)
);

-- Bảng Khách Hàng
CREATE TABLE KhachHang (
    MaKH VARCHAR(20) PRIMARY KEY,  
    TenKH NVARCHAR(100) NOT NULL,
    SoDienThoai VARCHAR(15),
    DiaChi NVARCHAR(200),
    TrangThai INT DEFAULT 1 -- 1: Đang giao dịch, 0: Ngừng giao dịch
);

-- Bảng Nhà Cung Cấp
CREATE TABLE NhaCungCap (
    MaNCC VARCHAR(20) PRIMARY KEY, 
    TenNCC NVARCHAR(100) NOT NULL,
    SoDienThoai VARCHAR(15),
    DiaChi NVARCHAR(200),
    TrangThai INT DEFAULT 1 -- 1: Đang giao dịch, 0: Ngừng giao dịch
);

-- Bảng Sản Phẩm
CREATE TABLE SanPham (
    MaSP VARCHAR(20) PRIMARY KEY,  
    TenSP NVARCHAR(100) NOT NULL,
    MaNCC VARCHAR(20),
    GiaBan DECIMAL(18, 2),
    SoLuongTon INT DEFAULT 0,
    TrangThai INT DEFAULT 1, -- 1: Đang bán, 0: Ngừng kinh doanh
    FOREIGN KEY (MaNCC) REFERENCES NhaCungCap(MaNCC)
);

-- Bảng Hóa Đơn
CREATE TABLE HoaDon (
    MaHD VARCHAR(20) PRIMARY KEY,  
    MaNV VARCHAR(20),
    MaKH VARCHAR(20),
    NgayLap DATETIME DEFAULT GETDATE(),
    TongTien DECIMAL(18, 2) DEFAULT 0,
    FOREIGN KEY (MaNV) REFERENCES NhanVien(MaNV),
    FOREIGN KEY (MaKH) REFERENCES KhachHang(MaKH)
);

-- Bảng Chi Tiết Hóa Đơn
CREATE TABLE ChiTietHoaDon (
    MaHD VARCHAR(20),
    MaSP VARCHAR(20),
    SoLuong INT NOT NULL,
    DonGia DECIMAL(18, 2),
    ThanhTien DECIMAL(18, 2),
    PRIMARY KEY (MaHD, MaSP),
    FOREIGN KEY (MaHD) REFERENCES HoaDon(MaHD),
    FOREIGN KEY (MaSP) REFERENCES SanPham(MaSP)
);

-- =========================================================
-- 3. CHÈN DỮ LIỆU MẪU (INSERT SAMPLE DATA)
-- =========================================================

-- 3.1. Bảng Nhân Viên (phải có trước để lấy MaNV gán vào TaiKhoan)
INSERT INTO NhanVien (MaNV, TenNV, GioiTinh, NgaySinh, SoDienThoai, DiaChi, TrangThai) VALUES
('NV001', N'Nguyễn Văn An', N'Nam', '1985-06-15', '0901234567', N'Hà Nội', 1),
('NV002', N'Trần Thị Bình', N'Nữ', '1990-08-20', '0987654321', N'Hải Phòng', 1),
('NV003', N'Lê Văn Cường', N'Nam', '1995-12-10', '0912345678', N'Đà Nẵng', 1),
('NV004', N'Phạm Thị Dung', N'Nữ', '1988-03-25', '0977778888', N'TP.HCM', 1);

-- 3.2. Bảng Tài Khoản (MaNV đã có trong NhanVien)
INSERT INTO TaiKhoan (TenDangNhap, MatKhau, Quyen, MaNV) VALUES
('admin', '123', N'Admin', 'NV001'),
('nv002', 'pass123', N'Nhân viên', 'NV002'),
('nv003', 'pass456', N'Nhân viên', 'NV003'),
('nv004', 'pass789', N'Nhân viên', 'NV004');

-- 3.3. Bảng Khách Hàng
INSERT INTO KhachHang (MaKH, TenKH, SoDienThoai, DiaChi, TrangThai) VALUES
('KH001', N'Công ty TNHH ABC', '0241234567', N'Hà Nội', 1),
('KH002', N'Bà Nguyễn Thị Hoa', '0908889999', N'Hải Phòng', 1),
('KH003', N'Ông Phạm Văn Dũng', '0911112222', N'TP.HCM', 1),
('KH004', N'Cửa hàng tiện lợi X', '0283334444', N'Bình Dương', 1),
('KH005', N'Siêu thị Mini Y', '0245556666', N'Hà Nội', 1);

-- 3.4. Bảng Nhà Cung Cấp
INSERT INTO NhaCungCap (MaNCC, TenNCC, SoDienThoai, DiaChi, TrangThai) VALUES
('NCC01', N'Công ty Thực phẩm sạch', '0245556666', N'Hà Nội', 1),
('NCC02', N'Công ty Đồ uống Việt', '0287778888', N'TP.HCM', 1),
('NCC03', N'Công ty Bánh kẹo Hương Việt', '0229990000', N'Hải Phòng', 1);

-- 3.5. Bảng Sản Phẩm
INSERT INTO SanPham (MaSP, TenSP, MaNCC, GiaBan, SoLuongTon, TrangThai) VALUES
('SP001', N'Gạo ST25 5kg', 'NCC01', 120000, 100, 1),
('SP002', N'Sữa tươi TH 1L', 'NCC02', 32000, 200, 1),
('SP003', N'Bánh quy Cosy 200g', 'NCC03', 25000, 150, 1),
('SP004', N'Nước suối Aquafina 500ml', 'NCC02', 8000, 500, 1),
('SP005', N'Mì tôm Hảo Hảo 30 gói', 'NCC01', 95000, 80, 1),
('SP006', N'Trứng gà ta 10 quả', 'NCC01', 35000, 60, 1),
('SP007', N'Kẹo mút Chupa Chups', 'NCC03', 5000, 300, 1),
('SP008', N'Dầu ăn Simply 1L', 'NCC01', 45000, 40, 1),
('SP009', N'Nước ngọt Coca 330ml', 'NCC02', 10000, 250, 1);

-- 3.6. Bảng Hóa Đơn
INSERT INTO HoaDon (MaHD, MaNV, MaKH, NgayLap, TongTien) VALUES
('HD001', 'NV001', 'KH001', '2025-05-28 09:30:00', 0),
('HD002', 'NV002', 'KH002', '2025-05-28 14:15:00', 0),
('HD003', 'NV003', 'KH003', '2025-05-29 10:00:00', 0),
('HD004', 'NV001', 'KH001', '2025-05-29 16:20:00', 0),
('HD005', 'NV002', 'KH004', '2025-05-30 11:10:00', 0),
('HD006', 'NV004', 'KH005', '2025-05-31 08:45:00', 0);

-- 3.7. Bảng Chi Tiết Hóa Đơn (tính sẵn cột ThanhTien)
INSERT INTO ChiTietHoaDon (MaHD, MaSP, SoLuong, DonGia, ThanhTien) VALUES
-- HD001
('HD001', 'SP001', 2, 120000, 240000),
('HD001', 'SP003', 3, 25000, 75000),
-- HD002
('HD002', 'SP002', 5, 32000, 160000),
('HD002', 'SP004', 10, 8000, 80000),
('HD002', 'SP006', 2, 35000, 70000),
-- HD003
('HD003', 'SP005', 1, 95000, 95000),
('HD003', 'SP007', 20, 5000, 100000),
-- HD004
('HD004', 'SP001', 1, 120000, 120000),
('HD004', 'SP002', 3, 32000, 96000),
('HD004', 'SP004', 12, 8000, 96000),
-- HD005
('HD005', 'SP003', 2, 25000, 50000),
('HD005', 'SP006', 4, 35000, 140000),
-- HD006
('HD006', 'SP008', 2, 45000, 90000),
('HD006', 'SP009', 15, 10000, 150000);

-- 3.8. Cập nhật tổng tiền cho các hóa đơn (dựa vào ChiTietHoaDon)
UPDATE HoaDon SET TongTien = (
    SELECT SUM(ThanhTien) 
    FROM ChiTietHoaDon 
    WHERE ChiTietHoaDon.MaHD = HoaDon.MaHD
)
WHERE MaHD IN ('HD001','HD002','HD003','HD004','HD005','HD006');

-- 3.9. Cập nhật lại số lượng tồn kho sau khi bán (giảm tồn theo chi tiết)
UPDATE SanPham 
SET SoLuongTon = SoLuongTon - (
    SELECT ISNULL(SUM(SoLuong), 0)
    FROM ChiTietHoaDon
    WHERE ChiTietHoaDon.MaSP = SanPham.MaSP
)
WHERE MaSP IN (SELECT DISTINCT MaSP FROM ChiTietHoaDon);