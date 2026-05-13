using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using DAL; 
using DTO; 
using System.Data; 
using System.Data.SqlClient;

namespace BUS
{
    public class BUS_qlsv
    {
        private MyDatabase db = new MyDatabase();
        public DataSet getDataset()
        {
            return db.getDBtoDataset();
        }
        public DataTable getTableLop()
        {
            DataTable dt = db.getTable("lop");
            return dt;
        }
        public List<Lop> getDsLop()
        {
            List<Lop> dsLop = new List<Lop>();
            DataTable dt = db.getTable("lop");
            Lop lp;
            DataRow r;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                lp = new Lop();
                r = dt.Rows[i];
                lp.Malop = r["malop"].ToString();
                lp.Tenlop = r["tenlop"].ToString();
                dsLop.Add(lp);
            }
            return dsLop;
        }
        public DataTable getTableSinhvien()
        {
            DataTable dt = db.getTable("sinhvien");
            return dt;
        }
        public DataTable getFilter_Hoten_SV(string strFilter)
        {
            DataRow[] rows = db.getTable("sinhvien").Select(strFilter);
            DataTable dt = rows.CopyToDataTable();
            return dt;
        }
        public Boolean add_New_SV(Sinhvien s)
        {
            Boolean kq = false;
            if (Masv_not_Exist(s.Masv))
            {
                DataRow r = db.getTable("sinhvien").NewRow();
                r["masv"] = s.Masv;
                r["hoten"] = s.Hoten;
                if (s.Gioitinh)
                {
                    r["gioitinh"] = true;
                }
                else
                {
                    r["gioitinh"] = false;
                }
                r["ngaysinh"] = s.Ngaysinh;
                r["diachi"] = s.Diachi;
                r["malop"] = s.Malop;

                db.addRowtoSinhvien(r);
                kq = true;
            }
            return kq;
        }
        private Boolean Masv_not_Exist(string masv)
        {
            Boolean kq = true;
            DataRow[] rows = db.getTable("sinhvien").Select("masv='" + masv + "'");
            if (rows.Length > 0)
            {
                kq = false;
            }
            return kq;
        }
        // Cập nhật thông tin sinh viên
        public bool update_SV(Sinhvien s)
        {
            DataRow[] rows = db.getTable("sinhvien").Select("masv = '" + s.Masv.Replace("'", "''") + "'");
            if (rows.Length == 0)
                return false;
            DataRow r = rows[0];
            r.BeginEdit();
            r["hoten"] = s.Hoten;
            r["gioitinh"] = s.Gioitinh;
            r["ngaysinh"] = s.Ngaysinh;
            r["diachi"] = s.Diachi;
            r["malop"] = s.Malop;
            r.EndEdit();
            db.updateSinhvien();
            return true;
        }

        // Xoá sinh viên
        public bool delete_SV(string masv)
        {
            if (Masv_not_Exist(masv)) // nếu không tồn tại
                return false;
            db.deleteSinhvien(masv);
            return true;
        }
    }
}