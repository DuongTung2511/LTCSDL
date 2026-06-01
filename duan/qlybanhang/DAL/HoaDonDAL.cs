using System;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class HoaDonDAL : DBContext
    {
        private SqlDataAdapter da;
        public DataSet ds;

        public HoaDonDAL()
        {
            ds = new DataSet();
            da = new SqlDataAdapter();
            SqlCommandBuilder cb = new SqlCommandBuilder(da);
            da.SelectCommand = new SqlCommand("SELECT * FROM HoaDon", conn);
            da.TableMappings.Add("Table", "HoaDon");
            da.Fill(ds, "HoaDon");
        }

        public DataSet getDBtoDataset()
        {
            return ds;
        }

        public DataTable getTable()
        {
            return ds.Tables["HoaDon"];
        }

        public void addRow(DataRow r)
        {
            try
            {
                ds.Tables["HoaDon"].Rows.Add(r);
                da.Update(ds, "HoaDon");
                ds.AcceptChanges();
            }
            catch (Exception ex) { throw new Exception("Lỗi cập nhật CSDL: " + ex.Message); }
        }

        public void update()
        {
            da.Update(ds, "HoaDon");
            ds.AcceptChanges();
        }

        public void delete(string maHD)
        {
            DataRow[] rows = ds.Tables["HoaDon"].Select("MaHD = '" + maHD.Replace("'", "''") + "'");
            if (rows.Length > 0)
            {
                rows[0].Delete();
                da.Update(ds, "HoaDon");
                ds.AcceptChanges();
            }
        }
    }
}
