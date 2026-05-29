using System;
using System.Data;
using System.Windows.Forms;
using BUS;
using DTO;

namespace qlybanhang
{
    public partial class frmQuanLyKhachHang : Form
    {
        MyBUS bus = new MyBUS();

        public frmQuanLyKhachHang()
        {
            InitializeComponent();
        }

        private void frmQuanLyKhachHang_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            dgvKhachHang.DataSource = bus.getTableKhachHang();
            if (dgvKhachHang.Columns.Count > 0)
            {
                dgvKhachHang.Columns["MaKH"].HeaderText = "Mã KH";
                dgvKhachHang.Columns["TenKH"].HeaderText = "Tên khách hàng";
                dgvKhachHang.Columns["SoDienThoai"].HeaderText = "Số điện thoại";
                dgvKhachHang.Columns["DiaChi"].HeaderText = "Địa chỉ";
            }
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtTimKiem.Text.Trim().Replace("'", "''");
            DataRow[] rows = bus.getFilter_KH(string.Format("TenKH LIKE '%{0}%' OR SoDienThoai LIKE '%{0}%'", keyword));
            if (rows.Length > 0)
            {
                dgvKhachHang.DataSource = rows.CopyToDataTable();
            }
            else
            {
                dgvKhachHang.DataSource = bus.getTableKhachHang().Clone();
            }
        }

        private bool checkInput()
        {
            if (string.IsNullOrEmpty(txtTenKH.Text.Trim()))
            {
                MessageBox.Show("Vui lòng nhập tên khách hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenKH.Focus();
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

        private void dgvKhachHang_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvKhachHang.Rows.Count) return;
            var dgvRow = dgvKhachHang.Rows[e.RowIndex];
            if (dgvRow.IsNewRow) return;

            DataRowView row = dgvRow.DataBoundItem as DataRowView;
            if (row == null) return;

            txtTenKH.Text = row["TenKH"].ToString();
            txtSoDienThoai.Text = row["SoDienThoai"].ToString();
            txtDiaChi.Text = row["DiaChi"].ToString();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                if (!checkInput()) return;
                KhachHangDTO kh = new KhachHangDTO();
                kh.TenKH = txtTenKH.Text.Trim();
                kh.SoDienThoai = txtSoDienThoai.Text.Trim();
                kh.DiaChi = txtDiaChi.Text.Trim();
                if (bus.add_New_KH(kh))
                {
                    LoadData();
                    LamMoi();
                    MessageBox.Show("Thêm khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Thêm khách hàng thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                if (dgvKhachHang.CurrentRow == null || dgvKhachHang.CurrentRow.IsNewRow)
                {
                    MessageBox.Show("Chưa chọn khách hàng cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (!checkInput()) return;
                int maKH = Convert.ToInt32(dgvKhachHang.CurrentRow.Cells["MaKH"].Value);
                KhachHangDTO kh = new KhachHangDTO();
                kh.MaKH = maKH;
                kh.TenKH = txtTenKH.Text.Trim();
                kh.SoDienThoai = txtSoDienThoai.Text.Trim();
                kh.DiaChi = txtDiaChi.Text.Trim();
                
                if (bus.update_KH(kh))
                {
                    LoadData();
                    LamMoi();
                    MessageBox.Show("Sửa khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Không tìm thấy khách hàng để sửa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                if (dgvKhachHang.CurrentRow == null || dgvKhachHang.CurrentRow.IsNewRow)
                {
                    MessageBox.Show("Chưa chọn khách hàng cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                DialogResult dr = MessageBox.Show("Bạn có chắc muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    int maKH = Convert.ToInt32(dgvKhachHang.CurrentRow.Cells["MaKH"].Value);
                    if (bus.delete_KH(maKH))
                    {
                        LoadData();
                        LamMoi();
                        MessageBox.Show("Xóa khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy khách hàng để xóa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            txtTenKH.Clear();
            txtSoDienThoai.Clear();
            txtDiaChi.Clear();
            txtTimKiem.Clear();
            dgvKhachHang.ClearSelection();
            txtTenKH.Focus();
        }
    }
}
