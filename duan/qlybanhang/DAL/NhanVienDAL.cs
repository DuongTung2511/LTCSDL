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

        public DataRow[] TimKiemTheoMa(string maNV)
        {
            return ds.Tables["NhanVien"].Select("MaNV = '" + maNV.Replace("'", "''") + "'");
        }

        public DataRow[] TimKiemTheoDieuKien(string strFilter)
        {
            return ds.Tables["NhanVien"].Select(strFilter);
        }

        public void Add(DTO.NhanVienDTO nv)
        {
            try
            {
                DataRow r = ds.Tables["NhanVien"].NewRow();
                r["MaNV"] = nv.MaNV;
                r["TenNV"] = nv.TenNV;
                r["GioiTinh"] = nv.GioiTinh;
                r["NgaySinh"] = nv.NgaySinh;
                r["SoDienThoai"] = nv.SoDienThoai;
                r["DiaChi"] = nv.DiaChi;
                r["TrangThai"] = nv.TrangThai; // Default from BUS
                ds.Tables["NhanVien"].Rows.Add(r);
                da.Update(ds, "NhanVien");
                ds.AcceptChanges();
            }
            catch { }
        }

        public void Update(DTO.NhanVienDTO nv)
        {
            DataRow[] rows = TimKiemTheoMa(nv.MaNV.ToString());
            if (rows.Length > 0)
            {
                DataRow r = rows[0];
                r["TenNV"] = nv.TenNV;
                r["GioiTinh"] = nv.GioiTinh;
                r["NgaySinh"] = nv.NgaySinh;
                r["SoDienThoai"] = nv.SoDienThoai;
                r["DiaChi"] = nv.DiaChi;
                r["TrangThai"] = nv.TrangThai;
                da.Update(ds, "NhanVien");
                ds.AcceptChanges();
            }
        }

        public void delete(string maNV)
        {
            DataRow[] rows = TimKiemTheoMa(maNV);
            if (rows.Length > 0)
            {
                rows[0]["TrangThai"] = 0;
                da.Update(ds, "NhanVien");
                ds.AcceptChanges();
            }
        }

        public void hardDelete(string maNV)
        {
            DataRow[] rows = TimKiemTheoMa(maNV);
            if (rows.Length > 0)
            {
                rows[0].Delete();
                da.Update(ds, "NhanVien");
                ds.AcceptChanges();
            }
        }
    }
}
