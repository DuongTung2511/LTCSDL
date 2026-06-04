# ToÃ n bá»™ mÃ£ nguá»“n dá»± Ã¡n
## BUS\ChiTietHoaDonBUS.cs
```csharp
using System;
using System.Data;
using DAL;
using DTO;

namespace BUS
{
    public class ChiTietHoaDonBUS
    {
        private MyDatabase db = MyDatabase.Instance;

        public DataSet getDataset()
        {
            return db.getDataSet();
        }

        public DataTable getTableChiTietHD()
        {
            return db.getTable("ChiTietHoaDon");
        }

        public DataTable LayDanhSachChiTietHDDayDu(string maHD)
        {
            DataTable dtChiTiet = db.getTable("ChiTietHoaDon");
            DataTable dtSanPham = db.getTable("SanPham");
            
            DataTable result = new DataTable();
            result.Columns.Add("MaHD", typeof(string));
            result.Columns.Add("MaSP", typeof(string));
            result.Columns.Add("TenSP", typeof(string));
            result.Columns.Add("SoLuong", typeof(int));
            result.Columns.Add("DonGia", typeof(decimal));
            result.Columns.Add("ThanhTien", typeof(decimal));

            DataRow[] rowsCT = dtChiTiet.Select("MaHD = '" + maHD.Replace("'", "''") + "'");
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
                    DataRow[] rowsSP = dtSanPham.Select("MaSP = '" + r["MaSP"].ToString().Replace("'", "''") + "'");
                    if (rowsSP.Length > 0)
                        rNew["TenSP"] = rowsSP[0]["TenSP"];
                }
                result.Rows.Add(rNew);
            }
            return result;
        }

        private void CapNhatTongTien(string maHD)
        {
            DataRow[] rowsCT = db.getTable("ChiTietHoaDon").Select("MaHD = '" + maHD.Replace("'", "''") + "'");
            decimal tongTien = 0;
            foreach (DataRow r in rowsCT)
            {
                tongTien += Convert.ToInt32(r["SoLuong"]) * Convert.ToDecimal(r["DonGia"]);
            }
            
            DataRow[] hdRows = db.getTable("HoaDon").Select("MaHD = '" + maHD.Replace("'", "''") + "'");
            if (hdRows.Length > 0)
            {
                hdRows[0].BeginEdit();
                hdRows[0]["TongTien"] = tongTien;
                hdRows[0].EndEdit();
                db.update("HoaDon");
            }
        }

        public bool ThemChiTiet(string maHD, string maSP, int soLuong, decimal donGia)
        {
            bool kq = false;
            DataRow[] exist = db.getTable("ChiTietHoaDon").Select("MaHD = '" + maHD.Replace("'", "''") + "' AND MaSP = '" + maSP.Replace("'", "''") + "'");
            if (exist.Length == 0)
            {
                DataRow r = db.getTable("ChiTietHoaDon").NewRow();
                r["MaHD"] = maHD;
                r["MaSP"] = maSP;
                r["SoLuong"] = soLuong;
                r["DonGia"] = donGia;
                r["ThanhTien"] = soLuong * donGia;
                db.addRow("ChiTietHoaDon", r);

                DataRow[] spRows = db.getTable("SanPham").Select("MaSP = '" + maSP.Replace("'", "''") + "'");
                if (spRows.Length > 0)
                {
                    spRows[0].BeginEdit();
                    int currentStock = Convert.ToInt32(spRows[0]["SoLuongTon"]);
                    spRows[0]["SoLuongTon"] = currentStock - soLuong;
                    spRows[0].EndEdit();
                    db.update("SanPham");
                }

                CapNhatTongTien(maHD);
                kq = true;
            }
            return kq;
        }

        public bool SuaChiTiet(string maHD, string maSP, int soLuongMoi, decimal donGiaMoi)
        {
            bool kq = false;
            DataRow[] exist = db.getTable("ChiTietHoaDon").Select("MaHD = '" + maHD.Replace("'", "''") + "' AND MaSP = '" + maSP.Replace("'", "''") + "'");
            if (exist.Length > 0)
            {
                int soLuongCu = Convert.ToInt32(exist[0]["SoLuong"]);
                int chechLech = soLuongMoi - soLuongCu;

                DataRow r = exist[0];
                r.BeginEdit();
                r["SoLuong"] = soLuongMoi;
                r["DonGia"] = donGiaMoi;
                r["ThanhTien"] = soLuongMoi * donGiaMoi;
                r.EndEdit();
                db.update("ChiTietHoaDon");
                
                DataRow[] spRows = db.getTable("SanPham").Select("MaSP = '" + maSP.Replace("'", "''") + "'");
                if (spRows.Length > 0)
                {
                    spRows[0].BeginEdit();
                    int currentStock = Convert.ToInt32(spRows[0]["SoLuongTon"]);
                    spRows[0]["SoLuongTon"] = currentStock - chechLech;
                    spRows[0].EndEdit();
                    db.update("SanPham");
                }
                
                CapNhatTongTien(maHD);
                kq = true;
            }
            return kq;
        }

        public bool XoaChiTiet(string maHD, string maSP)
        {
            bool kq = false;
            DataRow[] exist = db.getTable("ChiTietHoaDon").Select("MaHD = '" + maHD.Replace("'", "''") + "' AND MaSP = '" + maSP.Replace("'", "''") + "'");
            if (exist.Length > 0)
            {
                int soLuongCu = Convert.ToInt32(exist[0]["SoLuong"]);

                db.deleteRow("ChiTietHoaDon", "MaHD = '" + maHD.Replace("'", "''") + "' AND MaSP = '" + maSP.Replace("'", "''") + "'");
                
                DataRow[] spRows = db.getTable("SanPham").Select("MaSP = '" + maSP.Replace("'", "''") + "'");
                if (spRows.Length > 0)
                {
                    spRows[0].BeginEdit();
                    int currentStock = Convert.ToInt32(spRows[0]["SoLuongTon"]);
                    spRows[0]["SoLuongTon"] = currentStock + soLuongCu;
                    spRows[0].EndEdit();
                    db.update("SanPham");
                }
                
                CapNhatTongTien(maHD);
                kq = true;
            }
            return kq;
        }
    }
}

```

## BUS\HoaDonBUS.cs
```csharp
using System;
using System.Data;
using DAL;
using DTO;

namespace BUS
{
    public class HoaDonBUS
    {
        private MyDatabase db = MyDatabase.Instance;

        public DataSet getDataset()
        {
            return db.getDataSet();
        }

        public DataTable getTableHoaDon()
        {
            return db.getTable("HoaDon");
        }

        public Boolean MaHD_not_Exist(string maHD)
        {
            Boolean kq = true;
            DataRow[] rows = db.getTable("HoaDon").Select("MaHD='" + maHD.Replace("'", "''") + "'");
            if (rows.Length > 0)
            {
                kq = false;
            }
            return kq;
        }

        public DataTable LayDanhSachHoaDonDayDu()
        {
            DataTable dtHoaDon = db.getTable("HoaDon");
            DataTable dtKhachHang = db.getTable("KhachHang");
            DataTable dtNhanVien = db.getTable("NhanVien");
            
            DataTable result = new DataTable();
            result.Columns.Add("MaHD", typeof(string));
            result.Columns.Add("MaKH", typeof(string));
            result.Columns.Add("TenKH", typeof(string));
            result.Columns.Add("MaNV", typeof(string));
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
                    DataRow[] rowsKH = dtKhachHang.Select("MaKH = '" + rHD["MaKH"].ToString().Replace("'", "''") + "'");
                    if (rowsKH.Length > 0)
                        rNew["TenKH"] = rowsKH[0]["TenKH"];
                }

                if (rHD["MaNV"] != DBNull.Value)
                {
                    DataRow[] rowsNV = dtNhanVien.Select("MaNV = '" + rHD["MaNV"].ToString().Replace("'", "''") + "'");
                    if (rowsNV.Length > 0)
                        rNew["TenNV"] = rowsNV[0]["TenNV"];
                }
                    
                result.Rows.Add(rNew);
            }
            return result;
        }

        public string LayNextMaHD()
        {
            DataTable dt = db.getTable("HoaDon");
            int max = 0;
            foreach (DataRow r in dt.Rows)
            {
                string maHD = r["MaHD"].ToString();
                if (maHD.StartsWith("HD"))
                {
                    string numPart = maHD.Substring(2);
                    if (int.TryParse(numPart, out int num))
                    {
                        if (num > max) max = num;
                    }
                }
            }
            return "HD" + (max + 1).ToString("D3");
        }

        public DataRow[] getFilter_HDDayDu(string strFilter)
        {
            DataTable dt = LayDanhSachHoaDonDayDu();
            return dt.Select(strFilter);
        }

        public void TaoHoaDon(string maHD, string maKH, string maNV, DataTable gioHang)
        {
            decimal tongTien = 0;
            foreach (DataRow r in gioHang.Rows)
                tongTien += Convert.ToDecimal(r["ThanhTien"]);

            // LÆ°u cÃ¡c thay Ä‘á»•i sá»‘ lÆ°á»£ng tá»“n (do giao diá»‡n Ä‘Ã£ trá»«) vÃ o CSDL trÆ°á»›c
            // Ä‘á»ƒ trÃ¡nh bá»‹ ds.AcceptChanges() xÃ³a máº¥t tráº¡ng thÃ¡i Modified
            db.update("SanPham");

            DataRow newHD = db.getTable("HoaDon").NewRow();
            newHD["MaHD"] = maHD;
            newHD["MaKH"] = maKH;
            newHD["MaNV"] = maNV;
            newHD["NgayLap"] = DateTime.Now;
            newHD["TongTien"] = tongTien;
            db.addRow("HoaDon", newHD);

            foreach (DataRow r in gioHang.Rows)
            {
                DataRow newCT = db.getTable("ChiTietHoaDon").NewRow();
                newCT["MaHD"] = maHD;
                string maSP = r["MaSP"].ToString();
                newCT["MaSP"] = maSP;
                int soLuong = Convert.ToInt32(r["SoLuong"]);
                newCT["SoLuong"] = soLuong;
                newCT["DonGia"] = Convert.ToDecimal(r["DonGia"]);
                newCT["ThanhTien"] = Convert.ToDecimal(r["ThanhTien"]);
                db.addRow("ChiTietHoaDon", newCT);
            }
        }

        public void XoaHoaDon(string maHD)
        {
            DataRow[] cthdRows = db.getTable("ChiTietHoaDon").Select("MaHD = '" + maHD.Replace("'", "''") + "'");
            foreach (DataRow r in cthdRows)
            {
                string maSP = r["MaSP"].ToString();
                int soLuong = Convert.ToInt32(r["SoLuong"]);
                DataRow[] spRows = db.getTable("SanPham").Select("MaSP = '" + maSP.Replace("'", "''") + "'");
                if (spRows.Length > 0)
                {
                    spRows[0].BeginEdit();
                    int currentStock = Convert.ToInt32(spRows[0]["SoLuongTon"]);
                    spRows[0]["SoLuongTon"] = currentStock + soLuong; // Refund stock
                    spRows[0].EndEdit();
                }
            }
            db.update("SanPham");
            db.deleteRow("ChiTietHoaDon", "MaHD = '" + maHD.Replace("'", "''") + "'");
            db.deleteRow("HoaDon", "MaHD = '" + maHD.Replace("'", "''") + "'");
        }
    }
}

```

## BUS\KhachHangBUS.cs
```csharp
using System;
using System.Data;
using DAL;
using DTO;

namespace BUS
{
    public class KhachHangBUS
    {
        private MyDatabase db = MyDatabase.Instance;

        public DataSet getDataset()
        {
            return db.getDataSet();
        }

        public DataTable getTableKhachHang()
        {
            return db.getTable("KhachHang");
        }

        public DataRow[] getFilter_KhachHang(string strFilter)
        {
            return db.getTable("KhachHang").Select(strFilter);
        }

        public Boolean MaKH_not_Exist(string maKH)
        {
            Boolean kq = true;
            DataRow[] rows = db.getTable("KhachHang").Select("MaKH = '" + maKH.Replace("'", "''") + "'");
            if (rows.Length > 0)
            {
                kq = false;
            }
            return kq;
        }

        public Boolean add_New_KH(KhachHangDTO kh)
        {
            Boolean kq = false;
            if (MaKH_not_Exist(kh.MaKH))
            {
                DataRow r = db.getTable("KhachHang").NewRow();
                r["MaKH"] = kh.MaKH;
                r["TenKH"] = kh.TenKH;
                r["SoDienThoai"] = kh.SoDienThoai;
                r["DiaChi"] = kh.DiaChi;
                r["TrangThai"] = 1;

                db.addRow("KhachHang", r);
                kq = true;
            }
            return kq;
        }

        public bool update_KH(KhachHangDTO kh)
        {
            bool kq = false;
            DataRow[] rows = db.getTable("KhachHang").Select("MaKH = '" + kh.MaKH.Replace("'", "''") + "'");
            if (rows.Length > 0)
            {
                DataRow r = rows[0];
                r.BeginEdit();
                r["TenKH"] = kh.TenKH;
                r["SoDienThoai"] = kh.SoDienThoai;
                r["DiaChi"] = kh.DiaChi;
                r["TrangThai"] = kh.TrangThai;
                r.EndEdit();
                
                try 
                {
                    db.update("KhachHang");
                    kq = true;
                }
                catch { }
            }
            return kq;
        }

        public bool delete_KH(string maKH)
        {
            bool kq = false;
            DataRow[] rows = db.getTable("KhachHang").Select("MaKH = '" + maKH.Replace("'", "''") + "'");
            if (rows.Length > 0)
            {
                DataRow r = rows[0];
                r.BeginEdit();
                r["TrangThai"] = 0;
                r.EndEdit();

                try 
                {
                    db.update("KhachHang");
                    kq = true;
                }
                catch { }
            }
            return kq;
        }

        public string XoaVinhVien(string maKH)
        {
            string kq = "";
            if (MaKH_not_Exist(maKH))
            {
                kq = "KhÃ¡ch hÃ ng khÃ´ng tá»“n táº¡i!";
            }
            else
            {
                DataRow[] hdRows = db.getTable("HoaDon").Select("MaKH = '" + maKH.Replace("'", "''") + "'");
                if (hdRows.Length > 0)
                {
                    kq = "KhÃ¡ch hÃ ng Ä‘Ã£ phÃ¡t sinh HÃ³a ÄÆ¡n, khÃ´ng thá»ƒ xÃ³a vÄ©nh viá»…n!";
                }
                else
                {
                    db.deleteRow("KhachHang", "MaKH = '" + maKH.Replace("'", "''") + "'");
                }
            }
            return kq;
        }
        
        public DataRow[] getFilter_KH(string strFilter)
        {
            return getFilter_KhachHang(strFilter);
        }
    }
}

```

