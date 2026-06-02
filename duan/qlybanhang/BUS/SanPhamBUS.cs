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
                kq = "Sản phẩm không tồn tại!";
            }
            else
            {
                DataRow[] hdRows = db.getTable("ChiTietHoaDon").Select("MaSP = '" + maSP.Replace("'", "''") + "'");
                if (hdRows.Length > 0)
                {
                    kq = "Sản phẩm đã phát sinh Hóa Đơn, không thể xóa vĩnh viễn!";
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
