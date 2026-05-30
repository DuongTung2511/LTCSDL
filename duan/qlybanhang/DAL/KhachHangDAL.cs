using System;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class KhachHangDAL : DBContext
    {
        private SqlDataAdapter da;
        public DataSet ds { get; private set; }

        public KhachHangDAL()
        {
            ds = new DataSet();
            da = new SqlDataAdapter("SELECT * FROM KhachHang", conn);
            new SqlCommandBuilder(da);
            da.Fill(ds, "KhachHang");
        }

        public DataTable getTable()
        {
            return ds.Tables["KhachHang"];
        }

        public void addRow(DataRow r)
        {
            try
            {
                ds.Tables["KhachHang"].Rows.Add(r);
                da.Update(ds, "KhachHang");
                ds.AcceptChanges();
            }
            catch { }
            reload();
        }

        public void update()
        {
            da.Update(ds, "KhachHang");
            ds.AcceptChanges();
        }

        public void delete(string maKH)
        {
            DataRow[] rows = ds.Tables["KhachHang"].Select("MaKH = '" + maKH.Replace("'", "''") + "'");
            if (rows.Length > 0)
            {
                rows[0].Delete();
                da.Update(ds, "KhachHang");
                ds.AcceptChanges();
            }
        }

        public void reload()
        {
            ds.Tables["KhachHang"].Clear();
            da.Fill(ds, "KhachHang");
        }
    }
}