## BUS\NhaCungCapBUS.cs
```csharp
using System;
using System.Data;
using DAL;
using DTO;

namespace BUS
{
    public class NhaCungCapBUS
    {
        private MyDatabase db = MyDatabase.Instance;

        public DataSet getDataset()
        {
            return db.getDataSet();
        }

        public DataTable getTableNhaCungCap()
        {
            return db.getTable("NhaCungCap");
        }

        public DataRow[] getFilter_NhaCungCap(string strFilter)
        {
            return db.getTable("NhaCungCap").Select(strFilter);
        }

        public Boolean MaNCC_not_Exist(string maNCC)
        {
            Boolean kq = true;
            DataRow[] rows = db.getTable("NhaCungCap").Select("MaNCC = '" + maNCC.Replace("'", "''") + "'");
            if (rows.Length > 0)
            {
                kq = false;
            }
            return kq;
        }

        public Boolean add_New_NCC(NhaCungCapDTO ncc)
        {
            Boolean kq = false;
            if (MaNCC_not_Exist(ncc.MaNCC))
            {
                DataRow r = db.getTable("NhaCungCap").NewRow();
                r["MaNCC"] = ncc.MaNCC;
                r["TenNCC"] = ncc.TenNCC;
                r["SoDienThoai"] = ncc.SoDienThoai;
                r["DiaChi"] = ncc.DiaChi;
                r["TrangThai"] = 1;

                db.addRow("NhaCungCap", r);
                kq = true;
            }
            return kq;
        }

        public bool update_NCC(NhaCungCapDTO ncc)
        {
            bool kq = false;
            DataRow[] rows = db.getTable("NhaCungCap").Select("MaNCC = '" + ncc.MaNCC.Replace("'", "''") + "'");
            if (rows.Length > 0)
            {
                DataRow r = rows[0];
                r.BeginEdit();
                r["TenNCC"] = ncc.TenNCC;
                r["SoDienThoai"] = ncc.SoDienThoai;
                r["DiaChi"] = ncc.DiaChi;
                r["TrangThai"] = ncc.TrangThai;
                r.EndEdit();
                
                try 
                {
                    db.update("NhaCungCap");
                    kq = true;
                }
                catch { }
            }
            return kq;
        }

        public bool delete_NCC(string maNCC)
        {
            bool kq = false;
            DataRow[] rows = db.getTable("NhaCungCap").Select("MaNCC = '" + maNCC.Replace("'", "''") + "'");
            if (rows.Length > 0)
            {
                DataRow r = rows[0];
                r.BeginEdit();
                r["TrangThai"] = 0;
                r.EndEdit();

                try 
                {
                    db.update("NhaCungCap");
                    kq = true;
                }
                catch { }
            }
            return kq;
        }

        public string XoaVinhVien(string maNCC)
        {
            string kq = "";
            if (MaNCC_not_Exist(maNCC))
            {
                kq = "NhÃ  cung cáº¥p khÃ´ng tá»“n táº¡i!";
            }
            else
            {
                DataRow[] spRows = db.getTable("SanPham").Select("MaNCC = '" + maNCC.Replace("'", "''") + "'");
                if (spRows.Length > 0)
                {
                    kq = "NhÃ  cung cáº¥p Ä‘Ã£ cÃ³ Sáº£n Pháº©m, khÃ´ng thá»ƒ xÃ³a vÄ©nh viá»…n!";
                }
                else
                {
                    db.deleteRow("NhaCungCap", "MaNCC = '" + maNCC.Replace("'", "''") + "'");
                }
            }
            return kq;
        }

        public DataTable LayDanhSachNCCDangHoatDong()
        {
            DataRow[] rows = db.getTable("NhaCungCap").Select("TrangThai = 1 OR TrangThai IS NULL");
            if (rows.Length > 0) return rows.CopyToDataTable();
            return db.getTable("NhaCungCap").Clone();
        }
    }
}

```

## BUS\NhanVienBUS.cs
```csharp
using System;
using System.Data;
using DAL;
using DTO;

namespace BUS
{
    public class NhanVienBUS
    {
        private MyDatabase db = MyDatabase.Instance;

        public DataSet getDataset()
        {
            return db.getDataSet();
        }

        public DataTable getTableNhanVien()
        {
            return db.getTable("NhanVien");
        }

        public DataRow[] getFilter_NhanVien(string strFilter)
        {
            return db.getTable("NhanVien").Select(strFilter);
        }

        public Boolean MaNV_not_Exist(string maNV)
        {
            Boolean kq = true;
            DataRow[] rows = db.getTable("NhanVien").Select("MaNV = '" + maNV.Replace("'", "''") + "'");
            if (rows.Length > 0)
            {
                kq = false;
            }
            return kq;
        }

        public Boolean add_New_NV(NhanVienDTO nv)
        {
            Boolean kq = false;
            if (MaNV_not_Exist(nv.MaNV))
            {
                DataRow r = db.getTable("NhanVien").NewRow();
                r["MaNV"] = nv.MaNV;
                r["TenNV"] = nv.TenNV;
                r["GioiTinh"] = nv.GioiTinh;
                r["NgaySinh"] = nv.NgaySinh;
                r["SoDienThoai"] = nv.SoDienThoai;
                r["DiaChi"] = nv.DiaChi;
                r["TrangThai"] = 1;

                db.addRow("NhanVien", r);
                kq = true;
            }
            return kq;
        }

        public bool update_NV(NhanVienDTO nv)
        {
            bool kq = false;
            DataRow[] rows = db.getTable("NhanVien").Select("MaNV = '" + nv.MaNV.Replace("'", "''") + "'");
            if (rows.Length > 0)
            {
                DataRow r = rows[0];
                r.BeginEdit();
                r["TenNV"] = nv.TenNV;
                r["GioiTinh"] = nv.GioiTinh;
                r["NgaySinh"] = nv.NgaySinh;
                r["SoDienThoai"] = nv.SoDienThoai;
                r["DiaChi"] = nv.DiaChi;
                r["TrangThai"] = nv.TrangThai;
                r.EndEdit();
                
                try 
                {
                    db.update("NhanVien");
                    kq = true;
                }
                catch { }
            }
            return kq;
        }

        public bool delete_NV(string maNV)
        {
            bool kq = false;
            DataRow[] rows = db.getTable("NhanVien").Select("MaNV = '" + maNV.Replace("'", "''") + "'");
            if (rows.Length > 0)
            {
                DataRow r = rows[0];
                r.BeginEdit();
                r["TrangThai"] = 0;
                r.EndEdit();

                try 
                {
                    db.update("NhanVien");
                    kq = true;
                }
                catch { }
            }
            return kq;
        }

        public string XoaVinhVien(string maNV)
        {
            string kq = "";
            if (MaNV_not_Exist(maNV))
            {
                kq = "NhÃ¢n viÃªn khÃ´ng tá»“n táº¡i!";
            }
            else
            {
                DataRow[] hdRows = db.getTable("HoaDon").Select("MaNV = '" + maNV.Replace("'", "''") + "'");
                if (hdRows.Length > 0)
                {
                    kq = "NhÃ¢n viÃªn Ä‘Ã£ phÃ¡t sinh HÃ³a ÄÆ¡n, khÃ´ng thá»ƒ xÃ³a vÄ©nh viá»…n!";
                }
                else
                {
                    DataRow[] tkRows = db.getTable("TaiKhoan").Select("MaNV = '" + maNV.Replace("'", "''") + "'");
                    if (tkRows.Length > 0)
                    {
                        foreach (DataRow r in tkRows)
                        {
                            db.deleteRow("TaiKhoan", "TenDangNhap = '" + r["TenDangNhap"].ToString().Replace("'", "''") + "'");
                        }
                    }

                    db.deleteRow("NhanVien", "MaNV = '" + maNV.Replace("'", "''") + "'");
                }
            }
            return kq;
        }

        public string LayMaNV(string tenDangNhap)
        {
            string kq = "";
            DataRow[] rows = db.getTable("TaiKhoan").Select("TenDangNhap = '" + tenDangNhap.Replace("'", "''") + "'");
            if (rows.Length > 0)
            {
                if (rows[0]["MaNV"] != DBNull.Value)
                {
                    kq = rows[0]["MaNV"].ToString();
                }
            }
            return kq;
        }
    }
}

```

## BUS\SanPhamBUS.cs
```csharp
using System;
using System.Data;
using DAL;
using DTO;

namespace BUS
{
    public class SanPhamBUS
    {
        private MyDatabase db = MyDatabase.Instance;

        public DataSet getDataset()
        {
            return db.getDataSet();
        }

        public DataTable getTableSanPham()
        {
            return db.getTable("SanPham");
        }

        public DataRow[] getFilter_SanPham(string strFilter)
        {
            return db.getTable("SanPham").Select(strFilter);
        }

        public Boolean MaSP_not_Exist(string maSP)
        {
            Boolean kq = true;
            DataRow[] rows = db.getTable("SanPham").Select("MaSP = '" + maSP.Replace("'", "''") + "'");
            if (rows.Length > 0)
            {
                kq = false;
            }
            return kq;
        }

        public Boolean add_New_SP(SanPhamDTO sp)
        {
            Boolean kq = false;
            if (MaSP_not_Exist(sp.MaSP))
            {
                DataRow r = db.getTable("SanPham").NewRow();
                r["MaSP"] = sp.MaSP;
                r["TenSP"] = sp.TenSP;
                r["MaNCC"] = sp.MaNCC;
                r["GiaBan"] = sp.GiaBan;
                r["SoLuongTon"] = sp.SoLuongTon;
                r["TrangThai"] = 1;

                db.addRow("SanPham", r);
                kq = true;
            }
            return kq;
        }

        public bool update_SP(SanPhamDTO sp)
        {
            bool kq = false;
            DataRow[] rows = db.getTable("SanPham").Select("MaSP = '" + sp.MaSP.Replace("'", "''") + "'");
            if (rows.Length > 0)
            {
                DataRow r = rows[0];
                r.BeginEdit();
                r["TenSP"] = sp.TenSP;
                r["MaNCC"] = sp.MaNCC;
                r["GiaBan"] = sp.GiaBan;
                r["SoLuongTon"] = sp.SoLuongTon;
                r["TrangThai"] = sp.TrangThai;
                r.EndEdit();
                
                try 
                {
                    db.update("SanPham");
                    kq = true;
                }
                catch { }
            }
            return kq;
        }

        public bool delete_SP(string maSP)
        {
            bool kq = false;
            DataRow[] rows = db.getTable("SanPham").Select("MaSP = '" + maSP.Replace("'", "''") + "'");
            if (rows.Length > 0)
            {
                DataRow r = rows[0];
                r.BeginEdit();
                r["TrangThai"] = 0;
                r.EndEdit();

                try 
                {
                    db.update("SanPham");
                    kq = true;
                }
                catch { }
            }
            return kq;
        }

        public string XoaVinhVien(string maSP)
        {
            string kq = "";
            if (MaSP_not_Exist(maSP))
            {
                kq = "Sáº£n pháº©m khÃ´ng tá»“n táº¡i!";
            }
            else
            {
                DataRow[] hdRows = db.getTable("ChiTietHoaDon").Select("MaSP = '" + maSP.Replace("'", "''") + "'");
                if (hdRows.Length > 0)
                {
                    kq = "Sáº£n pháº©m Ä‘Ã£ phÃ¡t sinh HÃ³a ÄÆ¡n, khÃ´ng thá»ƒ xÃ³a vÄ©nh viá»…n!";
                }
                else
                {
                    db.deleteRow("SanPham", "MaSP = '" + maSP.Replace("'", "''") + "'");
                }
            }
            return kq;
        }

        public DataRow[] getFilter_SP(string strFilter)
        {
            return getFilter_SanPham(strFilter);
        }
    }
}

```

