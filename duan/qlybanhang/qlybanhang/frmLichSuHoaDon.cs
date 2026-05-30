using System;
using System.Data;
using System.Windows.Forms;
using BUS;

namespace qlybanhang
{
    public partial class frmLichSuHoaDon : Form
    {
        HoaDonBUS hdBus = new HoaDonBUS();

        public frmLichSuHoaDon()
        {
            InitializeComponent();
        }

        private void frmLichSuHoaDon_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            dgvHoaDon.DataSource = hdBus.LayDanhSachHoaDonDayDu();
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
            DataRow[] rows = hdBus.getFilter_HDDayDu(string.Format("TenKH LIKE '%{0}%'", keyword));
            if (rows.Length > 0)
            {
                dgvHoaDon.DataSource = rows.CopyToDataTable();
            }
            else
            {
                dgvHoaDon.DataSource = hdBus.LayDanhSachHoaDonDayDu().Clone();
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvHoaDon.CurrentRow != null && dgvHoaDon.CurrentRow.Cells["MaHD"].Value != DBNull.Value)
            {
                string maHD = dgvHoaDon.CurrentRow.Cells["MaHD"].Value.ToString();
                DialogResult dr = MessageBox.Show("Bạn có chắc chắn muốn xóa hóa đơn này (cũng sẽ khôi phục lại số lượng tồn kho sản phẩm)?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dr == DialogResult.Yes)
                {
                    hdBus.XoaHoaDon(maHD);
                    MessageBox.Show("Xóa hóa đơn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                }
            }
        }

        private void btnXemChiTiet_Click(object sender, EventArgs e)
        {
            if (dgvHoaDon.CurrentRow != null && dgvHoaDon.CurrentRow.Cells["MaHD"].Value != DBNull.Value)
            {
                string maHD = dgvHoaDon.CurrentRow.Cells["MaHD"].Value.ToString();
                frmChiTietHoaDon frm = new frmChiTietHoaDon(maHD);
                frm.ShowDialog();
                // Tải lại dữ liệu hóa đơn (do tổng tiền có thể thay đổi sau khi sửa chi tiết)
                LoadData();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một hóa đơn để xem chi tiết!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
