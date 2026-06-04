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
            dgvNhanVien.CellFormatting += dgvNhanVien_CellFormatting;

            LoadData();
        }

        private void dgvNhanVien_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvNhanVien.Columns[e.ColumnIndex].Name == "TrangThai" && e.Value != null)
            {
                if (e.Value.ToString() == "1" )
                    e.Value = "Đang làm";
                else
                    e.Value = "Đã nghỉ";
            }
        }

        private void LoadData()
        {
            DataView dv = bus.getTableNhanVien().DefaultView;
            if (!chkHienThiDaXoa.Checked)
            {
                dv.RowFilter = "TrangThai = 1 OR TrangThai IS NULL";
            }
            else
            {
                dv.RowFilter = "";
            }
            dgvNhanVien.DataSource = dv;

            if (dgvNhanVien.Columns.Count > 0)
            {
                if(dgvNhanVien.Columns.Contains("MaNV")) dgvNhanVien.Columns["MaNV"].HeaderText = "Mã NV";
                if(dgvNhanVien.Columns.Contains("TenNV")) dgvNhanVien.Columns["TenNV"].HeaderText = "Tên nhân viên";
                if(dgvNhanVien.Columns.Contains("GioiTinh")) dgvNhanVien.Columns["GioiTinh"].HeaderText = "Giới tính";
                if(dgvNhanVien.Columns.Contains("NgaySinh")) dgvNhanVien.Columns["NgaySinh"].HeaderText = "Ngày sinh";
                if(dgvNhanVien.Columns.Contains("SoDienThoai")) dgvNhanVien.Columns["SoDienThoai"].HeaderText = "Số điện thoại";
                if(dgvNhanVien.Columns.Contains("DiaChi")) dgvNhanVien.Columns["DiaChi"].HeaderText = "Địa chỉ";
                if(dgvNhanVien.Columns.Contains("TrangThai")) dgvNhanVien.Columns["TrangThai"].HeaderText = "Trạng thái";
            }
            dgvNhanVien.ReadOnly = true;
            dtpNgaySinh.Value = DateTime.Now;
        }

        private void filter_dsnv()
        {
            string keyword = txtTimKiem.Text.Replace("'", "''");
            string strFilter = "";
            
            if (!string.IsNullOrEmpty(keyword))
            {
                strFilter = "(TenNV LIKE '%" + keyword + "%' OR MaNV LIKE '%" + keyword + "%')";
            }

            if (!chkHienThiDaXoa.Checked)
            {
                if (strFilter != "") strFilter += " AND ";
                strFilter += "(TrangThai = 1 OR TrangThai IS NULL)";
            }

            DataRow[] rows = bus.getFilter_NhanVien(strFilter);
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
            if (string.IsNullOrEmpty(txtMaNV.Text))
            {
                kq = false;
                txtMaNV.Focus();
            }
            else if (string.IsNullOrEmpty(txtTenNV.Text))
            {
                kq = false;
                txtTenNV.Focus();
            }
            else if (cboGioiTinh.SelectedIndex < 0)
            {
                kq = false;
                cboGioiTinh.Focus();
            }
            else if (string.IsNullOrEmpty(txtSoDienThoai.Text))
            {
                kq = false;
                txtSoDienThoai.Focus();
            }
            else if (string.IsNullOrEmpty(txtDiaChi.Text))
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
            cboGioiTinh.Text = row["GioiTinh"].ToString();
            if (row["NgaySinh"] != DBNull.Value) dtpNgaySinh.Value = Convert.ToDateTime(row["NgaySinh"]);
            txtSoDienThoai.Text = row["SoDienThoai"].ToString();
            txtDiaChi.Text = row["DiaChi"].ToString();
           
            if (row["TrangThai"] != DBNull.Value)
                cboTrangThai.SelectedIndex = (row["TrangThai"].ToString() == "1") ? 1 : 0;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (!checkInput())
            {
                MessageBox.Show("Bạn chưa nhập đủ dữ liệu!");
                return;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(txtSoDienThoai.Text, @"^0\d{9}$"))
            {
                MessageBox.Show("Số điện thoại không hợp lệ! Vui lòng nhập 10 số bắt đầu bằng 0.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoDienThoai.Focus();
                return;
            }

            NhanVienDTO nv = new NhanVienDTO();
            nv.MaNV = txtMaNV.Text;
            nv.TenNV = txtTenNV.Text;
            nv.GioiTinh = cboGioiTinh.SelectedItem.ToString();
            nv.NgaySinh = dtpNgaySinh.Value.Date;
            nv.SoDienThoai = txtSoDienThoai.Text;
            nv.DiaChi = txtDiaChi.Text;
            // TrangThai  BUS đã gán mặc định = 1

            bool kq = bus.add_New_NV(nv);
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

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dgvNhanVien.CurrentRow == null || dgvNhanVien.CurrentRow.IsNewRow)
            {
                MessageBox.Show("Chưa chọn nhân viên cần sửa!", "Thông báo");
                return;
            }

            if (!checkInput())
            {
                MessageBox.Show("Bạn chưa nhập đủ dữ liệu!");
                return;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(txtSoDienThoai.Text, @"^0\d{9}$"))
            {
                MessageBox.Show("Số điện thoại không hợp lệ! Vui lòng nhập 10 số bắt đầu bằng 0.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoDienThoai.Focus();
                return;
            }

            NhanVienDTO nv = new NhanVienDTO();
            nv.MaNV = txtMaNV.Text.Trim();
            nv.TenNV = txtTenNV.Text.Trim();
            nv.GioiTinh = cboGioiTinh.SelectedItem.ToString();
            nv.NgaySinh = dtpNgaySinh.Value.Date;
            nv.SoDienThoai = txtSoDienThoai.Text.Trim();
            nv.DiaChi = txtDiaChi.Text.Trim();
            nv.TrangThai = cboTrangThai.SelectedIndex; // 1 hoặc 0

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

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvNhanVien.CurrentRow == null || dgvNhanVien.CurrentRow.IsNewRow)
            {
                MessageBox.Show("Chưa chọn nhân viên cần thao tác!", "Thông báo");
                return;
            }

            string maNV = dgvNhanVien.CurrentRow.Cells["MaNV"].Value.ToString();
            DialogResult ret = MessageBox.Show("Bạn có chắc chắn muốn cho nhân viên " + maNV + " nghỉ việc?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (ret == DialogResult.Yes)
            {
                if (bus.delete_NV(maNV))
                {
                    LoadData();
                    lammoi();
                    MessageBox.Show("Nhân viên đã được chuyển sang trạng thái Đã nghỉ!", "Thông báo");
                }
                else
                {
                    MessageBox.Show("Thao tác thất bại!", "Lỗi");
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
            if(cboGioiTinh.Items.Count > 0) cboGioiTinh.SelectedIndex = -1;
            dtpNgaySinh.Value = DateTime.Now;
            txtSoDienThoai.Clear();
            txtDiaChi.Clear();
            txtTimKiem.Clear();
            if(cboTrangThai != null) cboTrangThai.SelectedIndex = 1;
            dgvNhanVien.ClearSelection();
            txtMaNV.Focus();
        }

        private void chkHienThiDaXoa_CheckedChanged(object sender, EventArgs e)
        {
            filter_dsnv();
        }

        private void btnXoaVinhVien_Click(object sender, EventArgs e)
        {
            if (dgvNhanVien.CurrentRow == null || dgvNhanVien.CurrentRow.IsNewRow)
            {
                MessageBox.Show("Chưa chọn nhân viên cần thao tác!", "Thông báo");
                return;
            }

            string maNV = dgvNhanVien.CurrentRow.Cells["MaNV"].Value.ToString();
            DialogResult ret = MessageBox.Show("Bạn có chắc chắn muốn xóa VĨNH VIỄN nhân viên " + maNV + "? Hành động này không thể hoàn tác và sẽ xóa luôn tài khoản tương ứng!", "Cảnh báo",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (ret == DialogResult.Yes)
            {
                string msg = bus.XoaVinhVien(maNV);
                if (msg == "")
                {
                    bus = new NhanVienBUS(); // Reload từ DB
                    LoadData();
                    lammoi();
                    MessageBox.Show("Đã xóa vĩnh viễn nhân viên và tài khoản!", "Thông báo");
                }
                else
                {
                    MessageBox.Show(msg, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
