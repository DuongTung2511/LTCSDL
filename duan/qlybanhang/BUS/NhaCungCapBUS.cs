using System;
using System.Data;
using DAL;
using DTO;

namespace BUS
{
    public class NhaCungCapBUS
    {
        private NhaCungCapDAL dal = new NhaCungCapDAL();

        public DataSet getDataset()
        {
            return dal.getDBtoDataset();
        }

        public DataTable getTableNhaCungCap()
        {
            return dal.getTable();
        }

        public DataRow[] getFilter_NhaCungCap(string strFilter)
        {
            return dal.getTable().Select(strFilter);
        }

        public Boolean MaNCC_not_Exist(string maNCC)
        {
            DataRow[] rows = dal.getTable().Select("MaNCC = '" + maNCC.Replace("'", "''") + "'");
            return rows.Length == 0;
        }

        public Boolean add_New_NCC(NhaCungCapDTO ncc)
        {
            if (MaNCC_not_Exist(ncc.MaNCC))
            {
                DataRow r = dal.getTable().NewRow();
                r["MaNCC"] = ncc.MaNCC;
                r["TenNCC"] = ncc.TenNCC;
                r["SoDienThoai"] = ncc.SoDienThoai;
                r["DiaChi"] = ncc.DiaChi;
                r["TrangThai"] = 1;

                dal.addRow(r);
                return true;
            }
            return false;
        }

        public bool update_NCC(NhaCungCapDTO ncc)
        {
            DataRow[] rows = dal.getTable().Select("MaNCC = '" + ncc.MaNCC.Replace("'", "''") + "'");
            if (rows.Length == 0) return false;

            DataRow r = rows[0];
            r.BeginEdit();
            r["TenNCC"] = ncc.TenNCC;
            r["SoDienThoai"] = ncc.SoDienThoai;
            r["DiaChi"] = ncc.DiaChi;
            r.EndEdit();
            
            try 
            {
                dal.update();
                return true;
            }
            catch (DBConcurrencyException ex) { Console.WriteLine("Lỗi đồng thời: " + ex.Message); return false; }
            catch (Exception ex) { Console.WriteLine("Lỗi: " + ex.Message); return false; }
        }

        public bool delete_NCC(string maNCC)
        {
            // Xóa mềm
            DataRow[] rows = dal.getTable().Select("MaNCC = '" + maNCC.Replace("'", "''") + "'");
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

        public string XoaVinhVien(string maNCC)
        {
            if (MaNCC_not_Exist(maNCC)) return "Nhà cung cấp không tồn tại!";
            SanPhamDAL spDal = new SanPhamDAL();
            DataRow[] spRows = spDal.getTable().Select("MaNCC = '" + maNCC.Replace("'", "''") + "'");
            if (spRows.Length > 0) return "Nhà cung cấp đã có Sản Phẩm, không thể xóa vĩnh viễn!";
            dal.delete(maNCC);
            return "";
        }

        public DataTable LayDanhSachNCCDangHoatDong()
        {
            DataRow[] rows = dal.getTable().Select("TrangThai = 1 OR TrangThai IS NULL");
            if (rows.Length > 0) return rows.CopyToDataTable();
            return dal.getTable().Clone();
        }
    }
}
