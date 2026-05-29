using System;
using System.Data;
using DAL;
using DTO;

namespace BUS
{
    public class MyBUS
    {
        private MyDatabase db = new MyDatabase();

        // ==================== TÀI KHOẢN ====================

        public DataRow DangNhap(string tenDangNhap, string matKhau)
        {
            DataTable dt = db.getTable("TaiKhoan");
            DataRow[] rows = dt.Select(
                "TenDangNhap = '" + tenDangNhap.Replace("'", "''") + "' AND MatKhau = '" + matKhau.Replace("'", "''") + "'");
            return rows.Length > 0 ? rows[0] : null;
        }

        public Boolean DangKy(TaiKhoanDTO tk)
        {
            Boolean kq = false;
            DataTable dt = db.getTable("TaiKhoan");
            DataRow[] existing = dt.Select("TenDangNhap = '" + tk.TenDangNhap.Replace("'", "''") + "'");
            if (existing.Length == 0)
            {
                DataRow r = dt.NewRow();
                r["TenDangNhap"] = tk.TenDangNhap;
                r["MatKhau"] = tk.MatKhau;
                r["Quyen"] = "nhanvien";
                db.addRowtoTaiKhoan(r);
                kq = true;
            }
            return kq;
        }

        public DataTable getTableTaiKhoan()
        {
            return db.getTable("TaiKhoan");
        }

        public Boolean add_New_TK(TaiKhoanDTO tk)
        {
            Boolean kq = false;
            DataTable dt = db.getTable("TaiKhoan");
            DataRow[] existing = dt.Select("TenDangNhap = '" + tk.TenDangNhap.Replace("'", "''") + "'");
            if (existing.Length == 0)
            {
                DataRow r = dt.NewRow();
                r["TenDangNhap"] = tk.TenDangNhap;
                r["MatKhau"] = tk.MatKhau;
                r["Quyen"] = tk.Quyen;
                db.addRowtoTaiKhoan(r);
                kq = true;
            }
            return kq;
        }

        public DataRow[] getFilter_TK(string strFilter)
        {
            return db.getTable("TaiKhoan").Select(strFilter);
        }

        public Boolean update_TK(TaiKhoanDTO tk)
        {
            Boolean kq = false;
            DataRow[] rows = db.getTable("TaiKhoan").Select("TenDangNhap = '" + tk.TenDangNhap.Replace("'", "''") + "'");
            if (rows.Length > 0)
            {
                DataRow r = rows[0];
                r.BeginEdit();
                r["MatKhau"] = tk.MatKhau;
                r["Quyen"] = tk.Quyen;
                r.EndEdit();
                db.updateTaiKhoan();
                kq = true;
            }
            return kq;
        }

        public Boolean delete_TK(string tenDangNhap)
        {
            Boolean kq = false;
            DataRow[] rows = db.getTable("TaiKhoan").Select("TenDangNhap = '" + tenDangNhap.Replace("'", "''") + "'");
            if (rows.Length > 0)
            {
                db.deleteTaiKhoan(tenDangNhap);
                kq = true;
            }
            return kq;
        }

        // ==================== KHÁCH HÀNG ====================

        public DataTable getTableKhachHang()
        {
            return db.getTable("KhachHang");
        }

        public Boolean add_New_KH(KhachHangDTO kh)
        {
            Boolean kq = false;
            try
            {
                DataRow r = db.getTable("KhachHang").NewRow();
                r["TenKH"] = kh.TenKH;
                r["SoDienThoai"] = kh.SoDienThoai;
                r["DiaChi"] = kh.DiaChi;
                db.addRowtoKhachHang(r);
                kq = true;
            }
            catch { }
            return kq;
        }

        public DataRow[] getFilter_KH(string strFilter)
        {
            return db.getTable("KhachHang").Select(strFilter);
        }

        public Boolean update_KH(KhachHangDTO kh)
        {
            Boolean kq = false;
            DataRow[] rows = db.getTable("KhachHang").Select("MaKH = " + kh.MaKH);
            if (rows.Length > 0)
            {
                DataRow r = rows[0];
                r.BeginEdit();
                r["TenKH"] = kh.TenKH;
                r["SoDienThoai"] = kh.SoDienThoai;
                r["DiaChi"] = kh.DiaChi;
                r.EndEdit();
                db.updateKhachHang();
                kq = true;
            }
            return kq;
        }

        public Boolean delete_KH(int maKH)
        {
            Boolean kq = false;
            DataRow[] rows = db.getTable("KhachHang").Select("MaKH = " + maKH);
            if (rows.Length > 0)
            {
                db.deleteKhachHang(maKH);
                kq = true;
            }
            return kq;
        }

        // ==================== NHÂN VIÊN ====================

        public DataTable getTableNhanVien()
        {
            return db.getTable("NhanVien");
        }

