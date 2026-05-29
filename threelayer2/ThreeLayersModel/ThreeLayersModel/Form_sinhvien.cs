using BUS;
using DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.LinkLabel;

namespace ThreeLayersModel
{
    public partial class Form_sinhvien : Form
    {
        BUS_SinhVien busSV = new BUS_SinhVien();
        BUS_Lop busLop = new BUS_Lop();
        public Form_sinhvien()
        {
            InitializeComponent();
        }
        BUS_SinhVien qlsv = new BUS_SinhVien();

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadDataSV();
            LoadComboBoxLop();
        }

        private void LoadDataSV()
        {
            dtgdanhsach.DataSource = busSV.GetSinhVien();

            if (dtgdanhsach.Columns.Count > 0)
            {
                dtgdanhsach.Columns["masv"].Width = 60;
                dtgdanhsach.Columns["masv"].HeaderText = "Mã SV";

                dtgdanhsach.Columns["hoten"].HeaderText = "Họ và tên";

                dtgdanhsach.Columns["gioitinh"].Width = 40;
                dtgdanhsach.Columns["gioitinh"].HeaderText = "Giới tính";

                dtgdanhsach.Columns["ngaysinh"].Width = 70;
                dtgdanhsach.Columns["ngaysinh"].HeaderText = "Ngày sinh";

                dtgdanhsach.Columns["diachi"].Width = 100;
                dtgdanhsach.Columns["diachi"].HeaderText = "Địa chỉ";

                dtgdanhsach.Columns["malop"].Visible = false;
                dtgdanhsach.Columns["tenlop"].Width = 150;
                dtgdanhsach.Columns["tenlop"].HeaderText = "Tên lớp";

                dtgdanhsach.ReadOnly = true;
            }
        }

        private void LoadComboBoxLop()
        {
            DataTable dtLop = busLop.GetLop();

            // 1. Đổ dữ liệu cho ComboBox Tên lớp
            cbb_tenlop.DataSource = dtLop;
            cbb_tenlop.DisplayMember = "tenlop"; // Hiện tên
            cbb_tenlop.ValueMember = "malop";    // Chạy ngầm mã

            // 2. Đổ dữ liệu cho ComboBox Mã lớp
            cbb_malop.DataSource = dtLop.Copy();
            cbb_malop.DisplayMember = "malop";   // Hiện mã
            cbb_malop.ValueMember = "malop";
        }

