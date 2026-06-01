using System;
using System.Data;
using DAL;
using DTO;

namespace BUS
{
    public class KhachHangBUS
    {
        private KhachHangDAL dal = new KhachHangDAL();

        public DataSet getDataset()
        {
            return dal.getDBtoDataset();
        }

        public DataTable getTableKhachHang()
        {
            DataTable dt = dal.getTable();
            return dt;
        }

        public Boolean MaKH_not_Exist(string maKH)
        {
            Boolean kq = true;
            DataRow[] rows = dal.TimKiemTheoMa(maKH);
            if (rows.Length > 0)
            {
                kq = false;
            }
            return kq;
        }

        public Boolean add_New_KH(KhachHangDTO kh)
        {
            Boolean kq = false;
            if (MaKH_not_Exist(kh.MaKH))
            {
                kh.TrangThai = 1;
                dal.Add(kh);
                kq = true;
            }
            return kq;
        }

        public DataRow[] getFilter_KH(string strFilter)
        {
            return dal.TimKiemTheoDieuKien(strFilter);
        }

        public Boolean update_KH(KhachHangDTO kh)
        {
            if (MaKH_not_Exist(kh.MaKH))
                return false;
            
            dal.Update(kh);
            return true;
        }

        public Boolean delete_KH(string maKH)
        {
            if (MaKH_not_Exist(maKH)) 
                return false;
            dal.delete(maKH);
            return true;
        }

        public string XoaVinhVien(string maKH)
        {
            if (MaKH_not_Exist(maKH))
                return "Khách hàng không tồn tại!";

            HoaDonDAL hdDal = new HoaDonDAL();
            if (hdDal.KiemTraKhachHangTonTai(maKH))
                return "Khách hàng đã phát sinh Hóa Đơn, không thể xóa vĩnh viễn!";

            dal.hardDelete(maKH);
            return ""; 
        }
    }
}
