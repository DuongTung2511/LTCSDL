using System;
using System.Data;
using DAL;
using DTO;

namespace BUS
{
    public class ChiTietHoaDonBUS
    {
        private ChiTietHoaDonDAL dal = new ChiTietHoaDonDAL();
        private SanPhamDAL spDal = new SanPhamDAL();

        public DataSet getDataset()
        {
            return dal.getDBtoDataset();
        }

        public DataTable getTableChiTietHD()
        {
            DataTable dt = dal.getTable();
            return dt;
        }

        public DataTable LayDanhSachChiTietHDDayDu(string maHD)
        {
            DataTable dtChiTiet = dal.getTable();
            DataTable dtSanPham = spDal.getTable();
            
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
            HoaDonDAL hdDal = new HoaDonDAL();
            DataTable dtChiTiet = dal.getTable();
            DataRow[] rowsCT = dtChiTiet.Select("MaHD = '" + maHD.Replace("'", "''") + "'");
            decimal tongTien = 0;
            foreach (DataRow r in rowsCT)
            {
                tongTien += Convert.ToInt32(r["SoLuong"]) * Convert.ToDecimal(r["DonGia"]);
            }
            hdDal.capNhatTongTien(maHD, tongTien);
        }

        public bool ThemChiTiet(string maHD, string maSP, int soLuong, decimal donGia)
        {
            // Check if product already in invoice
            DataRow[] exist = dal.getTable().Select("MaHD = '" + maHD.Replace("'", "''") + "' AND MaSP = '" + maSP.Replace("'", "''") + "'");
            if (exist.Length > 0) return false;

            dal.themChiTietHoaDon(maHD, maSP, soLuong, donGia);
            dal.reload();

            spDal.capNhatTonKho(maSP, soLuong);
            spDal.reload();

            CapNhatTongTien(maHD);
            return true;
        }

        public bool SuaChiTiet(string maHD, string maSP, int soLuongMoi, decimal donGiaMoi)
        {
            DataRow[] exist = dal.getTable().Select("MaHD = '" + maHD.Replace("'", "''") + "' AND MaSP = '" + maSP.Replace("'", "''") + "'");
            if (exist.Length == 0) return false;

            int soLuongCu = Convert.ToInt32(exist[0]["SoLuong"]);
            int chechLech = soLuongMoi - soLuongCu;

            dal.suaChiTietHoaDon(maHD, maSP, soLuongMoi, donGiaMoi);
            
            spDal.capNhatTonKho(maSP, chechLech);
            
            CapNhatTongTien(maHD);
            return true;
        }

        public bool XoaChiTiet(string maHD, string maSP)
        {
            DataRow[] exist = dal.getTable().Select("MaHD = '" + maHD.Replace("'", "''") + "' AND MaSP = '" + maSP.Replace("'", "''") + "'");
            if (exist.Length == 0) return false;

            int soLuongCu = Convert.ToInt32(exist[0]["SoLuong"]);

            dal.xoaChiTietHoaDon(maHD, maSP);
            
            // Hoàn trả tồn kho
            spDal.capNhatTonKho(maSP, -soLuongCu);
            
            CapNhatTongTien(maHD);
            return true;
        }
    }
}
