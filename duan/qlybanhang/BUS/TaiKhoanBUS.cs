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
            return dal.getDBtoDataset();
        }

        public DataTable getTableTaiKhoan()
        {
            DataTable dt = dal.getTable();
            return dt;
        }

        public DataRow DangNhap(string tenDangNhap, string matKhau)
        {
            string filter = "TenDangNhap = '" + tenDangNhap.Replace("'", "''") + "' AND MatKhau = '" + matKhau.Replace("'", "''") + "'";
            DataRow[] rows = dal.TimKiemTheoDieuKien(filter);
            return rows.Length > 0 ? rows[0] : null;
        }

        public DataTable LayDanhSachTaiKhoanDayDu()
        {
            DataTable dtTK = dal.getTable();
            NhanVienDAL nvDal = new NhanVienDAL();
            DataTable dtNV = nvDal.getTable();

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

        public bool KiemTraNhanVienDaCoTaiKhoan(string maNV)
        {
            DataRow[] rows = dal.TimKiemTheoDieuKien("MaNV = '" + maNV.Replace("'", "''") + "'");
            return rows.Length > 0;
        }

        public Boolean TenDangNhap_not_Exist(string tenDN)
        {
            DataRow[] rows = dal.TimKiemTheoTenDangNhap(tenDN);
            return rows.Length == 0;
        }

        public Boolean add_New_TK(TaiKhoanDTO tk)
        {
            if (!TenDangNhap_not_Exist(tk.TenDangNhap)) return false;

            dal.Add(tk);
            return true;
        }

        public Boolean update_TK(TaiKhoanDTO tk)
        {
            if (TenDangNhap_not_Exist(tk.TenDangNhap)) return false;
            
            dal.Update(tk);
            return true;
        }

        public Boolean delete_TK(string tenDN)
        {
            if (TenDangNhap_not_Exist(tenDN)) return false;
            dal.hardDelete(tenDN);
            return true;
        }

        public Boolean DangKy(TaiKhoanDTO tk)
        {
            DataRow[] existing = dal.TimKiemTheoTenDangNhap(tk.TenDangNhap);
            if (existing.Length > 0) return false;

            tk.Quyen = "Nhân viên"; // Default role
            return add_New_TK(tk);
        }
    }
}
