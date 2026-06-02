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
                kq = "Nhà cung cấp không tồn tại!";
            }
            else
            {
                DataRow[] spRows = db.getTable("SanPham").Select("MaNCC = '" + maNCC.Replace("'", "''") + "'");
                if (spRows.Length > 0)
                {
                    kq = "Nhà cung cấp đã có Sản Phẩm, không thể xóa vĩnh viễn!";
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
