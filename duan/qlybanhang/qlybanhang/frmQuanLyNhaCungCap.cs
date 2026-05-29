using System;
using System.Data;
using System.Windows.Forms;
using BUS;
using DTO;

namespace qlybanhang
{
    public partial class frmQuanLyNhaCungCap : Form
    {
        MyBUS bus = new MyBUS();

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
            dgvNhaCungCap.DataSource = bus.getTableNhaCungCap();
            if (dgvNhaCungCap.Columns.Count > 0)
            {
                dgvNhaCungCap.Columns["MaNCC"].HeaderText = "Mã NCC";
                dgvNhaCungCap.Columns["TenNCC"].HeaderText = "Tên nhà cung cấp";
                dgvNhaCungCap.Columns["SoDienThoai"].HeaderText = "Số điện thoại";
                dgvNhaCungCap.Columns["DiaChi"].HeaderText = "Địa chỉ";
            }
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtTimKiem.Text.Trim().Replace("'", "''");
            DataRow[] rows = bus.getFilter_NCC(string.Format("TenNCC LIKE '%{0}%' OR SoDienThoai LIKE '%{0}%'", keyword));
            if (rows.Length > 0)
            {
                dgvNhaCungCap.DataSource = rows.CopyToDataTable();
            }
            else
            {
                dgvNhaCungCap.DataSource = bus.getTableNhaCungCap().Clone();
            }
        }

        private bool checkInput()
        {
            if (string.IsNullOrEmpty(txtTenNCC.Text.Trim()))
            {
                MessageBox.Show("Vui lòng nhập tên nhà cung cấp!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenNCC.Focus();
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

        private void dgvNhaCungCap_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvNhaCungCap.Rows.Count) return;
            var dgvRow = dgvNhaCungCap.Rows[e.RowIndex];
            if (dgvRow.IsNewRow) return;

            DataRowView row = dgvRow.DataBoundItem as DataRowView;
            if (row == null) return;

            txtTenNCC.Text = row["TenNCC"].ToString();
            txtSoDienThoai.Text = row["SoDienThoai"].ToString();
            txtDiaChi.Text = row["DiaChi"].ToString();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                if (!checkInput()) return;
                NhaCungCapDTO ncc = new NhaCungCapDTO();
                ncc.TenNCC = txtTenNCC.Text.Trim();
                ncc.SoDienThoai = txtSoDienThoai.Text.Trim();
                ncc.DiaChi = txtDiaChi.Text.Trim();
                if (bus.add_New_NCC(ncc))
                {
                    LoadData();
                    LamMoi();
                    MessageBox.Show("Thêm nhà cung cấp thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Thêm nhà cung cấp thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                if (dgvNhaCungCap.CurrentRow == null || dgvNhaCungCap.CurrentRow.IsNewRow)
                {
                    MessageBox.Show("Chưa chọn nhà cung cấp cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (!checkInput()) return;
                int maNCC = Convert.ToInt32(dgvNhaCungCap.CurrentRow.Cells["MaNCC"].Value);
                NhaCungCapDTO ncc = new NhaCungCapDTO();
                ncc.MaNCC = maNCC;
                ncc.TenNCC = txtTenNCC.Text.Trim();
                ncc.SoDienThoai = txtSoDienThoai.Text.Trim();
                ncc.DiaChi = txtDiaChi.Text.Trim();
                
                if (bus.update_NCC(ncc))
                {
                    LoadData();
                    LamMoi();
                    MessageBox.Show("Sửa nhà cung cấp thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Không tìm thấy nhà cung cấp để sửa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                if (dgvNhaCungCap.CurrentRow == null || dgvNhaCungCap.CurrentRow.IsNewRow)
                {
                    MessageBox.Show("Chưa chọn nhà cung cấp cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                DialogResult dr = MessageBox.Show("Bạn có chắc muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    int maNCC = Convert.ToInt32(dgvNhaCungCap.CurrentRow.Cells["MaNCC"].Value);
                    if (bus.delete_NCC(maNCC))
                    {
                        LoadData();
                        LamMoi();
                        MessageBox.Show("Xóa nhà cung cấp thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy nhà cung cấp để xóa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            txtTenNCC.Clear();
            txtSoDienThoai.Clear();
            txtDiaChi.Clear();
            txtTimKiem.Clear();
            dgvNhaCungCap.ClearSelection();
            txtTenNCC.Focus();
        }
    }
}
