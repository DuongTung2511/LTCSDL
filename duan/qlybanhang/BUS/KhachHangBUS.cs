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
            return dal.ds;
        }

        public DataTable getTableKhachHang()
        {
            return dal.getTable();
        }

        public Boolean MaKH_not_Exist(string maKH)
        {
            Boolean kq = true;
            DataRow[] rows = dal.getTable().Select("MaKH='" + maKH.Replace("'", "''") + "'");
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
                DataRow r = dal.ds.Tables["KhachHang"].NewRow();
                r["MaKH"] = kh.MaKH;
                r["TenKH"] = kh.TenKH;
                r["SoDienThoai"] = kh.SoDienThoai;
                r["DiaChi"] = kh.DiaChi;
                dal.addRow(r);
                kq = true;
            }
            return kq;
        }

        public DataRow[] getFilter_KH(string strFilter)
        {
            return dal.getTable().Select(strFilter);
        }

        public Boolean update_KH(KhachHangDTO kh)
        {
            DataRow[] rows = dal.getTable().Select("MaKH = '" + kh.MaKH.Replace("'", "''") + "'");
            if (rows.Length == 0)
                return false;
            
            DataRow r = rows[0];
            r.BeginEdit();
            r["TenKH"] = kh.TenKH;
            r["SoDienThoai"] = kh.SoDienThoai;
            r["DiaChi"] = kh.DiaChi;
            r.EndEdit();
            dal.update();
            return true;
        }

        public Boolean delete_KH(string maKH)
        {
            if (MaKH_not_Exist(maKH)) 
                return false;
            dal.delete(maKH);
            return true;
        }
    }
}
