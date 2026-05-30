CREATE DATABASE QuanLyBanHang;

USE QuanLyBanHang;

-- Bảng Tài Khoản (Phải tạo trước)
CREATE TABLE TaiKhoan (
    TenDangNhap VARCHAR(50) PRIMARY KEY,
    MatKhau VARCHAR(255) NOT NULL,
    Quyen NVARCHAR(50) DEFAULT N'Nhân viên' -- Phân quyền, ví dụ: 'Admin', 'Nhân viên'
);

-- Bảng Nhân Viên (Đã liên kết Tên đăng nhập)
CREATE TABLE NhanVien (
    MaNV VARCHAR(20) PRIMARY KEY,
    TenNV NVARCHAR(100) NOT NULL,
    GioiTinh NVARCHAR(10),
    NgaySinh DATE,
    SoDienThoai VARCHAR(15),
    DiaChi NVARCHAR(200),
    TenDangNhap VARCHAR(50) UNIQUE, -- Thêm Tên đăng nhập (UNIQUE để đảm bảo 1 tài khoản chỉ cấp cho 1 NV)
    FOREIGN KEY (TenDangNhap) REFERENCES TaiKhoan(TenDangNhap)
);

-- Bảng Khách Hàng
CREATE TABLE KhachHang (
    MaKH VARCHAR(20) PRIMARY KEY,  
    TenKH NVARCHAR(100) NOT NULL,
    SoDienThoai VARCHAR(15),
    DiaChi NVARCHAR(200)
);

-- Bảng Nhà Cung Cấp
CREATE TABLE NhaCungCap (
    MaNCC VARCHAR(20) PRIMARY KEY, 
    TenNCC NVARCHAR(100) NOT NULL,
    SoDienThoai VARCHAR(15),
    DiaChi NVARCHAR(200)
);

-- Bảng Sản Phẩm
CREATE TABLE SanPham (
    MaSP VARCHAR(20) PRIMARY KEY,  
    TenSP NVARCHAR(100) NOT NULL,
    MaNCC VARCHAR(20),
    GiaBan DECIMAL(18, 2),
    SoLuongTon INT DEFAULT 0,
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
    ThanhTien AS (SoLuong * DonGia),
    PRIMARY KEY (MaHD, MaSP),
    FOREIGN KEY (MaHD) REFERENCES HoaDon(MaHD),
    FOREIGN KEY (MaSP) REFERENCES SanPham(MaSP)
);
