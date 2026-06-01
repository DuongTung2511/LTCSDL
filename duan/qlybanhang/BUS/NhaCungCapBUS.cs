using System;
using System.Data;
using DAL;
using DTO;

namespace BUS
{
    public class NhaCungCapBUS
    {
        private NhaCungCapDAL dal = new NhaCungCapDAL();

        public DataSet getDataset()
        {
            return dal.getDBtoDataset();
        }

        public DataTable getTableNhaCungCap()
        {
            DataTable dt = dal.getTable();
            return dt;
        }

        public DataTable LayDanhSachNCCDangHoatDong()
        {
            return dal.LayDanhSachNCCDangHoatDong();
        }

        public Boolean MaNCC_not_Exist(string maNCC)
        {
            Boolean kq = true;
            DataRow[] rows = dal.TimKiemTheoMa(maNCC);
            if (rows.Length > 0)
            {
                kq = false;
            }
            return kq;
        }

        public Boolean add_New_NCC(NhaCungCapDTO ncc)
        {
            Boolean kq = false;
            if (MaNCC_not_Exist(ncc.MaNCC.ToString()))
            {
                ncc.TrangThai = 1;
                dal.Add(ncc);
                kq = true;
            }
            return kq;
        }

        public DataRow[] getFilter_NCC(string strFilter)
        {
            return dal.TimKiemTheoDieuKien(strFilter);
        }

        public Boolean update_NCC(NhaCungCapDTO ncc)
        {
            if (MaNCC_not_Exist(ncc.MaNCC.ToString()))
                return false;

            dal.Update(ncc);
            return true;
        }

        public Boolean delete_NCC(string maNCC)
        {
            if (MaNCC_not_Exist(maNCC)) 
                return false;
            dal.delete(maNCC);
            return true;
        }

        public string XoaVinhVien(string maNCC)
        {
            if (MaNCC_not_Exist(maNCC))
                return "Nhà cung cấp không tồn tại!";

            SanPhamDAL spDal = new SanPhamDAL();
            if (spDal.KiemTraNhaCungCapTonTai(maNCC))
                return "Nhà cung cấp đã có Sản Phẩm, không thể xóa vĩnh viễn!";

            dal.hardDelete(maNCC);
            return ""; 
        }
    }
}