## BUS\TaiKhoanBUS.cs
```csharp
using System;
using System.Data;
using DAL;
using DTO;

namespace BUS
{
    public class TaiKhoanBUS
    {
        private MyDatabase db = MyDatabase.Instance;

        public DataSet getDataset()
        {
            return db.getDataSet();
        }

        public DataTable getTableTaiKhoan()
        {
            return db.getTable("TaiKhoan");
        }

        public DataRow DangNhap(string tenDangNhap, string matKhau)
        {
            DataRow kq = null;
            string filter = "TenDangNhap = '" + tenDangNhap.Replace("'", "''") + "' AND MatKhau = '" + matKhau.Replace("'", "''") + "'";
            DataRow[] rows = db.getTable("TaiKhoan").Select(filter);
            if (rows.Length > 0)
            {
                kq = rows[0];
            }
            return kq;
        }

        public DataTable LayDanhSachTaiKhoanDayDu()
        {
            DataTable dtTK = db.getTable("TaiKhoan");
            DataTable dtNV = db.getTable("NhanVien");

            DataTable result = new DataTable();
            result.Columns.Add("TenDangNhap", typeof(string));
            result.Columns.Add("MatKhau", typeof(string));
            result.Columns.Add("Quyen", typeof(string));
            result.Columns.Add("MaNV", typeof(string));
            result.Columns.Add("TenNV", typeof(string));

            foreach (DataRow r in dtTK.Rows)
            {
                DataRow rNew = result.NewRow();
                rNew["TenDangNhap"] = r["TenDangNhap"];
                rNew["MatKhau"] = r["MatKhau"];
                rNew["Quyen"] = r["Quyen"];
                
                if (r["MaNV"] != DBNull.Value)
                {
                    string maNV = r["MaNV"].ToString();
                    rNew["MaNV"] = maNV;
                    DataRow[] rowsNV = dtNV.Select("MaNV = '" + maNV.Replace("'", "''") + "'");
                    if (rowsNV.Length > 0)
                    {
                        rNew["TenNV"] = rowsNV[0]["TenNV"];
                    }
                }
                result.Rows.Add(rNew);
            }
            return result;
        }

        public DataRow[] getFilter_TKDayDu(string strFilter)
        {
            DataTable dt = LayDanhSachTaiKhoanDayDu();
            return dt.Select(strFilter);
        }

        public bool KiemTraNhanVienDaCoTaiKhoan(string maNV)
        {
            bool kq = false;
            DataRow[] rows = db.getTable("TaiKhoan").Select("MaNV = '" + maNV.Replace("'", "''") + "'");
            if (rows.Length > 0)
            {
                kq = true;
            }
            return kq;
        }

        public Boolean TenDangNhap_not_Exist(string tenDN)
        {
            Boolean kq = true;
            DataRow[] rows = db.getTable("TaiKhoan").Select("TenDangNhap = '" + tenDN.Replace("'", "''") + "'");
            if (rows.Length > 0)
            {
                kq = false;
            }
            return kq;
        }

        public Boolean add_New_TK(TaiKhoanDTO tk)
        {
            Boolean kq = false;
            if (TenDangNhap_not_Exist(tk.TenDangNhap))
            {
                DataRow r = db.getTable("TaiKhoan").NewRow();
                r["TenDangNhap"] = tk.TenDangNhap;
                r["MatKhau"] = tk.MatKhau;
                r["Quyen"] = tk.Quyen;
                if (!string.IsNullOrEmpty(tk.MaNV))
                    r["MaNV"] = tk.MaNV;
                else
                    r["MaNV"] = DBNull.Value;
                
                db.addRow("TaiKhoan", r);
                kq = true;
            }
            return kq;
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
                if (!string.IsNullOrEmpty(tk.MaNV))
                    r["MaNV"] = tk.MaNV;
                else
                    r["MaNV"] = DBNull.Value;
                r.EndEdit();

                try
                {
                    db.update("TaiKhoan");
                    kq = true;
                }
                catch { }
            }
            return kq;
        }

        public Boolean delete_TK(string tenDN)
        {
            Boolean kq = false;
            if (!TenDangNhap_not_Exist(tenDN))
            {
                db.deleteRow("TaiKhoan", "TenDangNhap = '" + tenDN.Replace("'", "''") + "'");
                kq = true;
            }
            return kq;
        }

        public Boolean DangKy(TaiKhoanDTO tk)
        {
            Boolean kq = false;
            tk.Quyen = "NhÃ¢n viÃªn"; 
            if (add_New_TK(tk))
            {
                kq = true;
            }
            return kq;
        }
    }
}

```

## DAL\MyDatabase.cs
```csharp
using System;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class MyDatabase
    {
        private static MyDatabase _instance = null;
        public static MyDatabase Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MyDatabase();
                }
                return _instance;
            }
        }

        private SqlConnection conn = null;
        private DataSet ds = null;

        private SqlDataAdapter daTaiKhoan = null;
        private SqlDataAdapter daKhachHang = null;
        private SqlDataAdapter daNhanVien = null;
        private SqlDataAdapter daNhaCungCap = null;
        private SqlDataAdapter daSanPham = null;
        private SqlDataAdapter daHoaDon = null;
        private SqlDataAdapter daChiTietHD = null;

        private MyDatabase()
        {
            conn = new SqlConnection(Properties.Settings.Default.strconnect);
            ds = new DataSet();

            daTaiKhoan = new SqlDataAdapter("SELECT * FROM TaiKhoan", conn);
            new SqlCommandBuilder(daTaiKhoan);
            daTaiKhoan.Fill(ds, "TaiKhoan");

            daKhachHang = new SqlDataAdapter("SELECT * FROM KhachHang", conn);
            new SqlCommandBuilder(daKhachHang);
            daKhachHang.Fill(ds, "KhachHang");

            daNhanVien = new SqlDataAdapter("SELECT * FROM NhanVien", conn);
            new SqlCommandBuilder(daNhanVien);
            daNhanVien.Fill(ds, "NhanVien");

            daNhaCungCap = new SqlDataAdapter("SELECT * FROM NhaCungCap", conn);
            new SqlCommandBuilder(daNhaCungCap);
            daNhaCungCap.Fill(ds, "NhaCungCap");

            daSanPham = new SqlDataAdapter("SELECT * FROM SanPham", conn);
            new SqlCommandBuilder(daSanPham);
            daSanPham.Fill(ds, "SanPham");

            daHoaDon = new SqlDataAdapter("SELECT * FROM HoaDon", conn);
            new SqlCommandBuilder(daHoaDon);
            daHoaDon.Fill(ds, "HoaDon");

            daChiTietHD = new SqlDataAdapter("SELECT * FROM ChiTietHoaDon", conn);
            new SqlCommandBuilder(daChiTietHD);
            daChiTietHD.Fill(ds, "ChiTietHoaDon");

            DataRelation rel_NhanVien_TaiKhoan = new DataRelation("NhanVien_TaiKhoan",
                ds.Tables["NhanVien"].Columns["MaNV"],
                ds.Tables["TaiKhoan"].Columns["MaNV"]);
            ds.Relations.Add(rel_NhanVien_TaiKhoan);

            DataRelation rel_NhaCungCap_SanPham = new DataRelation("NhaCungCap_SanPham",
                ds.Tables["NhaCungCap"].Columns["MaNCC"],
                ds.Tables["SanPham"].Columns["MaNCC"]);
            ds.Relations.Add(rel_NhaCungCap_SanPham);

            DataRelation rel_KhachHang_HoaDon = new DataRelation("KhachHang_HoaDon",
                ds.Tables["KhachHang"].Columns["MaKH"],
                ds.Tables["HoaDon"].Columns["MaKH"]);
            ds.Relations.Add(rel_KhachHang_HoaDon);

            DataRelation rel_NhanVien_HoaDon = new DataRelation("NhanVien_HoaDon",
                ds.Tables["NhanVien"].Columns["MaNV"],
                ds.Tables["HoaDon"].Columns["MaNV"]);
            ds.Relations.Add(rel_NhanVien_HoaDon);

            DataRelation rel_HoaDon_ChiTietHoaDon = new DataRelation("HoaDon_ChiTietHoaDon",
                ds.Tables["HoaDon"].Columns["MaHD"],
                ds.Tables["ChiTietHoaDon"].Columns["MaHD"]);
            ds.Relations.Add(rel_HoaDon_ChiTietHoaDon);

            DataRelation rel_SanPham_ChiTietHoaDon = new DataRelation("SanPham_ChiTietHoaDon",
                ds.Tables["SanPham"].Columns["MaSP"],
                ds.Tables["ChiTietHoaDon"].Columns["MaSP"]);
            ds.Relations.Add(rel_SanPham_ChiTietHoaDon);
        }

        public DataSet getDataSet()
        {
            return ds;
        }

        public DataTable getTable(string tableName)
        {
            return ds.Tables[tableName];
        }

        public void addRow(string tableName, DataRow r)
        {
                ds.Tables[tableName].Rows.Add(r);
                update(tableName);
        }

        public void update(string tableName)
        {
            try
            {
                switch (tableName)
                {
                    case "TaiKhoan": daTaiKhoan.Update(ds, "TaiKhoan"); break;
                    case "KhachHang": daKhachHang.Update(ds, "KhachHang"); break;
                    case "NhanVien": daNhanVien.Update(ds, "NhanVien"); break;
                    case "NhaCungCap": daNhaCungCap.Update(ds, "NhaCungCap"); break;
                    case "SanPham": daSanPham.Update(ds, "SanPham"); break;
                    case "HoaDon": daHoaDon.Update(ds, "HoaDon"); break;
                    case "ChiTietHoaDon": daChiTietHD.Update(ds, "ChiTietHoaDon"); break;
                }
                ds.AcceptChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Loi khi cap nhat SQL: " + ex.Message);
            }
        }

        public void deleteRow(string tableName, string condition)
        {
            DataRow[] rows = ds.Tables[tableName].Select(condition);
            if (rows.Length > 0)
            {
                foreach (DataRow r in rows)
                {
                    r.Delete();
                }
                update(tableName);
            }
        }
    }
}

```

## DTO\ChiTietHoaDonDTO.cs
```csharp
using System;

namespace DTO
{
    public class ChiTietHoaDonDTO
    {
        public string MaHD { get; set; }
        public string MaSP { get; set; }
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
        public decimal ThanhTien { get; set; }
    }
}

```

## DTO\HoaDonDTO.cs
```csharp
using System;

namespace DTO
{
    public class HoaDonDTO
    {
        public string MaHD { get; set; }
        public string MaKH { get; set; }
        public string MaNV { get; set; }
        public DateTime NgayLap { get; set; }
        public decimal TongTien { get; set; }
    }
}

```

## DTO\KhachHangDTO.cs
```csharp
using System;

namespace DTO
{
    public class KhachHangDTO
    {
        public string MaKH { get; set; }
        public string TenKH { get; set; }
        public string SoDienThoai { get; set; }
        public string DiaChi { get; set; }
        public int TrangThai { get; set; }
    }
}

```

## DTO\NhaCungCapDTO.cs
```csharp
using System;

namespace DTO
{
    public class NhaCungCapDTO
    {
        public string MaNCC { get; set; }
        public string TenNCC { get; set; }
        public string SoDienThoai { get; set; }
        public string DiaChi { get; set; }
        public int TrangThai { get; set; }
    }
}

```

## DTO\NhanVienDTO.cs
```csharp
using System;

namespace DTO
{
    public class NhanVienDTO
    {
        public string MaNV { get; set; }
        public string TenNV { get; set; }
        public string SoDienThoai { get; set; }
        public string DiaChi { get; set; }
        public string GioiTinh { get; set; }
        public DateTime NgaySinh { get; set; }
        public int TrangThai { get; set; }
    }
}

```

## DTO\SanPhamDTO.cs
```csharp
using System;

namespace DTO
{
    public class SanPhamDTO
    {
        public string MaSP { get; set; }
        public string TenSP { get; set; }
        public string MaNCC { get; set; }
        public decimal GiaBan { get; set; }
        public int SoLuongTon { get; set; }
        public int TrangThai { get; set; }
    }
}

```

## DTO\TaiKhoanDTO.cs
```csharp
using System;

namespace DTO
{
    public class TaiKhoanDTO
    {
        public string TenDangNhap { get; set; }
        public string MatKhau { get; set; }
        public string Quyen { get; set; }
        public string MaNV { get; set; }
    }
}

```

## qlybanhang\Form1.cs
```csharp
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace qlybanhang
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
    }
}

```

