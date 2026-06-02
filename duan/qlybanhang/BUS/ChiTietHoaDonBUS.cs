using System;
using System.Data;
using DAL;
using DTO;

namespace BUS
{
    public class ChiTietHoaDonBUS
    {
        private MyDatabase db = MyDatabase.Instance;

        public DataSet getDataset()
        {
            return db.getDataSet();
        }

        public DataTable getTableChiTietHD()
        {
            return db.getTable("ChiTietHoaDon");
        }

        public DataTable LayDanhSachChiTietHDDayDu(string maHD)
        {
            DataTable dtChiTiet = db.getTable("ChiTietHoaDon");
            DataTable dtSanPham = db.getTable("SanPham");
            
            DataTable result = new DataTable();
            result.Columns.Add("MaHD", typeof(string));
            result.Columns.Add("MaSP", typeof(string));
            result.Columns.Add("TenSP", typeof(string));
            result.Columns.Add("SoLuong", typeof(int));
            result.Columns.Add("DonGia", typeof(decimal));
            result.Columns.Add("ThanhTien", typeof(decimal));

            DataRow[] rowsCT = dtChiTiet.Select("MaHD = '" + maHD.Replace("'", "''") + "'");
            foreach (DataRow r in rowsCT)
            {
                DataRow rNew = result.NewRow();
                rNew["MaHD"] = r["MaHD"];
                rNew["MaSP"] = r["MaSP"];
                rNew["SoLuong"] = r["SoLuong"];
                rNew["DonGia"] = r["DonGia"];
                rNew["ThanhTien"] = Convert.ToInt32(r["SoLuong"]) * Convert.ToDecimal(r["DonGia"]);

                if (r["MaSP"] != DBNull.Value)
                {
                    DataRow[] rowsSP = dtSanPham.Select("MaSP = '" + r["MaSP"].ToString().Replace("'", "''") + "'");
                    if (rowsSP.Length > 0)
                        rNew["TenSP"] = rowsSP[0]["TenSP"];
                }
                result.Rows.Add(rNew);
            }
            return result;
        }

        private void CapNhatTongTien(string maHD)
        {
            DataRow[] rowsCT = db.getTable("ChiTietHoaDon").Select("MaHD = '" + maHD.Replace("'", "''") + "'");
            decimal tongTien = 0;
            foreach (DataRow r in rowsCT)
            {
                tongTien += Convert.ToInt32(r["SoLuong"]) * Convert.ToDecimal(r["DonGia"]);
            }
            
            DataRow[] hdRows = db.getTable("HoaDon").Select("MaHD = '" + maHD.Replace("'", "''") + "'");
            if (hdRows.Length > 0)
            {
                hdRows[0].BeginEdit();
                hdRows[0]["TongTien"] = tongTien;
                hdRows[0].EndEdit();
                db.update("HoaDon");
            }
        }

        public bool ThemChiTiet(string maHD, string maSP, int soLuong, decimal donGia)
        {
            bool kq = false;
            DataRow[] exist = db.getTable("ChiTietHoaDon").Select("MaHD = '" + maHD.Replace("'", "''") + "' AND MaSP = '" + maSP.Replace("'", "''") + "'");
            if (exist.Length == 0)
            {
                DataRow r = db.getTable("ChiTietHoaDon").NewRow();
                r["MaHD"] = maHD;
                r["MaSP"] = maSP;
                r["SoLuong"] = soLuong;
                r["DonGia"] = donGia;
                r["ThanhTien"] = soLuong * donGia;
                db.addRow("ChiTietHoaDon", r);

                DataRow[] spRows = db.getTable("SanPham").Select("MaSP = '" + maSP.Replace("'", "''") + "'");
                if (spRows.Length > 0)
                {
                    spRows[0].BeginEdit();
                    int currentStock = Convert.ToInt32(spRows[0]["SoLuongTon"]);
                    spRows[0]["SoLuongTon"] = currentStock - soLuong;
                    spRows[0].EndEdit();
                    db.update("SanPham");
                }

                CapNhatTongTien(maHD);
                kq = true;
            }
            return kq;
        }

        public bool SuaChiTiet(string maHD, string maSP, int soLuongMoi, decimal donGiaMoi)
        {
            bool kq = false;
            DataRow[] exist = db.getTable("ChiTietHoaDon").Select("MaHD = '" + maHD.Replace("'", "''") + "' AND MaSP = '" + maSP.Replace("'", "''") + "'");
            if (exist.Length > 0)
            {
                int soLuongCu = Convert.ToInt32(exist[0]["SoLuong"]);
                int chechLech = soLuongMoi - soLuongCu;

                DataRow r = exist[0];
                r.BeginEdit();
                r["SoLuong"] = soLuongMoi;
                r["DonGia"] = donGiaMoi;
                r["ThanhTien"] = soLuongMoi * donGiaMoi;
                r.EndEdit();
                db.update("ChiTietHoaDon");
                
                DataRow[] spRows = db.getTable("SanPham").Select("MaSP = '" + maSP.Replace("'", "''") + "'");
                if (spRows.Length > 0)
                {
                    spRows[0].BeginEdit();
                    int currentStock = Convert.ToInt32(spRows[0]["SoLuongTon"]);
                    spRows[0]["SoLuongTon"] = currentStock - chechLech;
                    spRows[0].EndEdit();
                    db.update("SanPham");
                }
                
                CapNhatTongTien(maHD);
                kq = true;
            }
            return kq;
        }

        public bool XoaChiTiet(string maHD, string maSP)
        {
            bool kq = false;
            DataRow[] exist = db.getTable("ChiTietHoaDon").Select("MaHD = '" + maHD.Replace("'", "''") + "' AND MaSP = '" + maSP.Replace("'", "''") + "'");
            if (exist.Length > 0)
            {
                int soLuongCu = Convert.ToInt32(exist[0]["SoLuong"]);

                db.deleteRow("ChiTietHoaDon", "MaHD = '" + maHD.Replace("'", "''") + "' AND MaSP = '" + maSP.Replace("'", "''") + "'");
                
                DataRow[] spRows = db.getTable("SanPham").Select("MaSP = '" + maSP.Replace("'", "''") + "'");
                if (spRows.Length > 0)
                {
                    spRows[0].BeginEdit();
                    int currentStock = Convert.ToInt32(spRows[0]["SoLuongTon"]);
                    spRows[0]["SoLuongTon"] = currentStock + soLuongCu;
                    spRows[0].EndEdit();
                    db.update("SanPham");
                }
                
                CapNhatTongTien(maHD);
                kq = true;
            }
            return kq;
        }
    }
}
