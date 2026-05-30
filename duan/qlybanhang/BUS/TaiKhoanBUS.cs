using System;
using System.Data;
using DAL;
using DTO;

namespace BUS
{
    public class TaiKhoanBUS
    {
        private TaiKhoanDAL dal = new TaiKhoanDAL();

        public DataSet getDataset()
        {
            return dal.ds;
        }

        public DataRow DangNhap(string tenDangNhap, string matKhau)
        {
            dal.reload();
            DataTable dt = dal.getTable();
            DataRow[] rows = dt.Select(
                "TenDangNhap = '" + tenDangNhap.Replace("'", "''") + "' AND MatKhau = '" + matKhau.Replace("'", "''") + "'");
            return rows.Length > 0 ? rows[0] : null;
        }

        public Boolean MaTK_not_Exist(string tenDangNhap)
        {
            Boolean kq = true;
            DataRow[] rows = dal.getTable().Select("TenDangNhap='" + tenDangNhap.Replace("'", "''") + "'");
            if (rows.Length > 0)
            {
                kq = false;
            }
            return kq;
        }

        public Boolean DangKy(TaiKhoanDTO tk)
        {
            Boolean kq = false;
            if (MaTK_not_Exist(tk.TenDangNhap))
            {
                DataRow r = dal.ds.Tables["TaiKhoan"].NewRow();
                r["TenDangNhap"] = tk.TenDangNhap;
                r["MatKhau"] = tk.MatKhau;
                r["Quyen"] = "nhanvien";
                dal.addRow(r);
                kq = true;
            }
            return kq;
        }

        public DataTable getTableTaiKhoan()
        {
            return dal.getTable();
        }

        public Boolean add_New_TK(TaiKhoanDTO tk)
        {
            Boolean kq = false;
            if (MaTK_not_Exist(tk.TenDangNhap))
            {
                DataRow r = dal.ds.Tables["TaiKhoan"].NewRow();
                r["TenDangNhap"] = tk.TenDangNhap;
                r["MatKhau"] = tk.MatKhau;
                r["Quyen"] = tk.Quyen;
                dal.addRow(r);
                kq = true;
            }
            return kq;
        }

        public DataRow[] getFilter_TK(string strFilter)
        {
            return dal.getTable().Select(strFilter);
        }

        public Boolean update_TK(TaiKhoanDTO tk)
        {
            DataRow[] rows = dal.getTable().Select("TenDangNhap = '" + tk.TenDangNhap.Replace("'", "''") + "'");
            if (rows.Length == 0)
                return false;
            
            DataRow r = rows[0];
            r.BeginEdit();
            r["MatKhau"] = tk.MatKhau;
            r["Quyen"] = tk.Quyen;
            r.EndEdit();
            dal.update();
            return true;
        }

        public Boolean delete_TK(string tenDangNhap)
        {
            if (MaTK_not_Exist(tenDangNhap)) 
                return false;
            dal.delete(tenDangNhap);
            return true;
        }
    }
}
