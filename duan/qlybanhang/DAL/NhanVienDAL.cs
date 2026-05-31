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
            da = new SqlDataAdapter("SELECT * FROM NhanVien", conn);
            new SqlCommandBuilder(da);
            da.Fill(ds, "NhanVien");
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
            catch { }
            reload();
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
                rows[0]["TrangThai"] = 0;
                da.Update(ds, "NhanVien");
                ds.AcceptChanges();
            }
        }

        public void reload()
        {
            ds.Tables["NhanVien"].Clear();
            da.Fill(ds, "NhanVien");
        }

        public void hardDelete(string maNV)
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
