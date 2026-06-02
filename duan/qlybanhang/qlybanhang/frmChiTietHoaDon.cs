using System;
using System.Data;
using System.Windows.Forms;
using BUS;

namespace qlybanhang
{
    public partial class frmChiTietHoaDon : Form
    {
        private string maHD;
        ChiTietHoaDonBUS cthdBus = new ChiTietHoaDonBUS();
        SanPhamBUS spBus = new SanPhamBUS();

        public frmChiTietHoaDon(string maHD)
        {
            InitializeComponent();
            this.maHD = maHD;
        }

        private void frmChiTietHoaDon_Load(object sender, EventArgs e)
        {
            txtMaHD.Text = maHD;
            LoadSanPham();
            LoadData();
        }

        private void LoadSanPham()
        {
            DataTable dt = spBus.getTableSanPham();
            cboSanPham.DataSource = dt;
            cboSanPham.DisplayMember = "TenSP";
            cboSanPham.ValueMember = "MaSP";
        }

        private void LoadData()
        {
            dgvChiTiet.DataSource = cthdBus.LayDanhSachChiTietHDDayDu(maHD);
            if (dgvChiTiet.Columns.Count > 0)
            {
                dgvChiTiet.Columns["MaHD"].Visible = false;
                dgvChiTiet.Columns["MaSP"].HeaderText = "Mã sản phẩm";
                dgvChiTiet.Columns["TenSP"].HeaderText = "Tên sản phẩm";
                dgvChiTiet.Columns["SoLuong"].HeaderText = "Số lượng";
                dgvChiTiet.Columns["DonGia"].HeaderText = "Đơn giá";
                dgvChiTiet.Columns["ThanhTien"].HeaderText = "Thành tiền";
            }
        }

        private void cboSanPham_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboSanPham.SelectedValue != null && cboSanPham.SelectedValue is int == false)
            {
                DataRowView drv = (DataRowView)cboSanPham.SelectedItem;
                txtDonGia.Text = drv["GiaBan"].ToString();
            }
            else if (cboSanPham.SelectedValue != null)
            {
                // In case ValueMember returns directly the object
                string maSP = cboSanPham.SelectedValue.ToString();
                DataRow[] rows = spBus.getTableSanPham().Select("MaSP = '" + maSP.Replace("'", "''") + "'");
                if (rows.Length > 0)
                {
                    txtDonGia.Text = rows[0]["GiaBan"].ToString();
                }
            }
        }

        private void dgvChiTiet_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvChiTiet.Rows.Count) return;
            var dgvRow = dgvChiTiet.Rows[e.RowIndex];
            if (dgvRow.IsNewRow) return;
            // Kiểm tra e.RowIndex >= 0 để đảm bảo người dùng không click vào tiêu đề cột
            //  // Lấy ra dòng hiện tại đang được click
            DataGridViewRow row = dgvChiTiet.Rows[e.RowIndex];

            // Đảm bảo dữ liệu không bị Null (tránh click vào dòng trống dưới cùng)
            if (row == null) return;
            cboSanPham.SelectedValue = row.Cells["MaSP"].Value.ToString();
            nudSoLuong.Value = Convert.ToDecimal(row.Cells["SoLuong"].Value);
            txtDonGia.Text = row.Cells["DonGia"].Value.ToString();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (cboSanPham.SelectedValue == null) return;
            string maSP = cboSanPham.SelectedValue.ToString();
            int soLuong = (int)nudSoLuong.Value;
            decimal donGia = Convert.ToDecimal(txtDonGia.Text);

            // Kiểm tra tồn kho
            DataRow[] rows = spBus.getTableSanPham().Select("MaSP = '" + maSP.Replace("'", "''") + "'");
            if (rows.Length > 0)
            {
                int tonKho = Convert.ToInt32(rows[0]["SoLuongTon"]);
                if (soLuong > tonKho)
                {
                    MessageBox.Show("Số lượng trong kho không đủ (chỉ còn " + tonKho + ")!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            bool result = cthdBus.ThemChiTiet(maHD, maSP, soLuong, donGia);
            if (result)
            {
                MessageBox.Show("Thêm sản phẩm vào hóa đơn thành công!");
                LoadData();
            }
            else
            {
                MessageBox.Show("Sản phẩm đã tồn tại trong hóa đơn. Vui lòng sử dụng tính năng Cập nhật!");
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (cboSanPham.SelectedValue == null) return;
            string maSP = cboSanPham.SelectedValue.ToString();
            int soLuongMoi = (int)nudSoLuong.Value;
            decimal donGiaMoi = Convert.ToDecimal(txtDonGia.Text);

            // Cần tính tồn kho có đủ không
            // Tồn kho thực tế = tồn kho hiện tại + số lượng cũ
            // Nhưng để nhanh, ta có thể cho qua nếu âm thì báo lỗi ở DB, 
            // hoặc kiểm tra kỹ hơn
            
            bool result = cthdBus.SuaChiTiet(maHD, maSP, soLuongMoi, donGiaMoi);
            if (result)
            {
                MessageBox.Show("Cập nhật số lượng thành công!");
                LoadData();
            }
            else
            {
                MessageBox.Show("Cập nhật thất bại. Sản phẩm không có trong hóa đơn!");
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (cboSanPham.SelectedValue == null) return;
            string maSP = cboSanPham.SelectedValue.ToString();
            
            DialogResult dr = MessageBox.Show("Bạn có chắc muốn xóa sản phẩm này khỏi hóa đơn?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                bool result = cthdBus.XoaChiTiet(maHD, maSP);
                if (result)
                {
                    MessageBox.Show("Xóa sản phẩm thành công!");
                    LoadData();
                }
                else
                {
                    MessageBox.Show("Xóa thất bại!");
                }
            }
        }
    }
}
