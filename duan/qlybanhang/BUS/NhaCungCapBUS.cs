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
            return dal.ds;
        }

        public DataTable getTableNhaCungCap()
        {
            return dal.getTable();
        }

        public Boolean MaNCC_not_Exist(string maNCC)
        {
            Boolean kq = true;
            DataRow[] rows = dal.getTable().Select("MaNCC='" + maNCC.Replace("'", "''") + "'");
            if (rows.Length > 0)
            {
                kq = false;
            }
            return kq;
        }

        public Boolean add_New_NCC(NhaCungCapDTO ncc)
        {
            Boolean kq = false;
            if (MaNCC_not_Exist(ncc.MaNCC))
            {
                DataRow r = dal.ds.Tables["NhaCungCap"].NewRow();
                r["MaNCC"] = ncc.MaNCC;
                r["TenNCC"] = ncc.TenNCC;
                r["SoDienThoai"] = ncc.SoDienThoai;
                r["DiaChi"] = ncc.DiaChi;
                dal.addRow(r);
                kq = true;
            }
            return kq;
        }

        public DataRow[] getFilter_NCC(string strFilter)
        {
            return dal.getTable().Select(strFilter);
        }

        public Boolean update_NCC(NhaCungCapDTO ncc)
        {
            DataRow[] rows = dal.getTable().Select("MaNCC = '" + ncc.MaNCC.Replace("'", "''") + "'");
            if (rows.Length == 0)
                return false;

            DataRow r = rows[0];
            r.BeginEdit();
            r["TenNCC"] = ncc.TenNCC;
            r["SoDienThoai"] = ncc.SoDienThoai;
            r["DiaChi"] = ncc.DiaChi;
            r.EndEdit();
            dal.update();
            return true;
        }

        public Boolean delete_NCC(string maNCC)
        {
            if (MaNCC_not_Exist(maNCC)) 
                return false;
            dal.delete(maNCC);
            return true;
        }
    }
}
