# Hệ thống Quản lý Sinh viên - Mã nguồn 3 lớp (BUS, DAL, DTO, GUI)

Dưới đây là toàn bộ mã nguồn của ứng dụng quản lý sinh viên theo mô hình 3 lớp, được trình bày trong một file Markdown duy nhất.

## 1. BUS (Business Logic Layer) - `BUS_qlsv.cs`

```csharp
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
        
        public DataRow[] getFilter_Hoten_SV(string strFilter)
        {
            DataRow[] rows = db.getTable("sinhvien").Select(strFilter);
            return rows;
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

        public bool delete_SV(string masv)
        {
            if (Masv_not_Exist(masv)) 
                return false;
            db.deleteSinhvien(masv);
            return true;
        }
    }
}
##2. DAL (Data Access Layer)
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
##3. DTO (Data Transfer Objects)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class Lop
    {
        public string Malop { get; set; }
        public string Tenlop { get; set; }
    }

    public class Sinhvien
    {
        public string Masv { get; set; }
        public string Hoten { get; set; }
        public bool Gioitinh { get; set; }
        public DateTime Ngaysinh { get; set; }
        public string Diachi { get; set; }
        public string Malop { get; set; }
    }
}
##4. GUI (Windows Forms)
using BUS;
using DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.LinkLabel;

namespace ThreeLayersModel
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        BUS_qlsv qlsv = new BUS_qlsv();
        private Boolean checkInput()
        {
            Boolean kq = true;
            if (txt_masv.Text == "")
            {
                kq = false;
                txt_masv.Focus();
            }
            else if (txt_tensv.Text == "")
            {
                kq = false;
                txt_tensv.Focus();
            }
            else if (txt_diachi.Text == "")
            {
                kq = false;
                txt_diachi.Focus();
            }
            else if (cbb_tenlop.SelectedIndex < 0)
            {
                kq = false;
                cbb_tenlop.Focus();
            }
            return kq;
        }
        private void getGridSinhvien()
        {
            DataViewManager dvm = qlsv.getDataset().DefaultViewManager;
            dtgdanhsach.DataSource = dvm;
            dtgdanhsach.DataMember = "sinhvien";

            dtgdanhsach.Columns["masv"].Width = 60;
            dtgdanhsach.Columns["masv"].HeaderText = "Mã SV";
            dtgdanhsach.Columns["hoten"].HeaderText = "Họ và tên";
            dtgdanhsach.Columns["gioitinh"].Width = 40;
            dtgdanhsach.Columns["gioitinh"].HeaderText = "Giới tính";

            dtgdanhsach.Columns["ngaysinh"].Width = 70;
            dtgdanhsach.Columns["ngaysinh"].HeaderText = "Ngày sinh";
            dtgdanhsach.Columns["diachi"].Width = 100;
            dtgdanhsach.Columns["diachi"].HeaderText = "Địa chỉ";
            dtgdanhsach.Columns["malop"].HeaderText ="Mã lớp" ;
            dtgdanhsach.ReadOnly = true;
        }
        private void getLop()
        {
            cbb_malop.Items.Clear();
            cbb_tenlop.Items.Clear();
            List<Lop> dsLop = qlsv.getDsLop();
            foreach (Lop lp in dsLop)
            {
                cbb_tenlop.Items.Add(lp.Tenlop);
                cbb_malop.Items.Add(lp.Malop);
            }
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            getGridSinhvien();
            getLop();
        }

        private void btn_them_Click(object sender, EventArgs e)
        {
            if (checkInput())
            {
                Sinhvien s = new Sinhvien();
                s.Masv = txt_masv.Text;
                s.Hoten = txt_tensv.Text;
                if (rdb_nam.Checked)
                {
                    s.Gioitinh = true;
                }
                else
                {
                    s.Gioitinh = false;
                }
                s.Ngaysinh = dtp_ngsinh.Value;
                s.Diachi = txt_diachi.Text;
                s.Malop = cbb_malop.Items[cbb_tenlop.SelectedIndex].ToString();

                Boolean kq = qlsv.add_New_SV(s);
                if (!kq)
                {
                    MessageBox.Show("Thêm mới không thành công. Có thể mã sinh viên đã tồn tại!"); 
                }
            }
            else
            {
                MessageBox.Show("Bạn chưa nhập đủ dữ liệu!");
            }
        }

        private void cbb_tenlop_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbb_tenlop.SelectedIndex >= 0)
                cbb_malop.SelectedIndex = cbb_tenlop.SelectedIndex;
        }

        private void dtgdanhsach_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dtgdanhsach.Rows.Count) return;
            var dgvRow = dtgdanhsach.Rows[e.RowIndex];
            if (dgvRow.IsNewRow) return;

            DataRowView row = dgvRow.DataBoundItem as DataRowView;
            if (row == null) return;

            txt_masv.Text = row["masv"].ToString();
            txt_tensv.Text = row["hoten"].ToString();
            bool gt = Convert.ToBoolean(row["gioitinh"]);
            rdb_nam.Checked = gt;
            rdb_nu.Checked = !gt;
            dtp_ngsinh.Value = Convert.ToDateTime(row["ngaysinh"]);
            txt_diachi.Text = row["diachi"].ToString();

            string malop = row["malop"].ToString();
            int idx = cbb_malop.Items.IndexOf(malop);
            cbb_malop.SelectedIndex = idx;
            cbb_tenlop.SelectedIndex = idx;
        }

        private void btn_sua_Click(object sender, EventArgs e)
        {
            if (dtgdanhsach.CurrentRow == null || dtgdanhsach.CurrentRow.IsNewRow)
            {
                MessageBox.Show("Chưa chọn sinh viên cần sửa!", "Thông báo");
                return;
            }
            if (!checkInput()) return;

            Sinhvien s = new Sinhvien();
            s.Masv = txt_masv.Text.Trim(); 
            s.Hoten = txt_tensv.Text.Trim();
            s.Gioitinh = rdb_nam.Checked;
            s.Ngaysinh = dtp_ngsinh.Value;
            s.Diachi = txt_diachi.Text.Trim();
            s.Malop = cbb_malop.SelectedItem.ToString();

            if (qlsv.update_SV(s))
            {
                getGridSinhvien();
                lammoi();
                MessageBox.Show("Cập nhật thành công!", "Thông báo");
            }
            else
            {
                MessageBox.Show("Cập nhật thất bại!", "Lỗi");
            }
        }

        private void btn_xoa_Click(object sender, EventArgs e)
        {
            if (dtgdanhsach.CurrentRow == null || dtgdanhsach.CurrentRow.IsNewRow)
            {
                MessageBox.Show("Chưa chọn sinh viên cần xoá!", "Thông báo");
                return;
            }

            string masv = dtgdanhsach.CurrentRow.Cells["masv"].Value.ToString();
            DialogResult ret = MessageBox.Show("Bạn có chắc chắn muốn xoá sinh viên " + masv + "?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (ret == DialogResult.Yes)
            {
                if (qlsv.delete_SV(masv))
                {
                    getGridSinhvien();
                    lammoi();
                    MessageBox.Show("Xoá thành công!", "Thông báo");
                }
                else
                {
                    MessageBox.Show("Xoá thất bại!", "Lỗi");
                }
            }
        }

        private void bnt_lammoi_Click(object sender, EventArgs e)
        {
            lammoi();
        }
        private void lammoi()
        {
            txt_masv.Clear();
            txt_tensv.Clear();
            txt_diachi.Clear();
            txtkeyword.Clear();
            dtp_ngsinh.Value = DateTime.Now;
            rdb_nam.Checked = false;
            rdb_nu.Checked = false;
            cbb_tenlop.SelectedIndex = -1;
            cbb_malop.SelectedIndex = -1;
            dtgdanhsach.ClearSelection();
            txt_masv.Focus();
        }
        private void filter_dssv()
        {
            DataRow[] rows = qlsv.getFilter_Hoten_SV("hoten LIKE '%" + txtkeyword.Text.Replace("'", "''") + "%'");

            if (rows.Length > 0)
            {
                dtgdanhsach.DataSource = rows.CopyToDataTable();
            }
        }

        private void txtkeyword_TextChanged(object sender, EventArgs e)
        {
            filter_dssv();
        }
    }
}