## qlybanhang\frmBanHang.cs
```csharp
using System;
using System.Data;
using System.Windows.Forms;
using BUS;

namespace qlybanhang
{
    public partial class frmBanHang : Form
    {
        KhachHangBUS khBus = new KhachHangBUS();
        SanPhamBUS spBus = new SanPhamBUS();
        HoaDonBUS hdBus = new HoaDonBUS();

        public string MaNV { get; set; }
        private DataTable gioHang;

        public frmBanHang()
        {
            InitializeComponent();
        }

        private void frmBanHang_Load(object sender, EventArgs e)
        {
            
            DataRow[] activeKHs = khBus.getFilter_KH("TrangThai = 1 OR TrangThai IS NULL");
            DataTable dtKH = khBus.getTableKhachHang().Clone();
            if (activeKHs.Length > 0) dtKH = activeKHs.CopyToDataTable();
            
            cboKhachHang.DataSource = dtKH;
            cboKhachHang.DisplayMember = "TenKH";
            cboKhachHang.ValueMember = "MaKH";


            LoadDataSanPham();

            KhoiTaoGioHang();
            LoadmaHD();
        }

        private void KhoiTaoGioHang()
        {
            gioHang = new DataTable();
            gioHang.Columns.Add("MaSP", typeof(string));
            gioHang.Columns.Add("TenSP", typeof(string));
            gioHang.Columns.Add("SoLuong", typeof(int));
            gioHang.Columns.Add("DonGia", typeof(decimal));
            gioHang.Columns.Add("ThanhTien", typeof(decimal));
            dgvGioHang.DataSource = gioHang;
            
            if (dgvGioHang.Columns.Count > 0)
            {
                dgvGioHang.Columns["MaSP"].HeaderText = "MÃ£ SP";
                dgvGioHang.Columns["TenSP"].HeaderText = "TÃªn sáº£n pháº©m";
                dgvGioHang.Columns["SoLuong"].HeaderText = "Sá»‘ lÆ°á»£ng";
                dgvGioHang.Columns["DonGia"].HeaderText = "ÄÆ¡n giÃ¡";
                dgvGioHang.Columns["ThanhTien"].HeaderText = "ThÃ nh tiá»n";
            }
            dgvGioHang.ReadOnly = true;
        }

        private void LoadDataSanPham()
        {
            filter_dssp();

            if (dgvSanPham.Columns.Count > 0)
            {
                if (dgvSanPham.Columns.Contains("MaSP")) dgvSanPham.Columns["MaSP"].HeaderText = "MÃ£ SP";
                if (dgvSanPham.Columns.Contains("TenSP")) dgvSanPham.Columns["TenSP"].HeaderText = "TÃªn sáº£n pháº©m";
                if (dgvSanPham.Columns.Contains("MaNCC")) dgvSanPham.Columns["MaNCC"].HeaderText = "MÃ£ NCC";
                if (dgvSanPham.Columns.Contains("GiaBan")) dgvSanPham.Columns["GiaBan"].HeaderText = "GiÃ¡ bÃ¡n";
                if (dgvSanPham.Columns.Contains("SoLuongTon")) dgvSanPham.Columns["SoLuongTon"].HeaderText = "Sá»‘ lÆ°á»£ng tá»“n";
            }
            dgvSanPham.ReadOnly = true;
        }

        private void LoadmaHD()
        {
            string maHD = hdBus.LayNextMaHD();
            txtMaHD.Text = maHD;
        }

        private void filter_dssp()
        {
            string keyword = txtTimKiemSanPham.Text.Replace("'", "''");
            string strFilter = $"(TenSP LIKE '%{keyword}%') AND (TrangThai = 1 OR TrangThai IS NULL)";

            DataRow[] rows = spBus.getFilter_SP(strFilter);
            if (rows.Length > 0)
            {
                dgvSanPham.DataSource = rows.CopyToDataTable();
            }
        }

        private void txtTimKiemSanPham_TextChanged(object sender, EventArgs e)
        {
            filter_dssp();
        }

        private void btnThemGioHang_Click(object sender, EventArgs e)
        {
            if (dgvSanPham.CurrentRow == null) return;
            DataRowView drvSP = dgvSanPham.CurrentRow.DataBoundItem as DataRowView;
            if (drvSP == null) return;

            string maSPStr = drvSP["MaSP"].ToString();
            string tenSP = drvSP["TenSP"].ToString();
            decimal donGia = Convert.ToDecimal(drvSP["GiaBan"]);
            int soLuongTon = Convert.ToInt32(drvSP["SoLuongTon"]);
            int soLuongThem = (int)nudSoLuong.Value;

            if (soLuongTon < soLuongThem)
            {
                MessageBox.Show("Sá»‘ lÆ°á»£ng tá»“n khÃ´ng Ä‘á»§!");
                return;
            }
            
            // Check if exist
            DataRow[] existing = gioHang.Select("MaSP = '" + maSPStr.Replace("'", "''") + "'");
            if (existing.Length > 0)
            {
                int soLuongHienTai = Convert.ToInt32(existing[0]["SoLuong"]);
                existing[0]["SoLuong"] = soLuongHienTai + soLuongThem;
                existing[0]["ThanhTien"] = (soLuongHienTai + soLuongThem) * donGia;
            }
            else
            {
                DataRow r = gioHang.NewRow();
                r["MaSP"] = maSPStr;
                r["TenSP"] = tenSP;
                r["SoLuong"] = soLuongThem;
                r["DonGia"] = donGia;
                r["ThanhTien"] = soLuongThem * donGia;
                gioHang.Rows.Add(r);
            }

            // Trá»« sá»‘ lÆ°á»£ng tá»“n ngay trÃªn UI (cáº­p nháº­t vÃ o Dataset global)
            DataRow[] globalRows = spBus.getTableSanPham().Select("MaSP = '" + maSPStr.Replace("'", "''") + "'");
            if (globalRows.Length > 0)
            {
                globalRows[0].BeginEdit();
                globalRows[0]["SoLuongTon"] = soLuongTon - soLuongThem;
                globalRows[0].EndEdit();
            }
            filter_dssp();

            CapNhatTongTien();
        }

        private void btnXoaGioHang_Click(object sender, EventArgs e)
        {
            if (dgvGioHang.CurrentRow == null) return;
            DataRowView drvGH = dgvGioHang.CurrentRow.DataBoundItem as DataRowView;
            if (drvGH != null)
            {
                string maSPStr = drvGH["MaSP"].ToString();
                int soLuongTraLai = Convert.ToInt32(drvGH["SoLuong"]);

                // HoÃ n láº¡i sá»‘ lÆ°á»£ng tá»“n trÃªn UI
                DataRow[] rowsSP = spBus.getTableSanPham().Select("MaSP = '" + maSPStr.Replace("'", "''") + "'");
                if (rowsSP.Length > 0)
                {
                    DataRow rSP = rowsSP[0];
                    rSP.BeginEdit();
                    int tonHienTai = Convert.ToInt32(rSP["SoLuongTon"]);
                    rSP["SoLuongTon"] = tonHienTai + soLuongTraLai;
                    rSP.EndEdit();
                }

                drvGH.Row.Delete();
                gioHang.AcceptChanges();
                filter_dssp();
                CapNhatTongTien();
            }
        }

        private Boolean checkInputThanhToan()
        {
            Boolean kq = true;
            if (gioHang.Rows.Count == 0)
            {
                kq = false;
                MessageBox.Show("Giá» hÃ ng trá»‘ng!");
            }
            else if (cboKhachHang.SelectedIndex < 0)
            {
                kq = false;
                cboKhachHang.Focus();
                MessageBox.Show("Vui lÃ²ng chá»n khÃ¡ch hÃ ng!");
            }
            else if (string.IsNullOrWhiteSpace(txtMaHD.Text))
            {
                kq = false;
                txtMaHD.Focus();
                MessageBox.Show("Vui lÃ²ng nháº­p mÃ£ hÃ³a Ä‘Æ¡n!");
            }
            return kq;
        }

        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            if (!checkInputThanhToan()) return;

            if (!hdBus.MaHD_not_Exist(txtMaHD.Text))
            {
                MessageBox.Show("MÃ£ hÃ³a Ä‘Æ¡n Ä‘Ã£ tá»“n táº¡i!");
                txtMaHD.Focus();
                return;
            }

            try
            {
                string maKH = cboKhachHang.SelectedValue.ToString();
                string maHD = txtMaHD.Text.Trim();
                hdBus.TaoHoaDon(maHD, maKH, MaNV, gioHang);
                MessageBox.Show("Thanh toÃ¡n thÃ nh cÃ´ng!", "ThÃ´ng bÃ¡o");
                
                gioHang.Rows.Clear();
                txtMaHD.Clear();
                CapNhatTongTien();
                spBus = new SanPhamBUS(); 
                LoadDataSanPham();
                LoadmaHD();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lá»—i: " + ex.Message, "Lá»—i");
            }
        }

        private void CapNhatTongTien()
        {
            decimal tong = 0;
            foreach (DataRow r in gioHang.Rows)
                tong += Convert.ToDecimal(r["ThanhTien"]);
            lblTongTien.Text = "Tá»•ng tiá»n: " + tong.ToString("N0") + " VNÄ";
        }
    }
}

```

## qlybanhang\frmChiTietHoaDon.cs
```csharp
using System;
using System.Data;
using System.Windows.Forms;
using BUS;

namespace qlybanhang
{
    public partial class frmChiTietHoaDon : Form
    {
        private string maHD;
        ChiTietHoaDonBUS cthdBus = new ChiTietHoaDonBUS();
        SanPhamBUS spBus = new SanPhamBUS();

        public frmChiTietHoaDon(string maHD)
        {
            InitializeComponent();
            this.maHD = maHD;
        }

        private void frmChiTietHoaDon_Load(object sender, EventArgs e)
        {
            txtMaHD.Text = maHD;
            LoadSanPham();
            LoadData();
        }

        private void LoadSanPham()
        {
            DataTable dt = spBus.getTableSanPham();
            cboSanPham.DataSource = dt;
            cboSanPham.DisplayMember = "TenSP";
            cboSanPham.ValueMember = "MaSP";
        }

        private void LoadData()
        {
            dgvChiTiet.DataSource = cthdBus.LayDanhSachChiTietHDDayDu(maHD);
            if (dgvChiTiet.Columns.Count > 0)
            {
                dgvChiTiet.Columns["MaHD"].Visible = false;
                dgvChiTiet.Columns["MaSP"].HeaderText = "MÃ£ sáº£n pháº©m";
                dgvChiTiet.Columns["TenSP"].HeaderText = "TÃªn sáº£n pháº©m";
                dgvChiTiet.Columns["SoLuong"].HeaderText = "Sá»‘ lÆ°á»£ng";
                dgvChiTiet.Columns["DonGia"].HeaderText = "ÄÆ¡n giÃ¡";
                dgvChiTiet.Columns["ThanhTien"].HeaderText = "ThÃ nh tiá»n";
            }
        }

        private void cboSanPham_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboSanPham.SelectedValue != null && cboSanPham.SelectedValue is int == false)
            {
                DataRowView drv = (DataRowView)cboSanPham.SelectedItem;
                txtDonGia.Text = drv["GiaBan"].ToString();
            }
            else if (cboSanPham.SelectedValue != null)
            {
                
                string maSP = cboSanPham.SelectedValue.ToString();
                DataRow[] rows = spBus.getTableSanPham().Select("MaSP = '" + maSP.Replace("'", "''") + "'");
                if (rows.Length > 0)
                {
                    txtDonGia.Text = rows[0]["GiaBan"].ToString();
                }
            }
        }

        private void dgvChiTiet_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvChiTiet.Rows.Count) return;
            var dgvRow = dgvChiTiet.Rows[e.RowIndex];
            if (dgvRow.IsNewRow) return;

            DataGridViewRow row = dgvChiTiet.Rows[e.RowIndex];
           
            if (row == null) return;
            cboSanPham.SelectedValue = row.Cells["MaSP"].Value.ToString();
            nudSoLuong.Value = Convert.ToDecimal(row.Cells["SoLuong"].Value);
            txtDonGia.Text = row.Cells["DonGia"].Value.ToString();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (cboSanPham.SelectedValue == null) return;
            string maSP = cboSanPham.SelectedValue.ToString();
            int soLuong = (int)nudSoLuong.Value;
            decimal donGia = Convert.ToDecimal(txtDonGia.Text);

            // Kiá»ƒm tra tá»“n kho
            DataRow[] rows = spBus.getTableSanPham().Select("MaSP = '" + maSP.Replace("'", "''") + "'");
            if (rows.Length > 0)
            {
                int tonKho = Convert.ToInt32(rows[0]["SoLuongTon"]);
                if (soLuong > tonKho)
                {
                    MessageBox.Show("Sá»‘ lÆ°á»£ng trong kho khÃ´ng Ä‘á»§ (chá»‰ cÃ²n " + tonKho + ")!", "Cáº£nh bÃ¡o", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            bool result = cthdBus.ThemChiTiet(maHD, maSP, soLuong, donGia);
            if (result)
            {
                MessageBox.Show("ThÃªm sáº£n pháº©m vÃ o hÃ³a Ä‘Æ¡n thÃ nh cÃ´ng!");
                LoadData();
            }
            else
            {
                MessageBox.Show("Sáº£n pháº©m Ä‘Ã£ tá»“n táº¡i trong hÃ³a Ä‘Æ¡n. Vui lÃ²ng sá»­ dá»¥ng tÃ­nh nÄƒng Cáº­p nháº­t!");
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (cboSanPham.SelectedValue == null) return;
            string maSP = cboSanPham.SelectedValue.ToString();
            int soLuongMoi = (int)nudSoLuong.Value;
            decimal donGiaMoi = Convert.ToDecimal(txtDonGia.Text);
         
            bool result = cthdBus.SuaChiTiet(maHD, maSP, soLuongMoi, donGiaMoi);
            if (result)
            {
                MessageBox.Show("Cáº­p nháº­t sá»‘ lÆ°á»£ng thÃ nh cÃ´ng!");
                LoadData();
            }
            else
            {
                MessageBox.Show("Cáº­p nháº­t tháº¥t báº¡i. Sáº£n pháº©m khÃ´ng cÃ³ trong hÃ³a Ä‘Æ¡n!");
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (cboSanPham.SelectedValue == null) return;
            string maSP = cboSanPham.SelectedValue.ToString();
            
            DialogResult dr = MessageBox.Show("Báº¡n cÃ³ cháº¯c muá»‘n xÃ³a sáº£n pháº©m nÃ y khá»i hÃ³a Ä‘Æ¡n?", "XÃ¡c nháº­n", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                bool result = cthdBus.XoaChiTiet(maHD, maSP);
                if (result)
                {
                    MessageBox.Show("XÃ³a sáº£n pháº©m thÃ nh cÃ´ng!");
                    LoadData();
                }
                else
                {
                    MessageBox.Show("XÃ³a tháº¥t báº¡i!");
                }
            }
        }
    }
}

```

## qlybanhang\frmDangKy.cs
```csharp
using System;
using System.Windows.Forms;
using BUS;
using DTO;

namespace qlybanhang
{
    public partial class frmDangKy : Form
    {
        TaiKhoanBUS bus = new TaiKhoanBUS();

        public frmDangKy()
        {
            InitializeComponent();
        }

        private void btnDangKy_Click(object sender, EventArgs e)
        {
            string user = txtTenDangNhap.Text.Trim();
            string pass = txtMatKhau.Text.Trim();
            string confirm = txtXacNhan.Text.Trim();
            string maNV = txtMaNV.Text.Trim();

            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass) || string.IsNullOrEmpty(maNV))
            {
                MessageBox.Show("Vui lÃ²ng nháº­p Ä‘áº§y Ä‘á»§ thÃ´ng tin (bao gá»“m mÃ£ nhÃ¢n viÃªn)!", "ThÃ´ng bÃ¡o",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (pass != confirm)
            {
                MessageBox.Show("Máº­t kháº©u xÃ¡c nháº­n khÃ´ng khá»›p!", "Lá»—i",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            NhanVienBUS nvBus = new NhanVienBUS();
            if (nvBus.MaNV_not_Exist(maNV))
            {
                MessageBox.Show("MÃ£ nhÃ¢n viÃªn khÃ´ng tá»“n táº¡i trong há»‡ thá»‘ng!", "Lá»—i",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (bus.KiemTraNhanVienDaCoTaiKhoan(maNV))
            {
                MessageBox.Show("NhÃ¢n viÃªn nÃ y Ä‘Ã£ cÃ³ tÃ i khoáº£n, khÃ´ng thá»ƒ táº¡o thÃªm!", "Lá»—i",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            TaiKhoanDTO tk = new TaiKhoanDTO();
            tk.TenDangNhap = user;
            tk.MatKhau = pass;
            tk.MaNV = maNV;

            bool result = bus.DangKy(tk);
            if (result)
            {
                MessageBox.Show("ÄÄƒng kÃ½ thÃ nh cÃ´ng!", "ThÃ´ng bÃ¡o",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("TÃªn Ä‘Äƒng nháº­p Ä‘Ã£ tá»“n táº¡i!", "Lá»—i",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

```

## qlybanhang\frmDangNhap.cs
```csharp
using System;
using System.Data;
using System.Windows.Forms;
using BUS;
using DTO;

namespace qlybanhang
{
    public partial class frmDangNhap : Form
    {
        TaiKhoanBUS bus = new TaiKhoanBUS();

        public frmDangNhap()
        {
            InitializeComponent();
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            
            string tenDangNhap = txtTenDangNhap.Text.Trim();
            string matKhau = txtMatKhau.Text.Trim();

            
            if (tenDangNhap == "" || matKhau == "")
            {
                MessageBox.Show("Vui lÃ²ng nháº­p Ä‘áº§y Ä‘á»§ tÃªn Ä‘Äƒng nháº­p vÃ  máº­t kháº©u!", "ThÃ´ng bÃ¡o",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; 
            }

            DataRow row = bus.DangNhap(tenDangNhap, matKhau);

            if (row != null) 
            {
                MessageBox.Show("ÄÄƒng nháº­p thÃ nh cÃ´ng!", "ThÃ´ng bÃ¡o",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                
               
                frmMain frm = new frmMain();
                frm.TenDangNhap = tenDangNhap;           
                frm.Quyen = row["Quyen"].ToString();     
                
                this.Hide(); 

                frm.ShowDialog();

                if (frm.IsLogout == true)
                {
                    this.Show();
                    txtTenDangNhap.Clear();
                    txtMatKhau.Clear();
                    txtTenDangNhap.Focus();
                }
                else
                {
                    this.Close();
                }
            }
            else 
            {
                MessageBox.Show("Sai tÃªn Ä‘Äƒng nháº­p hoáº·c máº­t kháº©u!", "Lá»—i",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lnkDangKy_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmDangKy frm = new frmDangKy();
            frm.ShowDialog();
        }
    }
}

```