        public Boolean add_New_NV(NhanVienDTO nv)
        {
            Boolean kq = false;
            try
            {
                DataRow r = db.getTable("NhanVien").NewRow();
                r["TenNV"] = nv.TenNV;
                r["TenDangNhap"] = nv.TenDangNhap;
                r["SoDienThoai"] = nv.SoDienThoai;
                r["DiaChi"] = nv.DiaChi;
                db.addRowtoNhanVien(r);
                kq = true;
            }
            catch { }
            return kq;
        }

        public DataRow[] getFilter_NV(string strFilter)
        {
            return db.getTable("NhanVien").Select(strFilter);
        }

        public Boolean update_NV(NhanVienDTO nv)
        {
            Boolean kq = false;
            DataRow[] rows = db.getTable("NhanVien").Select("MaNV = " + nv.MaNV);
            if (rows.Length > 0)
            {
                DataRow r = rows[0];
                r.BeginEdit();
                r["TenNV"] = nv.TenNV;
                r["TenDangNhap"] = nv.TenDangNhap;
                r["SoDienThoai"] = nv.SoDienThoai;
                r["DiaChi"] = nv.DiaChi;
                r.EndEdit();
                db.updateNhanVien();
                kq = true;
            }
            return kq;
        }

        public Boolean delete_NV(int maNV)
        {
            Boolean kq = false;
            DataRow[] rows = db.getTable("NhanVien").Select("MaNV = " + maNV);
            if (rows.Length > 0)
            {
                db.deleteNhanVien(maNV);
                kq = true;
            }
            return kq;
        }

        // ==================== NHÀ CUNG CẤP ====================

        public DataTable getTableNhaCungCap()
        {
            return db.getTable("NhaCungCap");
        }

        public Boolean add_New_NCC(NhaCungCapDTO ncc)
        {
            Boolean kq = false;
            try
            {
                DataRow r = db.getTable("NhaCungCap").NewRow();
                r["TenNCC"] = ncc.TenNCC;
                r["SoDienThoai"] = ncc.SoDienThoai;
                r["DiaChi"] = ncc.DiaChi;
                db.addRowtoNhaCungCap(r);
                kq = true;
            }
            catch { }
            return kq;
        }

        public DataRow[] getFilter_NCC(string strFilter)
        {
            return db.getTable("NhaCungCap").Select(strFilter);
        }

        public Boolean update_NCC(NhaCungCapDTO ncc)
        {
            Boolean kq = false;
            DataRow[] rows = db.getTable("NhaCungCap").Select("MaNCC = " + ncc.MaNCC);
            if (rows.Length > 0)
            {
                DataRow r = rows[0];
                r.BeginEdit();
                r["TenNCC"] = ncc.TenNCC;
                r["SoDienThoai"] = ncc.SoDienThoai;
                r["DiaChi"] = ncc.DiaChi;
                r.EndEdit();
                db.updateNhaCungCap();
                kq = true;
            }
            return kq;
        }

        public Boolean delete_NCC(int maNCC)
        {
            Boolean kq = false;
            DataRow[] rows = db.getTable("NhaCungCap").Select("MaNCC = " + maNCC);
            if (rows.Length > 0)
            {
                db.deleteNhaCungCap(maNCC);
                kq = true;
            }
            return kq;
        }

        // ==================== SẢN PHẨM ====================

        public DataTable getTableSanPham()
        {
            return db.getTable("SanPham");
        }

        public Boolean add_New_SP(SanPhamDTO sp)
        {
            Boolean kq = false;
            try
            {
                DataRow r = db.getTable("SanPham").NewRow();
                r["TenSP"] = sp.TenSP;
                r["MaNCC"] = sp.MaNCC;
                r["GiaNhap"] = sp.GiaNhap;
                r["GiaBan"] = sp.GiaBan;
                r["SoLuongTon"] = sp.SoLuongTon;
                db.addRowtoSanPham(r);
                kq = true;
            }
            catch { }
            return kq;
        }

        public DataRow[] getFilter_SP(string strFilter)
        {
            return db.getTable("SanPham").Select(strFilter);
        }

        public Boolean update_SP(SanPhamDTO sp)
        {
            Boolean kq = false;
            DataRow[] rows = db.getTable("SanPham").Select("MaSP = " + sp.MaSP);
            if (rows.Length > 0)
            {
                DataRow r = rows[0];
                r.BeginEdit();
                r["TenSP"] = sp.TenSP;
                r["MaNCC"] = sp.MaNCC;
                r["GiaNhap"] = sp.GiaNhap;
                r["GiaBan"] = sp.GiaBan;
                r["SoLuongTon"] = sp.SoLuongTon;
                r.EndEdit();
                db.updateSanPham();
                kq = true;
            }
            return kq;
        }

        public Boolean delete_SP(int maSP)
        {
            Boolean kq = false;
            DataRow[] rows = db.getTable("SanPham").Select("MaSP = " + maSP);
            if (rows.Length > 0)
            {
                db.deleteSanPham(maSP);
                kq = true;
            }
            return kq;
        }

        // ==================== HÓA ĐƠN / BÁN HÀNG ====================

        public DataTable getTableHoaDon()
        {
            return db.getTable("HoaDon");
        }

