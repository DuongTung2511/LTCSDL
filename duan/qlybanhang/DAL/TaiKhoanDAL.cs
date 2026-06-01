using System;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class TaiKhoanDAL : DBContext
    {
        private SqlDataAdapter da;
        public DataSet ds;

        public TaiKhoanDAL()
        {
            ds = new DataSet();
            da = new SqlDataAdapter();
            SqlCommandBuilder cb = new SqlCommandBuilder(da);
            da.SelectCommand = new SqlCommand("SELECT * FROM TaiKhoan", conn);
            da.TableMappings.Add("Table", "TaiKhoan");
            da.Fill(ds, "TaiKhoan");
        }

        public DataSet getDBtoDataset()
        {
            return ds;
        }

        public DataTable getTable()
        {
            return ds.Tables["TaiKhoan"];
        }

        public DataRow[] TimKiemTheoTenDangNhap(string tenDangNhap)
        {
            return ds.Tables["TaiKhoan"].Select("TenDangNhap = '" + tenDangNhap.Replace("'", "''") + "'");
        }

        public DataRow[] TimKiemTheoDieuKien(string strFilter)
        {
            return ds.Tables["TaiKhoan"].Select(strFilter);
        }

        public void Add(DTO.TaiKhoanDTO tk)
        {
            try
            {
                DataRow r = ds.Tables["TaiKhoan"].NewRow();
                r["TenDangNhap"] = tk.TenDangNhap;
                r["MatKhau"] = tk.MatKhau;
                r["Quyen"] = tk.Quyen;
                if (!string.IsNullOrEmpty(tk.MaNV))
                    r["MaNV"] = tk.MaNV;
                else
                    r["MaNV"] = DBNull.Value;
                ds.Tables["TaiKhoan"].Rows.Add(r);
                da.Update(ds, "TaiKhoan");
                ds.AcceptChanges();
            }
            catch { }
        }

        public void Update(DTO.TaiKhoanDTO tk)
        {
            DataRow[] rows = TimKiemTheoTenDangNhap(tk.TenDangNhap);
            if (rows.Length > 0)
            {
                DataRow r = rows[0];
                r["MatKhau"] = tk.MatKhau;
                r["Quyen"] = tk.Quyen;
                if (!string.IsNullOrEmpty(tk.MaNV))
                    r["MaNV"] = tk.MaNV;
                else
                    r["MaNV"] = DBNull.Value;
                da.Update(ds, "TaiKhoan");
                ds.AcceptChanges();
            }
        }

        public void deleteByMaNV(string maNV)
        {
            // First we need to find the TenDangNhap associated with the maNV.
            // But TaiKhoanDAL shouldn't ideally read NhanVien table. 
            // However, doing it here is fine, or we let NhanVienDAL do it.
            // Since this method was added to support hard delete of NhanVien:
            // We can just execute a direct SQL delete, or find the account using another query.
            // The safest is direct execute because DataSet doesn't have cross-table here.
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM TaiKhoan WHERE TenDangNhap IN (SELECT TenDangNhap FROM NhanVien WHERE MaNV = @MaNV AND TenDangNhap IS NOT NULL)", conn);
                cmd.Parameters.AddWithValue("@MaNV", maNV);
                cmd.ExecuteNonQuery();
                conn.Close();
                // Then refresh dataset
                ds.Tables["TaiKhoan"].Clear();
                da.Fill(ds, "TaiKhoan");
            }
            catch { if(conn.State == ConnectionState.Open) conn.Close(); }
        }
        
        public void hardDelete(string tenDangNhap)
        {
            DataRow[] rows = TimKiemTheoTenDangNhap(tenDangNhap);
            if (rows.Length > 0)
            {
                rows[0].Delete();
                da.Update(ds, "TaiKhoan");
                ds.AcceptChanges();
            }
        }
    }
}
