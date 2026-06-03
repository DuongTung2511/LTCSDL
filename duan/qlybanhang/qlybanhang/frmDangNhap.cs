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
            
            string tenDangNhap = txtTenDangNhap.Text.Trim();
            string matKhau = txtMatKhau.Text.Trim();

            
            if (tenDangNhap == "" || matKhau == "")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ tên đăng nhập và mật khẩu!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; 
            }

            DataRow row = bus.DangNhap(tenDangNhap, matKhau);

            if (row != null) 
            {
                MessageBox.Show("Đăng nhập thành công!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                
               
                frmMain frm = new frmMain();
                frm.TenDangNhap = tenDangNhap;           
                frm.Quyen = row["Quyen"].ToString();     
                
                this.Hide(); 

                frm.ShowDialog();

                if (frm.IsLogout == true)
                {
                    this.Show();
                    txtTenDangNhap.Clear();
                    txtMatKhau.Clear();
                    txtTenDangNhap.Focus();
                }
                else
                {
                    this.Close();
                }
            }
            else 
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
