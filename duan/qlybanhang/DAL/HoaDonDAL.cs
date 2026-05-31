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
            da = new SqlDataAdapter("SELECT * FROM HoaDon", conn);
            new SqlCommandBuilder(da);
            da.Fill(ds, "HoaDon");
        }

        public DataTable getTable()
        {
            return ds.Tables["HoaDon"];
        }

        public int taoHoaDon(string maHD, string maKH, string maNV, decimal tongTien)
        {
            conn.Open();
            try
            {
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO HoaDon (MaHD, MaKH, MaNV, NgayLap, TongTien) " +
                    "VALUES (@MaHD, @MaKH, @MaNV, GETDATE(), @TongTien)", conn);
                cmd.Parameters.AddWithValue("@MaHD", maHD);
                cmd.Parameters.AddWithValue("@MaKH", maKH);
                cmd.Parameters.AddWithValue("@MaNV", maNV);
                cmd.Parameters.AddWithValue("@TongTien", tongTien);
                cmd.ExecuteNonQuery();
                return 1;
            }
            finally
            {
                conn.Close();
            }
        }

        public void deleteHoaDon(string maHD)
        {
            DataRow[] rows = ds.Tables["HoaDon"].Select("MaHD = '" + maHD.Replace("'", "''") + "'");
            if (rows.Length > 0)
            {
                rows[0].Delete();
                da.Update(ds, "HoaDon");
                ds.AcceptChanges();
            }
        }

        public void capNhatTongTien(string maHD, decimal tongTien)
        {
            DataRow[] rows = ds.Tables["HoaDon"].Select("MaHD = '" + maHD.Replace("'", "''") + "'");
            if (rows.Length > 0)
            {
                rows[0]["TongTien"] = tongTien;
                da.Update(ds, "HoaDon");
                ds.AcceptChanges();
            }
        }

        public void reload()
        {
            ds.Tables["HoaDon"].Clear();
            da.Fill(ds, "HoaDon");
        }

        public bool KiemTraKhachHangTonTai(string maKH)
        {
            DataRow[] rows = ds.Tables["HoaDon"].Select("MaKH = '" + maKH.Replace("'", "''") + "'");
            return rows.Length > 0;
        }

        public bool KiemTraNhanVienTonTai(string maNV)
        {
            DataRow[] rows = ds.Tables["HoaDon"].Select("MaNV = '" + maNV.Replace("'", "''") + "'");
            return rows.Length > 0;
        }
    }
}
