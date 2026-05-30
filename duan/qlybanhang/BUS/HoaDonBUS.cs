using System;
using System.Data;
using DAL;
using DTO;

namespace BUS
{
    public class HoaDonBUS
    {
        private HoaDonDAL hdDal = new HoaDonDAL();
        private KhachHangDAL khDal = new KhachHangDAL();
        private NhanVienDAL nvDal = new NhanVienDAL();
        private ChiTietHoaDonDAL cthdDal = new ChiTietHoaDonDAL();
        private SanPhamDAL spDal = new SanPhamDAL();

        public DataSet getDataset()
        {
            return hdDal.ds;
        }

        public DataTable getTableHoaDon()
        {
            return hdDal.getTable();
        }

        public Boolean MaHD_not_Exist(string maHD)
        {
            Boolean kq = true;
            DataRow[] rows = hdDal.getTable().Select("MaHD='" + maHD.Replace("'", "''") + "'");
            if (rows.Length > 0)
            {
                kq = false;
            }
            return kq;
        }

        public DataTable LayDanhSachHoaDonDayDu()
        {
            DataTable dtHoaDon = hdDal.getTable();
            DataTable dtKhachHang = khDal.getTable();
            DataTable dtNhanVien = nvDal.getTable();
            
            DataTable result = new DataTable();
            result.Columns.Add("MaHD", typeof(string));
            result.Columns.Add("MaKH", typeof(string));
            result.Columns.Add("TenKH", typeof(string));
            result.Columns.Add("MaNV", typeof(string));
            result.Columns.Add("TenNV", typeof(string));
            result.Columns.Add("NgayLap", typeof(DateTime));
            result.Columns.Add("TongTien", typeof(decimal));

            foreach (DataRow rHD in dtHoaDon.Rows)
            {
                DataRow rNew = result.NewRow();
                rNew["MaHD"] = rHD["MaHD"];
                rNew["MaKH"] = rHD["MaKH"];
                rNew["MaNV"] = rHD["MaNV"];
                rNew["NgayLap"] = rHD["NgayLap"];
                rNew["TongTien"] = rHD["TongTien"];
                
                if (rHD["MaKH"] != DBNull.Value)
                {
                    DataRow[] rowsKH = dtKhachHang.Select("MaKH = '" + rHD["MaKH"].ToString().Replace("'", "''") + "'");
                    if (rowsKH.Length > 0)
                        rNew["TenKH"] = rowsKH[0]["TenKH"];
                }

                if (rHD["MaNV"] != DBNull.Value)
                {
                    DataRow[] rowsNV = dtNhanVien.Select("MaNV = '" + rHD["MaNV"].ToString().Replace("'", "''") + "'");
                    if (rowsNV.Length > 0)
                        rNew["TenNV"] = rowsNV[0]["TenNV"];
                }
                    
                result.Rows.Add(rNew);
            }
            return result;
        }

        public string LayNextMaHD()
        {
            DataTable dt = hdDal.getTable();
            int max = 0;
            foreach (DataRow r in dt.Rows)
            {
                string maHD = r["MaHD"].ToString();
                if (maHD.StartsWith("HD"))
                {
                    string numPart = maHD.Substring(2);
                    if (int.TryParse(numPart, out int num))
                    {
                        if (num > max) max = num;
                    }
                }
            }
            return "HD" + (max + 1).ToString("D3");
        }

        public DataRow[] getFilter_HDDayDu(string strFilter)
        {
            DataTable dt = LayDanhSachHoaDonDayDu();
            return dt.Select(strFilter);
        }

        public void TaoHoaDon(string maHD, string maKH, string maNV, DataTable gioHang)
        {
            decimal tongTien = 0;
            foreach (DataRow r in gioHang.Rows)
                tongTien += Convert.ToDecimal(r["ThanhTien"]);

            hdDal.taoHoaDon(maHD, maKH, maNV, tongTien);

            foreach (DataRow r in gioHang.Rows)
            {
                cthdDal.themChiTietHoaDon(maHD,
                    r["MaSP"].ToString(),
                    Convert.ToInt32(r["SoLuong"]),
                    Convert.ToDecimal(r["DonGia"]));
            }

            foreach (DataRow r in gioHang.Rows)
            {
                spDal.capNhatTonKho(
                    r["MaSP"].ToString(),
                    Convert.ToInt32(r["SoLuong"]));
            }

            hdDal.reload();
            cthdDal.reload();
            spDal.reload();
        }

        public void XoaHoaDon(string maHD)
        {
            // When deleting a HoaDon, we need to delete its details and refund stock
            // However, MyDatabase.deleteHoaDon did this: 
            // - delete details from ChiTietHoaDon
            // - update stock
            // - delete HoaDon.
            // Since we split the DALs, HoaDonBUS should handle this cross-DAL logic, or we rewrite it here.
            
            DataRow[] cthdRows = cthdDal.getTable().Select("MaHD = '" + maHD.Replace("'", "''") + "'");
            foreach (DataRow r in cthdRows)
            {
                string maSP = r["MaSP"].ToString();
                int soLuong = Convert.ToInt32(r["SoLuong"]);
                spDal.capNhatTonKho(maSP, -soLuong);
            }
            
            cthdDal.deleteChiTietByMaHD(maHD);
            hdDal.deleteHoaDon(maHD);
        }
    }
}
