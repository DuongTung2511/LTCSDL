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

        public DataRow[] TimKiemTheoMa(string maNCC)
        {
            return ds.Tables["NhaCungCap"].Select("MaNCC = '" + maNCC.Replace("'", "''") + "'");
        }

        public DataRow[] TimKiemTheoDieuKien(string strFilter)
        {
            return ds.Tables["NhaCungCap"].Select(strFilter);
        }

        public void Add(DTO.NhaCungCapDTO ncc)
        {
            try
            {
                DataRow r = ds.Tables["NhaCungCap"].NewRow();
                r["MaNCC"] = ncc.MaNCC;
                r["TenNCC"] = ncc.TenNCC;
                r["SoDienThoai"] = ncc.SoDienThoai;
                r["DiaChi"] = ncc.DiaChi;
                r["TrangThai"] = ncc.TrangThai; // Default from BUS
                ds.Tables["NhaCungCap"].Rows.Add(r);
                da.Update(ds, "NhaCungCap");
                ds.AcceptChanges();
            }
            catch { }
        }

        public void Update(DTO.NhaCungCapDTO ncc)
        {
            DataRow[] rows = TimKiemTheoMa(ncc.MaNCC.ToString());
            if (rows.Length > 0)
            {
                DataRow r = rows[0];
                r["TenNCC"] = ncc.TenNCC;
                r["SoDienThoai"] = ncc.SoDienThoai;
                r["DiaChi"] = ncc.DiaChi;
                r["TrangThai"] = ncc.TrangThai;
                da.Update(ds, "NhaCungCap");
                ds.AcceptChanges();
            }
        }

        public void delete(string maNCC)
        {
            DataRow[] rows = TimKiemTheoMa(maNCC);
            if (rows.Length > 0)
            {
                rows[0]["TrangThai"] = 0;
                da.Update(ds, "NhaCungCap");
                ds.AcceptChanges();
            }
        }

        public DataTable LayDanhSachNCCDangHoatDong()
        {
            DataRow[] rows = ds.Tables["NhaCungCap"].Select("TrangThai = 1");
            if (rows.Length > 0)
                return rows.CopyToDataTable();
            else
                return ds.Tables["NhaCungCap"].Clone();
        }

        public void hardDelete(string maNCC)
        {
            DataRow[] rows = TimKiemTheoMa(maNCC);
            if (rows.Length > 0)
            {
                rows[0].Delete();
                da.Update(ds, "NhaCungCap");
                ds.AcceptChanges();
            }
        }
    }
}
