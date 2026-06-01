using System;
using System.Data;
using DAL;
using DTO;

namespace BUS
{
    public class NhanVienBUS
    {
        private NhanVienDAL dal = new NhanVienDAL();

        public DataSet getDataset()
        {
            return dal.getDBtoDataset();
        }

        public DataTable getTableNhanVien()
        {
            DataTable dt = dal.getTable();
            return dt;
        }

        public Boolean MaNV_not_Exist(string maNV)
        {
            Boolean kq = true;
            DataRow[] rows = dal.TimKiemTheoMa(maNV);
            if (rows.Length > 0)
            {
                kq = false;
            }
            return kq;
        }

        public Boolean add_New_NV(NhanVienDTO nv)
        {
            Boolean kq = false;
            if (MaNV_not_Exist(nv.MaNV.ToString()))
            {
                nv.TrangThai = 1;
                dal.Add(nv);
                kq = true;
            }
            return kq;
        }

        public DataRow[] getFilter_NV(string strFilter)
        {
            return dal.TimKiemTheoDieuKien(strFilter);
        }

        public Boolean update_NV(NhanVienDTO nv)
        {
            if (MaNV_not_Exist(nv.MaNV.ToString()))
                return false;
            
            dal.Update(nv);
            return true;
        }

        public Boolean delete_NV(string maNV)
        {
            if (MaNV_not_Exist(maNV)) 
                return false;
            dal.delete(maNV);
            return true;
        }

        public string XoaVinhVien(string maNV)
        {
            if (MaNV_not_Exist(maNV))
                return "Nhân viên không tồn tại!";

            HoaDonDAL hdDal = new HoaDonDAL();
            if (hdDal.KiemTraNhanVienTonTai(maNV))
                return "Nhân viên đã phát sinh Hóa Đơn, không thể xóa vĩnh viễn!";

            TaiKhoanDAL tkDal = new TaiKhoanDAL();
            tkDal.deleteByMaNV(maNV);

            dal.hardDelete(maNV);
            return ""; 
        }

        public string LayMaNV(string tenDangNhap)
        {
            TaiKhoanDAL tkDal = new TaiKhoanDAL();
            DataRow[] rows = tkDal.TimKiemTheoTenDangNhap(tenDangNhap);
            if (rows.Length > 0 && rows[0]["MaNV"] != DBNull.Value)
            {
                return rows[0]["MaNV"].ToString();
            }
            return "";
        }
    }
}