## qlybanhang\frmLichSuHoaDon.cs
```csharp
using System;
using System.Data;
using System.Windows.Forms;
using BUS;

namespace qlybanhang
{
    public partial class frmLichSuHoaDon : Form
    {
        HoaDonBUS hdBus = new HoaDonBUS();

        public frmLichSuHoaDon()
        {
            InitializeComponent();
        }

        private void frmLichSuHoaDon_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            dgvHoaDon.DataSource = hdBus.LayDanhSachHoaDonDayDu();
            if (dgvHoaDon.Columns.Count > 0)
            {
                dgvHoaDon.Columns["MaHD"].HeaderText = "MÃ£ HÄ";
                dgvHoaDon.Columns["MaKH"].Visible = false;
                dgvHoaDon.Columns["TenKH"].HeaderText = "TÃªn khÃ¡ch hÃ ng";
                dgvHoaDon.Columns["MaNV"].Visible = false;
                dgvHoaDon.Columns["TenNV"].HeaderText = "TÃªn nhÃ¢n viÃªn";
                dgvHoaDon.Columns["NgayLap"].HeaderText = "NgÃ y láº­p";
                dgvHoaDon.Columns["TongTien"].HeaderText = "Tá»•ng tiá»n";
            }
        }

        private void txtTimKiemKH_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtTimKiemKH.Text.Trim().Replace("'", "''");
            DataRow[] rows = hdBus.getFilter_HDDayDu(string.Format("TenKH LIKE '%{0}%' OR MaHD LIKE '%{0}%'", keyword));
            if (rows.Length > 0)
            {
                dgvHoaDon.DataSource = rows.CopyToDataTable();
            }
            else
            {
                dgvHoaDon.DataSource = hdBus.LayDanhSachHoaDonDayDu().Clone();
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvHoaDon.CurrentRow != null && dgvHoaDon.CurrentRow.Cells["MaHD"].Value != DBNull.Value)
            {
                string maHD = dgvHoaDon.CurrentRow.Cells["MaHD"].Value.ToString();
                DialogResult dr = MessageBox.Show("Báº¡n cÃ³ cháº¯c cháº¯n muá»‘n xÃ³a hÃ³a Ä‘Æ¡n nÃ y (cÅ©ng sáº½ khÃ´i phá»¥c láº¡i sá»‘ lÆ°á»£ng tá»“n kho sáº£n pháº©m)?", "XÃ¡c nháº­n xÃ³a", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dr == DialogResult.Yes)
                {
                    hdBus.XoaHoaDon(maHD);
                    MessageBox.Show("XÃ³a hÃ³a Ä‘Æ¡n thÃ nh cÃ´ng!", "ThÃ´ng bÃ¡o", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                }
            }
        }

        private void btnXemChiTiet_Click(object sender, EventArgs e)
        {
            if (dgvHoaDon.CurrentRow != null && dgvHoaDon.CurrentRow.Cells["MaHD"].Value != DBNull.Value)
            {
                string maHD = dgvHoaDon.CurrentRow.Cells["MaHD"].Value.ToString();
                frmChiTietHoaDon frm = new frmChiTietHoaDon(maHD);
                frm.ShowDialog();
                // Táº£i láº¡i dá»¯ liá»‡u hÃ³a Ä‘Æ¡n (do tá»•ng tiá»n cÃ³ thá»ƒ thay Ä‘á»•i sau khi sá»­a chi tiáº¿t)
                LoadData();
            }
            else
            {
                MessageBox.Show("Vui lÃ²ng chá»n má»™t hÃ³a Ä‘Æ¡n Ä‘á»ƒ xem chi tiáº¿t!", "ThÃ´ng bÃ¡o", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}

```

## qlybanhang\frmMain.cs
```csharp
using System;
using System.Windows.Forms;
using BUS;

namespace qlybanhang
{
    public partial class frmMain : Form
    {
        NhanVienBUS bus = new NhanVienBUS();
        public string TenDangNhap { get; set; }
        public string Quyen { get; set; }
        public bool IsLogout { get; set; } = false;

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            string quyenHienThi = Quyen == "quanly" ? "Quáº£n lÃ½" : "NhÃ¢n viÃªn";
            lblUserInfo.Text = "NgÆ°á»i dÃ¹ng: " + TenDangNhap + " | Quyá»n: " + quyenHienThi;

            // nhÃ¢n viÃªn khÃ´ng Ä‘Æ°á»£c vÃ o menu Quáº£n lÃ½
            if (Quyen == "nhanvien" || Quyen == "NhÃ¢n viÃªn")
            {
                mnuQuanLy.Visible = false;
            }
        }

        //  Menu Quáº£n lÃ½ 
        private void mnuSanPham_Click(object sender, EventArgs e)
        {
            frmQuanLySanPham frm = new frmQuanLySanPham();
            frm.ShowDialog();
        }

        private void mnuKhachHang_Click(object sender, EventArgs e)
        {
            frmQuanLyKhachHang frm = new frmQuanLyKhachHang();
            frm.ShowDialog();
        }

        private void mnuNhanVien_Click(object sender, EventArgs e)
        {
            frmQuanLyNhanVien frm = new frmQuanLyNhanVien();
            frm.ShowDialog();
        }

        private void mnuNhaCungCap_Click(object sender, EventArgs e)
        {
            frmQuanLyNhaCungCap frm = new frmQuanLyNhaCungCap();
            frm.ShowDialog();
        }

        private void mnuTaiKhoan_Click(object sender, EventArgs e)
        {
            frmQuanLyTaiKhoan frm = new frmQuanLyTaiKhoan();
            frm.ShowDialog();
        }

        // menu BÃ¡n hÃ ng 
        private void mnuTaoHoaDon_Click(object sender, EventArgs e)
        {
            frmBanHang frm = new frmBanHang();
            frm.MaNV = bus.LayMaNV(TenDangNhap);
            frm.ShowDialog();
        }

        // menu Lá»‹ch sá»­ 
        private void mnuXemHoaDon_Click(object sender, EventArgs e)
        {
            frmLichSuHoaDon frm = new frmLichSuHoaDon();
            frm.ShowDialog();
        }

        // menu Há»‡ thá»‘ng
        private void mnuDangXuat_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Báº¡n cÃ³ muá»‘n Ä‘Äƒng xuáº¥t?", "XÃ¡c nháº­n",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                this.IsLogout = true;
                this.Close();
            }
        }

        private void mnuThoat_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Báº¡n cÃ³ muá»‘n thoÃ¡t á»©ng dá»¥ng?", "XÃ¡c nháº­n",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
}

```

## qlybanhang\frmQuanLyKhachHang.cs
```csharp
using System;
using System.Data;
using System.Windows.Forms;
using BUS;
using DTO;

namespace qlybanhang
{
    public partial class frmQuanLyKhachHang : Form
    {
        KhachHangBUS bus = new KhachHangBUS();

        public frmQuanLyKhachHang()
        {
            InitializeComponent();
        }

        private void frmQuanLyKhachHang_Load(object sender, EventArgs e)
        {
            dgvKhachHang.CellFormatting += dgvKhachHang_CellFormatting;
            LoadData();
        }

        private void dgvKhachHang_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvKhachHang.Columns[e.ColumnIndex].Name == "TrangThai" && e.Value != null)
            {
                if (e.Value.ToString() == "1" || e.Value.ToString() == "True")
                    e.Value = "Äang giao dá»‹ch";
                else
                    e.Value = "Ngá»«ng giao dá»‹ch";
            }
        }

        private void LoadData()
        {
            DataView dv = bus.getTableKhachHang().DefaultView;
            if (!chkHienThiDaXoa.Checked)
            {
                dv.RowFilter = "TrangThai = 1 OR TrangThai IS NULL";
            }
            else
            {
                dv.RowFilter = "";
            }
            dgvKhachHang.DataSource = dv;

            if (dgvKhachHang.Columns.Count > 0)
            {
                if(dgvKhachHang.Columns.Contains("MaKH")) dgvKhachHang.Columns["MaKH"].HeaderText = "MÃ£ KH";
                if(dgvKhachHang.Columns.Contains("TenKH")) dgvKhachHang.Columns["TenKH"].HeaderText = "TÃªn khÃ¡ch hÃ ng";
                if(dgvKhachHang.Columns.Contains("SoDienThoai")) dgvKhachHang.Columns["SoDienThoai"].HeaderText = "Sá»‘ Ä‘iá»‡n thoáº¡i";
                if(dgvKhachHang.Columns.Contains("DiaChi")) dgvKhachHang.Columns["DiaChi"].HeaderText = "Äá»‹a chá»‰";
                if(dgvKhachHang.Columns.Contains("TrangThai")) dgvKhachHang.Columns["TrangThai"].HeaderText = "Tráº¡ng thÃ¡i";
            }
            dgvKhachHang.ReadOnly = true;
        }

        private void filter_dskh()
        {
            string keyword = txtTimKiem.Text.Replace("'", "''");
            string strFilter = "";
            
            if (!string.IsNullOrEmpty(keyword))
            {
                strFilter = "(TenKH LIKE '%" + keyword + "%' OR SoDienThoai LIKE '%" + keyword + "%')";
            }

            if (!chkHienThiDaXoa.Checked)
            {
                if (strFilter != "") strFilter += " AND ";
                strFilter += "(TrangThai = 1 OR TrangThai IS NULL)";
            }

            DataRow[] rows = bus.getFilter_KH(strFilter);
            if (rows.Length > 0)
            {
                dgvKhachHang.DataSource = rows.CopyToDataTable();
            }
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            filter_dskh();
        }

        private Boolean checkInput()
        {
            Boolean kq = true;
            if (string.IsNullOrEmpty(txtMaKH.Text))
            {
                kq = false;
                txtMaKH.Focus();
            }
            else if (string.IsNullOrEmpty(txtTenKH.Text))
            {
                kq = false;
                txtTenKH.Focus();
            }
            else if (string.IsNullOrEmpty(txtSoDienThoai.Text))
            {
                kq = false;
                txtSoDienThoai.Focus();
            }
            else if (string.IsNullOrEmpty(txtDiaChi.Text))
            {
                kq = false;
                txtDiaChi.Focus();
            }
            return kq;
        }

        private void dgvKhachHang_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvKhachHang.Rows.Count) return;
            var dgvRow = dgvKhachHang.Rows[e.RowIndex];
            if (dgvRow.IsNewRow) return;

            DataRowView row = dgvRow.DataBoundItem as DataRowView;
            if (row == null) return;

            txtMaKH.Text = row["MaKH"].ToString();
            txtTenKH.Text = row["TenKH"].ToString();
            txtSoDienThoai.Text = row["SoDienThoai"].ToString();
            txtDiaChi.Text = row["DiaChi"].ToString();

            if (row["TrangThai"] != DBNull.Value)
                cboTrangThai.SelectedIndex = (row["TrangThai"].ToString() == "1") ? 1 : 0;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (!checkInput())
            {
                MessageBox.Show("Báº¡n chÆ°a nháº­p Ä‘á»§ dá»¯ liá»‡u!");
                return;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(txtSoDienThoai.Text, @"^0\d{9}$"))
            {
                MessageBox.Show("Sá»‘ Ä‘iá»‡n thoáº¡i khÃ´ng há»£p lá»‡! Vui lÃ²ng nháº­p 10 sá»‘ báº¯t Ä‘áº§u báº±ng 0.", "Lá»—i nháº­p liá»‡u", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoDienThoai.Focus();
                return;
            }

            KhachHangDTO kh = new KhachHangDTO();
            kh.MaKH = txtMaKH.Text;
            kh.TenKH = txtTenKH.Text;
            kh.SoDienThoai = txtSoDienThoai.Text;
            kh.DiaChi = txtDiaChi.Text;

            bool kq = bus.add_New_KH(kh);
            if (!kq)
            {
                MessageBox.Show("ThÃªm má»›i khÃ´ng thÃ nh cÃ´ng. CÃ³ thá»ƒ mÃ£ khÃ¡ch hÃ ng Ä‘Ã£ tá»“n táº¡i!");
            }
            else
            {
                LoadData();
                lammoi();
                MessageBox.Show("ThÃªm khÃ¡ch hÃ ng thÃ nh cÃ´ng!", "ThÃ´ng bÃ¡o");
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dgvKhachHang.CurrentRow == null || dgvKhachHang.CurrentRow.IsNewRow)
            {
                MessageBox.Show("ChÆ°a chá»n khÃ¡ch hÃ ng cáº§n sá»­a!", "ThÃ´ng bÃ¡o");
                return;
            }
            
            if (!checkInput())
            {
                MessageBox.Show("Báº¡n chÆ°a nháº­p Ä‘á»§ dá»¯ liá»‡u!");
                return;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(txtSoDienThoai.Text, @"^0\d{9}$"))
            {
                MessageBox.Show("Sá»‘ Ä‘iá»‡n thoáº¡i khÃ´ng há»£p lá»‡! Vui lÃ²ng nháº­p 10 sá»‘ báº¯t Ä‘áº§u báº±ng 0.", "Lá»—i nháº­p liá»‡u", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoDienThoai.Focus();
                return;
            }

            KhachHangDTO kh = new KhachHangDTO();
            kh.MaKH = txtMaKH.Text.Trim();
            kh.TenKH = txtTenKH.Text.Trim();
            kh.SoDienThoai = txtSoDienThoai.Text.Trim();
            kh.DiaChi = txtDiaChi.Text.Trim();
            kh.TrangThai = cboTrangThai.SelectedIndex;

            if (bus.update_KH(kh))
            {
                LoadData();
                lammoi();
                MessageBox.Show("Cáº­p nháº­t thÃ nh cÃ´ng!", "ThÃ´ng bÃ¡o");
            }
            else
            {
                MessageBox.Show("Cáº­p nháº­t tháº¥t báº¡i!", "Lá»—i");
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvKhachHang.CurrentRow == null || dgvKhachHang.CurrentRow.IsNewRow)
            {
                MessageBox.Show("ChÆ°a chá»n khÃ¡ch hÃ ng cáº§n thao tÃ¡c!", "ThÃ´ng bÃ¡o");
                return;
            }

            string maKH = dgvKhachHang.CurrentRow.Cells["MaKH"].Value.ToString();
            DialogResult ret = MessageBox.Show("Báº¡n cÃ³ cháº¯c cháº¯n muá»‘n ngá»«ng giao dá»‹ch vá»›i khÃ¡ch hÃ ng " + maKH + "?", "XÃ¡c nháº­n",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (ret == DialogResult.Yes)
            {
                if (bus.delete_KH(maKH))
                {
                    LoadData();
                    lammoi();
                    MessageBox.Show("ÄÃ£ chuyá»ƒn tráº¡ng thÃ¡i sang Ngá»«ng giao dá»‹ch!", "ThÃ´ng bÃ¡o");
                }
                else
                {
                    MessageBox.Show("Thao tÃ¡c tháº¥t báº¡i!", "Lá»—i");
                }
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            lammoi();
        }

        private void lammoi()
        {
            txtMaKH.Enabled = true;
            txtMaKH.Clear();
            txtTenKH.Clear();
            txtSoDienThoai.Clear();
            txtDiaChi.Clear();
            txtTimKiem.Clear();
            if (cboTrangThai != null) cboTrangThai.SelectedIndex = 1;
            dgvKhachHang.ClearSelection();
            txtMaKH.Focus();
        }

        private void chkHienThiDaXoa_CheckedChanged(object sender, EventArgs e)
        {
            filter_dskh();
        }

        private void btnXoaVinhVien_Click(object sender, EventArgs e)
        {
            if (dgvKhachHang.CurrentRow == null || dgvKhachHang.CurrentRow.IsNewRow)
            {
                MessageBox.Show("ChÆ°a chá»n khÃ¡ch hÃ ng cáº§n thao tÃ¡c!", "ThÃ´ng bÃ¡o");
                return;
            }

            string maKH = dgvKhachHang.CurrentRow.Cells["MaKH"].Value.ToString();
            DialogResult ret = MessageBox.Show("Báº¡n cÃ³ cháº¯c cháº¯n muá»‘n xÃ³a VÄ¨NH VIá»„N khÃ¡ch hÃ ng " + maKH + "? HÃ nh Ä‘á»™ng nÃ y khÃ´ng thá»ƒ hoÃ n tÃ¡c!", "Cáº£nh bÃ¡o",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (ret == DialogResult.Yes)
            {
                string msg = bus.XoaVinhVien(maKH);
                if (msg == "")
                {
                    bus = new KhachHangBUS(); // Reload tá»« DB
                    LoadData();
                    lammoi();
                    MessageBox.Show("ÄÃ£ xÃ³a vÄ©nh viá»…n khÃ¡ch hÃ ng!", "ThÃ´ng bÃ¡o");
                }
                else
                {
                    MessageBox.Show(msg, "Lá»—i", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}

```

