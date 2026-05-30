using System;
using System.Data;
using DAL;
using DTO;

namespace BUS
{
    public class TaiKhoanBUS
    {
        private TaiKhoanDAL dal = new TaiKhoanDAL();

        public DataSet getDataset()
        {
            return dal.ds;
        }

        public DataRow DangNhap(string tenDangNhap, string matKhau)
        {
            dal.reload();
            DataTable dt = dal.getTable();
            DataRow[] rows = dt.Select(
                "TenDangNhap = '" + tenDangNhap.Replace("'", "''") + "' AND MatKhau = '" + matKhau.Replace("'", "''") + "'");
            return rows.Length > 0 ? rows[0] : null;
        }

        public Boolean MaTK_not_Exist(string tenDangNhap)
        {
            Boolean kq = true;
            DataRow[] rows = dal.getTable().Select("TenDangNhap='" + tenDangNhap.Replace("'", "''") + "'");
            if (rows.Length > 0)
            {
                kq = false;
            }
            return kq;
        }

        public Boolean DangKy(TaiKhoanDTO tk)
        {
            Boolean kq = false;
            if (MaTK_not_Exist(tk.TenDangNhap))
            {
                DataRow r = dal.ds.Tables["TaiKhoan"].NewRow();
                r["TenDangNhap"] = tk.TenDangNhap;
                r["MatKhau"] = tk.MatKhau;
                r["Quyen"] = "nhanvien";
                dal.addRow(r);
                kq = true;
            }
            return kq;
        }

        public DataTable getTableTaiKhoan()
        {
            return dal.getTable();
        }

        public DataTable LayDanhSachTaiKhoanDayDu()
        {
            DataTable dtTaiKhoan = dal.getTable();
            NhanVienDAL nvDal = new NhanVienDAL();
            DataTable dtNhanVien = nvDal.getTable();

            DataTable result = new DataTable();
            result.Columns.Add("TenDangNhap", typeof(string));
            result.Columns.Add("MatKhau", typeof(string));
            result.Columns.Add("Quyen", typeof(string));
            result.Columns.Add("TenNV", typeof(string));
            result.Columns.Add("MaNV", typeof(string));

            foreach (DataRow rTK in dtTaiKhoan.Rows)
            {
                DataRow rNew = result.NewRow();
                rNew["TenDangNhap"] = rTK["TenDangNhap"];
                rNew["MatKhau"] = rTK["MatKhau"];
                rNew["Quyen"] = rTK["Quyen"];
                rNew["MaNV"] = rTK["MaNV"];
                
                DataRow[] rowsNV = null;
                if(rTK["MaNV"] != DBNull.Value && rTK["MaNV"].ToString() != "")
                {
                    rowsNV = dtNhanVien.Select("MaNV = '" + rTK["MaNV"].ToString().Replace("'", "''") + "'");
                }

                if (rowsNV != null && rowsNV.Length > 0)
                {
                    rNew["TenNV"] = rowsNV[0]["TenNV"];
                }
                else
                {
                    rNew["TenNV"] = "Chưa gắn nhân viên";
                }
                
                result.Rows.Add(rNew);
            }
            return result;
        }

        public Boolean add_New_TK(TaiKhoanDTO tk)
        {
            Boolean kq = false;
            if (MaTK_not_Exist(tk.TenDangNhap))
            {
                DataRow r = dal.ds.Tables["TaiKhoan"].NewRow();
                r["TenDangNhap"] = tk.TenDangNhap;
                r["MatKhau"] = tk.MatKhau;
                r["Quyen"] = tk.Quyen;
                r["MaNV"] = string.IsNullOrEmpty(tk.MaNV) ? (object)DBNull.Value : tk.MaNV;
                dal.addRow(r);
                kq = true;
            }
            return kq;
        }

        public DataRow[] getFilter_TK(string strFilter)
        {
            return dal.getTable().Select(strFilter);
        }

        public Boolean update_TK(TaiKhoanDTO tk)
        {
            DataRow[] rows = dal.getTable().Select("TenDangNhap = '" + tk.TenDangNhap.Replace("'", "''") + "'");
            if (rows.Length == 0)
                return false;
            
            DataRow r = rows[0];
            r.BeginEdit();
            r["MatKhau"] = tk.MatKhau;
            r["Quyen"] = tk.Quyen;
            r["MaNV"] = string.IsNullOrEmpty(tk.MaNV) ? (object)DBNull.Value : tk.MaNV;
            r.EndEdit();
            dal.update();
            return true;
        }

        public Boolean delete_TK(string tenDangNhap)
        {
            if (MaTK_not_Exist(tenDangNhap)) 
                return false;

            // Kiểm tra xem tài khoản này có đang được sử dụng bởi nhân viên nào không
            DataRow[] rTks = dal.getTable().Select("TenDangNhap = '" + tenDangNhap.Replace("'", "''") + "'");
            if (rTks.Length > 0 && rTks[0]["MaNV"] != DBNull.Value && rTks[0]["MaNV"].ToString() != "")
            {
                throw new Exception("Tài khoản này đang được gắn cho nhân viên mã: " + rTks[0]["MaNV"] + ", không thể xóa. Vui lòng gỡ tài khoản khỏi nhân viên trước.");
            }

            dal.delete(tenDangNhap);
            return true;
        }
    }
}
