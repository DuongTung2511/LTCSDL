namespace ThreeLayersModel
{
    partial class Form_Lop
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
            this.txt_tenlop = new System.Windows.Forms.TextBox();
            this.txt_malop = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnthemlop = new System.Windows.Forms.Button();
            this.btnsualop = new System.Windows.Forms.Button();
            this.btnxoalop = new System.Windows.Forms.Button();
            this.btnlammoilop = new System.Windows.Forms.Button();
            this.dtglop = new System.Windows.Forms.DataGridView();
            this.btntimlop = new System.Windows.Forms.Button();
            this.txtkeylop = new System.Windows.Forms.TextBox();
            this.lb_timkiemlop = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dtglop)).BeginInit();
            this.SuspendLayout();
            // 
            // txt_tenlop
            // 
            this.txt_tenlop.Location = new System.Drawing.Point(90, 39);
            this.txt_tenlop.Name = "txt_tenlop";
            this.txt_tenlop.Size = new System.Drawing.Size(198, 22);
            this.txt_tenlop.TabIndex = 0;
            // 
            // txt_malop
            // 
            this.txt_malop.Location = new System.Drawing.Point(90, 99);
            this.txt_malop.Name = "txt_malop";
            this.txt_malop.Size = new System.Drawing.Size(198, 22);
            this.txt_malop.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "Tên lớp:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 105);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 16);
            this.label2.TabIndex = 3;
            this.label2.Text = "Mã lớp:";
            // 
            // btnthemlop
            // 
            this.btnthemlop.Location = new System.Drawing.Point(50, 293);
            this.btnthemlop.Name = "btnthemlop";
            this.btnthemlop.Size = new System.Drawing.Size(75, 23);
            this.btnthemlop.TabIndex = 4;
            this.btnthemlop.Text = "Thêm";
            this.btnthemlop.UseVisualStyleBackColor = true;
            this.btnthemlop.Click += new System.EventHandler(this.btnthemlop_Click);
            // 
            // btnsualop
            // 
            this.btnsualop.Location = new System.Drawing.Point(173, 293);
            this.btnsualop.Name = "btnsualop";
            this.btnsualop.Size = new System.Drawing.Size(75, 23);
            this.btnsualop.TabIndex = 5;
            this.btnsualop.Text = "Sửa";
            this.btnsualop.UseVisualStyleBackColor = true;
            this.btnsualop.Click += new System.EventHandler(this.btnsualop_Click);
            // 
            // btnxoalop
            // 
            this.btnxoalop.Location = new System.Drawing.Point(50, 351);
            this.btnxoalop.Name = "btnxoalop";
            this.btnxoalop.Size = new System.Drawing.Size(75, 23);
            this.btnxoalop.TabIndex = 6;
            this.btnxoalop.Text = "Xóa";
            this.btnxoalop.UseVisualStyleBackColor = true;
            this.btnxoalop.Click += new System.EventHandler(this.btnxoalop_Click);
            // 
            // btnlammoilop
            // 
            this.btnlammoilop.Location = new System.Drawing.Point(173, 351);
            this.btnlammoilop.Name = "btnlammoilop";
            this.btnlammoilop.Size = new System.Drawing.Size(75, 23);
            this.btnlammoilop.TabIndex = 7;
            this.btnlammoilop.Text = "Làm mới";
            this.btnlammoilop.UseVisualStyleBackColor = true;
            this.btnlammoilop.Click += new System.EventHandler(this.btnlammoilop_Click);
            // 
            // dtglop
            // 
            this.dtglop.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtglop.Location = new System.Drawing.Point(364, 84);
            this.dtglop.Name = "dtglop";
            this.dtglop.RowHeadersWidth = 51;
            this.dtglop.RowTemplate.Height = 24;
            this.dtglop.Size = new System.Drawing.Size(424, 354);
            this.dtglop.TabIndex = 8;
            this.dtglop.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dtglop_CellEnter);
            // 
            // btntimlop
            // 
            this.btntimlop.Location = new System.Drawing.Point(695, 35);
            this.btntimlop.Name = "btntimlop";
            this.btntimlop.Size = new System.Drawing.Size(93, 23);
            this.btntimlop.TabIndex = 78;
            this.btntimlop.Text = "Tìm kiếm";
            this.btntimlop.UseVisualStyleBackColor = true;
            // 
            // txtkeylop
            // 
            this.txtkeylop.Location = new System.Drawing.Point(471, 35);
            this.txtkeylop.Name = "txtkeylop";
            this.txtkeylop.Size = new System.Drawing.Size(203, 22);
            this.txtkeylop.TabIndex = 77;
            this.txtkeylop.TextChanged += new System.EventHandler(this.txtkeylop_TextChanged);
            // 
            // lb_timkiemlop
            // 
            this.lb_timkiemlop.AutoSize = true;
            this.lb_timkiemlop.Location = new System.Drawing.Point(361, 38);
            this.lb_timkiemlop.Name = "lb_timkiemlop";
            this.lb_timkiemlop.Size = new System.Drawing.Size(87, 16);
            this.lb_timkiemlop.TabIndex = 76;
            this.lb_timkiemlop.Text = "Tìm kiếm lớp:";
            // 
            // Form_Lop
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btntimlop);
            this.Controls.Add(this.txtkeylop);
            this.Controls.Add(this.lb_timkiemlop);
            this.Controls.Add(this.dtglop);
            this.Controls.Add(this.btnlammoilop);
            this.Controls.Add(this.btnxoalop);
            this.Controls.Add(this.btnsualop);
            this.Controls.Add(this.btnthemlop);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txt_malop);
            this.Controls.Add(this.txt_tenlop);
            this.Name = "Form_Lop";
            this.Text = "Form_Lop";
            this.Load += new System.EventHandler(this.Form_Lop_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dtglop)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt_tenlop;
        private System.Windows.Forms.TextBox txt_malop;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnthemlop;
        private System.Windows.Forms.Button btnsualop;
        private System.Windows.Forms.Button btnxoalop;
        private System.Windows.Forms.Button btnlammoilop;
        private System.Windows.Forms.DataGridView dtglop;
        private System.Windows.Forms.Button btntimlop;
        private System.Windows.Forms.TextBox txtkeylop;
        private System.Windows.Forms.Label lb_timkiemlop;
    }
}