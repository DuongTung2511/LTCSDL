using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS
{
    public class BUS_Lop
    {
        private DAL_Lop dalLop = new DAL_Lop();

        public DataTable GetLop() { return dalLop.GetTableLop(); }

        public bool ThemLop(Lop lp) { return dalLop.InsertLop(lp); }

        public bool SuaLop(string maLopCu, Lop lp)
        {
            return dalLop.UpdateLop(maLopCu, lp);
        }

        public bool XoaLop(string malop) { return dalLop.DeleteLop(malop); }
        public DataTable TimKiemLop(string keyword)
        {
            return dalLop.SearchLop(keyword);
        }
    }
}
