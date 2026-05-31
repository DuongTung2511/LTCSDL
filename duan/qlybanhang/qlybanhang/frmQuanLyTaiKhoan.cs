using System;
using System.Data;
using System.Windows.Forms;
using BUS;
using DTO;

namespace qlybanhang
{
    public partial class frmQuanLyTaiKhoan : Form
    {
        TaiKhoanBUS bus = new TaiKhoanBUS();

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
            DataTable dtDayDu = bus.LayDanhSachTaiKhoanDayDu();
            dgvTaiKhoan.DataSource = dtDayDu;

            if (dgvTaiKhoan.Columns.Count > 0)
            {
                dgvTaiKhoan.Columns["TenDangNhap"].HeaderText = "Tên đăng nhập";
                dgvTaiKhoan.Columns["MatKhau"].HeaderText = "Mật khẩu";
                dgvTaiKhoan.Columns["Quyen"].HeaderText = "Quyền";
                dgvTaiKhoan.Columns["TenNV"].HeaderText = "Tên nhân viên";
                if(dgvTaiKhoan.Columns.Contains("MaNV")) dgvTaiKhoan.Columns["MaNV"].Visible = false;
            }
            dgvTaiKhoan.ReadOnly = true;
        }

        private void filter_dstk()
        {
            string keyword = txtTimKiem.Text.Replace("'", "''");
            DataTable dtDayDu = bus.LayDanhSachTaiKhoanDayDu();
            DataRow[] rows = dtDayDu.Select("TenDangNhap LIKE '%" + keyword + "%' OR TenNV LIKE '%" + keyword + "%'");
            if (rows.Length > 0)
            {
                dgvTaiKhoan.DataSource = rows.CopyToDataTable();
            }
            else
            {
                dgvTaiKhoan.DataSource = dtDayDu.Clone();
            }
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            filter_dstk();
        }

        private Boolean checkInput()
        {
            Boolean kq = true;
            if (txtTenDangNhap.Text == "")
            {
                kq = false;
                txtTenDangNhap.Focus();
            }
            else if (txtMatKhau.Text == "")
            {
                kq = false;
                txtMatKhau.Focus();
            }
            return kq;
        }

        private void dgvTaiKhoan_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvTaiKhoan.Rows.Count) return;
            var dgvRow = dgvTaiKhoan.Rows[e.RowIndex];
            if (dgvRow.IsNewRow) return;

            DataRowView row = dgvRow.DataBoundItem as DataRowView;
            if (row != null)
            {
                txtTenDangNhap.Text = row["TenDangNhap"].ToString();
                txtMatKhau.Text = row["MatKhau"].ToString();
                string roleDB = row["Quyen"].ToString();
                cboQuyen.Text = roleDB == "quanly" ? "Quản lý" : "Nhân viên";
                if(row["MaNV"] != DBNull.Value)
                    txtMaNV.Text = row["MaNV"].ToString();
                else
                    txtMaNV.Clear();
            }
            else
            {
                DataRow dataRow = (dgvRow.DataBoundItem as DataRowView)?.Row;
                if(dataRow != null)
                {
                    txtTenDangNhap.Text = dataRow["TenDangNhap"].ToString();
                    txtMatKhau.Text = dataRow["MatKhau"].ToString();
                    string roleDB = dataRow["Quyen"].ToString();
                    cboQuyen.Text = roleDB == "quanly" ? "Quản lý" : "Nhân viên";
                    if(dataRow["MaNV"] != DBNull.Value)
                        txtMaNV.Text = dataRow["MaNV"].ToString();
                    else
                        txtMaNV.Clear();
                }
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (checkInput())
            {
                string dbRole = cboQuyen.Text == "Quản lý" ? "quanly" : "nhanvien";
                TaiKhoanDTO tk = new TaiKhoanDTO();
                tk.TenDangNhap = txtTenDangNhap.Text;
                tk.MatKhau = txtMatKhau.Text;
                tk.Quyen = dbRole;
                tk.MaNV = txtMaNV.Text.Trim();

                Boolean kq = bus.add_New_TK(tk);
                if (!kq)
                {
                    MessageBox.Show("Thêm mới không thành công. Có thể tên đăng nhập đã tồn tại!");
                }
                else
                {
                    LoadData();
                    lammoi();
                    MessageBox.Show("Thêm tài khoản thành công!", "Thông báo");
                }
            }
            else
            {
                MessageBox.Show("Bạn chưa nhập đủ dữ liệu!");
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dgvTaiKhoan.CurrentRow == null || dgvTaiKhoan.CurrentRow.IsNewRow)
            {
                MessageBox.Show("Chưa chọn tài khoản cần sửa!", "Thông báo");
                return;
            }

            if (checkInput())
            {
                string dbRole = cboQuyen.Text == "Quản lý" ? "quanly" : "nhanvien";
                TaiKhoanDTO tk = new TaiKhoanDTO();
                tk.TenDangNhap = txtTenDangNhap.Text.Trim();
                tk.MatKhau = txtMatKhau.Text.Trim();
                tk.Quyen = dbRole;
                tk.MaNV = txtMaNV.Text.Trim();

                if (bus.update_TK(tk))
                {
                    LoadData();
                    lammoi();
                    MessageBox.Show("Cập nhật thành công!", "Thông báo");
                }
                else
                {
                    MessageBox.Show("Cập nhật thất bại!", "Lỗi");
                }
            }
            else
            {
                MessageBox.Show("Bạn chưa nhập đủ dữ liệu!");
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvTaiKhoan.CurrentRow == null || dgvTaiKhoan.CurrentRow.IsNewRow)
            {
                MessageBox.Show("Chưa chọn tài khoản cần xoá!", "Thông báo");
                return;
            }

            string tenDN = dgvTaiKhoan.CurrentRow.Cells["TenDangNhap"].Value.ToString();
            DialogResult ret = MessageBox.Show("Bạn có chắc chắn muốn xoá tài khoản " + tenDN + "?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (ret == DialogResult.Yes)
            {
                try
                {
                    if (bus.delete_TK(tenDN))
                    {
                        LoadData();
                        lammoi();
                        MessageBox.Show("Xoá thành công!", "Thông báo");
                    }
                    else
                    {
                        MessageBox.Show("Xoá thất bại!", "Lỗi");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Lỗi xóa tài khoản", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            lammoi();
        }

        private void lammoi()
        {
            txtTenDangNhap.Enabled = true;
            txtTenDangNhap.Clear();
            txtMatKhau.Clear();
            if(txtMaNV != null) txtMaNV.Clear();
            cboQuyen.SelectedIndex = 1;
            txtTimKiem.Clear();
            dgvTaiKhoan.ClearSelection();
            txtTenDangNhap.Focus();
        }
    }
}