        public DataTable LayDanhSachHoaDonDayDu()
        {
            DataTable dtHoaDon = db.getTable("HoaDon");
            DataTable dtKhachHang = db.getTable("KhachHang");
            DataTable dtNhanVien = db.getTable("NhanVien");
            
            DataTable result = new DataTable();
            result.Columns.Add("MaHD", typeof(int));
            result.Columns.Add("MaKH", typeof(int));
            result.Columns.Add("TenKH", typeof(string));
            result.Columns.Add("MaNV", typeof(int));
            result.Columns.Add("TenNV", typeof(string));
            result.Columns.Add("NgayLap", typeof(DateTime));
            result.Columns.Add("TongTien", typeof(decimal));

            foreach (DataRow rHD in dtHoaDon.Rows)
            {
                DataRow rNew = result.NewRow();
                rNew["MaHD"] = rHD["MaHD"];
                rNew["MaKH"] = rHD["MaKH"];
                rNew["MaNV"] = rHD["MaNV"];
                rNew["NgayLap"] = rHD["NgayLap"];
                rNew["TongTien"] = rHD["TongTien"];
                
                if (rHD["MaKH"] != DBNull.Value)
                {
                    DataRow[] rowsKH = dtKhachHang.Select("MaKH = " + rHD["MaKH"]);
                    if (rowsKH.Length > 0)
                        rNew["TenKH"] = rowsKH[0]["TenKH"];
                }

                if (rHD["MaNV"] != DBNull.Value)
                {
                    DataRow[] rowsNV = dtNhanVien.Select("MaNV = " + rHD["MaNV"]);
                    if (rowsNV.Length > 0)
                        rNew["TenNV"] = rowsNV[0]["TenNV"];
                }
                    
                result.Rows.Add(rNew);
            }
            return result;
        }

        public DataRow[] getFilter_HDDayDu(string strFilter)
        {
            DataTable dt = LayDanhSachHoaDonDayDu();
            return dt.Select(strFilter);
        }


        public DataTable getTableChiTietHD()
        {
            return db.getTable("ChiTietHoaDon");
        }

        public DataTable LayDanhSachChiTietHDDayDu(int maHD)
        {
            DataTable dtChiTiet = db.getTable("ChiTietHoaDon");
            DataTable dtSanPham = db.getTable("SanPham");
            
            DataTable result = new DataTable();
            result.Columns.Add("MaHD", typeof(int));
            result.Columns.Add("MaSP", typeof(int));
            result.Columns.Add("TenSP", typeof(string));
            result.Columns.Add("SoLuong", typeof(int));
            result.Columns.Add("DonGia", typeof(decimal));
            result.Columns.Add("ThanhTien", typeof(decimal));

            DataRow[] rowsCT = dtChiTiet.Select("MaHD = " + maHD);
            foreach (DataRow r in rowsCT)
            {
                DataRow rNew = result.NewRow();
                rNew["MaHD"] = r["MaHD"];
                rNew["MaSP"] = r["MaSP"];
                rNew["SoLuong"] = r["SoLuong"];
                rNew["DonGia"] = r["DonGia"];
                rNew["ThanhTien"] = Convert.ToInt32(r["SoLuong"]) * Convert.ToDecimal(r["DonGia"]);

                if (r["MaSP"] != DBNull.Value)
                {
                    DataRow[] rowsSP = dtSanPham.Select("MaSP = " + r["MaSP"]);
                    if (rowsSP.Length > 0)
                        rNew["TenSP"] = rowsSP[0]["TenSP"];
                }
                result.Rows.Add(rNew);
            }
            return result;
        }

        public void TaoHoaDon(int maKH, int maNV, DataTable gioHang)
        {
            decimal tongTien = 0;
            foreach (DataRow r in gioHang.Rows)
                tongTien += Convert.ToDecimal(r["ThanhTien"]);

            int maHD = db.taoHoaDon(maKH, maNV, tongTien);

            foreach (DataRow r in gioHang.Rows)
            {
                db.themChiTietHoaDon(maHD,
                    Convert.ToInt32(r["MaSP"]),
                    Convert.ToInt32(r["SoLuong"]),
                    Convert.ToDecimal(r["DonGia"]));
            }

            foreach (DataRow r in gioHang.Rows)
            {
                db.capNhatTonKho(
                    Convert.ToInt32(r["MaSP"]),
                    Convert.ToInt32(r["SoLuong"]));
            }

            db.reloadTable("HoaDon");
            db.reloadTable("ChiTietHoaDon");
            db.reloadTable("SanPham");
        }

        public void XoaHoaDon(int maHD)
        {
            db.deleteHoaDon(maHD);
        }

        public int LayMaNV(string tenDangNhap)
        {
            DataRow[] rows = db.getTable("NhanVien").Select(
                "TenDangNhap = '" + tenDangNhap.Replace("'", "''") + "'");
            return rows.Length > 0 ? Convert.ToInt32(rows[0]["MaNV"]) : -1;
        }
    }
}
