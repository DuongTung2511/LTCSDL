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
            return dal.ds;
        }

        public DataTable getTableNhanVien()
        {
            return dal.getTable();
        }

        public Boolean MaNV_not_Exist(string maNV)
        {
            Boolean kq = true;
            DataRow[] rows = dal.getTable().Select("MaNV='" + maNV.Replace("'", "''") + "'");
            if (rows.Length > 0)
            {
                kq = false;
            }
            return kq;
        }

        public Boolean add_New_NV(NhanVienDTO nv)
        {
            Boolean kq = false;
            if (MaNV_not_Exist(nv.MaNV))
            {
                DataRow r = dal.ds.Tables["NhanVien"].NewRow();
                r["MaNV"] = nv.MaNV;
                r["TenNV"] = nv.TenNV;
                r["GioiTinh"] = nv.GioiTinh;
                r["NgaySinh"] = nv.NgaySinh;
                r["SoDienThoai"] = nv.SoDienThoai;
                r["DiaChi"] = nv.DiaChi;
                r["TrangThai"] = 1;
                dal.addRow(r);
                kq = true;
            }
            return kq;
        }

        public DataRow[] getFilter_NV(string strFilter)
        {
            return dal.getTable().Select(strFilter);
        }

        public Boolean update_NV(NhanVienDTO nv)
        {
            DataRow[] rows = dal.getTable().Select("MaNV = '" + nv.MaNV.Replace("'", "''") + "'");
            if (rows.Length == 0)
                return false;

            DataRow r = rows[0];
            r.BeginEdit();
            r["TenNV"] = nv.TenNV;
            r["GioiTinh"] = nv.GioiTinh;
            r["NgaySinh"] = nv.NgaySinh;
            r["SoDienThoai"] = nv.SoDienThoai;
            r["DiaChi"] = nv.DiaChi;
            r["TrangThai"] = nv.TrangThai;
            r.EndEdit();
            dal.update();
            return true;
        }

        public Boolean delete_NV(string maNV)
        {
            if (MaNV_not_Exist(maNV)) 
                return false;
            dal.delete(maNV);
            return true;
        }

        public string LayMaNV(string tenDangNhap)
        {
            TaiKhoanBUS tkBus = new TaiKhoanBUS();
            DataTable dtTK = tkBus.getTableTaiKhoan();
            DataRow[] rows = dtTK.Select("TenDangNhap = '" + tenDangNhap.Replace("'", "''") + "'");
            return rows.Length > 0 ? rows[0]["MaNV"].ToString() : "";
        }

        public DataTable LayDanhSachNVDangLam()
        {
            DataTable dt = dal.getTable();
            DataRow[] rows = dt.Select("TrangThai = 1 OR TrangThai IS NULL");
            if (rows.Length > 0)
                return rows.CopyToDataTable();
            else
                return dt.Clone();
        }
    }
}
