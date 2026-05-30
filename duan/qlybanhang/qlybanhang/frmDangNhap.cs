using System;
using System.Data;
using System.Windows.Forms;
using BUS;
using DTO;

namespace qlybanhang
{
    public partial class frmDangNhap : Form
    {
        TaiKhoanBUS bus = new TaiKhoanBUS();

        public frmDangNhap()
        {
            InitializeComponent();
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            // Bước 1: Lấy thông tin người dùng nhập vào
            string tenDangNhap = txtTenDangNhap.Text.Trim();
            string matKhau = txtMatKhau.Text.Trim();

            // Bước 2: Kiểm tra xem có để trống không
            if (tenDangNhap == "" || matKhau == "")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ tên đăng nhập và mật khẩu!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Dừng hàm lại, không chạy tiếp xuống dưới
            }

            // Bước 3: Gọi tầng BUS (MyBUS) để kiểm tra trong CSDL
            DataRow row = bus.DangNhap(tenDangNhap, matKhau);

            // Bước 4: Xử lý kết quả trả về
            if (row != null) // Nếu row có dữ liệu (không null) -> Đăng nhập thành công
            {
                MessageBox.Show("Đăng nhập thành công!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                // Tạo form chính (frmMain)
                frmMain frm = new frmMain();
                frm.TenDangNhap = tenDangNhap;           // Truyền tên đăng nhập qua form chính
                frm.Quyen = row["Quyen"].ToString();     // Truyền quyền qua form chính
                
                this.Hide(); // Ẩn form đăng nhập đi

                // Hiển thị form chính lên và chờ cho đến khi form chính bị đóng lại
                frm.ShowDialog();

                // Dòng lệnh này chỉ chạy tiếp khi frmMain đã bị tắt
                if (frm.IsLogout == true)
                {
                    // Nếu người dùng chọn "Đăng xuất" -> Hiển thị lại form đăng nhập
                    this.Show();
                    txtTenDangNhap.Clear();
                    txtMatKhau.Clear();
                    txtTenDangNhap.Focus();
                }
                else
                {
                    // Nếu tắt bằng dấu X hoặc nút Thoát -> Tắt luôn toàn bộ chương trình
                    this.Close();
                }
            }
            else // Nếu row là null -> Không tìm thấy tài khoản
            {
                MessageBox.Show("Sai tên đăng nhập hoặc mật khẩu!", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lnkDangKy_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmDangKy frm = new frmDangKy();
            frm.ShowDialog();
        }
    }
}