## qlybanhang\frmQuanLyNhaCungCap.cs
```csharp
using System;
using System.Data;
using System.Windows.Forms;
using BUS;
using DTO;

namespace qlybanhang
{
    public partial class frmQuanLyNhaCungCap : Form
    {
        NhaCungCapBUS bus = new NhaCungCapBUS();

        public frmQuanLyNhaCungCap()
        {
            InitializeComponent();
        }

        private void frmQuanLyNhaCungCap_Load(object sender, EventArgs e)
        {
            dgvNhaCungCap.CellFormatting += dgvNhaCungCap_CellFormatting;
            LoadData();
        }

        private void dgvNhaCungCap_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvNhaCungCap.Columns[e.ColumnIndex].Name == "TrangThai" && e.Value != null)
            {
                if (e.Value.ToString() == "1" || e.Value.ToString() == "True")
                    e.Value = "Äang giao dá»‹ch";
                else
                    e.Value = "Ngá»«ng giao dá»‹ch";
            }
        }

        private void LoadData()
        {
            DataView dv = bus.getTableNhaCungCap().DefaultView;
            if (!chkHienThiDaXoa.Checked)
            {
                dv.RowFilter = "TrangThai = 1 OR TrangThai IS NULL";
            }
            else
            {
                dv.RowFilter = "";
            }
            dgvNhaCungCap.DataSource = dv;

            if (dgvNhaCungCap.Columns.Count > 0)
            {
                if(dgvNhaCungCap.Columns.Contains("MaNCC")) dgvNhaCungCap.Columns["MaNCC"].HeaderText = "MÃ£ nhÃ  cung cáº¥p";
                if(dgvNhaCungCap.Columns.Contains("TenNCC")) dgvNhaCungCap.Columns["TenNCC"].HeaderText = "TÃªn nhÃ  cung cáº¥p";
                if(dgvNhaCungCap.Columns.Contains("SoDienThoai")) dgvNhaCungCap.Columns["SoDienThoai"].HeaderText = "Sá»‘ Ä‘iá»‡n thoáº¡i";
                if(dgvNhaCungCap.Columns.Contains("DiaChi")) dgvNhaCungCap.Columns["DiaChi"].HeaderText = "Äá»‹a chá»‰";
                if(dgvNhaCungCap.Columns.Contains("TrangThai")) dgvNhaCungCap.Columns["TrangThai"].HeaderText = "Tráº¡ng thÃ¡i";
            }
            dgvNhaCungCap.ReadOnly = true;
        }

        private void filter_dsncc()
        {
            string keyword = txtTimKiem.Text.Replace("'", "''");
            string strFilter = "";
            
            if (!string.IsNullOrEmpty(keyword))
            {
                strFilter = "(TenNCC LIKE '%" + keyword + "%' OR MaNCC LIKE '%" + keyword + "%')";
            }

            if (!chkHienThiDaXoa.Checked)
            {
                if (strFilter != "") strFilter += " AND ";
                strFilter += "(TrangThai = 1 OR TrangThai IS NULL)";
            }

            DataRow[] rows = bus.getFilter_NhaCungCap(strFilter);
            if (rows.Length > 0)
            {
                dgvNhaCungCap.DataSource = rows.CopyToDataTable();
            }
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            filter_dsncc();
        }

        private Boolean checkInput()
        {
            Boolean kq = true;
            if (string.IsNullOrEmpty(txtMaNCC.Text))
            {
                kq = false;
                txtMaNCC.Focus();
            }
            else if (string.IsNullOrEmpty(txtTenNCC.Text))
            {
                kq = false;
                txtTenNCC.Focus();
            }
            else if (string.IsNullOrEmpty(txtSoDienThoai.Text))
            {
                kq = false;
                txtSoDienThoai.Focus();
            }
            else if (string.IsNullOrEmpty(txtDiaChi.Text))
            {
                kq = false;
                txtDiaChi.Focus();
            }
            return kq;
        }

        private void dgvNhaCungCap_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvNhaCungCap.Rows.Count) return;
            var dgvRow = dgvNhaCungCap.Rows[e.RowIndex];
            if (dgvRow.IsNewRow) return;
            DataRowView row = dgvRow.DataBoundItem as DataRowView;
            if (row == null) return;
            txtMaNCC.Text = row["MaNCC"].ToString();
            txtTenNCC.Text = row["TenNCC"].ToString();
            txtSoDienThoai.Text = row["SoDienThoai"].ToString();
            txtDiaChi.Text = row["DiaChi"].ToString();
            
            if (row["TrangThai"] != DBNull.Value)
                cboTrangThai.SelectedIndex = (row["TrangThai"].ToString() == "1") ? 1 : 0;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (!checkInput())
            {
                MessageBox.Show("Báº¡n chÆ°a nháº­p Ä‘á»§ dá»¯ liá»‡u!");
                return;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(txtSoDienThoai.Text, @"^0\d{9}$"))
            {
                MessageBox.Show("Sá»‘ Ä‘iá»‡n thoáº¡i khÃ´ng há»£p lá»‡! Vui lÃ²ng nháº­p 10 sá»‘ báº¯t Ä‘áº§u báº±ng 0.", "Lá»—i nháº­p liá»‡u", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoDienThoai.Focus();
                return;
            }

            NhaCungCapDTO ncc = new NhaCungCapDTO();
            ncc.MaNCC = txtMaNCC.Text;
            ncc.TenNCC = txtTenNCC.Text;
            ncc.SoDienThoai = txtSoDienThoai.Text;
            ncc.DiaChi = txtDiaChi.Text;

            bool kq = bus.add_New_NCC(ncc);
            if (!kq)
            {
                MessageBox.Show("ThÃªm má»›i khÃ´ng thÃ nh cÃ´ng. CÃ³ thá»ƒ mÃ£ nhÃ  cung cáº¥p Ä‘Ã£ tá»“n táº¡i!");
            }
            else
            {
                LoadData();
                lammoi();
                MessageBox.Show("ThÃªm nhÃ  cung cáº¥p thÃ nh cÃ´ng!", "ThÃ´ng bÃ¡o");
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dgvNhaCungCap.CurrentRow == null || dgvNhaCungCap.CurrentRow.IsNewRow)
            {
                MessageBox.Show("ChÆ°a chá»n nhÃ  cung cáº¥p cáº§n sá»­a!", "ThÃ´ng bÃ¡o");
                return;
            }

            if (!checkInput())
            {
                MessageBox.Show("Báº¡n chÆ°a nháº­p Ä‘á»§ dá»¯ liá»‡u!");
                return;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(txtSoDienThoai.Text, @"^0\d{9}$"))
            {
                MessageBox.Show("Sá»‘ Ä‘iá»‡n thoáº¡i khÃ´ng há»£p lá»‡! Vui lÃ²ng nháº­p 10 sá»‘ báº¯t Ä‘áº§u báº±ng 0.", "Lá»—i nháº­p liá»‡u", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoDienThoai.Focus();
                return;
            }

            NhaCungCapDTO ncc = new NhaCungCapDTO();
            ncc.MaNCC = txtMaNCC.Text.Trim();
            ncc.TenNCC = txtTenNCC.Text.Trim();
            ncc.SoDienThoai = txtSoDienThoai.Text.Trim();
            ncc.DiaChi = txtDiaChi.Text.Trim();
            ncc.TrangThai = cboTrangThai.SelectedIndex;

            if (bus.update_NCC(ncc))
            {
                LoadData();
                lammoi();
                MessageBox.Show("Cáº­p nháº­t thÃ nh cÃ´ng!", "ThÃ´ng bÃ¡o");
            }
            else
            {
                MessageBox.Show("Cáº­p nháº­t tháº¥t báº¡i!", "Lá»—i");
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvNhaCungCap.CurrentRow == null || dgvNhaCungCap.CurrentRow.IsNewRow)
            {
                MessageBox.Show("ChÆ°a chá»n nhÃ  cung cáº¥p cáº§n thao tÃ¡c!", "ThÃ´ng bÃ¡o");
                return;
            }

            string maNCC = dgvNhaCungCap.CurrentRow.Cells["MaNCC"].Value.ToString();
            DialogResult ret = MessageBox.Show("Báº¡n cÃ³ cháº¯c cháº¯n muá»‘n ngá»«ng giao dá»‹ch vá»›i nhÃ  cung cáº¥p " + maNCC + "?", "XÃ¡c nháº­n",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (ret == DialogResult.Yes)
            {
                if (bus.delete_NCC(maNCC))
                {
                    LoadData();
                    lammoi();
                    MessageBox.Show("ÄÃ£ ngá»«ng giao dá»‹ch vá»›i nhÃ  cung cáº¥p nÃ y", "ThÃ´ng bÃ¡o");
                }
                else
                {
                    MessageBox.Show("Thao tÃ¡c tháº¥t báº¡i!", "Lá»—i");
                }
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            lammoi();
        }

        private void lammoi()
        {
            txtMaNCC.Enabled = true;
            txtMaNCC.Clear();
            txtTenNCC.Clear();
            txtSoDienThoai.Clear();
            txtDiaChi.Clear();
            txtTimKiem.Clear();
            if(cboTrangThai != null) cboTrangThai.SelectedIndex = 1;
            dgvNhaCungCap.ClearSelection();
            txtMaNCC.Focus();
        }

        private void chkHienThiDaXoa_CheckedChanged(object sender, EventArgs e)
        {
            filter_dsncc();
        }

        private void btnXoaVinhVien_Click(object sender, EventArgs e)
        {
            if (dgvNhaCungCap.CurrentRow == null || dgvNhaCungCap.CurrentRow.IsNewRow)
            {
                MessageBox.Show("ChÆ°a chá»n nhÃ  cung cáº¥p cáº§n thao tÃ¡c!", "ThÃ´ng bÃ¡o");
                return;
            }

            string maNCC = dgvNhaCungCap.CurrentRow.Cells["MaNCC"].Value.ToString();
            DialogResult ret = MessageBox.Show("Báº¡n cÃ³ cháº¯c cháº¯n muá»‘n xÃ³a VÄ¨NH VIá»„N nhÃ  cung cáº¥p " + maNCC + "? HÃ nh Ä‘á»™ng nÃ y khÃ´ng thá»ƒ hoÃ n tÃ¡c!", "Cáº£nh bÃ¡o",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (ret == DialogResult.Yes)
            {
                string msg = bus.XoaVinhVien(maNCC);
                if (msg == "")
                {
                    bus = new NhaCungCapBUS(); // Reload tá»« DB
                    LoadData();
                    lammoi();
                    MessageBox.Show("ÄÃ£ xÃ³a vÄ©nh viá»…n nhÃ  cung cáº¥p!", "ThÃ´ng bÃ¡o");
                }
                else
                {
                    MessageBox.Show(msg, "Lá»—i", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}

```

