using System;
using System.Data;
using DAL;
using DTO;

namespace BUS
{
    public class SanPhamBUS
    {
        private SanPhamDAL dal = new SanPhamDAL();

        public DataSet getDataset()
        {
            return dal.getDBtoDataset();
        }

        public DataTable getTableSanPham()
        {
            return dal.getTable();
        }

        public DataRow[] getFilter_SanPham(string strFilter)
        {
            return dal.getTable().Select(strFilter);
        }

        public Boolean MaSP_not_Exist(string maSP)
        {
            DataRow[] rows = dal.getTable().Select("MaSP = '" + maSP.Replace("'", "''") + "'");
            return rows.Length == 0;
        }

        public Boolean add_New_SP(SanPhamDTO sp)
        {
            if (MaSP_not_Exist(sp.MaSP))
            {
                DataRow r = dal.getTable().NewRow();
                r["MaSP"] = sp.MaSP;
                r["TenSP"] = sp.TenSP;
                r["MaNCC"] = sp.MaNCC;
                r["GiaBan"] = sp.GiaBan;
                r["SoLuongTon"] = sp.SoLuongTon;
                r["TrangThai"] = 1;

                dal.addRow(r);
                return true;
            }
            return false;
        }

        public bool update_SP(SanPhamDTO sp)
        {
            DataRow[] rows = dal.getTable().Select("MaSP = '" + sp.MaSP.Replace("'", "''") + "'");
            if (rows.Length == 0) return false;

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
                dal.update();
                return true;
            }
            catch (DBConcurrencyException ex) { Console.WriteLine("Lỗi đồng thời: " + ex.Message); return false; }
            catch (Exception ex) { Console.WriteLine("Lỗi: " + ex.Message); return false; }
        }

        public bool delete_SP(string maSP)
        {
            // Xóa mềm
            DataRow[] rows = dal.getTable().Select("MaSP = '" + maSP.Replace("'", "''") + "'");
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

        public string XoaVinhVien(string maSP)
        {
            if (MaSP_not_Exist(maSP)) return "Sản phẩm không tồn tại!";
            ChiTietHoaDonDAL cthdDal = new ChiTietHoaDonDAL();
            DataRow[] hdRows = cthdDal.getTable().Select("MaSP = '" + maSP.Replace("'", "''") + "'");
            if (hdRows.Length > 0) return "Sản phẩm đã phát sinh Hóa Đơn, không thể xóa vĩnh viễn!";
            dal.delete(maSP);
            return "";
        }

        public DataRow[] getFilter_SP(string strFilter)
        {
            return getFilter_SanPham(strFilter);
        }
    }
}
