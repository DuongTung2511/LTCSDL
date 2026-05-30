using System;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class NhaCungCapDAL : DBContext
    {
        private SqlDataAdapter da;
        public DataSet ds { get; private set; }

        public NhaCungCapDAL()
        {
            ds = new DataSet();
            da = new SqlDataAdapter("SELECT * FROM NhaCungCap", conn);
            new SqlCommandBuilder(da);
            da.Fill(ds, "NhaCungCap");
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
            reload();
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

        public void reload()
        {
            ds.Tables["NhaCungCap"].Clear();
            da.Fill(ds, "NhaCungCap");
        }
    }
}
