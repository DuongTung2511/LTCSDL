using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace thuchanh
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        DataTable dt = new DataTable();

        private string connectionString = Properties.Settings.Default.strconnect;
        private void Form1_Load(object sender, EventArgs e)
        {
            get_dssv();
            get_lop();
        }
        private void get_dssv()
        {
            dt.Clear();
            string strSql = "SELECT dbo.sinhvien.masv, dbo.sinhvien.hoten, dbo.sinhvien.gioitinh, dbo.sinhvien.ngaysinh, dbo.sinhvien.diachi, dbo.sinhvien.malop, dbo.lop.tenlop FROM dbo.sinhvien INNER JOIN dbo.lop ON dbo.sinhvien.malop = dbo.lop.malop";
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlDataAdapter da = new SqlDataAdapter(strSql, conn))
            {
                da.Fill(dt);
            }

            // Cấu hình DataGridView
            dtgdanhsach.DataSource = dt;
            dtgdanhsach.Columns["masv"].Width = 60;
            dtgdanhsach.Columns["masv"].HeaderText = "Mã SV";
            dtgdanhsach.Columns["hoten"].HeaderText = "Họ và tên";
            dtgdanhsach.Columns["gioitinh"].Width = 60;
            dtgdanhsach.Columns["gioitinh"].HeaderText = "Giới tính";
            dtgdanhsach.Columns["ngaysinh"].Width = 70;
            dtgdanhsach.Columns["ngaysinh"].HeaderText = "Ngày sinh";
            dtgdanhsach.Columns["diachi"].Width = 100;
            dtgdanhsach.Columns["diachi"].HeaderText = "Địa chỉ";
            dtgdanhsach.Columns["malop"].Visible = false;
            dtgdanhsach.Columns["tenlop"].Width = 150;
            dtgdanhsach.Columns["tenlop"].HeaderText = "Tên lớp";
            dtgdanhsach.ReadOnly = true;
        }
        private void get_lop()
        {
            cbb_tenlop.Items.Clear();
            cbb_malop.Items.Clear();

            string strsql = "SELECT malop, tenlop FROM lop";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(strsql, conn))
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        cbb_tenlop.Items.Add(dr["tenlop"].ToString());
                        cbb_malop.Items.Add(dr["malop"].ToString());
                    }
                }
            }
        }
        private void filter_dssv()
        {
            DataRow[] rows = dt.Select("hoten LIKE '%" + txtkeyword.Text + "%'");
            if (rows.Length > 0) dtgdanhsach.DataSource = rows.CopyToDataTable();
        }

        private void btntim_Click(object sender, EventArgs e)
        {
            filter_dssv();
        }

        private void txtkeyword_TextChanged(object sender, EventArgs e)
        {
            filter_dssv();
        }
       
        private bool validate_input(bool type)
        {
            bool err = true;
            if (txt_masv.Text.Trim() == "")
            {
                MessageBox.Show("Chưa nhập mã sinh viên!", "Lỗi nhập dữ liệu", MessageBoxButtons.OK,MessageBoxIcon.Warning); 
                txt_masv.Focus();
                err = false;
            }
            else if (type && exist_svid())
            {
                MessageBox.Show("Mã sinh viên đã tồn tại!","Lỗi nhập dữ liệu", MessageBoxButtons.OK,MessageBoxIcon.Warning);
                txt_masv.Focus();
                txt_masv.SelectionStart = 0;
                txt_masv.SelectionLength = txt_masv.Text.Length;
                err = false;
            }
            else if (!type &&(dtgdanhsach.Rows[dtgdanhsach.CurrentCell.RowIndex].Cells["masv"].Value.ToString()!=txt_masv.Text)) 
            {
                MessageBox.Show("Không được phép sửa mã sinh viên!", "Lỗi nhập dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_masv.Text =dtgdanhsach.Rows[dtgdanhsach.CurrentCell.RowIndex].Cells["masv"].Value.ToString();
                txt_masv.Focus();
                txt_masv.SelectionStart = 0;
                txt_masv.SelectionLength = txt_masv.Text.Length;
                err = false;
            }
            else if (txt_tensv.Text.Trim() == "")
            {
                MessageBox.Show("Chưa nhập họ và tên!", "Lỗi nhập dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_tensv.Focus();
                err = false;
            }
            else if (cbb_tenlop.Text.Trim() == "")
            {
                MessageBox.Show("Chưa chọn lớp học!", "Lỗi nhập dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning); 
                cbb_tenlop.Focus();
                err = false;
            }

            return err;
        }
        private bool exist_svid()
        {
            bool err = false;
            for (int i = 0; i < dt.Rows.Count; i++)
                if(dt.Rows[i]["masv"].ToString().Trim().ToUpper() == txt_masv.Text.Trim().ToUpper())
                {
                    err = true; break;
                }
            return err;
        }

        private void btn_them_Click(object sender, EventArgs e)
        {
            if (validate_input(true)) themsv();
        }

        private void dtgdanhsach_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dtgdanhsach.Rows.Count)
            {
                lammoi();
                return;
            }

            DataGridViewRow dgvRow = dtgdanhsach.Rows[e.RowIndex];
            if (dgvRow.IsNewRow)
            {
                lammoi();
                return;
            }
            DataRowView row = dgvRow.DataBoundItem as DataRowView;

            txt_masv.Text = row["masv"].ToString();
            txt_tensv.Text = row["hoten"].ToString();

            bool gt = Convert.ToBoolean(row["gioitinh"]);
            rdb_nam.Checked = gt;
            rdb_nu.Checked = !gt;

            dtp_ngsinh.Value = Convert.ToDateTime(row["ngaysinh"]);
            txt_diachi.Text = row["diachi"].ToString();

            // Đồng bộ combobox lớp
            string malop = row["malop"].ToString();
            string tenlop = row["tenlop"].ToString();
            int idx = cbb_malop.Items.IndexOf(malop);
            if (idx >= 0) 
                cbb_malop.SelectedIndex = idx;
            idx = cbb_tenlop.Items.IndexOf(tenlop);
            if (idx >= 0) 
                cbb_tenlop.SelectedIndex = idx;
        }
        private int themsv()
        {
            int kq = 0;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string selectCmd = "SELECT * FROM sinhvien WHERE masv = '-1'";
                using (SqlDataAdapter da = new SqlDataAdapter(selectCmd, conn))
                {
                    DataSet ds = new DataSet();
                    da.Fill(ds, "sinhvien");

                    DataTable tbl = ds.Tables["sinhvien"];
                    DataRow row = tbl.NewRow();

                    row.BeginEdit();
                    row["masv"] = txt_masv.Text;
                    row["hoten"] = txt_tensv.Text;
                    row["gioitinh"] = rdb_nam.Checked ? 1 : 0;
                    row["ngaysinh"] = dtp_ngsinh.Value;
                    row["diachi"] = txt_diachi.Text;
                    row["malop"] = cbb_malop.Items[cbb_tenlop.SelectedIndex].ToString();
                    row.EndEdit();

                    tbl.Rows.Add(row);

                    new SqlCommandBuilder(da);
                    kq = da.Update(ds, "sinhvien");
                    ds.AcceptChanges();
                }
            }

            if (kq > 0)
            {
                get_dssv();
                filter_dssv();
            }
            return kq;
        }

        private int suasv()
        {
            int kq = 0;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string selectCmd = "SELECT * FROM sinhvien WHERE masv = @masv";
                using (SqlDataAdapter da = new SqlDataAdapter(selectCmd, conn))
                {
                    da.SelectCommand.Parameters.AddWithValue("@masv", txt_masv.Text);

                    DataSet ds = new DataSet();
                    da.Fill(ds, "sinhvien");

                    if (ds.Tables["sinhvien"].Rows.Count == 0)
                        return 0;

                    DataRow row = ds.Tables["sinhvien"].Rows[0];

                    row.BeginEdit();
                    row["hoten"] = txt_tensv.Text;
                    row["gioitinh"] = rdb_nam.Checked ? 1 : 0;
                    row["ngaysinh"] = dtp_ngsinh.Value;
                    row["diachi"] = txt_diachi.Text;
                    row["malop"] = cbb_malop.Items[cbb_tenlop.SelectedIndex].ToString();
                    row.EndEdit();

                    new SqlCommandBuilder(da);
                    kq = da.Update(ds, "sinhvien");
                    ds.AcceptChanges();
                }
            }

            if (kq > 0) get_dssv(); 
            return kq;
        }
        private int xoasv()
        {
            int kq = 0;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string selectCmd = "SELECT * FROM sinhvien WHERE masv = @masv";
                using (SqlDataAdapter da = new SqlDataAdapter(selectCmd, conn))
                {
                    da.SelectCommand.Parameters.AddWithValue("@masv", txt_masv.Text);

                    DataSet ds = new DataSet();
                    da.Fill(ds, "sinhvien");

                    if (ds.Tables["sinhvien"].Rows.Count == 0)
                        return 0;

                    DataRow row = ds.Tables["sinhvien"].Rows[0];
                    row.Delete();

                    new SqlCommandBuilder(da);
                    kq = da.Update(ds, "sinhvien");
                    ds.AcceptChanges();
                }
            }

            if (kq > 0) get_dssv();
            return kq;
        }
        private void btn_sua_Click(object sender, EventArgs e)
        {
            if (validate_input(false))
            {
                suasv();
            }
        }

        private void btn_xoa_Click(object sender, EventArgs e)
        {
            if (validate_input(false) && MessageBox.Show("Bạn có chắc chắn muốn xóa không ? ","Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==DialogResult.Yes)
            {
                xoasv();
            }
        }

        private void cbb_tenlop_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbb_tenlop.SelectedIndex > 0)
            {
                cbb_malop.SelectedIndex = cbb_tenlop.SelectedIndex;
            }
        }
        private void lammoi()
        {
            txt_masv.Text = "";
            txt_tensv.Text = "";
            txt_diachi.Text = "";
            txtkeyword.Text = "";

            dtp_ngsinh.Value = DateTime.Now;

            rdb_nam.Checked = false;
            rdb_nu.Checked = false;

            cbb_tenlop.SelectedIndex = -1;
            cbb_malop.SelectedIndex = -1;

            dtgdanhsach.ClearSelection();
        }

        private void bnt_lammoi_Click(object sender, EventArgs e)
        {
            lammoi();
        }
    }
} 
