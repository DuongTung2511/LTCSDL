using System;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class MyDatabase
    {
        private static MyDatabase _instance = null;
        public static MyDatabase Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MyDatabase();
                }
                return _instance;
            }
        }

        private SqlConnection conn = null;
        private DataSet ds = null;

        private SqlDataAdapter daTaiKhoan = null;
        private SqlDataAdapter daKhachHang = null;
        private SqlDataAdapter daNhanVien = null;
        private SqlDataAdapter daNhaCungCap = null;
        private SqlDataAdapter daSanPham = null;
        private SqlDataAdapter daHoaDon = null;
        private SqlDataAdapter daChiTietHD = null;

        private MyDatabase()
        {
            conn = new SqlConnection(Properties.Settings.Default.strconnect);
            ds = new DataSet();

            daTaiKhoan = new SqlDataAdapter("SELECT * FROM TaiKhoan", conn);
            new SqlCommandBuilder(daTaiKhoan);
            daTaiKhoan.Fill(ds, "TaiKhoan");

            daKhachHang = new SqlDataAdapter("SELECT * FROM KhachHang", conn);
            new SqlCommandBuilder(daKhachHang);
            daKhachHang.Fill(ds, "KhachHang");

            daNhanVien = new SqlDataAdapter("SELECT * FROM NhanVien", conn);
            new SqlCommandBuilder(daNhanVien);
            daNhanVien.Fill(ds, "NhanVien");

            daNhaCungCap = new SqlDataAdapter("SELECT * FROM NhaCungCap", conn);
            new SqlCommandBuilder(daNhaCungCap);
            daNhaCungCap.Fill(ds, "NhaCungCap");

            daSanPham = new SqlDataAdapter("SELECT * FROM SanPham", conn);
            new SqlCommandBuilder(daSanPham);
            daSanPham.Fill(ds, "SanPham");

            daHoaDon = new SqlDataAdapter("SELECT * FROM HoaDon", conn);
            new SqlCommandBuilder(daHoaDon);
            daHoaDon.Fill(ds, "HoaDon");

            daChiTietHD = new SqlDataAdapter("SELECT * FROM ChiTietHoaDon", conn);
            new SqlCommandBuilder(daChiTietHD);
            daChiTietHD.Fill(ds, "ChiTietHoaDon");
        }

        public DataSet getDataSet()
        {
            return ds;
        }

        public DataTable getTable(string tableName)
        {
            return ds.Tables[tableName];
        }

        public void addRow(string tableName, DataRow r)
        {
            try
            {
                ds.Tables[tableName].Rows.Add(r);
                update(tableName);
            }
            catch { }
        }

        public void update(string tableName)
        {
            switch (tableName)
            {
                case "TaiKhoan": daTaiKhoan.Update(ds, "TaiKhoan"); break;
                case "KhachHang": daKhachHang.Update(ds, "KhachHang"); break;
                case "NhanVien": daNhanVien.Update(ds, "NhanVien"); break;
                case "NhaCungCap": daNhaCungCap.Update(ds, "NhaCungCap"); break;
                case "SanPham": daSanPham.Update(ds, "SanPham"); break;
                case "HoaDon": daHoaDon.Update(ds, "HoaDon"); break;
                case "ChiTietHoaDon": daChiTietHD.Update(ds, "ChiTietHoaDon"); break;
            }
            ds.AcceptChanges();
        }

        public void deleteRow(string tableName, string condition)
        {
            DataRow[] rows = ds.Tables[tableName].Select(condition);
            if (rows.Length > 0)
            {
                foreach (DataRow r in rows)
                {
                    r.Delete();
                }
                update(tableName);
            }
        }
    }
}
