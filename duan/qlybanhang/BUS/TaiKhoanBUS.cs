using System;
using System.Data;
using DAL;
using DTO;

namespace BUS
{
    public class TaiKhoanBUS
    {
        private MyDatabase db = MyDatabase.Instance;

        public DataSet getDataset()
        {
            return db.getDataSet();
        }

        public DataTable getTableTaiKhoan()
        {
            return db.getTable("TaiKhoan");
        }

        public DataRow DangNhap(string tenDangNhap, string matKhau)
        {
            DataRow kq = null;
            string filter = "TenDangNhap = '" + tenDangNhap.Replace("'", "''") + "' AND MatKhau = '" + matKhau.Replace("'", "''") + "'";
            DataRow[] rows = db.getTable("TaiKhoan").Select(filter);
            if (rows.Length > 0)
            {
                kq = rows[0];
            }
            return kq;
        }

        public DataTable LayDanhSachTaiKhoanDayDu()
        {
            DataTable dtTK = db.getTable("TaiKhoan");
            DataTable dtNV = db.getTable("NhanVien");

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
            bool kq = false;
            DataRow[] rows = db.getTable("TaiKhoan").Select("MaNV = '" + maNV.Replace("'", "''") + "'");
            if (rows.Length > 0)
            {
                kq = true;
            }
            return kq;
        }

        public Boolean TenDangNhap_not_Exist(string tenDN)
        {
            Boolean kq = true;
            DataRow[] rows = db.getTable("TaiKhoan").Select("TenDangNhap = '" + tenDN.Replace("'", "''") + "'");
            if (rows.Length > 0)
            {
                kq = false;
            }
            return kq;
        }

        public Boolean add_New_TK(TaiKhoanDTO tk)
        {
            Boolean kq = false;
            if (TenDangNhap_not_Exist(tk.TenDangNhap))
            {
                DataRow r = db.getTable("TaiKhoan").NewRow();
                r["TenDangNhap"] = tk.TenDangNhap;
                r["MatKhau"] = tk.MatKhau;
                r["Quyen"] = tk.Quyen;
                if (!string.IsNullOrEmpty(tk.MaNV))
                    r["MaNV"] = tk.MaNV;
                else
                    r["MaNV"] = DBNull.Value;
                
                db.addRow("TaiKhoan", r);
                kq = true;
            }
            return kq;
        }

        public Boolean update_TK(TaiKhoanDTO tk)
        {
            Boolean kq = false;
            DataRow[] rows = db.getTable("TaiKhoan").Select("TenDangNhap = '" + tk.TenDangNhap.Replace("'", "''") + "'");
            if (rows.Length > 0)
            {
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
                    db.update("TaiKhoan");
                    kq = true;
                }
                catch { }
            }
            return kq;
        }

        public Boolean delete_TK(string tenDN)
        {
            Boolean kq = false;
            if (!TenDangNhap_not_Exist(tenDN))
            {
                db.deleteRow("TaiKhoan", "TenDangNhap = '" + tenDN.Replace("'", "''") + "'");
                kq = true;
            }
            return kq;
        }

        public Boolean DangKy(TaiKhoanDTO tk)
        {
            Boolean kq = false;
            tk.Quyen = "Nhân viên"; 
            if (add_New_TK(tk))
            {
                kq = true;
            }
            return kq;
        }
    }
}
