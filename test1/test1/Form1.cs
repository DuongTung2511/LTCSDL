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

namespace test1
{
    public partial class Form1 : Form
    {
        //kết nối
        static string connString = @"Data Source=Admin-PC;Initial Catalog=TestDB;User ID=sa;Password=Duong25112005;TrustServerCertificate=True;";
        SqlConnection conn = new SqlConnection(connString);
        public Form1()
        {   
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Load_Data();
            LoadLopChoListBox();
            dtg_sv.SelectionChanged += dtg_sv_SelectionChanged;
         
        }
        private void Load_Data()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                using (SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM SinhVien", conn))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dtg_sv.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //throw ex;
            }
        }
        private void LoadLopChoListBox()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                using (SqlDataAdapter da = new SqlDataAdapter("SELECT MaLop, TenLop FROM Lop", conn))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    cbb_malop.DataSource = dt;
                    cbb_malop.DisplayMember = "TenLop";
                    cbb_malop.ValueMember = "MaLop";
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void dtg_sv_SelectionChanged(object sender, EventArgs e)
        {
            if (dtg_sv.CurrentRow != null && !dtg_sv.CurrentRow.IsNewRow)
            {
                DataGridViewRow row = dtg_sv.CurrentRow;

                // Gán giá trị cho các TextBox
                txt_masv.Text = row.Cells["MaSV"].Value.ToString();
                txt_tensv.Text = row.Cells["TenSV"].Value.ToString();
                dtp_ngsinh.Value = Convert.ToDateTime(row.Cells["NgaySinh"].Value);
                txt_diachi.Text = row.Cells["DiaChi"].Value.ToString();

                //chọn mã lớp 
                string maLop = row.Cells["MaLop"].Value.ToString();
                cbb_malop.SelectedValue = maLop;

                // Xử lý giới tính 
                String gioitinh = row.Cells["GioiTinh"].Value.ToString();
                if (gioitinh == "Nam")
                {
                    rdb_nam.Checked = true;
                    rdb_nu.Checked = false;
                }
                else
                {
                    rdb_nam.Checked = false;
                    rdb_nu.Checked = true;
                }
            }
        }
        private void lammoi()
        {
            txt_masv.Text = "";
            txt_tensv.Text = "";
            txt_diachi.Text = "";

            dtp_ngsinh.Value = DateTime.Now;

            rdb_nam.Checked = false;
            rdb_nu.Checked = false;

            cbb_malop.SelectedIndex = -1;

            dtg_sv.ClearSelection();
            
        }
        private void btn_refresh_Click(object sender, EventArgs e)
        {
            lammoi();
        }
        // Hàm thực thi lệnh SQL (INSERT, UPDATE, DELETE)
        private int ExecuteNonQuery(string sql, Action<SqlCommand> addParams)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    addParams(cmd);
                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
        }
        private void btn_them_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_masv.Text) || string.IsNullOrWhiteSpace(txt_tensv.Text))
            {
                MessageBox.Show("Nhập mã và tên sinh viên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string sql = @"INSERT INTO SinhVien (MaSV, TenSV, NgaySinh, DiaChi, GioiTinh, MaLop)
                   VALUES (@MaSV, @TenSV, @NgaySinh, @DiaChi, @GioiTinh, @MaLop)";

            int rows = ExecuteNonQuery(sql, cmd =>
            {
                cmd.Parameters.AddWithValue("@MaSV", txt_masv.Text.Trim());
                cmd.Parameters.AddWithValue("@TenSV", txt_tensv.Text.Trim());
                cmd.Parameters.AddWithValue("@NgaySinh", dtp_ngsinh.Value.Date);
                cmd.Parameters.AddWithValue("@DiaChi", txt_diachi.Text.Trim());
                cmd.Parameters.AddWithValue("@GioiTinh", rdb_nam.Checked ? "Nam" : "Nữ");
                cmd.Parameters.AddWithValue("@MaLop", cbb_malop.SelectedValue ?? DBNull.Value);
            });

            if (rows > 0)
            {
                MessageBox.Show("Thêm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                lammoi();
                Load_Data();
            }
        }

        private void btn_sua_Click(object sender, EventArgs e)
        {
            if (dtg_sv.CurrentRow == null || dtg_sv.CurrentRow.IsNewRow)
            {
                MessageBox.Show("Chọn sinh viên cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string maSV = dtg_sv.CurrentRow.Cells["MaSV"].Value?.ToString();
            if (string.IsNullOrEmpty(maSV)) return;

            string sql = @"UPDATE SinhVien SET TenSV=@TenSV, NgaySinh=@NgaySinh, DiaChi=@DiaChi,
                   GioiTinh=@GioiTinh, MaLop=@MaLop WHERE MaSV=@MaSV";

            int rows = ExecuteNonQuery(sql, cmd =>
            {
                cmd.Parameters.AddWithValue("@MaSV", maSV);
                cmd.Parameters.AddWithValue("@TenSV", txt_tensv.Text.Trim());
                cmd.Parameters.AddWithValue("@NgaySinh", dtp_ngsinh.Value.Date);
                cmd.Parameters.AddWithValue("@DiaChi", txt_diachi.Text.Trim());
                cmd.Parameters.AddWithValue("@GioiTinh", rdb_nam.Checked ? "Nam" : "Nữ");
                cmd.Parameters.AddWithValue("@MaLop", cbb_malop.SelectedValue ?? DBNull.Value);
            });

            if (rows > 0)
            {
                MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                lammoi();
                Load_Data();
            }
        }

        private void btn_xoa_Click(object sender, EventArgs e)
        {
            if (dtg_sv.CurrentRow == null || dtg_sv.CurrentRow.IsNewRow)
            {
                MessageBox.Show("Chọn sinh viên cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string maSV = dtg_sv.CurrentRow.Cells["MaSV"].Value?.ToString();
            if (string.IsNullOrEmpty(maSV)) return;

            if (MessageBox.Show($"Xóa sinh viên '{maSV}'?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            int rows = ExecuteNonQuery("DELETE FROM SinhVien WHERE MaSV = @MaSV", cmd =>
                cmd.Parameters.AddWithValue("@MaSV", maSV));

            if (rows > 0)
            {
                MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                lammoi();
                Load_Data();
            }
        }
    }
}
