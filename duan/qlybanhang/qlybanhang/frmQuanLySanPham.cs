using System;
using System.Data;
using System.Windows.Forms;
using BUS;
using DTO;

namespace qlybanhang
{
    public partial class frmQuanLySanPham : Form
    {
        MyBUS bus = new MyBUS();

        public frmQuanLySanPham()
        {
            InitializeComponent();
        }

        private void frmQuanLySanPham_Load(object sender, EventArgs e)
        {
            LoadData();
            
            cboNhaCungCap.DataSource = bus.getTableNhaCungCap();
            cboNhaCungCap.DisplayMember = "TenNCC";
            cboNhaCungCap.ValueMember = "MaNCC";
        }

        private void LoadData()
        {
            dgvSanPham.DataSource = bus.getTableSanPham();
            if (dgvSanPham.Columns.Count > 0)
            {
                dgvSanPham.Columns["MaSP"].HeaderText = "Mã SP";
                dgvSanPham.Columns["TenSP"].HeaderText = "Tên sản phẩm";
                dgvSanPham.Columns["MaNCC"].HeaderText = "Mã NCC";
                dgvSanPham.Columns["GiaNhap"].HeaderText = "Giá nhập";
                dgvSanPham.Columns["GiaBan"].HeaderText = "Giá bán";
                dgvSanPham.Columns["SoLuongTon"].HeaderText = "Số lượng tồn";
            }
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtTimKiem.Text.Trim().Replace("'", "''");
            DataRow[] rows = bus.getFilter_SP(string.Format("TenSP LIKE '%{0}%'", keyword));
            if (rows.Length > 0)
            {
                dgvSanPham.DataSource = rows.CopyToDataTable();
            }
            else
            {
                dgvSanPham.DataSource = bus.getTableSanPham().Clone();
            }
        }

        private bool checkInput()
        {
            if (string.IsNullOrEmpty(txtTenSP.Text.Trim()))
            {
                MessageBox.Show("Vui lòng nhập tên sản phẩm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenSP.Focus();
                return false;
            }
            if (cboNhaCungCap.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn nhà cung cấp!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboNhaCungCap.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtGiaNhap.Text.Trim()))
            {
                MessageBox.Show("Vui lòng nhập giá nhập!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtGiaNhap.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtGiaBan.Text.Trim()))
            {
                MessageBox.Show("Vui lòng nhập giá bán!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtGiaBan.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtSoLuongTon.Text.Trim()))
            {
                MessageBox.Show("Vui lòng nhập số lượng tồn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoLuongTon.Focus();
                return false;
            }
            return true;
        }

        private void dgvSanPham_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvSanPham.Rows.Count) return;
            var dgvRow = dgvSanPham.Rows[e.RowIndex];
            if (dgvRow.IsNewRow) return;

            DataRowView row = dgvRow.DataBoundItem as DataRowView;
            if (row == null) return;

            txtTenSP.Text = row["TenSP"].ToString();
            cboNhaCungCap.SelectedValue = row["MaNCC"];
            txtGiaNhap.Text = row["GiaNhap"].ToString();
            txtGiaBan.Text = row["GiaBan"].ToString();
            txtSoLuongTon.Text = row["SoLuongTon"].ToString();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                if (!checkInput()) return;
                SanPhamDTO sp = new SanPhamDTO();
                sp.TenSP = txtTenSP.Text.Trim();
                sp.MaNCC = Convert.ToInt32(cboNhaCungCap.SelectedValue);
                sp.GiaNhap = Convert.ToDecimal(txtGiaNhap.Text.Trim());
                sp.GiaBan = Convert.ToDecimal(txtGiaBan.Text.Trim());
                sp.SoLuongTon = Convert.ToInt32(txtSoLuongTon.Text.Trim());
                
                if (bus.add_New_SP(sp))
                {
                    LoadData();
                    LamMoi();
                    MessageBox.Show("Thêm sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Thêm sản phẩm thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                if (dgvSanPham.CurrentRow == null || dgvSanPham.CurrentRow.IsNewRow)
                {
                    MessageBox.Show("Chưa chọn sản phẩm cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (!checkInput()) return;
                int maSP = Convert.ToInt32(dgvSanPham.CurrentRow.Cells["MaSP"].Value);
                SanPhamDTO sp = new SanPhamDTO();
                sp.MaSP = maSP;
                sp.TenSP = txtTenSP.Text.Trim();
                sp.MaNCC = Convert.ToInt32(cboNhaCungCap.SelectedValue);
                sp.GiaNhap = Convert.ToDecimal(txtGiaNhap.Text.Trim());
                sp.GiaBan = Convert.ToDecimal(txtGiaBan.Text.Trim());
                sp.SoLuongTon = Convert.ToInt32(txtSoLuongTon.Text.Trim());
                
                if (bus.update_SP(sp))
                {
                    LoadData();
                    LamMoi();
                    MessageBox.Show("Sửa sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Không tìm thấy sản phẩm để sửa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                if (dgvSanPham.CurrentRow == null || dgvSanPham.CurrentRow.IsNewRow)
                {
                    MessageBox.Show("Chưa chọn sản phẩm cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                DialogResult dr = MessageBox.Show("Bạn có chắc muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    int maSP = Convert.ToInt32(dgvSanPham.CurrentRow.Cells["MaSP"].Value);
                    if (bus.delete_SP(maSP))
                    {
                        LoadData();
                        LamMoi();
                        MessageBox.Show("Xóa sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy sản phẩm để xóa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            txtTenSP.Clear();
            if (cboNhaCungCap.Items.Count > 0)
                cboNhaCungCap.SelectedIndex = 0;
            txtGiaNhap.Clear();
            txtGiaBan.Clear();
            txtSoLuongTon.Clear();
            txtTimKiem.Clear();
            dgvSanPham.ClearSelection();
            txtTenSP.Focus();
        }
    }
}
