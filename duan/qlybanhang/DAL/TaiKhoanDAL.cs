using System;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class TaiKhoanDAL : DBContext
    {
        private SqlDataAdapter da;
        public DataSet ds { get; private set; }

        public TaiKhoanDAL()
        {
            ds = new DataSet();
            da = new SqlDataAdapter("SELECT * FROM TaiKhoan", conn);
            new SqlCommandBuilder(da);
            da.Fill(ds, "TaiKhoan");
        }

        public DataTable getTable()
        {
            return ds.Tables["TaiKhoan"];
        }

        public void addRow(DataRow r)
        {
            try
            {
                ds.Tables["TaiKhoan"].Rows.Add(r);
                da.Update(ds, "TaiKhoan");
                ds.AcceptChanges();
            }
            catch { }
            reload();
        }

        public void update()
        {
            da.Update(ds, "TaiKhoan");
            ds.AcceptChanges();
        }

        public void delete(string tenDangNhap)
        {
            DataRow[] rows = ds.Tables["TaiKhoan"].Select("TenDangNhap = '" + tenDangNhap.Replace("'", "''") + "'");
            if (rows.Length > 0)
            {
                rows[0].Delete();
                da.Update(ds, "TaiKhoan");
                ds.AcceptChanges();
            }
        }

        public void reload()
        {
            ds.Tables["TaiKhoan"].Clear();
            da.Fill(ds, "TaiKhoan");
        }
    }
}
