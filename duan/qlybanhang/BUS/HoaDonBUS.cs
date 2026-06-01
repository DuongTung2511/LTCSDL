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
            return hdDal.getDBtoDataset();
        }

        public DataTable getTableHoaDon()
        {
            return hdDal.getTable();
        }

        public Boolean MaHD_not_Exist(string maHD)
        {
            DataRow[] rows = hdDal.getTable().Select("MaHD='" + maHD.Replace("'", "''") + "'");
            return rows.Length == 0;
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

            DataRow newHD = hdDal.getTable().NewRow();
            newHD["MaHD"] = maHD;
            newHD["MaKH"] = maKH;
            newHD["MaNV"] = maNV;
            newHD["NgayLap"] = DateTime.Now;
            newHD["TongTien"] = tongTien;
            hdDal.addRow(newHD);

            foreach (DataRow r in gioHang.Rows)
            {
                DataRow newCT = cthdDal.getTable().NewRow();
                newCT["MaHD"] = maHD;
                string maSP = r["MaSP"].ToString();
                newCT["MaSP"] = maSP;
                int soLuong = Convert.ToInt32(r["SoLuong"]);
                newCT["SoLuong"] = soLuong;
                newCT["DonGia"] = Convert.ToDecimal(r["DonGia"]);
                newCT["ThanhTien"] = Convert.ToDecimal(r["ThanhTien"]);
                cthdDal.addRow(newCT);

                // Update stock
                DataRow[] spRows = spDal.getTable().Select("MaSP = '" + maSP.Replace("'", "''") + "'");
                if (spRows.Length > 0)
                {
                    spRows[0].BeginEdit();
                    int currentStock = Convert.ToInt32(spRows[0]["SoLuongTon"]);
                    spRows[0]["SoLuongTon"] = currentStock - soLuong;
                    spRows[0].EndEdit();
                }
            }
            spDal.update();
        }

        public void XoaHoaDon(string maHD)
        {
            DataRow[] cthdRows = cthdDal.getTable().Select("MaHD = '" + maHD.Replace("'", "''") + "'");
            foreach (DataRow r in cthdRows)
            {
                string maSP = r["MaSP"].ToString();
                int soLuong = Convert.ToInt32(r["SoLuong"]);
                DataRow[] spRows = spDal.getTable().Select("MaSP = '" + maSP.Replace("'", "''") + "'");
                if (spRows.Length > 0)
                {
                    spRows[0].BeginEdit();
                    int currentStock = Convert.ToInt32(spRows[0]["SoLuongTon"]);
                    spRows[0]["SoLuongTon"] = currentStock + soLuong; // Refund stock
                    spRows[0].EndEdit();
                }
            }
            spDal.update();
            cthdDal.deleteByMaHD(maHD);
            hdDal.delete(maHD);
        }
    }
}
