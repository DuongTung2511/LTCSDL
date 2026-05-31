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
            return dal.ds;
        }

        public DataTable getTableSanPham()
        {
            return dal.getTable();
        }

        public Boolean MaSP_not_Exist(string maSP)
        {
            Boolean kq = true;
            DataRow[] rows = dal.getTable().Select("MaSP='" + maSP.Replace("'", "''") + "'");
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
                DataRow r = dal.ds.Tables["SanPham"].NewRow();
                r["MaSP"] = sp.MaSP;
                r["TenSP"] = sp.TenSP;
                r["MaNCC"] = sp.MaNCC;
                r["GiaBan"] = sp.GiaBan;
                r["SoLuongTon"] = sp.SoLuongTon;
                r["TrangThai"] = 1;
                dal.addRow(r);
                kq = true;
            }
            return kq;
        }

        public DataRow[] getFilter_SP(string strFilter)
        {
            return dal.getTable().Select(strFilter);
        }

        public Boolean update_SP(SanPhamDTO sp)
        {
            DataRow[] rows = dal.getTable().Select("MaSP = '" + sp.MaSP.Replace("'", "''") + "'");
            if (rows.Length == 0)
                return false;

            DataRow r = rows[0];
            r.BeginEdit();
            r["TenSP"] = sp.TenSP;
            r["MaNCC"] = sp.MaNCC;
            r["GiaBan"] = sp.GiaBan;
            r["SoLuongTon"] = sp.SoLuongTon;
            r["TrangThai"] = sp.TrangThai;
            r.EndEdit();
            dal.update();
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
