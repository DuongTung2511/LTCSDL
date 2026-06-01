using System;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class NhaCungCapDAL : DBContext
    {
        private SqlDataAdapter da;
        public DataSet ds;

        public NhaCungCapDAL()
        {
            ds = new DataSet();
            da = new SqlDataAdapter();
            SqlCommandBuilder cb = new SqlCommandBuilder(da);
            da.SelectCommand = new SqlCommand("SELECT * FROM NhaCungCap", conn);
            da.TableMappings.Add("Table", "NhaCungCap");
            da.Fill(ds, "NhaCungCap");
        }

        public DataSet getDBtoDataset()
        {
            return ds;
        }

        public DataTable getTable()
        {
            return ds.Tables["NhaCungCap"];
        }

        public void addRow(DataRow r)
        {
            try
            {
                ds.Tables["NhaCungCap"].Rows.Add(r);
                da.Update(ds, "NhaCungCap");
                ds.AcceptChanges();
            }
            catch { }
        }

        public void update()
        {
            da.Update(ds, "NhaCungCap");
            ds.AcceptChanges();
        }

        public void delete(string maNCC)
        {
            DataRow[] rows = ds.Tables["NhaCungCap"].Select("MaNCC = '" + maNCC.Replace("'", "''") + "'");
            if (rows.Length > 0)
            {
                rows[0].Delete();
                da.Update(ds, "NhaCungCap");
                ds.AcceptChanges();
            }
        }
    }
}
