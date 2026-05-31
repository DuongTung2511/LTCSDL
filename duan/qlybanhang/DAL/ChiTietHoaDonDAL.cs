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
            da = new SqlDataAdapter("SELECT * FROM ChiTietHoaDon", conn);
            new SqlCommandBuilder(da);
            da.Fill(ds, "ChiTietHoaDon");
        }

        public DataTable getTable()
        {
            return ds.Tables["ChiTietHoaDon"];
        }

        public void themChiTietHoaDon(string maHD, string maSP, int soLuong, decimal donGia)
        {
            conn.Open();
            try
            {
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO ChiTietHoaDon (MaHD, MaSP, SoLuong, DonGia, ThanhTien) " +
                    "VALUES (@MaHD, @MaSP, @SoLuong, @DonGia, @ThanhTien)", conn);
                cmd.Parameters.AddWithValue("@MaHD", maHD);
                cmd.Parameters.AddWithValue("@MaSP", maSP);
                cmd.Parameters.AddWithValue("@SoLuong", soLuong);
                cmd.Parameters.AddWithValue("@DonGia", donGia);
                cmd.Parameters.AddWithValue("@ThanhTien", soLuong * donGia);
                cmd.ExecuteNonQuery();
            }
            finally
            {
                conn.Close();
            }
        }

        public void deleteChiTietByMaHD(string maHD)
        {
            DataRow[] rows = ds.Tables["ChiTietHoaDon"].Select("MaHD = '" + maHD.Replace("'", "''") + "'");
            foreach (DataRow row in rows)
            {
                row.Delete();
            }
            da.Update(ds, "ChiTietHoaDon");
            ds.AcceptChanges();
        }

        public void suaChiTietHoaDon(string maHD, string maSP, int soLuongMoi, decimal donGiaMoi)
        {
            DataRow[] rows = ds.Tables["ChiTietHoaDon"].Select("MaHD = '" + maHD.Replace("'", "''") + "' AND MaSP = '" + maSP.Replace("'", "''") + "'");
            if (rows.Length > 0)
            {
                rows[0]["SoLuong"] = soLuongMoi;
                rows[0]["DonGia"] = donGiaMoi;
                rows[0]["ThanhTien"] = soLuongMoi * donGiaMoi;
                da.Update(ds, "ChiTietHoaDon");
                ds.AcceptChanges();
            }
        }

        public void xoaChiTietHoaDon(string maHD, string maSP)
        {
            DataRow[] rows = ds.Tables["ChiTietHoaDon"].Select("MaHD = '" + maHD.Replace("'", "''") + "' AND MaSP = '" + maSP.Replace("'", "''") + "'");
            if (rows.Length > 0)
            {
                rows[0].Delete();
                da.Update(ds, "ChiTietHoaDon");
                ds.AcceptChanges();
            }
        }

        public void reload()
        {
            ds.Tables["ChiTietHoaDon"].Clear();
            da.Fill(ds, "ChiTietHoaDon");
        }

        public bool KiemTraSanPhamTonTai(string maSP)
        {
            DataRow[] rows = ds.Tables["ChiTietHoaDon"].Select("MaSP = '" + maSP.Replace("'", "''") + "'");
            return rows.Length > 0;
        }
    }
}
