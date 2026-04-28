
USE master;
GO

BEGIN
    CREATE DATABASE TestDB;
END
GO

USE TestDB;
GO

CREATE TABLE Lop (
    MaLop NVARCHAR(10) PRIMARY KEY,
    TenLop NVARCHAR(100) NOT NULL,
    NienKhoa NVARCHAR(20)
);
GO

CREATE TABLE SinhVien (
    MaSV NVARCHAR(10) PRIMARY KEY,
    TenSV NVARCHAR(100) NOT NULL,
    NgaySinh DATE,
    GioiTinh NVARCHAR(10),
    MaLop NVARCHAR(10),
    DiaChi NVARCHAR(200),
    FOREIGN KEY (MaLop) REFERENCES Lop(MaLop)
);
GO

--thêm dữ liệu mẫu vào bảng Lop
INSERT INTO Lop (MaLop, TenLop, NienKhoa) VALUES
('CNTT01', N'Công nghệ thông tin 01', '2023-2027'),
('CNTT02', N'Công nghệ thông tin 02', '2023-2027'),
('KTPM01', N'Kỹ thuật phần mềm 01', '2023-2027'),
('KHMT01', N'Khoa học máy tính 01', '2023-2027'),
('HTTT01', N'Hệ thống thông tin 01', '2023-2027');
GO

--thêm dữ liệu mẫu vào bảng SinhVien
INSERT INTO SinhVien (MaSV, TenSV, NgaySinh, GioiTinh, MaLop, DiaChi) VALUES
('SV001', N'Nguyễn Văn An', '2005-01-15', N'Nam', 'CNTT01', N'Hà Nội'),
('SV002', N'Trần Thị Bình', '2005-03-20', N'Nữ', 'CNTT01', N'Hồ Chí Minh'),
('SV003', N'Lê Văn Cường', '2005-05-10', N'Nam', 'CNTT02', N'Đà Nẵng'),
('SV004', N'Phạm Thị Dung', '2005-07-25', N'Nữ', 'KTPM01', N'Hải Phòng'),
('SV005', N'Hoàng Văn Em', '2005-02-14', N'Nam', 'KTPM01', N'Cần Thơ'),
('SV006', N'Vũ Thị Phương', '2005-09-30', N'Nữ', 'KHMT01', N'Huế'),
('SV007', N'Đỗ Văn Giang', '2005-11-05', N'Nam', 'KHMT01', N'Nha Trang'),
('SV008', N'Bùi Thị Hoa', '2005-04-18', N'Nữ', 'HTTT01', N'Vũng Tàu'),
('SV009', N'Ngô Văn Inh', '2005-06-22', N'Nam', 'HTTT01', N'Quy Nhơn'),
('SV010', N'Đinh Thị Kim', '2005-08-12', N'Nữ', 'CNTT02', N'Vinh');
GO

--kiểm tra dữ liệu 
SELECT * FROM Lop;
SELECT * FROM SinhVien;
GO
