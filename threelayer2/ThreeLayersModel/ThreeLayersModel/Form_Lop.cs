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

namespace ThreeLayersModel
{
    public partial class Form_Lop : Form
    {
        BUS_Lop busLop = new BUS_Lop();
        string maLopCu = "";
        public Form_Lop()
        {
            InitializeComponent();
        }

        private void Form_Lop_Load(object sender, EventArgs e)
        {
            LoadDataLop();
        }
        private void LoadDataLop()
        {
            dtglop.DataSource = busLop.GetLop();

            // 2. Kiểm tra an toàn và định dạng tiêu đề, kích thước cột
            if (dtglop.Columns.Count > 0)
            {
                dtglop.Columns["malop"].Width = 100;
                dtglop.Columns["malop"].HeaderText = "Mã lớp";

                // Cột tên lớp thường dài hơn nên cho chiều rộng lớn hơn
                dtglop.Columns["tenlop"].Width = 250;
                dtglop.Columns["tenlop"].HeaderText = "Tên lớp";

                // Chỉ cho phép đọc, không cho sửa trực tiếp trên lưới
                dtglop.ReadOnly = true;
            }
        }
        private bool CheckInput()
        {
            if (string.IsNullOrWhiteSpace(txt_malop.Text))
            {
                MessageBox.Show("Vui lòng nhập mã lớp!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_malop.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txt_tenlop.Text))
            {
                MessageBox.Show("Vui lòng nhập tên lớp!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_tenlop.Focus();
                return false;
            }
            return true;
        }

        private void ClearInputs()
        {
            txt_malop.Clear();
            txt_tenlop.Clear();
            txt_malop.Focus();
        }

        private void btnthemlop_Click(object sender, EventArgs e)
        {
            if (!CheckInput()) return;

            Lop lp = new Lop
            {
                Malop = txt_malop.Text.Trim(),
                Tenlop = txt_tenlop.Text.Trim()
            };

            if (busLop.ThemLop(lp))
            {
                MessageBox.Show("Thêm lớp thành công!", "Thông báo");
                LoadDataLop();
                ClearInputs();
            }
            else
            {
                MessageBox.Show("Thêm thất bại! Mã lớp có thể đã tồn tại.", "Lỗi");
            }
        }

        private void btnsualop_Click(object sender, EventArgs e)
        {
            if (!CheckInput()) return;
            if (string.IsNullOrEmpty(maLopCu))
            {
                MessageBox.Show("Vui lòng chọn lớp cần sửa trên bảng trước!", "Thông báo");
                return;
            }

            Lop lp = new Lop
            {
                Malop = txt_malop.Text.Trim(), // Mã lớp mới (có thể thay đổi)
                Tenlop = txt_tenlop.Text.Trim() // Tên lớp mới
            };

            // Truyền cả mã cũ và đối tượng mới vào tầng BUS
            if (busLop.SuaLop(maLopCu, lp))
            {
                MessageBox.Show("Cập nhật thông tin lớp và đồng bộ sinh viên thành công!", "Thông báo");
                LoadDataLop();
                maLopCu = lp.Malop; // Cập nhật lại mã cũ thành mã vừa sửa để tránh lỗi cho lần bấm tiếp theo
            }
            else
            {
                MessageBox.Show("Sửa thất bại! Mã lớp mới có thể bị trùng.", "Lỗi");
            }
        }

        private void btnxoalop_Click(object sender, EventArgs e)
        {
            string malop = txt_malop.Text.Trim();
            if (string.IsNullOrEmpty(malop))
            {
                MessageBox.Show("Vui lòng chọn lớp cần xoá!", "Cảnh báo");
                return;
            }

            // Đổi lại lời cảnh báo cho rõ ràng vì hành động này sẽ xóa luôn sinh viên
            DialogResult confirm = MessageBox.Show($"CẢNH BÁO: Bạn có chắc chắn muốn xoá lớp '{malop}' không?\nHành động này sẽ XOÁ TOÀN BỘ sinh viên thuộc lớp này!",
                                                   "Xác nhận xóa hệ thống", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirm == DialogResult.Yes)
            {
                if (busLop.XoaLop(malop))
                {
                    MessageBox.Show("Xoá lớp và các sinh viên liên quan thành công!", "Thông báo");
                    LoadDataLop();
                    ClearInputs();
                }
                else
                {
                    MessageBox.Show("Xoá thất bại! Vui lòng thử lại.", "Lỗi");
                }
            }
        }

        private void btnlammoilop_Click(object sender, EventArgs e)
        {
            ClearInputs();
        }

        private void dtglop_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dtglop.Rows.Count) return;
            if (dtglop.Rows[e.RowIndex].IsNewRow) return;
            if (e.RowIndex >= 0 && e.RowIndex < dtglop.Rows.Count - 1) // Bỏ qua dòng trống cuối cùng
            {
                DataGridViewRow row = dtglop.Rows[e.RowIndex];
                txt_malop.Text = row.Cells["malop"].Value.ToString();
                txt_tenlop.Text = row.Cells["tenlop"].Value.ToString();
                maLopCu = txt_malop.Text;
            }
        }

        private void txtkeylop_TextChanged(object sender, EventArgs e)
        {
            dtglop.DataSource = busLop.TimKiemLop(txtkeylop.Text);
        }
    }
}
