using System;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class ChiTietHoaDonDAL : DBContext
    {
        private SqlDataAdapter da;
        public DataSet ds;

        public ChiTietHoaDonDAL()
        {
            ds = new DataSet();
            da = new SqlDataAdapter();
            SqlCommandBuilder cb = new SqlCommandBuilder(da);
            da.SelectCommand = new SqlCommand("SELECT * FROM ChiTietHoaDon", conn);
            da.TableMappings.Add("Table", "ChiTietHoaDon");
            da.Fill(ds, "ChiTietHoaDon");
        }

        public DataSet getDBtoDataset()
        {
            return ds;
        }

        public DataTable getTable()
        {
            return ds.Tables["ChiTietHoaDon"];
        }

        public void addRow(DataRow r)
        {
            try
            {
                ds.Tables["ChiTietHoaDon"].Rows.Add(r);
                da.Update(ds, "ChiTietHoaDon");
                ds.AcceptChanges();
            }
            catch { }
        }

        public void update()
        {
            da.Update(ds, "ChiTietHoaDon");
            ds.AcceptChanges();
        }

        public void delete(string maHD, string maSP)
        {
            DataRow[] rows = ds.Tables["ChiTietHoaDon"].Select("MaHD = '" + maHD.Replace("'", "''") + "' AND MaSP = '" + maSP.Replace("'", "''") + "'");
            if (rows.Length > 0)
            {
                rows[0].Delete();
                da.Update(ds, "ChiTietHoaDon");
                ds.AcceptChanges();
            }
        }
        
        public void deleteByMaHD(string maHD)
        {
            DataRow[] rows = ds.Tables["ChiTietHoaDon"].Select("MaHD = '" + maHD.Replace("'", "''") + "'");
            foreach(DataRow r in rows)
            {
                r.Delete();
            }
            if (rows.Length > 0)
            {
                da.Update(ds, "ChiTietHoaDon");
                ds.AcceptChanges();
            }
        }
    }
}
