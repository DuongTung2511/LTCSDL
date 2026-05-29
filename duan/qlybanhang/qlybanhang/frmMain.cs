using System;
using System.Windows.Forms;
using BUS;

namespace qlybanhang
{
    public partial class frmMain : Form
    {
        MyBUS bus = new MyBUS();
        public string TenDangNhap { get; set; }
        public string Quyen { get; set; }
        public bool IsLogout { get; set; } = false;

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            string quyenHienThi = Quyen == "quanly" ? "Quản lý" : "Nhân viên";
            lblUserInfo.Text = "Người dùng: " + TenDangNhap + " | Quyền: " + quyenHienThi;

            // Phân quyền: Nhân viên không được vào menu Quản lý
            if (Quyen == "nhanvien" || Quyen == "Nhân viên")
            {
                mnuQuanLy.Visible = false;
            }
        }

        // --- Menu Quản lý ---
        private void mnuSanPham_Click(object sender, EventArgs e)
        {
            frmQuanLySanPham frm = new frmQuanLySanPham();
            frm.ShowDialog();
        }

        private void mnuKhachHang_Click(object sender, EventArgs e)
        {
            frmQuanLyKhachHang frm = new frmQuanLyKhachHang();
            frm.ShowDialog();
        }

        private void mnuNhanVien_Click(object sender, EventArgs e)
        {
            frmQuanLyNhanVien frm = new frmQuanLyNhanVien();
            frm.ShowDialog();
        }

        private void mnuNhaCungCap_Click(object sender, EventArgs e)
        {
            frmQuanLyNhaCungCap frm = new frmQuanLyNhaCungCap();
            frm.ShowDialog();
        }

        private void mnuTaiKhoan_Click(object sender, EventArgs e)
        {
            frmQuanLyTaiKhoan frm = new frmQuanLyTaiKhoan();
            frm.ShowDialog();
        }

        // --- Menu Bán hàng ---
        private void mnuTaoHoaDon_Click(object sender, EventArgs e)
        {
            frmBanHang frm = new frmBanHang();
            frm.MaNV = bus.LayMaNV(TenDangNhap);
            frm.ShowDialog();
        }

        // --- Menu Lịch sử ---
        private void mnuXemHoaDon_Click(object sender, EventArgs e)
        {
            frmLichSuHoaDon frm = new frmLichSuHoaDon();
            frm.ShowDialog();
        }

        // --- Menu Hệ thống ---
        private void mnuDangXuat_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Bạn có muốn đăng xuất?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                this.IsLogout = true;
                this.Close();
            }
        }

        private void mnuThoat_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Bạn có muốn thoát ứng dụng?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
}
