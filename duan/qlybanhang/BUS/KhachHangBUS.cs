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
                kq = "Khách hàng không tồn tại!";
            }
            else
            {
                DataRow[] hdRows = db.getTable("HoaDon").Select("MaKH = '" + maKH.Replace("'", "''") + "'");
                if (hdRows.Length > 0)
                {
                    kq = "Khách hàng đã phát sinh Hóa Đơn, không thể xóa vĩnh viễn!";
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
