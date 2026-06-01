using System;
using System.Data;
using DAL;
using DTO;

namespace BUS
{
    public class KhachHangBUS
    {
        private KhachHangDAL dal = new KhachHangDAL();

        public DataSet getDataset()
        {
            return dal.getDBtoDataset();
        }

        public DataTable getTableKhachHang()
        {
            return dal.getTable();
        }

        public DataRow[] getFilter_KhachHang(string strFilter)
        {
            return dal.getTable().Select(strFilter);
        }

        public Boolean MaKH_not_Exist(string maKH)
        {
            DataRow[] rows = dal.getTable().Select("MaKH = '" + maKH.Replace("'", "''") + "'");
            return rows.Length == 0;
        }

        public Boolean add_New_KH(KhachHangDTO kh)
        {
            if (MaKH_not_Exist(kh.MaKH))
            {
                DataRow r = dal.getTable().NewRow();
                r["MaKH"] = kh.MaKH;
                r["TenKH"] = kh.TenKH;
                r["SoDienThoai"] = kh.SoDienThoai;
                r["DiaChi"] = kh.DiaChi;
                r["TrangThai"] = 1;

                dal.addRow(r);
                return true;
            }
            return false;
        }

        public bool update_KH(KhachHangDTO kh)
        {
            DataRow[] rows = dal.getTable().Select("MaKH = '" + kh.MaKH.Replace("'", "''") + "'");
            if (rows.Length == 0) return false;

            DataRow r = rows[0];
            r.BeginEdit();
            r["TenKH"] = kh.TenKH;
            r["SoDienThoai"] = kh.SoDienThoai;
            r["DiaChi"] = kh.DiaChi;
            r.EndEdit();
            
            try 
            {
                dal.update();
                return true;
            }
            catch (DBConcurrencyException ex) { Console.WriteLine("Lỗi đồng thời: " + ex.Message); return false; }
            catch (Exception ex) { Console.WriteLine("Lỗi: " + ex.Message); return false; }
        }

        public bool delete_KH(string maKH)
        {
            // Xóa mềm
            DataRow[] rows = dal.getTable().Select("MaKH = '" + maKH.Replace("'", "''") + "'");
            if (rows.Length == 0) return false;

            DataRow r = rows[0];
            r.BeginEdit();
            r["TrangThai"] = 0;
            r.EndEdit();

            try 
            {
                dal.update();
                return true;
            }
            catch (DBConcurrencyException ex) { Console.WriteLine("Lỗi đồng thời: " + ex.Message); return false; }
            catch (Exception ex) { Console.WriteLine("Lỗi: " + ex.Message); return false; }
        }

        public string XoaVinhVien(string maKH)
        {
            if (MaKH_not_Exist(maKH)) return "Khách hàng không tồn tại!";
            HoaDonDAL hdDal = new HoaDonDAL();
            DataRow[] hdRows = hdDal.getTable().Select("MaKH = '" + maKH.Replace("'", "''") + "'");
            if (hdRows.Length > 0) return "Khách hàng đã phát sinh Hóa Đơn, không thể xóa vĩnh viễn!";
            dal.delete(maKH);
            return "";
        }
        
        public DataRow[] getFilter_KH(string strFilter)
        {
            return getFilter_KhachHang(strFilter);
        }
    }
}
