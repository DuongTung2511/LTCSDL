using System;
using System.Data;
using System.Windows.Forms;
using BUS;

namespace qlybanhang
{
    public partial class frmBanHang : Form
    {
        MyBUS bus = new MyBUS();
        public int MaNV { get; set; }
        private DataTable gioHang;

        public frmBanHang()
        {
            InitializeComponent();
        }

        private void frmBanHang_Load(object sender, EventArgs e)
        {
            // Load Khách Hàng
            cboKhachHang.DataSource = bus.getTableKhachHang();
            cboKhachHang.DisplayMember = "TenKH";
            cboKhachHang.ValueMember = "MaKH";

            // Load Sản Phẩm
            LoadDataSanPham();

            KhoiTaoGioHang();
        }

        private void KhoiTaoGioHang()
        {
            gioHang = new DataTable();
            gioHang.Columns.Add("MaSP", typeof(int));
            gioHang.Columns.Add("TenSP", typeof(string));
            gioHang.Columns.Add("SoLuong", typeof(int));
            gioHang.Columns.Add("DonGia", typeof(decimal));
            gioHang.Columns.Add("ThanhTien", typeof(decimal));
            dgvGioHang.DataSource = gioHang;
            
            if (dgvGioHang.Columns.Count > 0)
            {
                dgvGioHang.Columns["MaSP"].HeaderText = "Mã SP";
                dgvGioHang.Columns["TenSP"].HeaderText = "Tên sản phẩm";
                dgvGioHang.Columns["SoLuong"].HeaderText = "Số lượng";
                dgvGioHang.Columns["DonGia"].HeaderText = "Đơn giá";
                dgvGioHang.Columns["ThanhTien"].HeaderText = "Thành tiền";
            }
        }

        private void LoadDataSanPham()
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

        private void txtTimKiemSanPham_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtTimKiemSanPham.Text.Trim().Replace("'", "''");
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

        private void btnThemGioHang_Click(object sender, EventArgs e)
        {
            if (dgvSanPham.CurrentRow == null) return;
            DataRowView drvSP = dgvSanPham.CurrentRow.DataBoundItem as DataRowView;
            if (drvSP == null) return;

            int maSP = Convert.ToInt32(drvSP["MaSP"]);
            string tenSP = drvSP["TenSP"].ToString();
            decimal donGia = Convert.ToDecimal(drvSP["GiaBan"]);
            int soLuongTon = Convert.ToInt32(drvSP["SoLuongTon"]);
            int soLuongThem = (int)nudSoLuong.Value;

            if (soLuongTon < soLuongThem)
            {
                MessageBox.Show("Số lượng tồn không đủ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Check if exist
            DataRow[] existing = gioHang.Select("MaSP = " + maSP);
            if (existing.Length > 0)
            {
                int soLuongHienTai = Convert.ToInt32(existing[0]["SoLuong"]);
                if (soLuongTon < soLuongHienTai + soLuongThem)
                {
                    MessageBox.Show("Số lượng tồn không đủ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                existing[0]["SoLuong"] = soLuongHienTai + soLuongThem;
                existing[0]["ThanhTien"] = (soLuongHienTai + soLuongThem) * donGia;
            }
            else
            {
                DataRow r = gioHang.NewRow();
                r["MaSP"] = maSP;
                r["TenSP"] = tenSP;
                r["SoLuong"] = soLuongThem;
                r["DonGia"] = donGia;
                r["ThanhTien"] = soLuongThem * donGia;
                gioHang.Rows.Add(r);
            }

            CapNhatTongTien();
        }

        private void btnXoaGioHang_Click(object sender, EventArgs e)
        {
            if (dgvGioHang.CurrentRow == null) return;
            DataRowView drvGH = dgvGioHang.CurrentRow.DataBoundItem as DataRowView;
            if (drvGH != null)
            {
                drvGH.Row.Delete();
                gioHang.AcceptChanges();
                CapNhatTongTien();
            }
        }

        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            if (gioHang.Rows.Count == 0)
            {
                MessageBox.Show("Giỏ hàng trống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (cboKhachHang.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn khách hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int maKH = Convert.ToInt32(cboKhachHang.SelectedValue);
                bus.TaoHoaDon(maKH, MaNV, gioHang);
                MessageBox.Show("Thanh toán thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                gioHang.Rows.Clear();
                CapNhatTongTien();
                // Reload san pham to update so luong ton
                LoadDataSanPham();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CapNhatTongTien()
        {
            decimal tong = 0;
            foreach (DataRow r in gioHang.Rows)
                tong += Convert.ToDecimal(r["ThanhTien"]);
            lblTongTien.Text = "Tổng tiền: " + tong.ToString("N0") + " VNĐ";
        }
    }
}
