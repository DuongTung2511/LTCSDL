using System;
using System.Data;
using DAL;
using DTO;

namespace BUS
{
    public class SanPhamBUS
    {
        private SanPhamDAL dal = new SanPhamDAL();

        public DataSet getDataset()
        {
            return dal.getDBtoDataset();
        }

        public DataTable getTableSanPham()
        {
            DataTable dt = dal.getTable();
            return dt;
        }

        public Boolean MaSP_not_Exist(string maSP)
        {
            Boolean kq = true;
            DataRow[] rows = dal.TimKiemTheoMa(maSP);
            if (rows.Length > 0)
            {
                kq = false;
            }
            return kq;
        }

        public Boolean add_New_SP(SanPhamDTO sp)
        {
            Boolean kq = false;
            if (MaSP_not_Exist(sp.MaSP.ToString()))
            {
                sp.TrangThai = 1;
                dal.Add(sp);
                kq = true;
            }
            return kq;
        }

        public DataRow[] getFilter_SP(string strFilter)
        {
            return dal.TimKiemTheoDieuKien(strFilter);
        }

        public Boolean update_SP(SanPhamDTO sp)
        {
            if (MaSP_not_Exist(sp.MaSP.ToString()))
                return false;

            dal.Update(sp);
            return true;
        }

        public Boolean delete_SP(string maSP)
        {
            if (MaSP_not_Exist(maSP)) 
                return false;
            dal.delete(maSP);
            return true;
        }

        public string XoaVinhVien(string maSP)
        {
            if (MaSP_not_Exist(maSP))
                return "Sản phẩm không tồn tại!";

            ChiTietHoaDonDAL ctDal = new ChiTietHoaDonDAL();
            if (ctDal.KiemTraSanPhamTonTai(maSP))
                return "Sản phẩm đã tồn tại trong Hóa Đơn, không thể xóa vĩnh viễn!";

            dal.hardDelete(maSP);
            return ""; 
        }
    }
}
