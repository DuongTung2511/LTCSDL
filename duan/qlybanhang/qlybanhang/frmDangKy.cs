using System;
using System.Windows.Forms;
using BUS;
using DTO;

namespace qlybanhang
{
    public partial class frmDangKy : Form
    {
        TaiKhoanBUS bus = new TaiKhoanBUS();

        public frmDangKy()
        {
            InitializeComponent();
        }

        private void btnDangKy_Click(object sender, EventArgs e)
        {
            string user = txtTenDangNhap.Text.Trim();
            string pass = txtMatKhau.Text.Trim();
            string confirm = txtXacNhan.Text.Trim();
            string maNV = txtMaNV.Text.Trim();

            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass) || string.IsNullOrEmpty(maNV))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin (bao gồm mã nhân viên)!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (pass != confirm)
            {
                MessageBox.Show("Mật khẩu xác nhận không khớp!", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            NhanVienBUS nvBus = new NhanVienBUS();
            if (nvBus.MaNV_not_Exist(maNV))
            {
                MessageBox.Show("Mã nhân viên không tồn tại trong hệ thống!", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (bus.KiemTraNhanVienDaCoTaiKhoan(maNV))
            {
                MessageBox.Show("Nhân viên này đã có tài khoản, không thể tạo thêm!", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            TaiKhoanDTO tk = new TaiKhoanDTO();
            tk.TenDangNhap = user;
            tk.MatKhau = pass;
            tk.MaNV = maNV;

            bool result = bus.DangKy(tk);
            if (result)
            {
                MessageBox.Show("Đăng ký thành công!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("Tên đăng nhập đã tồn tại!", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
