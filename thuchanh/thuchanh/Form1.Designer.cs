namespace thuchanh
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txt_diachi = new System.Windows.Forms.TextBox();
            this.lb_diachi = new System.Windows.Forms.Label();
            this.grb_gt = new System.Windows.Forms.GroupBox();
            this.rdb_nu = new System.Windows.Forms.RadioButton();
            this.rdb_nam = new System.Windows.Forms.RadioButton();
            this.dtp_ngsinh = new System.Windows.Forms.DateTimePicker();
            this.lb_ngaysinh = new System.Windows.Forms.Label();
            this.txt_tensv = new System.Windows.Forms.TextBox();
            this.lb_tensv = new System.Windows.Forms.Label();
            this.txt_masv = new System.Windows.Forms.TextBox();
            this.lb_masv = new System.Windows.Forms.Label();
            this.cbb_tenlop = new System.Windows.Forms.ComboBox();
            this.lb_tenlop = new System.Windows.Forms.Label();
            this.cbb_malop = new System.Windows.Forms.ComboBox();
            this.btn_xoa = new System.Windows.Forms.Button();
            this.btn_sua = new System.Windows.Forms.Button();
            this.btn_them = new System.Windows.Forms.Button();
            this.txtkeyword = new System.Windows.Forms.TextBox();
            this.lb_timkiem = new System.Windows.Forms.Label();
            this.btntim = new System.Windows.Forms.Button();
            this.dtgdanhsach = new System.Windows.Forms.DataGridView();
            this.bnt_lammoi = new System.Windows.Forms.Button();
            this.grb_gt.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtgdanhsach)).BeginInit();
            this.SuspendLayout();
            // 
            // txt_diachi
            // 
            this.txt_diachi.Location = new System.Drawing.Point(137, 310);
            this.txt_diachi.Name = "txt_diachi";
            this.txt_diachi.Size = new System.Drawing.Size(248, 22);
            this.txt_diachi.TabIndex = 26;
            // 
            // lb_diachi
            // 
            this.lb_diachi.AutoSize = true;
            this.lb_diachi.Location = new System.Drawing.Point(63, 313);
            this.lb_diachi.Name = "lb_diachi";
            this.lb_diachi.Size = new System.Drawing.Size(50, 16);
            this.lb_diachi.TabIndex = 25;
            this.lb_diachi.Text = "Địa chỉ:";
            // 
            // grb_gt
            // 
            this.grb_gt.Controls.Add(this.rdb_nu);
            this.grb_gt.Controls.Add(this.rdb_nam);
            this.grb_gt.Location = new System.Drawing.Point(61, 206);
            this.grb_gt.Name = "grb_gt";
            this.grb_gt.Size = new System.Drawing.Size(308, 69);
            this.grb_gt.TabIndex = 23;
            this.grb_gt.TabStop = false;
            this.grb_gt.Text = "Giới tính";
            // 
            // rdb_nu
            // 
            this.rdb_nu.AutoSize = true;
            this.rdb_nu.Location = new System.Drawing.Point(158, 32);
            this.rdb_nu.Name = "rdb_nu";
            this.rdb_nu.Size = new System.Drawing.Size(45, 20);
            this.rdb_nu.TabIndex = 1;
            this.rdb_nu.TabStop = true;
            this.rdb_nu.Text = "Nữ";
            this.rdb_nu.UseVisualStyleBackColor = true;
            // 
            // rdb_nam
            // 
            this.rdb_nam.AutoSize = true;
            this.rdb_nam.Location = new System.Drawing.Point(43, 32);
            this.rdb_nam.Name = "rdb_nam";
            this.rdb_nam.Size = new System.Drawing.Size(57, 20);
            this.rdb_nam.TabIndex = 0;
            this.rdb_nam.TabStop = true;
            this.rdb_nam.Text = "Nam";
            this.rdb_nam.UseVisualStyleBackColor = true;
            // 
            // dtp_ngsinh
            // 
            this.dtp_ngsinh.CustomFormat = "dd/MM/yyyy";
            this.dtp_ngsinh.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtp_ngsinh.Location = new System.Drawing.Point(137, 143);
            this.dtp_ngsinh.Name = "dtp_ngsinh";
            this.dtp_ngsinh.Size = new System.Drawing.Size(245, 22);
            this.dtp_ngsinh.TabIndex = 22;
            // 
            // lb_ngaysinh
            // 
            this.lb_ngaysinh.AutoSize = true;
            this.lb_ngaysinh.Location = new System.Drawing.Point(43, 146);
            this.lb_ngaysinh.Name = "lb_ngaysinh";
            this.lb_ngaysinh.Size = new System.Drawing.Size(70, 16);
            this.lb_ngaysinh.TabIndex = 21;
            this.lb_ngaysinh.Text = "Ngày sinh:";
            // 
            // txt_tensv
            // 
            this.txt_tensv.Location = new System.Drawing.Point(137, 89);
            this.txt_tensv.Name = "txt_tensv";
            this.txt_tensv.Size = new System.Drawing.Size(245, 22);
            this.txt_tensv.TabIndex = 20;
            // 
            // lb_tensv
            // 
            this.lb_tensv.AutoSize = true;
            this.lb_tensv.Location = new System.Drawing.Point(58, 92);
            this.lb_tensv.Name = "lb_tensv";
            this.lb_tensv.Size = new System.Drawing.Size(55, 16);
            this.lb_tensv.TabIndex = 19;
            this.lb_tensv.Text = "Tên SV:";
            // 
            // txt_masv
            // 
            this.txt_masv.Location = new System.Drawing.Point(137, 35);
            this.txt_masv.Name = "txt_masv";
            this.txt_masv.Size = new System.Drawing.Size(245, 22);
            this.txt_masv.TabIndex = 18;
            // 
            // lb_masv
            // 
            this.lb_masv.AutoSize = true;
            this.lb_masv.Location = new System.Drawing.Point(63, 38);
            this.lb_masv.Name = "lb_masv";
            this.lb_masv.Size = new System.Drawing.Size(50, 16);
            this.lb_masv.TabIndex = 17;
            this.lb_masv.Text = "Mã SV:";
            // 
            // cbb_tenlop
            // 
            this.cbb_tenlop.FormattingEnabled = true;
            this.cbb_tenlop.Location = new System.Drawing.Point(137, 354);
            this.cbb_tenlop.Name = "cbb_tenlop";
            this.cbb_tenlop.Size = new System.Drawing.Size(248, 24);
            this.cbb_tenlop.TabIndex = 28;
            this.cbb_tenlop.SelectedIndexChanged += new System.EventHandler(this.cbb_tenlop_SelectedIndexChanged);
            // 
            // lb_tenlop
            // 
            this.lb_tenlop.AutoSize = true;
            this.lb_tenlop.Location = new System.Drawing.Point(57, 358);
            this.lb_tenlop.Name = "lb_tenlop";
            this.lb_tenlop.Size = new System.Drawing.Size(56, 16);
            this.lb_tenlop.TabIndex = 27;
            this.lb_tenlop.Text = "Tên lớp:";
            // 
            // cbb_malop
            // 
            this.cbb_malop.FormattingEnabled = true;
            this.cbb_malop.Location = new System.Drawing.Point(240, 402);
            this.cbb_malop.Name = "cbb_malop";
            this.cbb_malop.Size = new System.Drawing.Size(145, 24);
            this.cbb_malop.TabIndex = 29;
            // 
            // btn_xoa
            // 
            this.btn_xoa.Location = new System.Drawing.Point(234, 485);
            this.btn_xoa.Name = "btn_xoa";
            this.btn_xoa.Size = new System.Drawing.Size(75, 23);
            this.btn_xoa.TabIndex = 32;
            this.btn_xoa.Text = "Xóa";
            this.btn_xoa.UseVisualStyleBackColor = true;
            this.btn_xoa.Click += new System.EventHandler(this.btn_xoa_Click);
            // 
            // btn_sua
            // 
            this.btn_sua.Location = new System.Drawing.Point(136, 485);
            this.btn_sua.Name = "btn_sua";
            this.btn_sua.Size = new System.Drawing.Size(75, 23);
            this.btn_sua.TabIndex = 31;
            this.btn_sua.Text = "Sửa";
            this.btn_sua.UseVisualStyleBackColor = true;
            this.btn_sua.Click += new System.EventHandler(this.btn_sua_Click);
            // 
            // btn_them
            // 
            this.btn_them.Location = new System.Drawing.Point(38, 485);
            this.btn_them.Name = "btn_them";
            this.btn_them.Size = new System.Drawing.Size(75, 23);
            this.btn_them.TabIndex = 30;
            this.btn_them.Text = "Thêm";
            this.btn_them.UseVisualStyleBackColor = true;
            this.btn_them.Click += new System.EventHandler(this.btn_them_Click);
            // 
            // txtkeyword
            // 
            this.txtkeyword.Location = new System.Drawing.Point(591, 32);
            this.txtkeyword.Name = "txtkeyword";
            this.txtkeyword.Size = new System.Drawing.Size(417, 22);
            this.txtkeyword.TabIndex = 34;
            this.txtkeyword.TextChanged += new System.EventHandler(this.txtkeyword_TextChanged);
            // 
            // lb_timkiem
            // 
            this.lb_timkiem.AutoSize = true;
            this.lb_timkiem.Location = new System.Drawing.Point(481, 35);
            this.lb_timkiem.Name = "lb_timkiem";
            this.lb_timkiem.Size = new System.Drawing.Size(104, 16);
            this.lb_timkiem.TabIndex = 33;
            this.lb_timkiem.Text = "Tìm kiếm họ tên:";
            // 
            // btntim
            // 
            this.btntim.Location = new System.Drawing.Point(1033, 31);
            this.btntim.Name = "btntim";
            this.btntim.Size = new System.Drawing.Size(93, 23);
            this.btntim.TabIndex = 35;
            this.btntim.Text = "Tìm kiếm";
            this.btntim.UseVisualStyleBackColor = true;
            this.btntim.Click += new System.EventHandler(this.btntim_Click);
            // 
            // dtgdanhsach
            // 
            this.dtgdanhsach.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dtgdanhsach.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtgdanhsach.Location = new System.Drawing.Point(442, 74);
            this.dtgdanhsach.Name = "dtgdanhsach";
            this.dtgdanhsach.RowHeadersWidth = 51;
            this.dtgdanhsach.RowTemplate.Height = 24;
            this.dtgdanhsach.Size = new System.Drawing.Size(720, 477);
            this.dtgdanhsach.TabIndex = 36;
            this.dtgdanhsach.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dtgdanhsach_CellEnter);
            // 
            // bnt_lammoi
            // 
            this.bnt_lammoi.Location = new System.Drawing.Point(332, 485);
            this.bnt_lammoi.Name = "bnt_lammoi";
            this.bnt_lammoi.Size = new System.Drawing.Size(75, 23);
            this.bnt_lammoi.TabIndex = 37;
            this.bnt_lammoi.Text = "Làm mới";
            this.bnt_lammoi.UseVisualStyleBackColor = true;
            this.bnt_lammoi.Click += new System.EventHandler(this.bnt_lammoi_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1243, 563);
            this.Controls.Add(this.bnt_lammoi);
            this.Controls.Add(this.dtgdanhsach);
            this.Controls.Add(this.btntim);
            this.Controls.Add(this.txtkeyword);
            this.Controls.Add(this.lb_timkiem);
            this.Controls.Add(this.btn_xoa);
            this.Controls.Add(this.btn_sua);
            this.Controls.Add(this.btn_them);
            this.Controls.Add(this.cbb_malop);
            this.Controls.Add(this.cbb_tenlop);
            this.Controls.Add(this.lb_tenlop);
            this.Controls.Add(this.txt_diachi);
            this.Controls.Add(this.lb_diachi);
            this.Controls.Add(this.grb_gt);
            this.Controls.Add(this.dtp_ngsinh);
            this.Controls.Add(this.lb_ngaysinh);
            this.Controls.Add(this.txt_tensv);
            this.Controls.Add(this.lb_tensv);
            this.Controls.Add(this.txt_masv);
            this.Controls.Add(this.lb_masv);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.grb_gt.ResumeLayout(false);
            this.grb_gt.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtgdanhsach)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox txt_diachi;
        private System.Windows.Forms.Label lb_diachi;
        private System.Windows.Forms.GroupBox grb_gt;
        private System.Windows.Forms.RadioButton rdb_nu;
        private System.Windows.Forms.RadioButton rdb_nam;
        private System.Windows.Forms.DateTimePicker dtp_ngsinh;
        private System.Windows.Forms.Label lb_ngaysinh;
        private System.Windows.Forms.TextBox txt_tensv;
        private System.Windows.Forms.Label lb_tensv;
        private System.Windows.Forms.TextBox txt_masv;
        private System.Windows.Forms.Label lb_masv;
        private System.Windows.Forms.ComboBox cbb_tenlop;
        private System.Windows.Forms.Label lb_tenlop;
        private System.Windows.Forms.ComboBox cbb_malop;
        private System.Windows.Forms.Button btn_xoa;
        private System.Windows.Forms.Button btn_sua;
        private System.Windows.Forms.Button btn_them;
        private System.Windows.Forms.TextBox txtkeyword;
        private System.Windows.Forms.Label lb_timkiem;
        private System.Windows.Forms.Button btntim;
        private System.Windows.Forms.DataGridView dtgdanhsach;
        private System.Windows.Forms.Button bnt_lammoi;
    }
}

