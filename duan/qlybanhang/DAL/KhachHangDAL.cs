using System;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class KhachHangDAL : DBContext
    {
        private SqlDataAdapter da;
        public DataSet ds;

        public KhachHangDAL()
        {
            ds = new DataSet();
            da = new SqlDataAdapter();
            SqlCommandBuilder cb = new SqlCommandBuilder(da);
            da.SelectCommand = new SqlCommand("SELECT * FROM KhachHang", conn);
            da.TableMappings.Add("Table", "KhachHang");
            da.Fill(ds, "KhachHang");
        }

        public DataSet getDBtoDataset()
        {
            return ds;
        }

        public DataTable getTable()
        {
            return ds.Tables["KhachHang"];
        }

        public DataRow[] TimKiemTheoMa(string maKH)
        {
            return ds.Tables["KhachHang"].Select("MaKH = '" + maKH.Replace("'", "''") + "'");
        }

        public DataRow[] TimKiemTheoDieuKien(string strFilter)
        {
            return ds.Tables["KhachHang"].Select(strFilter);
        }

        public void Add(DTO.KhachHangDTO kh)
        {
            try
            {
                DataRow r = ds.Tables["KhachHang"].NewRow();
                r["MaKH"] = kh.MaKH;
                r["TenKH"] = kh.TenKH;
                r["SoDienThoai"] = kh.SoDienThoai;
                r["DiaChi"] = kh.DiaChi;
                r["TrangThai"] = kh.TrangThai;
                ds.Tables["KhachHang"].Rows.Add(r);
                da.Update(ds, "KhachHang");
                ds.AcceptChanges();
            }
            catch { }
        }

        public void Update(DTO.KhachHangDTO kh)
        {
            DataRow[] rows = TimKiemTheoMa(kh.MaKH);
            if (rows.Length > 0)
            {
                DataRow r = rows[0];
                r["TenKH"] = kh.TenKH;
                r["SoDienThoai"] = kh.SoDienThoai;
                r["DiaChi"] = kh.DiaChi;
                r["TrangThai"] = kh.TrangThai;
                da.Update(ds, "KhachHang");
                ds.AcceptChanges();
            }
        }

        public void delete(string maKH)
        {
            DataRow[] rows = TimKiemTheoMa(maKH);
            if (rows.Length > 0)
            {
                rows[0]["TrangThai"] = 0;
                da.Update(ds, "KhachHang");
                ds.AcceptChanges();
            }
        }

        public void hardDelete(string maKH)
        {
            DataRow[] rows = TimKiemTheoMa(maKH);
            if (rows.Length > 0)
            {
                rows[0].Delete();
                da.Update(ds, "KhachHang");
                ds.AcceptChanges();
            }
        }
    }
}
