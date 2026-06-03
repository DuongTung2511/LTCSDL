using System;
using System.Data;
using System.Windows.Forms;
using BUS;
using DTO;

namespace qlybanhang
{
    public partial class frmQuanLyKhachHang : Form
    {
        KhachHangBUS bus = new KhachHangBUS();

        public frmQuanLyKhachHang()
        {
            InitializeComponent();
        }

        private void frmQuanLyKhachHang_Load(object sender, EventArgs e)
        {
            dgvKhachHang.CellFormatting += dgvKhachHang_CellFormatting;
            LoadData();
        }

        private void dgvKhachHang_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvKhachHang.Columns[e.ColumnIndex].Name == "TrangThai" && e.Value != null)
            {
                if (e.Value.ToString() == "1" || e.Value.ToString() == "True")
                    e.Value = "Đang giao dịch";
                else
                    e.Value = "Ngừng giao dịch";
            }
        }

        private void LoadData()
        {
            DataView dv = bus.getTableKhachHang().DefaultView;
            if (!chkHienThiDaXoa.Checked)
            {
                dv.RowFilter = "TrangThai = 1 OR TrangThai IS NULL";
            }
            else
            {
                dv.RowFilter = "";
            }
            dgvKhachHang.DataSource = dv;

            if (dgvKhachHang.Columns.Count > 0)
            {
                if(dgvKhachHang.Columns.Contains("MaKH")) dgvKhachHang.Columns["MaKH"].HeaderText = "Mã KH";
                if(dgvKhachHang.Columns.Contains("TenKH")) dgvKhachHang.Columns["TenKH"].HeaderText = "Tên khách hàng";
                if(dgvKhachHang.Columns.Contains("SoDienThoai")) dgvKhachHang.Columns["SoDienThoai"].HeaderText = "Số điện thoại";
                if(dgvKhachHang.Columns.Contains("DiaChi")) dgvKhachHang.Columns["DiaChi"].HeaderText = "Địa chỉ";
                if(dgvKhachHang.Columns.Contains("TrangThai")) dgvKhachHang.Columns["TrangThai"].HeaderText = "Trạng thái";
            }
            dgvKhachHang.ReadOnly = true;
        }

        private void filter_dskh()
        {
            string keyword = txtTimKiem.Text.Replace("'", "''");
            string strFilter = "";
            
            if (!string.IsNullOrEmpty(keyword))
            {
                strFilter = "(TenKH LIKE '%" + keyword + "%' OR SoDienThoai LIKE '%" + keyword + "%')";
            }

            if (!chkHienThiDaXoa.Checked)
            {
                if (strFilter != "") strFilter += " AND ";
                strFilter += "(TrangThai = 1 OR TrangThai IS NULL)";
            }

            DataView dv = bus.getTableKhachHang().DefaultView;
            dv.RowFilter = strFilter;
            dgvKhachHang.DataSource = dv;
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            filter_dskh();
        }

        private Boolean checkInput()
        {
            Boolean kq = true;
            if (string.IsNullOrEmpty(txtMaKH.Text))
            {
                kq = false;
                txtMaKH.Focus();
            }
            else if (string.IsNullOrEmpty(txtTenKH.Text))
            {
                kq = false;
                txtTenKH.Focus();
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

        private void dgvKhachHang_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvKhachHang.Rows.Count) return;
            var dgvRow = dgvKhachHang.Rows[e.RowIndex];
            if (dgvRow.IsNewRow) return;

            DataRowView row = dgvRow.DataBoundItem as DataRowView;
            if (row == null) return;

            txtMaKH.Text = row["MaKH"].ToString();
            txtTenKH.Text = row["TenKH"].ToString();
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

            KhachHangDTO kh = new KhachHangDTO();
            kh.MaKH = txtMaKH.Text;
            kh.TenKH = txtTenKH.Text;
            kh.SoDienThoai = txtSoDienThoai.Text;
            kh.DiaChi = txtDiaChi.Text;

            bool kq = bus.add_New_KH(kh);
            if (!kq)
            {
                MessageBox.Show("Thêm mới không thành công. Có thể mã khách hàng đã tồn tại!");
            }
            else
            {
                LoadData();
                lammoi();
                MessageBox.Show("Thêm khách hàng thành công!", "Thông báo");
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dgvKhachHang.CurrentRow == null || dgvKhachHang.CurrentRow.IsNewRow)
            {
                MessageBox.Show("Chưa chọn khách hàng cần sửa!", "Thông báo");
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

            KhachHangDTO kh = new KhachHangDTO();
            kh.MaKH = txtMaKH.Text.Trim();
            kh.TenKH = txtTenKH.Text.Trim();
            kh.SoDienThoai = txtSoDienThoai.Text.Trim();
            kh.DiaChi = txtDiaChi.Text.Trim();
            kh.TrangThai = cboTrangThai.SelectedIndex;

            if (bus.update_KH(kh))
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
            if (dgvKhachHang.CurrentRow == null || dgvKhachHang.CurrentRow.IsNewRow)
            {
                MessageBox.Show("Chưa chọn khách hàng cần thao tác!", "Thông báo");
                return;
            }

            string maKH = dgvKhachHang.CurrentRow.Cells["MaKH"].Value.ToString();
            DialogResult ret = MessageBox.Show("Bạn có chắc chắn muốn ngừng giao dịch với khách hàng " + maKH + "?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (ret == DialogResult.Yes)
            {
                if (bus.delete_KH(maKH))
                {
                    LoadData();
                    lammoi();
                    MessageBox.Show("Đã chuyển trạng thái sang Ngừng giao dịch!", "Thông báo");
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
            txtMaKH.Enabled = true;
            txtMaKH.Clear();
            txtTenKH.Clear();
            txtSoDienThoai.Clear();
            txtDiaChi.Clear();
            txtTimKiem.Clear();
            if (cboTrangThai != null) cboTrangThai.SelectedIndex = 1;
            dgvKhachHang.ClearSelection();
            txtMaKH.Focus();
        }

        private void chkHienThiDaXoa_CheckedChanged(object sender, EventArgs e)
        {
            filter_dskh();
        }

        private void btnXoaVinhVien_Click(object sender, EventArgs e)
        {
            if (dgvKhachHang.CurrentRow == null || dgvKhachHang.CurrentRow.IsNewRow)
            {
                MessageBox.Show("Chưa chọn khách hàng cần thao tác!", "Thông báo");
                return;
            }

            string maKH = dgvKhachHang.CurrentRow.Cells["MaKH"].Value.ToString();
            DialogResult ret = MessageBox.Show("Bạn có chắc chắn muốn xóa VĨNH VIỄN khách hàng " + maKH + "? Hành động này không thể hoàn tác!", "Cảnh báo",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (ret == DialogResult.Yes)
            {
                string msg = bus.XoaVinhVien(maKH);
                if (msg == "")
                {
                    bus = new KhachHangBUS(); // Reload từ DB
                    LoadData();
                    lammoi();
                    MessageBox.Show("Đã xóa vĩnh viễn khách hàng!", "Thông báo");
                }
                else
                {
                    MessageBox.Show(msg, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
