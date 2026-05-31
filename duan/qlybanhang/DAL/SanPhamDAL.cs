using System;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class SanPhamDAL : DBContext
    {
        private SqlDataAdapter da;
        public DataSet ds;

        public SanPhamDAL()
        {
            ds = new DataSet();
            da = new SqlDataAdapter();
            SqlCommandBuilder cb = new SqlCommandBuilder(da);
            da.SelectCommand = new SqlCommand("SELECT * FROM SanPham", conn);
            da.TableMappings.Add("Table", "SanPham");
            da.Fill(ds, "SanPham");
        }

        public DataSet getDBtoDataset()
        {
            return ds;
        }

        public DataTable getTable()
        {
            return ds.Tables["SanPham"];
        }

        public void addRow(DataRow r)
        {
            try
            {
                ds.Tables["SanPham"].Rows.Add(r);
                da.Update(ds, "SanPham");
                ds.AcceptChanges();
            }
            catch { }
            reload();
        }

        public void update()
        {
            da.Update(ds, "SanPham");
            ds.AcceptChanges();
        }

        public void delete(string maSP)
        {
            DataRow[] rows = ds.Tables["SanPham"].Select("MaSP = '" + maSP.Replace("'", "''") + "'");
            if (rows.Length > 0)
            {
                rows[0]["TrangThai"] = 0;
                da.Update(ds, "SanPham");
                ds.AcceptChanges();
            }
        }
        
        public void capNhatTonKho(string maSP, int soLuongBan)
        {
            DataRow[] rows = ds.Tables["SanPham"].Select("MaSP = '" + maSP.Replace("'", "''") + "'");
            if (rows.Length > 0)
            {
                int tonHienTai = Convert.ToInt32(rows[0]["SoLuongTon"]);
                rows[0]["SoLuongTon"] = tonHienTai - soLuongBan;
            }
            da.Update(ds, "SanPham");
            ds.AcceptChanges();
        }

        public void reload()
        {
            ds.Tables["SanPham"].Clear();
            da.Fill(ds, "SanPham");
        }

        public bool KiemTraNhaCungCapTonTai(string maNCC)
        {
            DataRow[] rows = ds.Tables["SanPham"].Select("MaNCC = '" + maNCC.Replace("'", "''") + "'");
            return rows.Length > 0;
        }

        public void hardDelete(string maSP)
        {
            DataRow[] rows = ds.Tables["SanPham"].Select("MaSP = '" + maSP.Replace("'", "''") + "'");
            if (rows.Length > 0)
            {
                rows[0].Delete();
                da.Update(ds, "SanPham");
                ds.AcceptChanges();
            }
        }
    }
}