## qlybanhang\frmQuanLyNhanVien.cs
```csharp
using System;
using System.Data;
using System.Windows.Forms;
using BUS;
using DTO;

namespace qlybanhang
{
    public partial class frmQuanLyNhanVien : Form
    {
        NhanVienBUS bus = new NhanVienBUS();

        public frmQuanLyNhanVien()
        {
            InitializeComponent();
        }

        private void frmQuanLyNhanVien_Load(object sender, EventArgs e)
        {
            dgvNhanVien.CellFormatting += dgvNhanVien_CellFormatting;

            LoadData();
        }

        private void dgvNhanVien_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvNhanVien.Columns[e.ColumnIndex].Name == "TrangThai" && e.Value != null)
            {
                if (e.Value.ToString() == "1" || e.Value.ToString() == "True")
                    e.Value = "Äang lÃ m";
                else
                    e.Value = "ÄÃ£ nghá»‰";
            }
        }

        private void LoadData()
        {
            DataView dv = bus.getTableNhanVien().DefaultView;
            if (!chkHienThiDaXoa.Checked)
            {
                dv.RowFilter = "TrangThai = 1 OR TrangThai IS NULL";
            }
            else
            {
                dv.RowFilter = "";
            }
            dgvNhanVien.DataSource = dv;

            if (dgvNhanVien.Columns.Count > 0)
            {
                if(dgvNhanVien.Columns.Contains("MaNV")) dgvNhanVien.Columns["MaNV"].HeaderText = "MÃ£ NV";
                if(dgvNhanVien.Columns.Contains("TenNV")) dgvNhanVien.Columns["TenNV"].HeaderText = "TÃªn nhÃ¢n viÃªn";
                if(dgvNhanVien.Columns.Contains("GioiTinh")) dgvNhanVien.Columns["GioiTinh"].HeaderText = "Giá»›i tÃ­nh";
                if(dgvNhanVien.Columns.Contains("NgaySinh")) dgvNhanVien.Columns["NgaySinh"].HeaderText = "NgÃ y sinh";
                if(dgvNhanVien.Columns.Contains("SoDienThoai")) dgvNhanVien.Columns["SoDienThoai"].HeaderText = "Sá»‘ Ä‘iá»‡n thoáº¡i";
                if(dgvNhanVien.Columns.Contains("DiaChi")) dgvNhanVien.Columns["DiaChi"].HeaderText = "Äá»‹a chá»‰";
                if(dgvNhanVien.Columns.Contains("TrangThai")) dgvNhanVien.Columns["TrangThai"].HeaderText = "Tráº¡ng thÃ¡i";
            }
            dgvNhanVien.ReadOnly = true;
            dtpNgaySinh.Value = DateTime.Now;
        }

        private void filter_dsnv()
        {
            string keyword = txtTimKiem.Text.Replace("'", "''");
            string strFilter = "";
            
            if (!string.IsNullOrEmpty(keyword))
            {
                strFilter = "(TenNV LIKE '%" + keyword + "%' OR MaNV LIKE '%" + keyword + "%')";
            }

            if (!chkHienThiDaXoa.Checked)
            {
                if (strFilter != "") strFilter += " AND ";
                strFilter += "(TrangThai = 1 OR TrangThai IS NULL)";
            }

            DataRow[] rows = bus.getFilter_NhanVien(strFilter);
            if (rows.Length > 0)
            {
                dgvNhanVien.DataSource = rows.CopyToDataTable();
            }
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            filter_dsnv();
        }

        private Boolean checkInput()
        {
            Boolean kq = true;
            if (string.IsNullOrEmpty(txtMaNV.Text))
            {
                kq = false;
                txtMaNV.Focus();
            }
            else if (string.IsNullOrEmpty(txtTenNV.Text))
            {
                kq = false;
                txtTenNV.Focus();
            }
            else if (cboGioiTinh.SelectedIndex < 0)
            {
                kq = false;
                cboGioiTinh.Focus();
            }
            else if (string.IsNullOrEmpty(txtSoDienThoai.Text))
            {
                kq = false;
                txtSoDienThoai.Focus();
            }
            else if (string.IsNullOrEmpty(txtDiaChi.Text))
            {
                kq = false;
                txtDiaChi.Focus();
            }
            return kq;
        }

        private void dgvNhanVien_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvNhanVien.Rows.Count) return;
            var dgvRow = dgvNhanVien.Rows[e.RowIndex];
            if (dgvRow.IsNewRow) return;

            DataRowView row = dgvRow.DataBoundItem as DataRowView;
            
            if (row == null) return;

            txtMaNV.Text = row["MaNV"].ToString();
            txtTenNV.Text = row["TenNV"].ToString();
            cboGioiTinh.Text = row["GioiTinh"].ToString();
            if (row["NgaySinh"] != DBNull.Value) dtpNgaySinh.Value = Convert.ToDateTime(row["NgaySinh"]);
            txtSoDienThoai.Text = row["SoDienThoai"].ToString();
            txtDiaChi.Text = row["DiaChi"].ToString();
           
            if (row["TrangThai"] != DBNull.Value)
                cboTrangThai.SelectedIndex = (row["TrangThai"].ToString() == "1") ? 1 : 0;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (!checkInput())
            {
                MessageBox.Show("Báº¡n chÆ°a nháº­p Ä‘á»§ dá»¯ liá»‡u!");
                return;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(txtSoDienThoai.Text, @"^0\d{9}$"))
            {
                MessageBox.Show("Sá»‘ Ä‘iá»‡n thoáº¡i khÃ´ng há»£p lá»‡! Vui lÃ²ng nháº­p 10 sá»‘ báº¯t Ä‘áº§u báº±ng 0.", "Lá»—i nháº­p liá»‡u", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoDienThoai.Focus();
                return;
            }

            NhanVienDTO nv = new NhanVienDTO();
            nv.MaNV = txtMaNV.Text;
            nv.TenNV = txtTenNV.Text;
            nv.GioiTinh = cboGioiTinh.SelectedItem.ToString();
            nv.NgaySinh = dtpNgaySinh.Value.Date;
            nv.SoDienThoai = txtSoDienThoai.Text;
            nv.DiaChi = txtDiaChi.Text;
            // TrangThai  BUS Ä‘Ã£ gÃ¡n máº·c Ä‘á»‹nh = 1

            bool kq = bus.add_New_NV(nv);
            if (!kq)
            {
                MessageBox.Show("ThÃªm má»›i khÃ´ng thÃ nh cÃ´ng. CÃ³ thá»ƒ mÃ£ nhÃ¢n viÃªn Ä‘Ã£ tá»“n táº¡i!");
            }
            else
            {
                LoadData();
                lammoi();
                MessageBox.Show("ThÃªm nhÃ¢n viÃªn thÃ nh cÃ´ng!", "ThÃ´ng bÃ¡o");
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dgvNhanVien.CurrentRow == null || dgvNhanVien.CurrentRow.IsNewRow)
            {
                MessageBox.Show("ChÆ°a chá»n nhÃ¢n viÃªn cáº§n sá»­a!", "ThÃ´ng bÃ¡o");
                return;
            }

            if (!checkInput())
            {
                MessageBox.Show("Báº¡n chÆ°a nháº­p Ä‘á»§ dá»¯ liá»‡u!");
                return;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(txtSoDienThoai.Text, @"^0\d{9}$"))
            {
                MessageBox.Show("Sá»‘ Ä‘iá»‡n thoáº¡i khÃ´ng há»£p lá»‡! Vui lÃ²ng nháº­p 10 sá»‘ báº¯t Ä‘áº§u báº±ng 0.", "Lá»—i nháº­p liá»‡u", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoDienThoai.Focus();
                return;
            }

            NhanVienDTO nv = new NhanVienDTO();
            nv.MaNV = txtMaNV.Text.Trim();
            nv.TenNV = txtTenNV.Text.Trim();
            nv.GioiTinh = cboGioiTinh.SelectedItem.ToString();
            nv.NgaySinh = dtpNgaySinh.Value.Date;
            nv.SoDienThoai = txtSoDienThoai.Text.Trim();
            nv.DiaChi = txtDiaChi.Text.Trim();
            nv.TrangThai = cboTrangThai.SelectedIndex; // 1 hoáº·c 0

            if (bus.update_NV(nv))
            {
                LoadData();
                lammoi();
                MessageBox.Show("Cáº­p nháº­t thÃ nh cÃ´ng!", "ThÃ´ng bÃ¡o");
            }
            else
            {
                MessageBox.Show("Cáº­p nháº­t tháº¥t báº¡i!", "Lá»—i");
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvNhanVien.CurrentRow == null || dgvNhanVien.CurrentRow.IsNewRow)
            {
                MessageBox.Show("ChÆ°a chá»n nhÃ¢n viÃªn cáº§n thao tÃ¡c!", "ThÃ´ng bÃ¡o");
                return;
            }

            string maNV = dgvNhanVien.CurrentRow.Cells["MaNV"].Value.ToString();
            DialogResult ret = MessageBox.Show("Báº¡n cÃ³ cháº¯c cháº¯n muá»‘n cho nhÃ¢n viÃªn " + maNV + " nghá»‰ viá»‡c?", "XÃ¡c nháº­n",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (ret == DialogResult.Yes)
            {
                if (bus.delete_NV(maNV))
                {
                    LoadData();
                    lammoi();
                    MessageBox.Show("NhÃ¢n viÃªn Ä‘Ã£ Ä‘Æ°á»£c chuyá»ƒn sang tráº¡ng thÃ¡i ÄÃ£ nghá»‰!", "ThÃ´ng bÃ¡o");
                }
                else
                {
                    MessageBox.Show("Thao tÃ¡c tháº¥t báº¡i!", "Lá»—i");
                }
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            lammoi();
        }

        private void lammoi()
        {
            txtMaNV.Enabled = true;
            txtMaNV.Clear();
            txtTenNV.Clear();
            if(cboGioiTinh.Items.Count > 0) cboGioiTinh.SelectedIndex = -1;
            dtpNgaySinh.Value = DateTime.Now;
            txtSoDienThoai.Clear();
            txtDiaChi.Clear();
            txtTimKiem.Clear();
            if(cboTrangThai != null) cboTrangThai.SelectedIndex = 1;
            dgvNhanVien.ClearSelection();
            txtMaNV.Focus();
        }

        private void chkHienThiDaXoa_CheckedChanged(object sender, EventArgs e)
        {
            filter_dsnv();
        }

        private void btnXoaVinhVien_Click(object sender, EventArgs e)
        {
            if (dgvNhanVien.CurrentRow == null || dgvNhanVien.CurrentRow.IsNewRow)
            {
                MessageBox.Show("ChÆ°a chá»n nhÃ¢n viÃªn cáº§n thao tÃ¡c!", "ThÃ´ng bÃ¡o");
                return;
            }

            string maNV = dgvNhanVien.CurrentRow.Cells["MaNV"].Value.ToString();
            DialogResult ret = MessageBox.Show("Báº¡n cÃ³ cháº¯c cháº¯n muá»‘n xÃ³a VÄ¨NH VIá»„N nhÃ¢n viÃªn " + maNV + "? HÃ nh Ä‘á»™ng nÃ y khÃ´ng thá»ƒ hoÃ n tÃ¡c vÃ  sáº½ xÃ³a luÃ´n tÃ i khoáº£n tÆ°Æ¡ng á»©ng!", "Cáº£nh bÃ¡o",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (ret == DialogResult.Yes)
            {
                string msg = bus.XoaVinhVien(maNV);
                if (msg == "")
                {
                    bus = new NhanVienBUS(); // Reload tá»« DB
                    LoadData();
                    lammoi();
                    MessageBox.Show("ÄÃ£ xÃ³a vÄ©nh viá»…n nhÃ¢n viÃªn vÃ  tÃ i khoáº£n!", "ThÃ´ng bÃ¡o");
                }
                else
                {
                    MessageBox.Show(msg, "Lá»—i", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}

```

