using System;
using System.Data;
using DAL;
using DTO;

namespace BUS
{
    public class NhanVienBUS
    {
        private NhanVienDAL dal = new NhanVienDAL();

        public DataSet getDataset()
        {
            return dal.getDBtoDataset();
        }

        public DataTable getTableNhanVien()
        {
            return dal.getTable();
        }

        public DataRow[] getFilter_NhanVien(string strFilter)
        {
            return dal.getTable().Select(strFilter);
        }

        public Boolean MaNV_not_Exist(string maNV)
        {
            DataRow[] rows = dal.getTable().Select("MaNV = '" + maNV.Replace("'", "''") + "'");
            return rows.Length == 0;
        }

        public Boolean add_New_NV(NhanVienDTO nv)
        {
            if (MaNV_not_Exist(nv.MaNV))
            {
                DataRow r = dal.getTable().NewRow();
                r["MaNV"] = nv.MaNV;
                r["TenNV"] = nv.TenNV;
                r["GioiTinh"] = nv.GioiTinh;
                r["NgaySinh"] = nv.NgaySinh;
                r["SoDienThoai"] = nv.SoDienThoai;
                r["DiaChi"] = nv.DiaChi;
                r["TrangThai"] = 1;

                dal.addRow(r);
                return true;
            }
            return false;
        }

        public bool update_NV(NhanVienDTO nv)
        {
            DataRow[] rows = dal.getTable().Select("MaNV = '" + nv.MaNV.Replace("'", "''") + "'");
            if (rows.Length == 0) return false;

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
                dal.update();
                return true;
            }
            catch (DBConcurrencyException ex) { Console.WriteLine("Lỗi đồng thời: " + ex.Message); return false; }
            catch (Exception ex) { Console.WriteLine("Lỗi: " + ex.Message); return false; }
        }

        public bool delete_NV(string maNV)
        {
            // Xóa mềm
            DataRow[] rows = dal.getTable().Select("MaNV = '" + maNV.Replace("'", "''") + "'");
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

        public string XoaVinhVien(string maNV)
        {
            if (MaNV_not_Exist(maNV))
                return "Nhân viên không tồn tại!";

            HoaDonDAL hdDal = new HoaDonDAL();
            DataRow[] hdRows = hdDal.getTable().Select("MaNV = '" + maNV.Replace("'", "''") + "'");
            if (hdRows.Length > 0)
                return "Nhân viên đã phát sinh Hóa Đơn, không thể xóa vĩnh viễn!";

            TaiKhoanDAL tkDal = new TaiKhoanDAL();
            DataRow[] tkRows = tkDal.getTable().Select("MaNV = '" + maNV.Replace("'", "''") + "'");
            if (tkRows.Length > 0)
            {
                foreach(DataRow r in tkRows)
                {
                    tkDal.delete(r["TenDangNhap"].ToString());
                }
            }

            dal.delete(maNV);
            return ""; 
        }

        public string LayMaNV(string tenDangNhap)
        {
            TaiKhoanDAL tkDal = new TaiKhoanDAL();
            DataRow[] rows = tkDal.getTable().Select("TenDangNhap = '" + tenDangNhap.Replace("'", "''") + "'");
            if (rows.Length > 0 && rows[0]["MaNV"] != DBNull.Value)
            {
                return rows[0]["MaNV"].ToString();
            }
            return "";
        }
    }
}
