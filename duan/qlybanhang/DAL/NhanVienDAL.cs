using System;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class NhanVienDAL : DBContext
    {
        private SqlDataAdapter da;
        public DataSet ds;

        public NhanVienDAL()
        {
            ds = new DataSet();
            da = new SqlDataAdapter();
            SqlCommandBuilder cb = new SqlCommandBuilder(da);
            da.SelectCommand = new SqlCommand("SELECT * FROM NhanVien", conn);
            da.TableMappings.Add("Table", "NhanVien");
            da.Fill(ds, "NhanVien");
        }

        public DataSet getDBtoDataset()
        {
            return ds;
        }

        public DataTable getTable()
        {
            return ds.Tables["NhanVien"];
        }

        public void addRow(DataRow r)
        {
            try
            {
                ds.Tables["NhanVien"].Rows.Add(r);
                da.Update(ds, "NhanVien");
                ds.AcceptChanges();
            }
            catch (Exception ex) { throw new Exception("Lỗi cập nhật CSDL: " + ex.Message); }
        }

        public void update()
        {
            da.Update(ds, "NhanVien");
            ds.AcceptChanges();
        }

        public void delete(string maNV)
        {
            DataRow[] rows = ds.Tables["NhanVien"].Select("MaNV = '" + maNV.Replace("'", "''") + "'");
            if (rows.Length > 0)
            {
                rows[0].Delete();
                da.Update(ds, "NhanVien");
                ds.AcceptChanges();
            }
        }
    }
}
