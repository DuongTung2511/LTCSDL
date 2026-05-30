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
            return dal.ds;
        }

        public DataTable getTableChiTietHD()
        {
            return dal.getTable();
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
    }
}
