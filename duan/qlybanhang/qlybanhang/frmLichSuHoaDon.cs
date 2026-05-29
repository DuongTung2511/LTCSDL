using System;
using System.Data;
using System.Windows.Forms;
using BUS;

namespace qlybanhang
{
    public partial class frmLichSuHoaDon : Form
    {
        MyBUS bus = new MyBUS();

        public frmLichSuHoaDon()
        {
            InitializeComponent();
        }

        private void frmLichSuHoaDon_Load(object sender, EventArgs e)
        {
            dgvHoaDon.DataSource = bus.LayDanhSachHoaDonDayDu();
            if (dgvHoaDon.Columns.Count > 0)
            {
                dgvHoaDon.Columns["MaHD"].HeaderText = "Mã HĐ";
                dgvHoaDon.Columns["MaKH"].Visible = false;
                dgvHoaDon.Columns["TenKH"].HeaderText = "Tên khách hàng";
                dgvHoaDon.Columns["MaNV"].Visible = false;
                dgvHoaDon.Columns["TenNV"].HeaderText = "Tên nhân viên";
                dgvHoaDon.Columns["NgayLap"].HeaderText = "Ngày lập";
                dgvHoaDon.Columns["TongTien"].HeaderText = "Tổng tiền";
            }
        }

        private void txtTimKiemKH_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtTimKiemKH.Text.Trim().Replace("'", "''");
            DataRow[] rows = bus.getFilter_HDDayDu(string.Format("TenKH LIKE '%{0}%'", keyword));
            if (rows.Length > 0)
            {
                dgvHoaDon.DataSource = rows.CopyToDataTable();
            }
            else
            {
                dgvHoaDon.DataSource = bus.LayDanhSachHoaDonDayDu().Clone();
            }
        }

        private void dgvHoaDon_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvHoaDon.CurrentRow != null && dgvHoaDon.CurrentRow.Cells["MaHD"].Value != DBNull.Value)
            {
                int maHD = Convert.ToInt32(dgvHoaDon.CurrentRow.Cells["MaHD"].Value);
                DataTable dtChiTietDayDu = bus.LayDanhSachChiTietHDDayDu(maHD);
                dgvChiTiet.DataSource = dtChiTietDayDu;
                if (dgvChiTiet.Columns.Count > 0)
                {
                    dgvChiTiet.Columns["MaHD"].Visible = false;
                    dgvChiTiet.Columns["MaSP"].Visible = false;
                    dgvChiTiet.Columns["TenSP"].HeaderText = "Tên sản phẩm";
                    dgvChiTiet.Columns["SoLuong"].HeaderText = "Số lượng";
                    dgvChiTiet.Columns["DonGia"].HeaderText = "Đơn giá";
                    dgvChiTiet.Columns["ThanhTien"].HeaderText = "Thành tiền";
                }
            }
            else
            {
                dgvChiTiet.DataSource = null;
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvHoaDon.CurrentRow != null)
            {
                int maHD = Convert.ToInt32(dgvHoaDon.CurrentRow.Cells["MaHD"].Value);
                DialogResult dr = MessageBox.Show("Bạn có chắc chắn muốn xóa hóa đơn này (cũng sẽ khôi phục lại số lượng tồn kho sản phẩm)?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dr == DialogResult.Yes)
                {
                    bus.XoaHoaDon(maHD);
                    MessageBox.Show("Xóa hóa đơn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // Load lại danh sách sau khi xóa
                    dgvHoaDon.DataSource = bus.LayDanhSachHoaDonDayDu();
                }
            }
        }
    }
}
