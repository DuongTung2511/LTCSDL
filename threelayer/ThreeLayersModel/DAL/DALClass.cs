using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class MyDatabase
    {
        private SqlConnection conn = null;
        private DataSet ds = null;
        private SqlDataAdapter da1 = null;
        private SqlDataAdapter da2 = null;
        SqlDataAdapter da_sv_lop = new SqlDataAdapter();
        public MyDatabase()
        {
            conn = new SqlConnection(Properties.Settings.Default.strconn);
            ds = new DataSet();
            da1 = new SqlDataAdapter();
            da2 = new SqlDataAdapter();
            SqlCommandBuilder cb1 = new SqlCommandBuilder(da1);
            SqlCommandBuilder cb2 = new SqlCommandBuilder(da2);

            da1.SelectCommand = new SqlCommand("Select * from sinhvien", conn);
            da1.TableMappings.Add("Table", "sinhvien");
            da1.Fill(ds, "sinhvien");

            da2.SelectCommand = new SqlCommand("Select * from lop", conn);
            da2.TableMappings.Add("Table", "lop");
            da2.Fill(ds, "lop");

            DataRelation rela_lop_sv = new DataRelation("Lop_Sinhvien",
                ds.Tables["lop"].Columns["malop"],
                ds.Tables["sinhvien"].Columns["malop"]);
            ds.Relations.Add(rela_lop_sv);

        }
        public DataSet getDBtoDataset()
        {
            return ds;
        }
        public DataTable getTable(string name)
        {
            return ds.Tables[name];
        }
        public DataTable getTable(int k)
        {
            DataTable dt = null;
            if (k >= 0 && k < ds.Tables.Count)
            {
                dt = ds.Tables[k];
            }
            return dt;
        }
        public void addRowtoSinhvien(DataRow r)
        {
            try
            {
                ds.Tables["sinhvien"].Rows.Add(r);
                da1.Update(ds, "sinhvien");
                ds.AcceptChanges();
            }
            catch { }
        }
        public void updateSinhvien()
        {
            da1.Update(ds, "sinhvien");
            ds.AcceptChanges();
        }

        public void deleteSinhvien(string masv)
        {
            DataRow[] rows = ds.Tables["sinhvien"].Select("masv = '" + masv.Replace("'", "''") + "'");
            if (rows.Length > 0)
            {
                rows[0].Delete();
                da1.Update(ds, "sinhvien");
                ds.AcceptChanges();
            }
        }
    }
}