## qlybanhang\frmQuanLySanPham.cs
```csharp
using System;
using System.Data;
using System.Windows.Forms;
using BUS;
using DTO;

namespace qlybanhang
{
    public partial class frmQuanLySanPham : Form
    {
        SanPhamBUS bus = new SanPhamBUS();
        NhaCungCapBUS nccBus = new NhaCungCapBUS();

        public frmQuanLySanPham()
        {
            InitializeComponent();
        }

        private void frmQuanLySanPham_Load(object sender, EventArgs e)
        {
            dgvSanPham.CellFormatting += dgvSanPham_CellFormatting;
            LoadData();
            
            cboNhaCungCap.DataSource = nccBus.LayDanhSachNCCDangHoatDong();
            cboNhaCungCap.DisplayMember = "TenNCC";
            cboNhaCungCap.ValueMember = "MaNCC";
        }

        private void dgvSanPham_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvSanPham.Columns[e.ColumnIndex].Name == "TrangThai" && e.Value != null)
            {
                if (e.Value.ToString() == "1" || e.Value.ToString() == "True")
                    e.Value = "Äang bÃ¡n";
                else
                    e.Value = "Ngá»«ng kinh doanh";
            }
        }

        private void LoadData()
        {
            DataView dv = bus.getTableSanPham().DefaultView;
            if (!chkHienThiDaXoa.Checked)
            {
                dv.RowFilter = "TrangThai = 1 OR TrangThai IS NULL";
            }
            else
            {
                dv.RowFilter = "";
            }
            dgvSanPham.DataSource = dv;

            if (dgvSanPham.Columns.Count > 0)
            {
                if(dgvSanPham.Columns.Contains("MaSP")) dgvSanPham.Columns["MaSP"].HeaderText = "MÃ£ SP";
                if(dgvSanPham.Columns.Contains("TenSP")) dgvSanPham.Columns["TenSP"].HeaderText = "TÃªn sáº£n pháº©m";
                if(dgvSanPham.Columns.Contains("MaNCC")) dgvSanPham.Columns["MaNCC"].HeaderText = "MÃ£ NCC";
                if(dgvSanPham.Columns.Contains("GiaBan")) dgvSanPham.Columns["GiaBan"].HeaderText = "GiÃ¡ bÃ¡n";
                if(dgvSanPham.Columns.Contains("SoLuongTon")) dgvSanPham.Columns["SoLuongTon"].HeaderText = "Sá»‘ lÆ°á»£ng tá»“n";
                if(dgvSanPham.Columns.Contains("TrangThai")) dgvSanPham.Columns["TrangThai"].HeaderText = "Tráº¡ng thÃ¡i";
            }
            dgvSanPham.ReadOnly = true;
        }

        private void filter_dssp()
        {
            string keyword = txtTimKiem.Text.Replace("'", "''");
            string strFilter = "";
            
            if (!string.IsNullOrEmpty(keyword))
            {
                strFilter = "(TenSP LIKE '%" + keyword + "%' OR MaSP LIKE '%" + keyword + "%')";
            }

            if (!chkHienThiDaXoa.Checked)
            {
                if (strFilter != "") strFilter += " AND ";
                strFilter += "(TrangThai = 1 OR TrangThai IS NULL)";
            }

            DataRow[] rows = bus.getFilter_SP(strFilter);
            if (rows.Length > 0)
            {
                dgvSanPham.DataSource = rows.CopyToDataTable();
            }
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            filter_dssp();
        }

        private Boolean checkInput()
        {
            Boolean kq = true;
            if (string.IsNullOrEmpty(txtMaSP.Text))
            {
                kq = false;
                txtMaSP.Focus();
            }
            else if (string.IsNullOrEmpty(txtTenSP.Text))
            {
                kq = false;
                txtTenSP.Focus();
            }
            else if (cboNhaCungCap.SelectedIndex < 0)
            {
                kq = false;
                cboNhaCungCap.Focus();
            }
            return kq;
        }

        private void dgvSanPham_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvSanPham.Rows.Count) return;
            var dgvRow = dgvSanPham.Rows[e.RowIndex];
            if (dgvRow.IsNewRow) return;

            DataRowView row = dgvRow.DataBoundItem as DataRowView;
            if (row == null) return;

            txtMaSP.Text = row["MaSP"].ToString();
            txtTenSP.Text = row["TenSP"].ToString();
            cboNhaCungCap.SelectedValue = row["MaNCC"];
            nudGiaBan.Value = Convert.ToDecimal(row["GiaBan"]);
            nudSoLuongTon.Value = Convert.ToDecimal(row["SoLuongTon"]);

            if (row["TrangThai"] != DBNull.Value)
                cboTrangThai.SelectedIndex = (row["TrangThai"].ToString() == "1" ) ? 1 : 0;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (!checkInput())
            {
                MessageBox.Show("Báº¡n chÆ°a nháº­p Ä‘á»§ dá»¯ liá»‡u!");
                return;
            }

            SanPhamDTO sp = new SanPhamDTO();
            sp.MaSP = txtMaSP.Text;
            sp.TenSP = txtTenSP.Text;
            sp.MaNCC = cboNhaCungCap.SelectedValue.ToString();
            sp.GiaBan = nudGiaBan.Value;
            sp.SoLuongTon = Convert.ToInt32(nudSoLuongTon.Value);

            bool kq = bus.add_New_SP(sp);
            if (!kq)
            {
                MessageBox.Show("ThÃªm má»›i khÃ´ng thÃ nh cÃ´ng. CÃ³ thá»ƒ mÃ£ sáº£n pháº©m Ä‘Ã£ tá»“n táº¡i!");
            }
            else
            {
                LoadData();
                lammoi();
                MessageBox.Show("ThÃªm sáº£n pháº©m thÃ nh cÃ´ng!", "ThÃ´ng bÃ¡o");
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dgvSanPham.CurrentRow == null || dgvSanPham.CurrentRow.IsNewRow)
            {
                MessageBox.Show("ChÆ°a chá»n sáº£n pháº©m cáº§n sá»­a!", "ThÃ´ng bÃ¡o");
                return;
            }

            if (!checkInput())
            {
                MessageBox.Show("Báº¡n chÆ°a nháº­p Ä‘á»§ dá»¯ liá»‡u!");
                return;
            }

            SanPhamDTO sp = new SanPhamDTO();
            sp.MaSP = txtMaSP.Text.Trim();
            sp.TenSP = txtTenSP.Text.Trim();
            sp.MaNCC = cboNhaCungCap.SelectedValue.ToString();
            sp.GiaBan = nudGiaBan.Value;
            sp.SoLuongTon = Convert.ToInt32(nudSoLuongTon.Value);
            sp.TrangThai = cboTrangThai.SelectedIndex;

            if (bus.update_SP(sp))
            {
                LoadData();
                lammoi();
                MessageBox.Show("Cáº­p nháº­t thÃ nh cÃ´ng!", "ThÃ´ng bÃ¡o");
            }
            else
            {
                MessageBox.Show("Cáº­p nháº­t tháº¥t báº¡i!", "Lá»—i");
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvSanPham.CurrentRow == null || dgvSanPham.CurrentRow.IsNewRow)
            {
                MessageBox.Show("ChÆ°a chá»n sáº£n pháº©m cáº§n thao tÃ¡c!", "ThÃ´ng bÃ¡o");
                return;
            }

            string maSP = dgvSanPham.CurrentRow.Cells["MaSP"].Value.ToString();
            DialogResult ret = MessageBox.Show("Báº¡n cÃ³ cháº¯c cháº¯n muá»‘n ngá»«ng kinh doanh sáº£n pháº©m " + maSP + "?", "XÃ¡c nháº­n",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (ret == DialogResult.Yes)
            {
                if (bus.delete_SP(maSP))
                {
                    LoadData();
                    lammoi();
                    MessageBox.Show("ÄÃ£ chuyá»ƒn tráº¡ng thÃ¡i sang Ngá»«ng kinh doanh!", "ThÃ´ng bÃ¡o");
                }
                else
                {
                    MessageBox.Show("Thao tÃ¡c tháº¥t báº¡i!", "Lá»—i");
                }
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            lammoi();
        }

        private void lammoi()
        {
            txtMaSP.Enabled = true;
            txtMaSP.Clear();
            txtTenSP.Clear();
            if (cboNhaCungCap.Items.Count > 0)
                cboNhaCungCap.SelectedIndex = 0;
            nudGiaBan.Value = 0;
            nudSoLuongTon.Value = 0;
            txtTimKiem.Clear();
            if (cboTrangThai != null) cboTrangThai.SelectedIndex = 1;
            dgvSanPham.ClearSelection();
            txtMaSP.Focus();
        }

        private void chkHienThiDaXoa_CheckedChanged(object sender, EventArgs e)
        {
            filter_dssp();
        }

        private void btnXoaVinhVien_Click(object sender, EventArgs e)
        {
            if (dgvSanPham.CurrentRow == null || dgvSanPham.CurrentRow.IsNewRow)
            {
                MessageBox.Show("ChÆ°a chá»n sáº£n pháº©m cáº§n thao tÃ¡c!", "ThÃ´ng bÃ¡o");
                return;
            }

            string maSP = dgvSanPham.CurrentRow.Cells["MaSP"].Value.ToString();
            DialogResult ret = MessageBox.Show("Báº¡n cÃ³ cháº¯c cháº¯n muá»‘n xÃ³a VÄ¨NH VIá»„N sáº£n pháº©m " + maSP + "? HÃ nh Ä‘á»™ng nÃ y khÃ´ng thá»ƒ hoÃ n tÃ¡c!", "Cáº£nh bÃ¡o",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (ret == DialogResult.Yes)
            {
                string msg = bus.XoaVinhVien(maSP);
                if (msg == "")
                {
                    bus = new SanPhamBUS(); // Reload tá»« DB
                    LoadData();
                    lammoi();
                    MessageBox.Show("ÄÃ£ xÃ³a vÄ©nh viá»…n sáº£n pháº©m!", "ThÃ´ng bÃ¡o");
                }
                else
                {
                    MessageBox.Show(msg, "Lá»—i", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}

```

## qlybanhang\frmQuanLyTaiKhoan.cs
```csharp
using System;
using System.Data;
using System.Windows.Forms;
using BUS;
using DTO;

namespace qlybanhang
{
    public partial class frmQuanLyTaiKhoan : Form
    {
        TaiKhoanBUS bus = new TaiKhoanBUS();

        public frmQuanLyTaiKhoan()
        {
            InitializeComponent();
        }

        private void frmQuanLyTaiKhoan_Load(object sender, EventArgs e)
        {
            cboQuyen.Items.Add("Quáº£n lÃ½");
            cboQuyen.Items.Add("NhÃ¢n viÃªn");
            cboQuyen.SelectedIndex = 1;

            dgvTaiKhoan.CellFormatting += dgvTaiKhoan_CellFormatting;

            LoadData();
        }

        private void dgvTaiKhoan_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvTaiKhoan.Columns[e.ColumnIndex].Name == "Quyen" && e.Value != null)
            {
                string val = e.Value.ToString();
                if (val == "quanly") e.Value = "Quáº£n lÃ½";
                else if (val == "nhanvien") e.Value = "NhÃ¢n viÃªn";
            }
        }

        private void LoadData()
        {
            DataTable dtDayDu = bus.LayDanhSachTaiKhoanDayDu();
            dgvTaiKhoan.DataSource = dtDayDu;

            if (dgvTaiKhoan.Columns.Count > 0)
            {
                dgvTaiKhoan.Columns["TenDangNhap"].HeaderText = "TÃªn Ä‘Äƒng nháº­p";
                dgvTaiKhoan.Columns["MatKhau"].HeaderText = "Máº­t kháº©u";
                dgvTaiKhoan.Columns["Quyen"].HeaderText = "Quyá»n";
                dgvTaiKhoan.Columns["TenNV"].HeaderText = "TÃªn nhÃ¢n viÃªn";
                if(dgvTaiKhoan.Columns.Contains("MaNV")) dgvTaiKhoan.Columns["MaNV"].Visible = false;
            }
            dgvTaiKhoan.ReadOnly = true;
        }

        private void filter_dstk()
        {
            string keyword = txtTimKiem.Text.Replace("'", "''");
            string strFilter = "";
            if (!string.IsNullOrEmpty(keyword))
            {
                strFilter = "TenDangNhap LIKE '%" + keyword + "%' OR TenNV LIKE '%" + keyword + "%'";
            }
            
            DataRow[] rows = bus.getFilter_TKDayDu(strFilter);
            if (rows.Length > 0)
            {
                dgvTaiKhoan.DataSource = rows.CopyToDataTable();
            }
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            filter_dstk();
        }

        private Boolean checkInput()
        {
            Boolean kq = true;
            if (string.IsNullOrEmpty(txtTenDangNhap.Text))
            {
                kq = false;
                txtTenDangNhap.Focus();
            }
            else if (string.IsNullOrEmpty(txtMatKhau.Text))
            {
                kq = false;
                txtMatKhau.Focus();
            }
            return kq;
        }

        private void dgvTaiKhoan_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvTaiKhoan.Rows.Count) return;
            var dgvRow = dgvTaiKhoan.Rows[e.RowIndex];
            if (dgvRow.IsNewRow) return;
            
            DataRowView row = dgvRow.DataBoundItem as DataRowView;

            if (row == null) return;
            txtTenDangNhap.Text = row["TenDangNhap"].ToString();
            txtMatKhau.Text = row["MatKhau"].ToString();
            cboQuyen.SelectedIndex = row["Quyen"].ToString() == "quanly" ? 0 : 1;

            if (row["MaNV"] != DBNull.Value)
                txtMaNV.Text = row["MaNV"].ToString();
            else
                txtMaNV.Clear(); 
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (!checkInput())
            {
                MessageBox.Show("Báº¡n chÆ°a nháº­p Ä‘á»§ dá»¯ liá»‡u!");
                return;
            }

            string dbRole = cboQuyen.Text == "Quáº£n lÃ½" ? "quanly" : "nhanvien";
            TaiKhoanDTO tk = new TaiKhoanDTO();
            tk.TenDangNhap = txtTenDangNhap.Text;
            tk.MatKhau = txtMatKhau.Text;
            tk.Quyen = dbRole;
            tk.MaNV = txtMaNV.Text.Trim();

            bool kq = bus.add_New_TK(tk);
            if (!kq)
            {
                MessageBox.Show("ThÃªm má»›i khÃ´ng thÃ nh cÃ´ng. CÃ³ thá»ƒ tÃªn Ä‘Äƒng nháº­p Ä‘Ã£ tá»“n táº¡i!");
            }
            else
            {
                LoadData();
                lammoi();
                MessageBox.Show("ThÃªm tÃ i khoáº£n thÃ nh cÃ´ng!", "ThÃ´ng bÃ¡o");
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dgvTaiKhoan.CurrentRow == null || dgvTaiKhoan.CurrentRow.IsNewRow)
            {
                MessageBox.Show("ChÆ°a chá»n tÃ i khoáº£n cáº§n sá»­a!", "ThÃ´ng bÃ¡o");
                return;
            }

            if (!checkInput())
            {
                MessageBox.Show("Báº¡n chÆ°a nháº­p Ä‘á»§ dá»¯ liá»‡u!");
                return;
            }

            string dbRole = cboQuyen.Text == "Quáº£n lÃ½" ? "quanly" : "nhanvien";
            TaiKhoanDTO tk = new TaiKhoanDTO();
            tk.TenDangNhap = txtTenDangNhap.Text.Trim();
            tk.MatKhau = txtMatKhau.Text.Trim();
            tk.Quyen = dbRole;
            tk.MaNV = txtMaNV.Text.Trim();

            if (bus.update_TK(tk))
            {
                LoadData();
                lammoi();
                MessageBox.Show("Cáº­p nháº­t thÃ nh cÃ´ng!", "ThÃ´ng bÃ¡o");
            }
            else
            {
                MessageBox.Show("Cáº­p nháº­t tháº¥t báº¡i!", "Lá»—i");
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvTaiKhoan.CurrentRow == null || dgvTaiKhoan.CurrentRow.IsNewRow)
            {
                MessageBox.Show("ChÆ°a chá»n tÃ i khoáº£n cáº§n xoÃ¡!", "ThÃ´ng bÃ¡o");
                return;
            }

            string tenDN = dgvTaiKhoan.CurrentRow.Cells["TenDangNhap"].Value.ToString();
            DialogResult ret = MessageBox.Show("Báº¡n cÃ³ cháº¯c cháº¯n muá»‘n xoÃ¡ tÃ i khoáº£n " + tenDN + "?", "XÃ¡c nháº­n",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (ret == DialogResult.Yes)
            {
                try
                {
                    if (bus.delete_TK(tenDN))
                    {
                        LoadData();
                        lammoi();
                        MessageBox.Show("XoÃ¡ thÃ nh cÃ´ng!", "ThÃ´ng bÃ¡o");
                    }
                    else
                    {
                        MessageBox.Show("XoÃ¡ tháº¥t báº¡i!", "Lá»—i");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Lá»—i xÃ³a tÃ i khoáº£n", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            lammoi();
        }

        private void lammoi()
        {
            txtTenDangNhap.Enabled = true;
            txtTenDangNhap.Clear();
            txtMatKhau.Clear();
            if(txtMaNV != null) txtMaNV.Clear();
            cboQuyen.SelectedIndex = 1;
            txtTimKiem.Clear();
            dgvTaiKhoan.ClearSelection();
            txtTenDangNhap.Focus();
        }
    }
}

```


