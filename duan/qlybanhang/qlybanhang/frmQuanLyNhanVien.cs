using System;
using System.Data;
using System.Windows.Forms;
using BUS;
using DTO;

namespace qlybanhang
{
    public partial class frmQuanLyNhanVien : Form
    {
        MyBUS bus = new MyBUS();

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
            dgvNhanVien.DataSource = bus.getTableNhanVien();
            if (dgvNhanVien.Columns.Count > 0)
            {
                dgvNhanVien.Columns["MaNV"].HeaderText = "Mã NV";
                dgvNhanVien.Columns["TenNV"].HeaderText = "Tên nhân viên";
                dgvNhanVien.Columns["TenDangNhap"].HeaderText = "Tên đăng nhập";
                dgvNhanVien.Columns["SoDienThoai"].HeaderText = "Số điện thoại";
                dgvNhanVien.Columns["DiaChi"].HeaderText = "Địa chỉ";
            }
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtTimKiem.Text.Trim().Replace("'", "''");
            DataRow[] rows = bus.getFilter_NV(string.Format("TenNV LIKE '%{0}%' OR TenDangNhap LIKE '%{0}%'", keyword));
            if (rows.Length > 0)
            {
                dgvNhanVien.DataSource = rows.CopyToDataTable();
            }
            else
            {
                dgvNhanVien.DataSource = bus.getTableNhanVien().Clone();
            }
        }

        private bool checkInput()
        {
            if (string.IsNullOrEmpty(txtTenNV.Text.Trim()))
            {
                MessageBox.Show("Vui lòng nhập tên nhân viên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenNV.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtTenDangNhap.Text.Trim()))
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenDangNhap.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtSoDienThoai.Text.Trim()))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoDienThoai.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtDiaChi.Text.Trim()))
            {
                MessageBox.Show("Vui lòng nhập địa chỉ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDiaChi.Focus();
                return false;
            }
            return true;
        }

        private void dgvNhanVien_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvNhanVien.Rows.Count) return;
            var dgvRow = dgvNhanVien.Rows[e.RowIndex];
            if (dgvRow.IsNewRow) return;

            DataRowView row = dgvRow.DataBoundItem as DataRowView;
            if (row == null) return;

            txtTenNV.Text = row["TenNV"].ToString();
            txtTenDangNhap.Text = row["TenDangNhap"].ToString();
            txtSoDienThoai.Text = row["SoDienThoai"].ToString();
            txtDiaChi.Text = row["DiaChi"].ToString();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                if (!checkInput()) return;
                NhanVienDTO nv = new NhanVienDTO();
                nv.TenNV = txtTenNV.Text.Trim();
                nv.TenDangNhap = txtTenDangNhap.Text.Trim();
                nv.SoDienThoai = txtSoDienThoai.Text.Trim();
                nv.DiaChi = txtDiaChi.Text.Trim();
                if (bus.add_New_NV(nv))
                {
                    LoadData();
                    LamMoi();
                    MessageBox.Show("Thêm nhân viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Thêm nhân viên thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvNhanVien.CurrentRow == null || dgvNhanVien.CurrentRow.IsNewRow)
                {
                    MessageBox.Show("Chưa chọn nhân viên cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (!checkInput()) return;
                int maNV = Convert.ToInt32(dgvNhanVien.CurrentRow.Cells["MaNV"].Value);
                NhanVienDTO nv = new NhanVienDTO();
                nv.MaNV = maNV;
                nv.TenNV = txtTenNV.Text.Trim();
                nv.TenDangNhap = txtTenDangNhap.Text.Trim();
                nv.SoDienThoai = txtSoDienThoai.Text.Trim();
                nv.DiaChi = txtDiaChi.Text.Trim();
                
                if (bus.update_NV(nv))
                {
                    LoadData();
                    LamMoi();
                    MessageBox.Show("Sửa nhân viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Không tìm thấy nhân viên để sửa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvNhanVien.CurrentRow == null || dgvNhanVien.CurrentRow.IsNewRow)
                {
                    MessageBox.Show("Chưa chọn nhân viên cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                DialogResult dr = MessageBox.Show("Bạn có chắc muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    int maNV = Convert.ToInt32(dgvNhanVien.CurrentRow.Cells["MaNV"].Value);
                    if (bus.delete_NV(maNV))
                    {
                        LoadData();
                        LamMoi();
                        MessageBox.Show("Xóa nhân viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy nhân viên để xóa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            LamMoi();
        }

        private void LamMoi()
        {
            txtTenNV.Clear();
            txtTenDangNhap.Clear();
            txtSoDienThoai.Clear();
            txtDiaChi.Clear();
            txtTimKiem.Clear();
            dgvNhanVien.ClearSelection();
            txtTenNV.Focus();
        }
    }
}
