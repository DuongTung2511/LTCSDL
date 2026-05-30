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
            DataViewManager dvm = bus.getDataset().DefaultViewManager;
            dgvKhachHang.DataSource = dvm;
            dgvKhachHang.DataMember = "KhachHang";

            if (dgvKhachHang.Columns.Count > 0)
            {
                dgvKhachHang.Columns["MaKH"].HeaderText = "Mã KH";
                dgvKhachHang.Columns["TenKH"].HeaderText = "Tên khách hàng";
                dgvKhachHang.Columns["SoDienThoai"].HeaderText = "Số điện thoại";
                dgvKhachHang.Columns["DiaChi"].HeaderText = "Địa chỉ";
                if(dgvKhachHang.Columns.Contains("TrangThai")) dgvKhachHang.Columns["TrangThai"].HeaderText = "Trạng thái";
            }
            dgvKhachHang.ReadOnly = true;
        }

        private void filter_dskh()
        {
            DataRow[] rows = bus.getFilter_KH("TenKH LIKE '%" + txtTimKiem.Text.Replace("'", "''") + "%' OR SoDienThoai LIKE '%" + txtTimKiem.Text.Replace("'", "''") + "%'");
            if (rows.Length > 0)
            {
                dgvKhachHang.DataSource = rows.CopyToDataTable();
            }
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            filter_dskh();
        }

        private Boolean checkInput()
        {
            Boolean kq = true;
            if (txtMaKH.Text == "")
            {
                kq = false;
                txtMaKH.Focus();
            }
            else if (txtTenKH.Text == "")
            {
                kq = false;
                txtTenKH.Focus();
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
                cboTrangThai.SelectedIndex = (row["TrangThai"].ToString() == "1" || row["TrangThai"].ToString() == "True") ? 1 : 0;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (checkInput())
            {
                KhachHangDTO kh = new KhachHangDTO();
                kh.MaKH = txtMaKH.Text;
                kh.TenKH = txtTenKH.Text;
                kh.SoDienThoai = txtSoDienThoai.Text;
                kh.DiaChi = txtDiaChi.Text;

                Boolean kq = bus.add_New_KH(kh);
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
            else
            {
                MessageBox.Show("Bạn chưa nhập đủ dữ liệu!");
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dgvKhachHang.CurrentRow == null || dgvKhachHang.CurrentRow.IsNewRow)
            {
                MessageBox.Show("Chưa chọn khách hàng cần sửa!", "Thông báo");
                return;
            }
            
            if (checkInput())
            {
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
            else
            {
                MessageBox.Show("Bạn chưa nhập đủ dữ liệu!");
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
    }
}
