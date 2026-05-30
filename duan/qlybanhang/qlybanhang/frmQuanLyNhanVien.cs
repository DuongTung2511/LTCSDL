using System;
using System.Data;
using System.Windows.Forms;
using BUS;
using DTO;

namespace qlybanhang
{
    public partial class frmQuanLyNhanVien : Form
    {
        NhanVienBUS bus = new NhanVienBUS();

        public frmQuanLyNhanVien()
        {
            InitializeComponent();
        }

        private void frmQuanLyNhanVien_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            DataViewManager dvm = bus.getDataset().DefaultViewManager;
            dgvNhanVien.DataSource = dvm;
            dgvNhanVien.DataMember = "NhanVien";

            if (dgvNhanVien.Columns.Count > 0)
            {
                dgvNhanVien.Columns["MaNV"].HeaderText = "Mã NV";
                dgvNhanVien.Columns["TenNV"].HeaderText = "Tên nhân viên";
                dgvNhanVien.Columns["TenDangNhap"].HeaderText = "Tên đăng nhập";
                dgvNhanVien.Columns["GioiTinh"].HeaderText = "Giới tính";
                dgvNhanVien.Columns["NgaySinh"].HeaderText = "Ngày sinh";
                dgvNhanVien.Columns["SoDienThoai"].HeaderText = "Số điện thoại";
                dgvNhanVien.Columns["DiaChi"].HeaderText = "Địa chỉ";
            }
            dgvNhanVien.ReadOnly = true;
        }

        private void filter_dsnv()
        {
            DataRow[] rows = bus.getFilter_NV("TenNV LIKE '%" + txtTimKiem.Text.Replace("'", "''") + "%' OR TenDangNhap LIKE '%" + txtTimKiem.Text.Replace("'", "''") + "%'");
            if (rows.Length > 0)
            {
                dgvNhanVien.DataSource = rows.CopyToDataTable();
            }
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            filter_dsnv();
        }

        private Boolean checkInput()
        {
            Boolean kq = true;
            if (txtMaNV.Text == "")
            {
                kq = false;
                txtMaNV.Focus();
            }
            else if (txtTenNV.Text == "")
            {
                kq = false;
                txtTenNV.Focus();
            }
            else if (txtTenDangNhap.Text == "")
            {
                kq = false;
                txtTenDangNhap.Focus();
            }
            else if (cboGioiTinh.SelectedIndex < 0)
            {
                kq = false;
                cboGioiTinh.Focus();
            }
            else if (txtSoDienThoai.Text == "")
            {
                kq = false;
                txtSoDienThoai.Focus();
            }
            else if (txtDiaChi.Text == "")
            {
                kq = false;
                txtDiaChi.Focus();
            }
            return kq;
        }

        private void dgvNhanVien_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvNhanVien.Rows.Count) return;
            var dgvRow = dgvNhanVien.Rows[e.RowIndex];
            if (dgvRow.IsNewRow) return;

            DataRowView row = dgvRow.DataBoundItem as DataRowView;
            if (row == null) return;

            txtMaNV.Text = row["MaNV"].ToString();
            txtTenNV.Text = row["TenNV"].ToString();
            txtTenDangNhap.Text = row["TenDangNhap"].ToString();
            
            string gioiTinh = row["GioiTinh"].ToString();
            if (gioiTinh != "")
                cboGioiTinh.SelectedItem = gioiTinh;
            else
                cboGioiTinh.SelectedIndex = -1;

            if (row["NgaySinh"] != DBNull.Value)
                dtpNgaySinh.Value = Convert.ToDateTime(row["NgaySinh"]);
            else
                dtpNgaySinh.Value = DateTime.Now;

            txtSoDienThoai.Text = row["SoDienThoai"].ToString();
            txtDiaChi.Text = row["DiaChi"].ToString();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (checkInput())
            {
                NhanVienDTO nv = new NhanVienDTO();
                nv.MaNV = txtMaNV.Text;
                nv.TenNV = txtTenNV.Text;
                nv.TenDangNhap = txtTenDangNhap.Text;
                nv.GioiTinh = cboGioiTinh.SelectedItem.ToString();
                nv.NgaySinh = dtpNgaySinh.Value.Date;
                nv.SoDienThoai = txtSoDienThoai.Text;
                nv.DiaChi = txtDiaChi.Text;

                Boolean kq = bus.add_New_NV(nv);
                if (!kq)
                {
                    MessageBox.Show("Thêm mới không thành công. Có thể mã nhân viên đã tồn tại!");
                }
                else
                {
                    LoadData();
                    lammoi();
                    MessageBox.Show("Thêm nhân viên thành công!", "Thông báo");
                }
            }
            else
            {
                MessageBox.Show("Bạn chưa nhập đủ dữ liệu!");
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dgvNhanVien.CurrentRow == null || dgvNhanVien.CurrentRow.IsNewRow)
            {
                MessageBox.Show("Chưa chọn nhân viên cần sửa!", "Thông báo");
                return;
            }

            if (checkInput())
            {
                NhanVienDTO nv = new NhanVienDTO();
                nv.MaNV = txtMaNV.Text.Trim();
                nv.TenNV = txtTenNV.Text.Trim();
                nv.TenDangNhap = txtTenDangNhap.Text.Trim();
                nv.GioiTinh = cboGioiTinh.SelectedItem.ToString();
                nv.NgaySinh = dtpNgaySinh.Value.Date;
                nv.SoDienThoai = txtSoDienThoai.Text.Trim();
                nv.DiaChi = txtDiaChi.Text.Trim();

                if (bus.update_NV(nv))
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
            if (dgvNhanVien.CurrentRow == null || dgvNhanVien.CurrentRow.IsNewRow)
            {
                MessageBox.Show("Chưa chọn nhân viên cần xoá!", "Thông báo");
                return;
            }

            string maNV = dgvNhanVien.CurrentRow.Cells["MaNV"].Value.ToString();
            DialogResult ret = MessageBox.Show("Bạn có chắc chắn muốn xoá nhân viên " + maNV + "?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (ret == DialogResult.Yes)
            {
                if (bus.delete_NV(maNV))
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
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            lammoi();
        }

        private void lammoi()
        {
            txtMaNV.Enabled = true;
            txtMaNV.Clear();
            txtTenNV.Clear();
            txtTenDangNhap.Clear();
            cboGioiTinh.SelectedIndex = -1;
            dtpNgaySinh.Value = DateTime.Now;
            txtSoDienThoai.Clear();
            txtDiaChi.Clear();
            txtTimKiem.Clear();
            dgvNhanVien.ClearSelection();
            txtMaNV.Focus();
        }
    }
}
