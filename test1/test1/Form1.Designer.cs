namespace test1
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
            this.lb_masv = new System.Windows.Forms.Label();
            this.txt_masv = new System.Windows.Forms.TextBox();
            this.txt_tensv = new System.Windows.Forms.TextBox();
            this.lb_tensv = new System.Windows.Forms.Label();
            this.lb_ngaysinh = new System.Windows.Forms.Label();
            this.dtp_ngsinh = new System.Windows.Forms.DateTimePicker();
            this.grb_gt = new System.Windows.Forms.GroupBox();
            this.rdb_nu = new System.Windows.Forms.RadioButton();
            this.rdb_nam = new System.Windows.Forms.RadioButton();
            this.lb_malop = new System.Windows.Forms.Label();
            this.txt_diachi = new System.Windows.Forms.TextBox();
            this.lb_diachi = new System.Windows.Forms.Label();
            this.dtg_sv = new System.Windows.Forms.DataGridView();
            this.btn_them = new System.Windows.Forms.Button();
            this.btn_sua = new System.Windows.Forms.Button();
            this.btn_xoa = new System.Windows.Forms.Button();
            this.btn_refresh = new System.Windows.Forms.Button();
            this.cbb_malop = new System.Windows.Forms.ComboBox();
            this.grb_gt.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtg_sv)).BeginInit();
            this.SuspendLayout();
            // 
            // lb_masv
            // 
            this.lb_masv.AutoSize = true;
            this.lb_masv.Location = new System.Drawing.Point(60, 36);
            this.lb_masv.Name = "lb_masv";
            this.lb_masv.Size = new System.Drawing.Size(50, 16);
            this.lb_masv.TabIndex = 0;
            this.lb_masv.Text = "Mã SV:";
            // 
            // txt_masv
            // 
            this.txt_masv.Location = new System.Drawing.Point(134, 33);
            this.txt_masv.Name = "txt_masv";
            this.txt_masv.Size = new System.Drawing.Size(213, 22);
            this.txt_masv.TabIndex = 1;
            // 
            // txt_tensv
            // 
            this.txt_tensv.Location = new System.Drawing.Point(134, 87);
            this.txt_tensv.Name = "txt_tensv";
            this.txt_tensv.Size = new System.Drawing.Size(213, 22);
            this.txt_tensv.TabIndex = 3;
            // 
            // lb_tensv
            // 
            this.lb_tensv.AutoSize = true;
            this.lb_tensv.Location = new System.Drawing.Point(55, 90);
            this.lb_tensv.Name = "lb_tensv";
            this.lb_tensv.Size = new System.Drawing.Size(55, 16);
            this.lb_tensv.TabIndex = 2;
            this.lb_tensv.Text = "Tên SV:";
            // 
            // lb_ngaysinh
            // 
            this.lb_ngaysinh.AutoSize = true;
            this.lb_ngaysinh.Location = new System.Drawing.Point(40, 144);
            this.lb_ngaysinh.Name = "lb_ngaysinh";
            this.lb_ngaysinh.Size = new System.Drawing.Size(70, 16);
            this.lb_ngaysinh.TabIndex = 4;
            this.lb_ngaysinh.Text = "Ngày sinh:";
            // 
            // dtp_ngsinh
            // 
            this.dtp_ngsinh.CustomFormat = "dd/MM/yyyy";
            this.dtp_ngsinh.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtp_ngsinh.Location = new System.Drawing.Point(134, 141);
            this.dtp_ngsinh.Name = "dtp_ngsinh";
            this.dtp_ngsinh.Size = new System.Drawing.Size(213, 22);
            this.dtp_ngsinh.TabIndex = 5;
            // 
            // grb_gt
            // 
            this.grb_gt.Controls.Add(this.rdb_nu);
            this.grb_gt.Controls.Add(this.rdb_nam);
            this.grb_gt.Location = new System.Drawing.Point(58, 204);
            this.grb_gt.Name = "grb_gt";
            this.grb_gt.Size = new System.Drawing.Size(276, 100);
            this.grb_gt.TabIndex = 6;
            this.grb_gt.TabStop = false;
            this.grb_gt.Text = "Giới tính";
            // 
            // rdb_nu
            // 
            this.rdb_nu.AutoSize = true;
            this.rdb_nu.Location = new System.Drawing.Point(183, 41);
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
            this.rdb_nam.Location = new System.Drawing.Point(74, 41);
            this.rdb_nam.Name = "rdb_nam";
            this.rdb_nam.Size = new System.Drawing.Size(57, 20);
            this.rdb_nam.TabIndex = 0;
            this.rdb_nam.TabStop = true;
            this.rdb_nam.Text = "Nam";
            this.rdb_nam.UseVisualStyleBackColor = true;
            // 
            // lb_malop
            // 
            this.lb_malop.AutoSize = true;
            this.lb_malop.Location = new System.Drawing.Point(570, 37);
            this.lb_malop.Name = "lb_malop";
            this.lb_malop.Size = new System.Drawing.Size(51, 16);
            this.lb_malop.TabIndex = 7;
            this.lb_malop.Text = "Mã lớp:";
            // 
            // txt_diachi
            // 
            this.txt_diachi.Location = new System.Drawing.Point(634, 141);
            this.txt_diachi.Name = "txt_diachi";
            this.txt_diachi.Size = new System.Drawing.Size(178, 22);
            this.txt_diachi.TabIndex = 10;
            // 
            // lb_diachi
            // 
            this.lb_diachi.AutoSize = true;
            this.lb_diachi.Location = new System.Drawing.Point(573, 144);
            this.lb_diachi.Name = "lb_diachi";
            this.lb_diachi.Size = new System.Drawing.Size(50, 16);
            this.lb_diachi.TabIndex = 9;
            this.lb_diachi.Text = "Địa chỉ:";
            // 
            // dtg_sv
            // 
            this.dtg_sv.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dtg_sv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtg_sv.Location = new System.Drawing.Point(3, 392);
            this.dtg_sv.Name = "dtg_sv";
            this.dtg_sv.RowHeadersWidth = 51;
            this.dtg_sv.RowTemplate.Height = 24;
            this.dtg_sv.Size = new System.Drawing.Size(926, 185);
            this.dtg_sv.TabIndex = 11;
            this.dtg_sv.SelectionChanged += new System.EventHandler(this.dtg_sv_SelectionChanged);
            // 
            // btn_them
            // 
            this.btn_them.Location = new System.Drawing.Point(98, 332);
            this.btn_them.Name = "btn_them";
            this.btn_them.Size = new System.Drawing.Size(75, 23);
            this.btn_them.TabIndex = 12;
            this.btn_them.Text = "Thêm";
            this.btn_them.UseVisualStyleBackColor = true;
            this.btn_them.Click += new System.EventHandler(this.btn_them_Click);
            // 
            // btn_sua
            // 
            this.btn_sua.Location = new System.Drawing.Point(298, 332);
            this.btn_sua.Name = "btn_sua";
            this.btn_sua.Size = new System.Drawing.Size(75, 23);
            this.btn_sua.TabIndex = 13;
            this.btn_sua.Text = "Sửa";
            this.btn_sua.UseVisualStyleBackColor = true;
            this.btn_sua.Click += new System.EventHandler(this.btn_sua_Click);
            // 
            // btn_xoa
            // 
            this.btn_xoa.Location = new System.Drawing.Point(498, 332);
            this.btn_xoa.Name = "btn_xoa";
            this.btn_xoa.Size = new System.Drawing.Size(75, 23);
            this.btn_xoa.TabIndex = 14;
            this.btn_xoa.Text = "Xóa";
            this.btn_xoa.UseVisualStyleBackColor = true;
            this.btn_xoa.Click += new System.EventHandler(this.btn_xoa_Click);
            // 
            // btn_refresh
            // 
            this.btn_refresh.Location = new System.Drawing.Point(698, 332);
            this.btn_refresh.Name = "btn_refresh";
            this.btn_refresh.Size = new System.Drawing.Size(75, 23);
            this.btn_refresh.TabIndex = 15;
            this.btn_refresh.Text = "Làm mới";
            this.btn_refresh.UseVisualStyleBackColor = true;
            this.btn_refresh.Click += new System.EventHandler(this.btn_refresh_Click);
            // 
            // cbb_malop
            // 
            this.cbb_malop.FormattingEnabled = true;
            this.cbb_malop.Location = new System.Drawing.Point(634, 33);
            this.cbb_malop.Name = "cbb_malop";
            this.cbb_malop.Size = new System.Drawing.Size(178, 24);
            this.cbb_malop.TabIndex = 16;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(933, 589);
            this.Controls.Add(this.cbb_malop);
            this.Controls.Add(this.btn_refresh);
            this.Controls.Add(this.btn_xoa);
            this.Controls.Add(this.btn_sua);
            this.Controls.Add(this.btn_them);
            this.Controls.Add(this.dtg_sv);
            this.Controls.Add(this.txt_diachi);
            this.Controls.Add(this.lb_diachi);
            this.Controls.Add(this.lb_malop);
            this.Controls.Add(this.grb_gt);
            this.Controls.Add(this.dtp_ngsinh);
            this.Controls.Add(this.lb_ngaysinh);
            this.Controls.Add(this.txt_tensv);
            this.Controls.Add(this.lb_tensv);
            this.Controls.Add(this.txt_masv);
            this.Controls.Add(this.lb_masv);
            this.Name = "Form1";
            this.Text = "Thông tin sinh viên";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.grb_gt.ResumeLayout(false);
            this.grb_gt.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtg_sv)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lb_masv;
        private System.Windows.Forms.TextBox txt_masv;
        private System.Windows.Forms.TextBox txt_tensv;
        private System.Windows.Forms.Label lb_tensv;
        private System.Windows.Forms.Label lb_ngaysinh;
        private System.Windows.Forms.DateTimePicker dtp_ngsinh;
        private System.Windows.Forms.GroupBox grb_gt;
        private System.Windows.Forms.RadioButton rdb_nu;
        private System.Windows.Forms.RadioButton rdb_nam;
        private System.Windows.Forms.Label lb_malop;
        private System.Windows.Forms.TextBox txt_diachi;
        private System.Windows.Forms.Label lb_diachi;
        private System.Windows.Forms.DataGridView dtg_sv;
        private System.Windows.Forms.Button btn_them;
        private System.Windows.Forms.Button btn_sua;
        private System.Windows.Forms.Button btn_xoa;
        private System.Windows.Forms.Button btn_refresh;
        private System.Windows.Forms.ComboBox cbb_malop;
    }
}

