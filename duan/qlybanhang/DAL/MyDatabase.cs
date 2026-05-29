using System;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class MyDatabase
    {
        private SqlConnection conn = null;
        private DataSet ds = null;

        private SqlDataAdapter daTaiKhoan = null;
        private SqlDataAdapter daKhachHang = null;
        private SqlDataAdapter daNhanVien = null;
        private SqlDataAdapter daNhaCungCap = null;
        private SqlDataAdapter daSanPham = null;
        private SqlDataAdapter daHoaDon = null;
        private SqlDataAdapter daChiTietHD = null;

        public MyDatabase()
        {
            conn = new SqlConnection(Properties.Settings.Default.strconnect);
            ds = new DataSet();

            // --- TaiKhoan ---
            daTaiKhoan = new SqlDataAdapter();
            daTaiKhoan.SelectCommand = new SqlCommand("SELECT * FROM TaiKhoan", conn);
            daTaiKhoan.TableMappings.Add("Table", "TaiKhoan");
            SqlCommandBuilder cbTaiKhoan = new SqlCommandBuilder(daTaiKhoan);
            daTaiKhoan.Fill(ds, "TaiKhoan");

            // --- KhachHang ---
            daKhachHang = new SqlDataAdapter();
            daKhachHang.SelectCommand = new SqlCommand("SELECT * FROM KhachHang", conn);
            daKhachHang.TableMappings.Add("Table", "KhachHang");
            SqlCommandBuilder cbKhachHang = new SqlCommandBuilder(daKhachHang);
            daKhachHang.Fill(ds, "KhachHang");

            // --- NhanVien ---
            daNhanVien = new SqlDataAdapter();
            daNhanVien.SelectCommand = new SqlCommand("SELECT * FROM NhanVien", conn);
            daNhanVien.TableMappings.Add("Table", "NhanVien");
            SqlCommandBuilder cbNhanVien = new SqlCommandBuilder(daNhanVien);
            daNhanVien.Fill(ds, "NhanVien");

            // --- NhaCungCap ---
            daNhaCungCap = new SqlDataAdapter();
            daNhaCungCap.SelectCommand = new SqlCommand("SELECT * FROM NhaCungCap", conn);
            daNhaCungCap.TableMappings.Add("Table", "NhaCungCap");
            SqlCommandBuilder cbNhaCungCap = new SqlCommandBuilder(daNhaCungCap);
            daNhaCungCap.Fill(ds, "NhaCungCap");

            // --- SanPham ---
            daSanPham = new SqlDataAdapter();
            daSanPham.SelectCommand = new SqlCommand("SELECT * FROM SanPham", conn);
            daSanPham.TableMappings.Add("Table", "SanPham");
            SqlCommandBuilder cbSanPham = new SqlCommandBuilder(daSanPham);
            daSanPham.Fill(ds, "SanPham");

            // --- HoaDon ---
            daHoaDon = new SqlDataAdapter();
            daHoaDon.SelectCommand = new SqlCommand("SELECT * FROM HoaDon", conn);
            daHoaDon.TableMappings.Add("Table", "HoaDon");
            SqlCommandBuilder cbHoaDon = new SqlCommandBuilder(daHoaDon);
            daHoaDon.Fill(ds, "HoaDon");

            // --- ChiTietHoaDon (exclude ThanhTien computed column) ---
            daChiTietHD = new SqlDataAdapter();
            daChiTietHD.SelectCommand = new SqlCommand("SELECT MaHD, MaSP, SoLuong, DonGia FROM ChiTietHoaDon", conn);
            daChiTietHD.TableMappings.Add("Table", "ChiTietHoaDon");
            SqlCommandBuilder cbChiTietHD = new SqlCommandBuilder(daChiTietHD);
            daChiTietHD.Fill(ds, "ChiTietHoaDon");

            // --- DataRelation: HoaDon -> ChiTietHoaDon ---
            DataRelation relHD_CTHD = new DataRelation("HD_CTHD",
                ds.Tables["HoaDon"].Columns["MaHD"],
                ds.Tables["ChiTietHoaDon"].Columns["MaHD"], false);
            ds.Relations.Add(relHD_CTHD);
        }

        // === Getter ===
        public DataSet getDataSet() { return ds; }
        public DataTable getTable(string name) { return ds.Tables[name]; }

        // === TaiKhoan ===
        public void addRowtoTaiKhoan(DataRow r)
        {
            try
            {
                ds.Tables["TaiKhoan"].Rows.Add(r);
                daTaiKhoan.Update(ds, "TaiKhoan");
                ds.AcceptChanges();
            }
            catch { }
            reloadTable("TaiKhoan");
        }

        public void updateTaiKhoan()
        {
            daTaiKhoan.Update(ds, "TaiKhoan");
            ds.AcceptChanges();
        }

        public void deleteTaiKhoan(string tenDangNhap)
        {
            DataRow[] rows = ds.Tables["TaiKhoan"].Select("TenDangNhap = '" + tenDangNhap.Replace("'", "''") + "'");
            if (rows.Length > 0)
            {
                rows[0].Delete();
                daTaiKhoan.Update(ds, "TaiKhoan");
                ds.AcceptChanges();
            }
        }

        // === KhachHang ===
        public void addRowtoKhachHang(DataRow r)
        {
            try
            {
                ds.Tables["KhachHang"].Rows.Add(r);
                daKhachHang.Update(ds, "KhachHang");
                ds.AcceptChanges();
            }
            catch { }
            reloadTable("KhachHang");
        }

        public void updateKhachHang()
        {
            daKhachHang.Update(ds, "KhachHang");
            ds.AcceptChanges();
        }

        public void deleteKhachHang(int maKH)
        {
            DataRow[] rows = ds.Tables["KhachHang"].Select("MaKH = " + maKH);
            if (rows.Length > 0)
            {
                rows[0].Delete();
                daKhachHang.Update(ds, "KhachHang");
                ds.AcceptChanges();
            }
        }

        // === NhanVien ===
        public void addRowtoNhanVien(DataRow r)
        {
            try
            {
                ds.Tables["NhanVien"].Rows.Add(r);
                daNhanVien.Update(ds, "NhanVien");
                ds.AcceptChanges();
            }
            catch { }
            reloadTable("NhanVien");
        }

        public void updateNhanVien()
        {
            daNhanVien.Update(ds, "NhanVien");
            ds.AcceptChanges();
        }

        public void deleteNhanVien(int maNV)
        {
            DataRow[] rows = ds.Tables["NhanVien"].Select("MaNV = " + maNV);
            if (rows.Length > 0)
            {
                rows[0].Delete();
                daNhanVien.Update(ds, "NhanVien");
                ds.AcceptChanges();
            }
        }

        // === NhaCungCap ===
        public void addRowtoNhaCungCap(DataRow r)
        {
            try
            {
                ds.Tables["NhaCungCap"].Rows.Add(r);
                daNhaCungCap.Update(ds, "NhaCungCap");
                ds.AcceptChanges();
            }
            catch { }
            reloadTable("NhaCungCap");
        }

        public void updateNhaCungCap()
        {
            daNhaCungCap.Update(ds, "NhaCungCap");
            ds.AcceptChanges();
        }

        public void deleteNhaCungCap(int maNCC)
        {
            DataRow[] rows = ds.Tables["NhaCungCap"].Select("MaNCC = " + maNCC);
            if (rows.Length > 0)
            {
                rows[0].Delete();
                daNhaCungCap.Update(ds, "NhaCungCap");
                ds.AcceptChanges();
            }
        }

        // === SanPham ===
        public void addRowtoSanPham(DataRow r)
        {
            try
            {
                ds.Tables["SanPham"].Rows.Add(r);
                daSanPham.Update(ds, "SanPham");
                ds.AcceptChanges();
            }
            catch { }
            reloadTable("SanPham");
        }

        public void updateSanPham()
        {
            daSanPham.Update(ds, "SanPham");
            ds.AcceptChanges();
        }

        public void deleteSanPham(int maSP)
        {
            DataRow[] rows = ds.Tables["SanPham"].Select("MaSP = " + maSP);
            if (rows.Length > 0)
            {
                rows[0].Delete();
                daSanPham.Update(ds, "SanPham");
                ds.AcceptChanges();
            }
        }

        // === HoaDon ===
        public int taoHoaDon(int maKH, int maNV, decimal tongTien)
        {
            try
            {
                DataRow r = ds.Tables["HoaDon"].NewRow();
                r["MaKH"] = maKH;
                r["MaNV"] = maNV;
                r["NgayLap"] = DateTime.Now;
                r["TongTien"] = tongTien;
                
                ds.Tables["HoaDon"].Rows.Add(r);
                daHoaDon.Update(ds, "HoaDon");
                ds.AcceptChanges();
            }
            catch { }
            reloadTable("HoaDon");

            // Tìm mã hóa đơn lớn nhất (vừa được tự động tăng thêm)
            int maHDMoi = 0;
            foreach (DataRow row in ds.Tables["HoaDon"].Rows)
            {
                int maHD = Convert.ToInt32(row["MaHD"]);
                if (maHD > maHDMoi)
                {
                    maHDMoi = maHD;
                }
            }
            return maHDMoi;
        }

        public void themChiTietHoaDon(int maHD, int maSP, int soLuong, decimal donGia)
        {
            try
            {
                DataRow r = ds.Tables["ChiTietHoaDon"].NewRow();
                r["MaHD"] = maHD;
                r["MaSP"] = maSP;
                r["SoLuong"] = soLuong;
                r["DonGia"] = donGia;

                ds.Tables["ChiTietHoaDon"].Rows.Add(r);
                daChiTietHD.Update(ds, "ChiTietHoaDon");
                ds.AcceptChanges();
            }
            catch { }
            reloadTable("ChiTietHoaDon");
        }

        public void deleteHoaDon(int maHD)
        {
            // Xóa chi tiết hóa đơn trước (và hoàn lại số lượng tồn kho)
            DataRow[] cthdRows = ds.Tables["ChiTietHoaDon"].Select("MaHD = " + maHD);
            foreach (DataRow r in cthdRows)
            {
                int maSP = Convert.ToInt32(r["MaSP"]);
                int soLuong = Convert.ToInt32(r["SoLuong"]);
                // Hoàn lại số lượng tồn kho (truyền số lượng âm sẽ làm cộng thêm)
                capNhatTonKho(maSP, -soLuong);
                r.Delete();
            }
            daChiTietHD.Update(ds, "ChiTietHoaDon");
            ds.AcceptChanges();

            // Sau đó xóa hóa đơn
            DataRow[] hdRows = ds.Tables["HoaDon"].Select("MaHD = " + maHD);
            if (hdRows.Length > 0)
            {
                hdRows[0].Delete();
                daHoaDon.Update(ds, "HoaDon");
                ds.AcceptChanges();
            }
            reloadTable("ChiTietHoaDon");
            reloadTable("HoaDon");
        }

        public void capNhatTonKho(int maSP, int soLuongBan)
        {
            DataRow[] rows = ds.Tables["SanPham"].Select("MaSP = " + maSP);
            if (rows.Length > 0)
            {
                int tonHienTai = Convert.ToInt32(rows[0]["SoLuongTon"]);
                rows[0]["SoLuongTon"] = tonHienTai - soLuongBan;
            }
            daSanPham.Update(ds, "SanPham");
            ds.AcceptChanges();
        }

        // === Reload table from DB ===
        public void reloadTable(string tableName)
        {
            ds.Tables[tableName].Clear();
            switch (tableName)
            {
                case "TaiKhoan": daTaiKhoan.Fill(ds, "TaiKhoan"); break;
                case "KhachHang": daKhachHang.Fill(ds, "KhachHang"); break;
                case "NhanVien": daNhanVien.Fill(ds, "NhanVien"); break;
                case "NhaCungCap": daNhaCungCap.Fill(ds, "NhaCungCap"); break;
                case "SanPham": daSanPham.Fill(ds, "SanPham"); break;
                case "HoaDon": daHoaDon.Fill(ds, "HoaDon"); break;
                case "ChiTietHoaDon": daChiTietHD.Fill(ds, "ChiTietHoaDon"); break;
            }
        }
    }
}
