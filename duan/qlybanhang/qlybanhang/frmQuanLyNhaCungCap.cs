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
            LoadData();
        }

        private void LoadData()
        {
            DataViewManager dvm = bus.getDataset().DefaultViewManager;
            dgvNhaCungCap.DataSource = dvm;
            dgvNhaCungCap.DataMember = "NhaCungCap";

            if (dgvNhaCungCap.Columns.Count > 0)
            {
                dgvNhaCungCap.Columns["MaNCC"].HeaderText = "Mã NCC";
                dgvNhaCungCap.Columns["TenNCC"].HeaderText = "Tên nhà cung cấp";
                dgvNhaCungCap.Columns["SoDienThoai"].HeaderText = "Số điện thoại";
                dgvNhaCungCap.Columns["DiaChi"].HeaderText = "Địa chỉ";
            }
            dgvNhaCungCap.ReadOnly = true;
        }

        private void filter_dsncc()
        {
            DataRow[] rows = bus.getFilter_NCC("TenNCC LIKE '%" + txtTimKiem.Text.Replace("'", "''") + "%' OR SoDienThoai LIKE '%" + txtTimKiem.Text.Replace("'", "''") + "%'");
            if (rows.Length > 0)
            {
                dgvNhaCungCap.DataSource = rows.CopyToDataTable();
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
            if (row == null) return;

            txtMaNCC.Text = row["MaNCC"].ToString();
            txtTenNCC.Text = row["TenNCC"].ToString();
            txtSoDienThoai.Text = row["SoDienThoai"].ToString();
            txtDiaChi.Text = row["DiaChi"].ToString();
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
                MessageBox.Show("Chưa chọn nhà cung cấp cần xoá!", "Thông báo");
                return;
            }

            string maNCC = dgvNhaCungCap.CurrentRow.Cells["MaNCC"].Value.ToString();
            DialogResult ret = MessageBox.Show("Bạn có chắc chắn muốn xoá nhà cung cấp " + maNCC + "?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (ret == DialogResult.Yes)
            {
                if (bus.delete_NCC(maNCC))
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
            txtMaNCC.Enabled = true;
            txtMaNCC.Clear();
            txtTenNCC.Clear();
            txtSoDienThoai.Clear();
            txtDiaChi.Clear();
            txtTimKiem.Clear();
            dgvNhaCungCap.ClearSelection();
            txtMaNCC.Focus();
        }
    }
}
