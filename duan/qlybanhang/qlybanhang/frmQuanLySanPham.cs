using System;
using System.Data;
using System.Windows.Forms;
using BUS;
using DTO;

namespace qlybanhang
{
    public partial class frmQuanLySanPham : Form
    {
        SanPhamBUS bus = new SanPhamBUS();
        NhaCungCapBUS nccBus = new NhaCungCapBUS();

        public frmQuanLySanPham()
        {
            InitializeComponent();
        }

        private void frmQuanLySanPham_Load(object sender, EventArgs e)
        {
            dgvSanPham.CellFormatting += dgvSanPham_CellFormatting;
            LoadData();
            
            cboNhaCungCap.DataSource = nccBus.LayDanhSachNCCDangHoatDong();
            cboNhaCungCap.DisplayMember = "TenNCC";
            cboNhaCungCap.ValueMember = "MaNCC";
        }

        private void dgvSanPham_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvSanPham.Columns[e.ColumnIndex].Name == "TrangThai" && e.Value != null)
            {
                if (e.Value.ToString() == "1" || e.Value.ToString() == "True")
                    e.Value = "Đang bán";
                else
                    e.Value = "Ngừng kinh doanh";
            }
        }

        private void LoadData()
        {
            DataViewManager dvm = bus.getDataset().DefaultViewManager;
            dgvSanPham.DataSource = dvm;
            dgvSanPham.DataMember = "SanPham";

            if (dgvSanPham.Columns.Count > 0)
            {
                dgvSanPham.Columns["MaSP"].HeaderText = "Mã SP";
                dgvSanPham.Columns["TenSP"].HeaderText = "Tên sản phẩm";
                dgvSanPham.Columns["MaNCC"].HeaderText = "Mã NCC";
                dgvSanPham.Columns["GiaBan"].HeaderText = "Giá bán";
                dgvSanPham.Columns["SoLuongTon"].HeaderText = "Số lượng tồn";
                if(dgvSanPham.Columns.Contains("TrangThai")) dgvSanPham.Columns["TrangThai"].HeaderText = "Trạng thái";
            }
            dgvSanPham.ReadOnly = true;
        }

        private void filter_dssp()
        {
            DataRow[] rows = bus.getFilter_SP("TenSP LIKE '%" + txtTimKiem.Text.Replace("'", "''") + "%'");
            if (rows.Length > 0)
            {
                dgvSanPham.DataSource = rows.CopyToDataTable();
            }
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            filter_dssp();
        }

        private Boolean checkInput()
        {
            Boolean kq = true;
            if (txtMaSP.Text == "")
            {
                kq = false;
                txtMaSP.Focus();
            }
            else if (txtTenSP.Text == "")
            {
                kq = false;
                txtTenSP.Focus();
            }
            else if (cboNhaCungCap.SelectedIndex < 0)
            {
                kq = false;
                cboNhaCungCap.Focus();
            }
            else if (txtGiaBan.Text == "")
            {
                kq = false;
                txtGiaBan.Focus();
            }
            else if (txtSoLuongTon.Text == "")
            {
                kq = false;
                txtSoLuongTon.Focus();
            }
            return kq;
        }

        private void dgvSanPham_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvSanPham.Rows.Count) return;
            var dgvRow = dgvSanPham.Rows[e.RowIndex];
            if (dgvRow.IsNewRow) return;

            DataRowView row = dgvRow.DataBoundItem as DataRowView;
            if (row == null) return;

            txtMaSP.Text = row["MaSP"].ToString();
            txtTenSP.Text = row["TenSP"].ToString();
            cboNhaCungCap.SelectedValue = row["MaNCC"];
            txtGiaBan.Text = row["GiaBan"].ToString();
            txtSoLuongTon.Text = row["SoLuongTon"].ToString();

            if (row["TrangThai"] != DBNull.Value)
                cboTrangThai.SelectedIndex = (row["TrangThai"].ToString() == "1" || row["TrangThai"].ToString() == "True") ? 1 : 0;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (checkInput())
            {
                SanPhamDTO sp = new SanPhamDTO();
                sp.MaSP = txtMaSP.Text;
                sp.TenSP = txtTenSP.Text;
                sp.MaNCC = cboNhaCungCap.SelectedValue.ToString();
                sp.GiaBan = Convert.ToDecimal(txtGiaBan.Text);
                sp.SoLuongTon = Convert.ToInt32(txtSoLuongTon.Text);

                Boolean kq = bus.add_New_SP(sp);
                if (!kq)
                {
                    MessageBox.Show("Thêm mới không thành công. Có thể mã sản phẩm đã tồn tại!");
                }
                else
                {
                    LoadData();
                    lammoi();
                    MessageBox.Show("Thêm sản phẩm thành công!", "Thông báo");
                }
            }
            else
            {
                MessageBox.Show("Bạn chưa nhập đủ dữ liệu!");
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dgvSanPham.CurrentRow == null || dgvSanPham.CurrentRow.IsNewRow)
            {
                MessageBox.Show("Chưa chọn sản phẩm cần sửa!", "Thông báo");
                return;
            }

            if (checkInput())
            {
                SanPhamDTO sp = new SanPhamDTO();
                sp.MaSP = txtMaSP.Text.Trim();
                sp.TenSP = txtTenSP.Text.Trim();
                sp.MaNCC = cboNhaCungCap.SelectedValue.ToString();
                sp.GiaBan = Convert.ToDecimal(txtGiaBan.Text.Trim());
                sp.SoLuongTon = Convert.ToInt32(txtSoLuongTon.Text.Trim());

                sp.TrangThai = cboTrangThai.SelectedIndex;

                if (bus.update_SP(sp))
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
            if (dgvSanPham.CurrentRow == null || dgvSanPham.CurrentRow.IsNewRow)
            {
                MessageBox.Show("Chưa chọn sản phẩm cần thao tác!", "Thông báo");
                return;
            }

            string maSP = dgvSanPham.CurrentRow.Cells["MaSP"].Value.ToString();
            DialogResult ret = MessageBox.Show("Bạn có chắc chắn muốn ngừng kinh doanh sản phẩm " + maSP + "?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (ret == DialogResult.Yes)
            {
                if (bus.delete_SP(maSP))
                {
                    LoadData();
                    lammoi();
                    MessageBox.Show("Đã chuyển trạng thái sang Ngừng kinh doanh!", "Thông báo");
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
            txtMaSP.Enabled = true;
            txtMaSP.Clear();
            txtTenSP.Clear();
            if (cboNhaCungCap.Items.Count > 0)
                cboNhaCungCap.SelectedIndex = 0;
            txtGiaBan.Clear();
            txtSoLuongTon.Clear();
            txtTimKiem.Clear();
            if (cboTrangThai != null) cboTrangThai.SelectedIndex = 1;
            dgvSanPham.ClearSelection();
            txtMaSP.Focus();
        }
    }
}
