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
                kq = "Nhân viên không tồn tại!";
            }
            else
            {
                DataRow[] hdRows = db.getTable("HoaDon").Select("MaNV = '" + maNV.Replace("'", "''") + "'");
                if (hdRows.Length > 0)
                {
                    kq = "Nhân viên đã phát sinh Hóa Đơn, không thể xóa vĩnh viễn!";
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
