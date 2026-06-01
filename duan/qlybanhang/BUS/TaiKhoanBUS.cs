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
            return dal.getTable();
        }

        public DataRow DangNhap(string tenDangNhap, string matKhau)
        {
            string filter = "TenDangNhap = '" + tenDangNhap.Replace("'", "''") + "' AND MatKhau = '" + matKhau.Replace("'", "''") + "'";
            DataRow[] rows = dal.getTable().Select(filter);
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
            DataRow[] rows = dal.getTable().Select("MaNV = '" + maNV.Replace("'", "''") + "'");
            return rows.Length > 0;
        }

        public Boolean TenDangNhap_not_Exist(string tenDN)
        {
            DataRow[] rows = dal.getTable().Select("TenDangNhap = '" + tenDN.Replace("'", "''") + "'");
            return rows.Length == 0;
        }

        public Boolean add_New_TK(TaiKhoanDTO tk)
        {
            if (TenDangNhap_not_Exist(tk.TenDangNhap))
            {
                DataRow r = dal.getTable().NewRow();
                r["TenDangNhap"] = tk.TenDangNhap;
                r["MatKhau"] = tk.MatKhau;
                r["Quyen"] = tk.Quyen;
                if (!string.IsNullOrEmpty(tk.MaNV))
                    r["MaNV"] = tk.MaNV;
                else
                    r["MaNV"] = DBNull.Value;
                
                dal.addRow(r);
                return true;
            }
            return false;
        }

        public Boolean update_TK(TaiKhoanDTO tk)
        {
            DataRow[] rows = dal.getTable().Select("TenDangNhap = '" + tk.TenDangNhap.Replace("'", "''") + "'");
            if (rows.Length == 0) return false;
            
            DataRow r = rows[0];
            r.BeginEdit();
            r["MatKhau"] = tk.MatKhau;
            r["Quyen"] = tk.Quyen;
            if (!string.IsNullOrEmpty(tk.MaNV))
                r["MaNV"] = tk.MaNV;
            else
                r["MaNV"] = DBNull.Value;
            r.EndEdit();

            try
            {
                dal.update();
                return true;
            }
            catch (DBConcurrencyException) { return false; }
            catch { return false; }
        }

        public Boolean delete_TK(string tenDN)
        {
            if (TenDangNhap_not_Exist(tenDN)) return false;
            dal.delete(tenDN);
            return true;
        }

        public Boolean DangKy(TaiKhoanDTO tk)
        {
            tk.Quyen = "Nhân viên"; // Default role
            return add_New_TK(tk);
        }
    }
}
