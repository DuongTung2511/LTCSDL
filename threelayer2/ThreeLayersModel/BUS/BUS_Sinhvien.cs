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
    public class BUS_SinhVien
    {
        private DAL_sinhvien dalSV = new DAL_sinhvien();

        public DataTable GetSinhVien() { return dalSV.GetTableSinhVien(); }

        public bool ThemSV(Sinhvien sv) { return dalSV.InsertSV(sv); }

        public bool SuaSV(Sinhvien sv) { return dalSV.UpdateSV(sv); }

        public bool XoaSV(string masv) { return dalSV.DeleteSV(masv); }

        public DataTable TimKiemSV(string keyword) { return dalSV.SearchSV(keyword); }
    }
}