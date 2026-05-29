using System;
using System.Data;
using System.Windows.Forms;
using BUS;
using DTO;

namespace qlybanhang
{
    public partial class frmQuanLyTaiKhoan : Form
    {
        MyBUS bus = new MyBUS();

        public frmQuanLyTaiKhoan()
        {
            InitializeComponent();
        }

        private void frmQuanLyTaiKhoan_Load(object sender, EventArgs e)
        {
            cboQuyen.Items.Add("Quản lý");
            cboQuyen.Items.Add("Nhân viên");
            cboQuyen.SelectedIndex = 1;

            dgvTaiKhoan.CellFormatting += dgvTaiKhoan_CellFormatting;

            LoadData();
        }

        private void dgvTaiKhoan_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvTaiKhoan.Columns[e.ColumnIndex].Name == "Quyen" && e.Value != null)
            {
                string val = e.Value.ToString();
                if (val == "quanly") e.Value = "Quản lý";
                else if (val == "nhanvien") e.Value = "Nhân viên";
            }
        }

        private void LoadData()
        {
            dgvTaiKhoan.DataSource = bus.getTableTaiKhoan();
            if (dgvTaiKhoan.Columns.Count > 0)
            {
                dgvTaiKhoan.Columns["TenDangNhap"].HeaderText = "Tên đăng nhập";
                dgvTaiKhoan.Columns["MatKhau"].HeaderText = "Mật khẩu";
                dgvTaiKhoan.Columns["Quyen"].HeaderText = "Quyền";
            }
        }

        private bool checkInput()
        {
            if (string.IsNullOrEmpty(txtTenDangNhap.Text.Trim()))
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenDangNhap.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtMatKhau.Text.Trim()))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMatKhau.Focus();
                return false;
            }
            return true;
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtTimKiem.Text.Trim().Replace("'", "''");
            DataRow[] rows = bus.getFilter_TK(string.Format("TenDangNhap LIKE '%{0}%'", keyword));
            if (rows.Length > 0)
            {
                dgvTaiKhoan.DataSource = rows.CopyToDataTable();
            }
            else
            {
                dgvTaiKhoan.DataSource = bus.getTableTaiKhoan().Clone();
            }
        }

        private void dgvTaiKhoan_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvTaiKhoan.Rows.Count) return;
            var dgvRow = dgvTaiKhoan.Rows[e.RowIndex];
            if (dgvRow.IsNewRow) return;

            DataRowView row = dgvRow.DataBoundItem as DataRowView;
            if (row == null) return;

            txtTenDangNhap.Text = row["TenDangNhap"].ToString();
            txtMatKhau.Text = row["MatKhau"].ToString();
            string roleDB = row["Quyen"].ToString();
            cboQuyen.Text = roleDB == "quanly" ? "Quản lý" : "Nhân viên";
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (!checkInput()) return;

            string dbRole = cboQuyen.Text == "Quản lý" ? "quanly" : "nhanvien";
            TaiKhoanDTO tk = new TaiKhoanDTO
            {
                TenDangNhap = txtTenDangNhap.Text.Trim(),
                MatKhau = txtMatKhau.Text.Trim(),
                Quyen = dbRole
            };

            if (bus.add_New_TK(tk))
            {
                MessageBox.Show("Thêm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
                btnLamMoi_Click(null, null);
            }
            else
            {
                MessageBox.Show("Tên đăng nhập đã tồn tại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dgvTaiKhoan.CurrentRow == null || dgvTaiKhoan.CurrentRow.IsNewRow)
            {
                MessageBox.Show("Vui lòng chọn tài khoản để sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!checkInput()) return;

            string dbRole = cboQuyen.Text == "Quản lý" ? "quanly" : "nhanvien";
            TaiKhoanDTO tk = new TaiKhoanDTO
            {
                TenDangNhap = txtTenDangNhap.Text.Trim(),
                MatKhau = txtMatKhau.Text.Trim(),
                Quyen = dbRole
            };

            if (bus.update_TK(tk))
            {
                MessageBox.Show("Sửa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
                btnLamMoi_Click(null, null);
            }
            else
            {
                MessageBox.Show("Không tìm thấy tài khoản để sửa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvTaiKhoan.CurrentRow == null || dgvTaiKhoan.CurrentRow.IsNewRow)
            {
                MessageBox.Show("Vui lòng chọn tài khoản để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string tenDN = dgvTaiKhoan.CurrentRow.Cells["TenDangNhap"].Value.ToString();
            if (MessageBox.Show("Bạn có chắc muốn xóa tài khoản " + tenDN + "?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (bus.delete_TK(tenDN))
                {
                    MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    btnLamMoi_Click(null, null);
                }
                else
                {
                    MessageBox.Show("Không tìm thấy tài khoản để xóa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtTenDangNhap.Clear();
            txtMatKhau.Clear();
            cboQuyen.SelectedIndex = 1;
            txtTenDangNhap.Focus();
        }
    }
}
