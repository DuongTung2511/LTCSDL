namespace qlybanhang
{
    partial class frmMain
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.menuMain = new System.Windows.Forms.MenuStrip();
            this.mnuQuanLy = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuTaiKhoan = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSanPham = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuKhachHang = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuNhanVien = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuNhaCungCap = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuBanHang = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuTaoHoaDon = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuLichSu = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuXemHoaDon = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuHeThong = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDangXuat = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuThoat = new System.Windows.Forms.ToolStripMenuItem();
            this.lblUserInfo = new System.Windows.Forms.Label();
            this.lblWelcome = new System.Windows.Forms.Label();
            this.menuMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuMain
            // 
            this.menuMain.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuQuanLy,
            this.mnuBanHang,
            this.mnuLichSu,
            this.mnuHeThong});
            this.menuMain.Location = new System.Drawing.Point(0, 0);
            this.menuMain.Name = "menuMain";
            this.menuMain.Padding = new System.Windows.Forms.Padding(8, 2, 0, 2);
            this.menuMain.Size = new System.Drawing.Size(1067, 30);
            this.menuMain.TabIndex = 0;
            this.menuMain.Text = "menuMain";
            // 
            // mnuQuanLy
            // 
            this.mnuQuanLy.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuTaiKhoan,
            this.mnuSanPham,
            this.mnuKhachHang,
            this.mnuNhanVien,
            this.mnuNhaCungCap});
            this.mnuQuanLy.Name = "mnuQuanLy";
            this.mnuQuanLy.Size = new System.Drawing.Size(73, 26);
            this.mnuQuanLy.Text = "Quản lý";
            // 
            // mnuTaiKhoan
            // 
            this.mnuTaiKhoan.Name = "mnuTaiKhoan";
            this.mnuTaiKhoan.Size = new System.Drawing.Size(224, 26);
            this.mnuTaiKhoan.Text = "Tài khoản";
            this.mnuTaiKhoan.Click += new System.EventHandler(this.mnuTaiKhoan_Click);
            // 
            // mnuSanPham
            // 
            this.mnuSanPham.Name = "mnuSanPham";
            this.mnuSanPham.Size = new System.Drawing.Size(224, 26);
            this.mnuSanPham.Text = "Sản phẩm";
            this.mnuSanPham.Click += new System.EventHandler(this.mnuSanPham_Click);
            // 
            // mnuKhachHang
            // 
            this.mnuKhachHang.Name = "mnuKhachHang";
            this.mnuKhachHang.Size = new System.Drawing.Size(224, 26);
            this.mnuKhachHang.Text = "Khách hàng";
            this.mnuKhachHang.Click += new System.EventHandler(this.mnuKhachHang_Click);
            // 
            // mnuNhanVien
            // 
            this.mnuNhanVien.Name = "mnuNhanVien";
            this.mnuNhanVien.Size = new System.Drawing.Size(224, 26);
            this.mnuNhanVien.Text = "Nhân viên";
            this.mnuNhanVien.Click += new System.EventHandler(this.mnuNhanVien_Click);
            // 
            // mnuNhaCungCap
            // 
            this.mnuNhaCungCap.Name = "mnuNhaCungCap";
            this.mnuNhaCungCap.Size = new System.Drawing.Size(224, 26);
            this.mnuNhaCungCap.Text = "Nhà cung cấp";
            this.mnuNhaCungCap.Click += new System.EventHandler(this.mnuNhaCungCap_Click);
            // 
            // mnuBanHang
            // 
            this.mnuBanHang.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuTaoHoaDon});
            this.mnuBanHang.Name = "mnuBanHang";
            this.mnuBanHang.Size = new System.Drawing.Size(85, 26);
            this.mnuBanHang.Text = "Bán hàng";
            // 
            // mnuTaoHoaDon
            // 
            this.mnuTaoHoaDon.Name = "mnuTaoHoaDon";
            this.mnuTaoHoaDon.Size = new System.Drawing.Size(176, 26);
            this.mnuTaoHoaDon.Text = "Tạo hóa đơn";
            this.mnuTaoHoaDon.Click += new System.EventHandler(this.mnuTaoHoaDon_Click);
            // 
            // mnuLichSu
            // 
            this.mnuLichSu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuXemHoaDon});
            this.mnuLichSu.Name = "mnuLichSu";
            this.mnuLichSu.Size = new System.Drawing.Size(68, 26);
            this.mnuLichSu.Text = "Lịch sử";
            // 
            // mnuXemHoaDon
            // 
            this.mnuXemHoaDon.Name = "mnuXemHoaDon";
            this.mnuXemHoaDon.Size = new System.Drawing.Size(181, 26);
            this.mnuXemHoaDon.Text = "Xem hóa đơn";
            this.mnuXemHoaDon.Click += new System.EventHandler(this.mnuXemHoaDon_Click);
            // 
            // mnuHeThong
            // 
            this.mnuHeThong.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuDangXuat,
            this.mnuThoat});
            this.mnuHeThong.Name = "mnuHeThong";
            this.mnuHeThong.Size = new System.Drawing.Size(85, 26);
            this.mnuHeThong.Text = "Hệ thống";
            // 
            // mnuDangXuat
            // 
            this.mnuDangXuat.Name = "mnuDangXuat";
            this.mnuDangXuat.Size = new System.Drawing.Size(160, 26);
            this.mnuDangXuat.Text = "Đăng xuất";
            this.mnuDangXuat.Click += new System.EventHandler(this.mnuDangXuat_Click);
            // 
            // mnuThoat
            // 
            this.mnuThoat.Name = "mnuThoat";
            this.mnuThoat.Size = new System.Drawing.Size(160, 26);
            this.mnuThoat.Text = "Thoát";
            this.mnuThoat.Click += new System.EventHandler(this.mnuThoat_Click);
            // 
            // lblUserInfo
            // 
            this.lblUserInfo.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblUserInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lblUserInfo.Location = new System.Drawing.Point(0, 528);
            this.lblUserInfo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblUserInfo.Name = "lblUserInfo";
            this.lblUserInfo.Size = new System.Drawing.Size(1067, 26);
            this.lblUserInfo.TabIndex = 1;
            this.lblUserInfo.Text = "UserInfo";
            this.lblUserInfo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblWelcome
            // 
            this.lblWelcome.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblWelcome.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold);
            this.lblWelcome.Location = new System.Drawing.Point(0, 30);
            this.lblWelcome.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblWelcome.Name = "lblWelcome";
            this.lblWelcome.Size = new System.Drawing.Size(1067, 498);
            this.lblWelcome.TabIndex = 2;
            this.lblWelcome.Text = "CHÀO MỪNG ĐẾN VỚI\r\nHỆ THỐNG QUẢN LÝ BÁN HÀNG";
            this.lblWelcome.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 554);
            this.Controls.Add(this.lblWelcome);
            this.Controls.Add(this.lblUserInfo);
            this.Controls.Add(this.menuMain);
            this.MainMenuStrip = this.menuMain;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Quản Lý Bán Hàng";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.menuMain.ResumeLayout(false);
            this.menuMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuMain;
        private System.Windows.Forms.ToolStripMenuItem mnuQuanLy;
        private System.Windows.Forms.ToolStripMenuItem mnuTaiKhoan;
        private System.Windows.Forms.ToolStripMenuItem mnuSanPham;
        private System.Windows.Forms.ToolStripMenuItem mnuKhachHang;
        private System.Windows.Forms.ToolStripMenuItem mnuNhanVien;
        private System.Windows.Forms.ToolStripMenuItem mnuNhaCungCap;
        private System.Windows.Forms.ToolStripMenuItem mnuBanHang;
        private System.Windows.Forms.ToolStripMenuItem mnuTaoHoaDon;
        private System.Windows.Forms.ToolStripMenuItem mnuLichSu;
        private System.Windows.Forms.ToolStripMenuItem mnuXemHoaDon;
        private System.Windows.Forms.ToolStripMenuItem mnuHeThong;
        private System.Windows.Forms.ToolStripMenuItem mnuDangXuat;
        private System.Windows.Forms.ToolStripMenuItem mnuThoat;
            private System.Windows.Forms.Label lblUserInfo;
        private System.Windows.Forms.Label lblWelcome;
    }
}
