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

        public void addRow(DataRow r)
        {
            try
            {
                ds.Tables["TaiKhoan"].Rows.Add(r);
                da.Update(ds, "TaiKhoan");
                ds.AcceptChanges();
            }
            catch (Exception ex) { throw new Exception("Lỗi cập nhật CSDL: " + ex.Message); }
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
    }
}
