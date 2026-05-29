CREATE DATABASE QuanLyBanHang;
GO

USE QuanLyBanHang;
GO

-- 1. Bảng Tài Khoản (Dùng cho Đăng nhập, Đăng ký)
CREATE TABLE TaiKhoan (
    TenDangNhap VARCHAR(50) PRIMARY KEY,
    MatKhau VARCHAR(50) NOT NULL, -- Không hash theo yêu cầu
    Quyen NVARCHAR(20) DEFAULT N'Nhân viên' -- 'Quản lý' hoặc 'Nhân viên'
);
GO

-- 2. Bảng Khách Hàng
CREATE TABLE KhachHang (
    MaKH INT IDENTITY(1,1) PRIMARY KEY,
    TenKH NVARCHAR(100) NOT NULL,
    SoDienThoai VARCHAR(15),
    DiaChi NVARCHAR(200)
);
GO

-- 3. Bảng Nhân Viên
CREATE TABLE NhanVien (
    MaNV INT IDENTITY(1,1) PRIMARY KEY,
    TenNV NVARCHAR(100) NOT NULL,
    TenDangNhap VARCHAR(50) FOREIGN KEY REFERENCES TaiKhoan(TenDangNhap),
    SoDienThoai VARCHAR(15),
    DiaChi NVARCHAR(200)
);
GO

-- 4. Bảng Nhà Cung Cấp
CREATE TABLE NhaCungCap (
    MaNCC INT IDENTITY(1,1) PRIMARY KEY,
    TenNCC NVARCHAR(100) NOT NULL,
    SoDienThoai VARCHAR(15),
    DiaChi NVARCHAR(200)
);
GO

-- 5. Bảng Sản Phẩm
CREATE TABLE SanPham (
    MaSP INT IDENTITY(1,1) PRIMARY KEY,
    TenSP NVARCHAR(100) NOT NULL,
    MaNCC INT FOREIGN KEY REFERENCES NhaCungCap(MaNCC),
    GiaNhap DECIMAL(18, 2) NOT NULL,
    GiaBan DECIMAL(18, 2) NOT NULL,
    SoLuongTon INT DEFAULT 0
);
GO

-- 6. Bảng Hóa Đơn (Phiếu xuất hàng / Bán hàng)
CREATE TABLE HoaDon (
    MaHD INT IDENTITY(1,1) PRIMARY KEY,
    MaKH INT FOREIGN KEY REFERENCES KhachHang(MaKH),
    MaNV INT FOREIGN KEY REFERENCES NhanVien(MaNV),
    NgayLap DATETIME DEFAULT GETDATE(),
    TongTien DECIMAL(18, 2) DEFAULT 0
);
GO

-- 7. Bảng Chi Tiết Hóa Đơn
CREATE TABLE ChiTietHoaDon (
    MaHD INT FOREIGN KEY REFERENCES HoaDon(MaHD),
    MaSP INT FOREIGN KEY REFERENCES SanPham(MaSP),
    SoLuong INT NOT NULL,
    DonGia DECIMAL(18, 2) NOT NULL,
    ThanhTien AS (SoLuong * DonGia), -- Cột tự tính
    PRIMARY KEY (MaHD, MaSP)
);
GO

-- THÊM DỮ LIỆU MẪU ĐỂ TEST
-- Thêm tài khoản
INSERT INTO TaiKhoan (TenDangNhap, MatKhau, Quyen) VALUES 
('admin', '123456', N'Quản lý'),
('nhanvien1', '123456', N'Nhân viên');

-- Thêm nhân viên
INSERT INTO NhanVien (TenNV, TenDangNhap, SoDienThoai, DiaChi) VALUES
(N'Nguyễn Văn Quản Lý', 'admin', '0987654321', N'Hà Nội'),
(N'Trần Thị Bán Hàng', 'nhanvien1', '0123456789', N'Hà Nội');

-- Thêm khách hàng
INSERT INTO KhachHang (TenKH, SoDienThoai, DiaChi) VALUES
(N'Lê Văn Khách', '0911222333', N'Hải Phòng');

-- Thêm nhà cung cấp
INSERT INTO NhaCungCap (TenNCC, SoDienThoai, DiaChi) VALUES
(N'Công ty TNHH ABC', '0999888777', N'Hồ Chí Minh');

-- Thêm sản phẩm
INSERT INTO SanPham (TenSP, MaNCC, GiaNhap, GiaBan, SoLuongTon) VALUES
(N'Laptop Dell XPS', 1, 20000000, 25000000, 10),
(N'Chuột Logitech', 1, 200000, 350000, 50);

-- Thêm hóa đơn
INSERT INTO HoaDon (MaKH, MaNV, NgayLap, TongTien) VALUES
(1, 2, GETDATE(), 25350000);

-- Thêm chi tiết hóa đơn
INSERT INTO ChiTietHoaDon (MaHD, MaSP, SoLuong, DonGia) VALUES
(1, 1, 1, 25000000),
(1, 2, 1, 350000);
GO
