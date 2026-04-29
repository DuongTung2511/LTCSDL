using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace thuchanh2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            get_database();
            view_dssv();
            view_lop();
        }
        SqlConnection conn = new SqlConnection(Properties.Settings.Default.strconnect);
        DataSet ds = new DataSet();
        SqlDataAdapter da_sv = new SqlDataAdapter();
        SqlDataAdapter da_lop = new SqlDataAdapter();
        SqlDataAdapter da_sv_lop = new SqlDataAdapter();
        private void get_database()
        {
            //Gắn các cầu nối DataAdapter vào các CommandBuilder 
            SqlCommandBuilder cmdbd_sv = new SqlCommandBuilder(da_sv);
            SqlCommandBuilder cmdbd_lop = new SqlCommandBuilder(da_lop);
            //Lấy dữ liệu từ bảng sinhvien về lưu vào DataSet 
            da_sv.SelectCommand = new SqlCommand("Select * from sinhvien order by masv",conn); 
            da_sv.TableMappings.Add("sinhvien", "sinhvien");
            da_sv.Fill(ds, "sinhvien");
            //Lấy dữ liệu từ bảng lop về lưu vào DataSet 
            da_lop.SelectCommand = new SqlCommand("Select * from lop order by malop", conn); 
            da_lop.TableMappings.Add("lop", "lop");
            da_lop.Fill(ds, "lop");
            //Thiết lập quan hệ giữa bảng Lop và bảng Sinhvien 
            DataRelation rela_lop_sv = new DataRelation("rela_lop_sv", ds.Tables["lop"].Columns["malop"],
            ds.Tables["Sinhvien"].Columns["malop"]);
            ds.Relations.Add(rela_lop_sv);
            //Lấy dữ liệu từ bảng sinhvien và bảng lop để tạo view sinhvien_lop 
            da_sv_lop.SelectCommand = new SqlCommand("SELECT  dbo.Sinhvien.masv, dbo.Sinhvien.hoten, dbo.Sinhvien.gioitinh,dbo.Sinhvien.ngaysinh, dbo.Sinhvien.diachi, dbo.Sinhvien.malop,dbo.Lop.tenlop FROM  dbo.Sinhvien INNER JOIN dbo.Lop ON dbo.Sinhvien.malop = dbo.Lop.malop ORDER BY dbo.Sinhvien.masv", conn);
            da_sv_lop.TableMappings.Add("sinhvien_lop","sinhvien_lop");
            da_sv_lop.Fill(ds, "sinhvien_lop");
        }
        private void view_dssv()
        {
            DataViewManager dvm = ds.DefaultViewManager;
            dtgdanhsach.DataSource = dvm;
            //Chỉ định bảng dữ liệu sẽ được hiển thị 
            dtgdanhsach.DataMember = "sinhvien_lop";
            dtgdanhsach.Columns["masv"].Width = 60;
            dtgdanhsach.Columns["masv"].HeaderText = "Mã SV";
            dtgdanhsach.Columns["hoten"].HeaderText = "Họ và tên";
            dtgdanhsach.Columns["gioitinh"].Width = 40;
            dtgdanhsach.Columns["gioitinh"].HeaderText = "Giới tính"; 
 
            dtgdanhsach.Columns["ngaysinh"].Width = 70;
            dtgdanhsach.Columns["ngaysinh"].HeaderText = "Ngày sinh";
            dtgdanhsach.Columns["diachi"].Width = 100;
            dtgdanhsach.Columns["diachi"].HeaderText = "Địa chỉ";
            dtgdanhsach.Columns["malop"].Visible = false;
            dtgdanhsach.Columns["tenlop"].Width = 80;
            dtgdanhsach.Columns["tenlop"].HeaderText = "Tên lớp";
            dtgdanhsach.ReadOnly = true;
        }
        private void view_lop()
        {
            cbb_tenlop.Items.Clear();
            cbb_malop.Items.Clear();
            DataTable dt = ds.Tables["lop"];
            foreach (DataRow row in dt.Rows)
            {
                cbb_malop.Items.Add(row["malop"]);
                cbb_tenlop.Items.Add(row["tenlop"]);
            }
        }
        private void filter_dssv()
        {
            DataTable dt = ds.Tables["sinhvien_lop"];
            DataRow[] rows = dt.Select("hoten LIKE '%" + txtkeyword.Text + "%'");
            if (rows.Length > 0) dtgdanhsach.DataSource = rows.CopyToDataTable();
        }

        private void txtkeyword_TextChanged(object sender, EventArgs e)
        {
            filter_dssv();
        }
        private void themsv()
        {
            //Tạo ra một DataRow chứa dữ liệu cần thêm để thêm vào sinhvien
            DataRow row = ds.Tables["sinhvien"].NewRow();
            row["masv"] = txt_masv.Text;
            row["hoten"] = txt_tensv.Text;
            row["gioitinh"] = rdb_nam.Checked ? 1 : 0;
            row["ngaysinh"] = dtp_ngsinh.Value;
            row["diachi"] = txt_diachi.Text;
            row["malop"] = cbb_malop.Items[cbb_tenlop.SelectedIndex].ToString();
            ds.Tables["sinhvien"].Rows.Add(row);
            //Tạo ra một DataRow chứa dữ liệu cần thêm để thêm vào sinhvien_lop để hiển thị ra lưới
            DataRow row1 = ds.Tables["sinhvien_lop"].NewRow();
            row1["masv"] = txt_masv.Text;
            row1["hoten"] = txt_tensv.Text;
            row1["gioitinh"] = rdb_nam.Checked ? 1 : 0;
            row1["ngaysinh"] = dtp_ngsinh.Value;
            row1["diachi"] = txt_diachi.Text;
            row1["malop"] =cbb_malop.Items[cbb_tenlop.SelectedIndex].ToString();
            row1["tenlop"] = cbb_tenlop.SelectedItem.ToString();
            ds.Tables["sinhvien_lop"].Rows.Add(row1);
            //Cập nhật thay đổi trong DataTable sinhvien lên server 
            da_sv.Update(ds, "sinhvien");

            ds.AcceptChanges();
        }
        private bool validate_input(bool type)
        {
            bool err = true;
            if (txt_masv.Text.Trim() == "")
            {
                MessageBox.Show("Chưa nhập mã sinh viên!", "Lỗi nhập dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning); 
                txt_masv.Focus();
                err = false;
            }
            else if (type && exist_svid())
            {
                MessageBox.Show("Mã sinh viên đã tồn tại!", "Lỗi nhập dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning); 
                txt_masv.Focus();
                txt_masv.SelectionStart = 0;
                txt_masv.SelectionLength = txt_masv.Text.Length;
                err = false;
            }
            else if (!type &&
            (dtgdanhsach.Rows[dtgdanhsach.CurrentCell.RowIndex].Cells["masv"].Value.ToString() != txt_masv.Text)) 
            {
                MessageBox.Show("Không được phép sửa mã sinh viên!","Lỗi nhập dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_masv.Text = dtgdanhsach.Rows[dtgdanhsach.CurrentCell.RowIndex].Cells["masv"].Value.ToString();
                txt_masv.Focus();
                txt_masv.SelectionStart = 0;
                txt_masv.SelectionLength = txt_masv.Text.Length;
                err = false;
            }else if (txt_tensv.Text.Trim() == "")
            {
                MessageBox.Show("Chưa nhập họ và tên!", "Lỗi nhập dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning); 
                txt_tensv.Focus();
                err = false;
            }
            else if (cbb_tenlop.SelectedIndex < 0)
            {
                MessageBox.Show("Chưa chọn lớp học!", "Lỗi nhập dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning); 
                cbb_tenlop.Focus();
                err = false;
            }
            return err;
        }
        private bool exist_svid()
        {
            DataTable dt = ds.Tables["sinhvien"];
            bool err = false;
            foreach (DataRow row in dt.Rows)
                if (row["masv"].ToString().Trim().ToUpper() == txt_masv.Text.Trim().ToUpper())
                {
                    err = true; break;
                }
            return err;
        }

        private void btn_them_Click(object sender, EventArgs e)
        {
            if (validate_input(true))
            {
                themsv();
                view_dssv();
            }
        }
        private void suasv()
        {
            int vt = dtgdanhsach.CurrentRow.Index;
            if (vt < dtgdanhsach.Rows.Count - 1)
            {
                //Sửa lại dữ liệu trong DataTable sinhvien 
                DataRow row = ds.Tables["sinhvien"].Rows[vt];
                row.BeginEdit();
                row["hoten"] = txt_tensv.Text;
                row["gioitinh"] = rdb_nam.Checked ? 1 : 0;
                row["ngaysinh"] = dtp_ngsinh.Value;
                row["diachi"] = txt_diachi.Text;

                row["malop"] = cbb_malop.Items[cbb_tenlop.SelectedIndex].ToString();
                row.EndEdit();
                //Sửa lại dữ liệu trong DataTable sinhvien_lop 
                DataRow row1 = ds.Tables["sinhvien_lop"].Rows[vt];
                row1.BeginEdit();
                row1["hoten"] = txt_tensv.Text;
                row1["gioitinh"] = rdb_nam.Checked ? 1 : 0;
                row1["ngaysinh"] = dtp_ngsinh.Value;
                row1["diachi"] = txt_diachi.Text;
                row1["malop"] = cbb_malop.Items[cbb_tenlop.SelectedIndex].ToString();
                row1["tenlop"] = cbb_tenlop.SelectedItem.ToString();
                row1.EndEdit();
                da_sv.Update(ds, "sinhvien");
                ds.AcceptChanges();
            }
        }

        private void dtgdanhsach_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            int i = e.RowIndex;
            if (i < dtgdanhsach.Rows.Count - 1)
            {
                txt_masv.Text = dtgdanhsach.Rows[i].Cells["masv"].Value.ToString();
                txt_tensv.Text = dtgdanhsach.Rows[i].Cells["hoten"].Value.ToString();
                if
                (Convert.ToBoolean(dtgdanhsach.Rows[i].Cells["gioitinh"].Value))
                {
                    rdb_nam.Checked = true;
                }
                else
                {
                    rdb_nu.Checked = true;
                }
                dtp_ngsinh.Value =Convert.ToDateTime(dtgdanhsach.Rows[i].Cells["ngaysinh"].Value);
                txt_diachi.Text = dtgdanhsach.Rows[i].Cells["diachi"].Value.ToString();
                cbb_tenlop.Text = dtgdanhsach.Rows[i].Cells["tenlop"].Value.ToString();
                cbb_malop.Text = dtgdanhsach.Rows[i].Cells["malop"].Value.ToString();
            }
        }

        private void btn_sua_Click(object sender, EventArgs e)
        {
            if (dtgdanhsach.CurrentRow.Index <dtgdanhsach.Rows.Count - 1 && validate_input(false))
            {
                suasv();
                view_dssv();
            }
        }
        private void xoasv()
        {
            int vt = dtgdanhsach.CurrentRow.Index;
            if (vt >= 0 && vt < ds.Tables["sinhvien"].Rows.Count)
            {
                ds.Tables["sinhvien"].Rows[vt].Delete();
                ds.Tables["sinhvien_lop"].Rows[vt].Delete();
                da_sv.Update(ds, "sinhvien");
                ds.AcceptChanges();
            }
            else MessageBox.Show("Hãy chọn một sinh viên trong danh sách trước khi xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btn_xoa_Click(object sender, EventArgs e)
        {
            int vt = dtgdanhsach.CurrentCell.RowIndex;
            if (vt >= 0 && vt < dtgdanhsach.Rows.Count - 1)
            {
                DialogResult dlog = MessageBox.Show("Bạn có chắc chắn xóa không ? ", "Thông báo", MessageBoxButtons.YesNo, 
                MessageBoxIcon.Question);
                if (dlog == DialogResult.Yes)
                {
                    xoasv();
                    view_dssv();
                }
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
