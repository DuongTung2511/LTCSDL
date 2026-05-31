using System;
using System.Data;
using System.Windows.Forms;
using BUS;
using DTO;

namespace qlybanhang
{
    public partial class frmQuanLyNhaCungCap : Form
    {
        NhaCungCapBUS bus = new NhaCungCapBUS();

        public frmQuanLyNhaCungCap()
        {
            InitializeComponent();
        }

        private void frmQuanLyNhaCungCap_Load(object sender, EventArgs e)
        {
            dgvNhaCungCap.CellFormatting += dgvNhaCungCap_CellFormatting;
            LoadData();
        }

        private void dgvNhaCungCap_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvNhaCungCap.Columns[e.ColumnIndex].Name == "TrangThai" && e.Value != null)
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
            if (!chkHienThiDaXoa.Checked)
            {
                dvm.DataViewSettings["NhaCungCap"].RowFilter = "TrangThai = 1 OR TrangThai IS NULL";
            }
            else
            {
                dvm.DataViewSettings["NhaCungCap"].RowFilter = "";
            }
            dgvNhaCungCap.DataSource = dvm;
            dgvNhaCungCap.DataMember = "NhaCungCap";

            if (dgvNhaCungCap.Columns.Count > 0)
            {
                if(dgvNhaCungCap.Columns.Contains("MaNCC")) dgvNhaCungCap.Columns["MaNCC"].HeaderText = "Mã nhà cung cấp";
                if(dgvNhaCungCap.Columns.Contains("TenNCC")) dgvNhaCungCap.Columns["TenNCC"].HeaderText = "Tên nhà cung cấp";
                if(dgvNhaCungCap.Columns.Contains("SoDienThoai")) dgvNhaCungCap.Columns["SoDienThoai"].HeaderText = "Số điện thoại";
                if(dgvNhaCungCap.Columns.Contains("DiaChi")) dgvNhaCungCap.Columns["DiaChi"].HeaderText = "Địa chỉ";
                if(dgvNhaCungCap.Columns.Contains("TrangThai")) dgvNhaCungCap.Columns["TrangThai"].HeaderText = "Trạng thái";
            }
            dgvNhaCungCap.ReadOnly = true;
        }

        private void filter_dsncc()
        {
            string keyword = txtTimKiem.Text.Replace("'", "''");
            if (string.IsNullOrEmpty(keyword))
            {
                LoadData();
                return;
            }

            string strFilter = "(TenNCC LIKE '%" + keyword + "%' OR MaNCC LIKE '%" + keyword + "%')";
            if (!chkHienThiDaXoa.Checked)
            {
                strFilter += " AND (TrangThai = 1 OR TrangThai IS NULL)";
            }

            DataTable dt = bus.getTableNhaCungCap();
            DataRow[] rows = bus.getFilter_NCC(strFilter);
            if (rows.Length > 0)
            {
                dgvNhaCungCap.DataSource = rows.CopyToDataTable();
            }
            else
            {
                dgvNhaCungCap.DataSource = dt.Clone();
            }
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            filter_dsncc();
        }

        private Boolean checkInput()
        {
            Boolean kq = true;
            if (txtMaNCC.Text == "")
            {
                kq = false;
                txtMaNCC.Focus();
            }
            else if (txtTenNCC.Text == "")
            {
                kq = false;
                txtTenNCC.Focus();
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

        private void dgvNhaCungCap_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvNhaCungCap.Rows.Count) return;
            var dgvRow = dgvNhaCungCap.Rows[e.RowIndex];
            if (dgvRow.IsNewRow) return;

            DataRowView row = dgvRow.DataBoundItem as DataRowView;
            if (row != null)
            {
                txtMaNCC.Text = row["MaNCC"].ToString();
                txtTenNCC.Text = row["TenNCC"].ToString();
                txtSoDienThoai.Text = row["SoDienThoai"].ToString();
                txtDiaChi.Text = row["DiaChi"].ToString();
                if(row["TrangThai"] != DBNull.Value)
                    cboTrangThai.SelectedIndex = Convert.ToInt32(row["TrangThai"]) == 1 ? 1 : 0;
            }
            else
            {
                DataRow dataRow = (dgvRow.DataBoundItem as DataRowView)?.Row;
                if(dataRow != null)
                {
                    txtMaNCC.Text = dataRow["MaNCC"].ToString();
                    txtTenNCC.Text = dataRow["TenNCC"].ToString();
                    txtSoDienThoai.Text = dataRow["SoDienThoai"].ToString();
                    txtDiaChi.Text = dataRow["DiaChi"].ToString();
                    if(dataRow["TrangThai"] != DBNull.Value)
                        cboTrangThai.SelectedIndex = Convert.ToInt32(dataRow["TrangThai"]) == 1 ? 1 : 0;
                }
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (checkInput())
            {
                NhaCungCapDTO ncc = new NhaCungCapDTO();
                ncc.MaNCC = txtMaNCC.Text;
                ncc.TenNCC = txtTenNCC.Text;
                ncc.SoDienThoai = txtSoDienThoai.Text;
                ncc.DiaChi = txtDiaChi.Text;
                // Hàm thêm mới luôn gán cứng TrangThai = 1 trong BUS, người dùng không tự nhập khi Thêm.
                // Trừ khi bạn muốn cho phép thêm NCC đã ngừng CC ngay từ đầu (ít xảy ra).

                Boolean kq = bus.add_New_NCC(ncc);
                if (!kq)
                {
                    MessageBox.Show("Thêm mới không thành công. Có thể mã nhà cung cấp đã tồn tại!");
                }
                else
                {
                    LoadData();
                    lammoi();
                    MessageBox.Show("Thêm nhà cung cấp thành công!", "Thông báo");
                }
            }
            else
            {
                MessageBox.Show("Bạn chưa nhập đủ dữ liệu!");
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dgvNhaCungCap.CurrentRow == null || dgvNhaCungCap.CurrentRow.IsNewRow)
            {
                MessageBox.Show("Chưa chọn nhà cung cấp cần sửa!", "Thông báo");
                return;
            }

            if (checkInput())
            {
                NhaCungCapDTO ncc = new NhaCungCapDTO();
                ncc.MaNCC = txtMaNCC.Text.Trim();
                ncc.TenNCC = txtTenNCC.Text.Trim();
                ncc.SoDienThoai = txtSoDienThoai.Text.Trim();
                ncc.DiaChi = txtDiaChi.Text.Trim();
                ncc.TrangThai = cboTrangThai.SelectedIndex; // 0 = Ngừng cung cấp, 1 = Đang cung cấp

                if (bus.update_NCC(ncc))
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
            if (dgvNhaCungCap.CurrentRow == null || dgvNhaCungCap.CurrentRow.IsNewRow)
            {
                MessageBox.Show("Chưa chọn nhà cung cấp cần thao tác!", "Thông báo");
                return;
            }

            string maNCC = dgvNhaCungCap.CurrentRow.Cells["MaNCC"].Value.ToString();
            DialogResult ret = MessageBox.Show("Bạn có chắc chắn muốn ngừng giao dịch với nhà cung cấp " + maNCC + "?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (ret == DialogResult.Yes)
            {
                if (bus.delete_NCC(maNCC))
                {
                    LoadData();
                    lammoi();
                    MessageBox.Show("Đã ngừng giao dịch với nhà cung cấp này", "Thông báo");
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
            txtMaNCC.Enabled = true;
            txtMaNCC.Clear();
            txtTenNCC.Clear();
            txtSoDienThoai.Clear();
            txtDiaChi.Clear();
            txtTimKiem.Clear();
            if(cboTrangThai != null) cboTrangThai.SelectedIndex = 1;
            dgvNhaCungCap.ClearSelection();
            txtMaNCC.Focus();
        }

        private void chkHienThiDaXoa_CheckedChanged(object sender, EventArgs e)
        {
            filter_dsncc();
        }

        private void btnXoaVinhVien_Click(object sender, EventArgs e)
        {
            if (dgvNhaCungCap.CurrentRow == null || dgvNhaCungCap.CurrentRow.IsNewRow)
            {
                MessageBox.Show("Chưa chọn nhà cung cấp cần thao tác!", "Thông báo");
                return;
            }

            string maNCC = dgvNhaCungCap.CurrentRow.Cells["MaNCC"].Value.ToString();
            DialogResult ret = MessageBox.Show("Bạn có chắc chắn muốn xóa VĨNH VIỄN nhà cung cấp " + maNCC + "? Hành động này không thể hoàn tác!", "Cảnh báo",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (ret == DialogResult.Yes)
            {
                string msg = bus.XoaVinhVien(maNCC);
                if (msg == "")
                {
                    bus = new NhaCungCapBUS(); // Reload từ DB
                    LoadData();
                    lammoi();
                    MessageBox.Show("Đã xóa vĩnh viễn nhà cung cấp!", "Thông báo");
                }
                else
                {
                    MessageBox.Show(msg, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