        private bool CheckInput()
        {
            Boolean kq = true;
            if (txt_masv.Text == "")
            {
                kq = false;
                txt_masv.Focus();
            }
            else if (txt_tensv.Text == "")
            {
                kq = false;
                txt_tensv.Focus();
            }
            else if (txt_diachi.Text == "")
            {
                kq = false;
                txt_diachi.Focus();
            }
            else if (cbb_tenlop.SelectedIndex < 0)
            {
                kq = false;
                cbb_tenlop.Focus();
            }
            return kq;
        }
        private void btn_them_Click(object sender, EventArgs e)
        {
            if (!CheckInput()) return;

            Sinhvien s = new Sinhvien
            {
                Masv = txt_masv.Text,
                Hoten = txt_tensv.Text,
                Gioitinh = rdb_nam.Checked,
                Ngaysinh = dtp_ngsinh.Value,
                Diachi = txt_diachi.Text,
                Malop = cbb_tenlop.SelectedValue.ToString() // Lấy ValueMember rất tiện
            };

            if (busSV.ThemSV(s))
            {
                MessageBox.Show("Thêm thành công!");
                LoadDataSV();
            }
            else MessageBox.Show("Thêm thất bại (Trùng mã SV hoặc lỗi hệ thống)!");
        }

        
        private void dtgdanhsach_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dtgdanhsach.Rows.Count) return;
            if (dtgdanhsach.Rows[e.RowIndex].IsNewRow) return;

            DataGridViewRow row = dtgdanhsach.Rows[e.RowIndex];

            txt_masv.Text = row.Cells["masv"].Value.ToString();
            txt_tensv.Text = row.Cells["hoten"].Value.ToString();

            bool gt = false;
            if (row.Cells["gioitinh"].Value != DBNull.Value)
            {
                gt = Convert.ToBoolean(row.Cells["gioitinh"].Value);
            }
            rdb_nam.Checked = gt;
            rdb_nu.Checked = !gt;

            if (row.Cells["ngaysinh"].Value != DBNull.Value)
            {
                dtp_ngsinh.Value = Convert.ToDateTime(row.Cells["ngaysinh"].Value);
            }

            txt_diachi.Text = row.Cells["diachi"].Value.ToString();

            string maLop = row.Cells["malop"].Value.ToString();
            cbb_tenlop.SelectedValue = maLop;
            cbb_malop.SelectedValue = maLop;
        }

        private void btn_sua_Click(object sender, EventArgs e)
        {
            if (!CheckInput()) return;

            Sinhvien s = new Sinhvien
            {
                Masv = txt_masv.Text, // Khóa chính không sửa
                Hoten = txt_tensv.Text,
                Gioitinh = rdb_nam.Checked,
                Ngaysinh = dtp_ngsinh.Value,
                Diachi = txt_diachi.Text,
                Malop = cbb_tenlop.SelectedValue.ToString()
            };

            if (busSV.SuaSV(s))
            {
                MessageBox.Show("Sửa thành công!");
                LoadDataSV();
            }
            else MessageBox.Show("Sửa thất bại!");
        }

        private void btn_xoa_Click(object sender, EventArgs e)
        {
            string masv = txt_masv.Text;
            if (string.IsNullOrEmpty(masv)) return;

            if (MessageBox.Show("Chắc chắn xoá?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (busSV.XoaSV(masv))
                {
                    MessageBox.Show("Xoá thành công!");
                    LoadDataSV();
                }
                else MessageBox.Show("Xoá thất bại!");
            }
        }

        private void bnt_lammoi_Click(object sender, EventArgs e)
        {
            lammoi();
        }
        private void lammoi()
        {
            txt_masv.Clear();
            txt_tensv.Clear();
            txt_diachi.Clear();
            txtkeyword.Clear();
            dtp_ngsinh.Value = DateTime.Now;
            rdb_nam.Checked = false;
            rdb_nu.Checked = false;
            cbb_tenlop.SelectedIndex = -1;
            cbb_malop.SelectedIndex = -1;
            dtgdanhsach.ClearSelection();
            txt_masv.Focus();
        }
        private void filter_dssv()
        {
            dtgdanhsach.DataSource = busSV.TimKiemSV(txtkeyword.Text);
        }

        private void txtkeyword_TextChanged(object sender, EventArgs e)
        {
            filter_dssv();
        }

        private void btn_dslop_Click(object sender, EventArgs e)
        {
            Form_Lop frmLop = new Form_Lop();

            // Mở Form Lớp lên và chờ người dùng thao tác xong...
            frmLop.ShowDialog();

            // KHI NGƯỜI DÙNG TẮT FORM LỚP, CODE SẼ CHẠY XUỐNG ĐÂY:
            // Gọi lại 2 hàm này để nạp lại dữ liệu mới nhất từ Database lên Form Sinh Viên
            LoadDataSV();       // Cập nhật lại Grid Sinh viên (vì sinh viên thuộc lớp bị xóa đã mất)
            LoadComboBoxLop();
        }

        private void cbb_tenlop_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbb_tenlop.SelectedValue != null && cbb_malop.DataSource != null)
            {
                // Ép Mã lớp chọn đúng cái giá trị (ValueMember) mà Tên lớp đang giữ
                cbb_malop.SelectedValue = cbb_tenlop.SelectedValue;
            }
        }
    }
}
