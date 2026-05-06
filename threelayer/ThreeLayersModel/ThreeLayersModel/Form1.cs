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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        BUS_qlsv qlsv = new BUS_qlsv();
        private Boolean checkInput()
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
        private void getGridSinhvien()
        {
            DataViewManager dvm = qlsv.getDataset().DefaultViewManager;
            dtgdanhsach.DataSource = dvm;
            dtgdanhsach.DataMember = "sinhvien";
            dtgdanhsach.Columns["masv"].HeaderText = "Mã SV";
            dtgdanhsach.Columns["hoten"].HeaderText = "Họ và tên";
            dtgdanhsach.Columns["gioitinh"].HeaderText = "Giới tính";
            dtgdanhsach.Columns["ngaysinh"].HeaderText = "Ngày sinh";
            dtgdanhsach.Columns["malop"].HeaderText = "Mã lớp";
        }
        private void getLop()
        {
            cbb_malop.Items.Clear();
            cbb_tenlop.Items.Clear();
            List<Lop> dsLop = qlsv.getDsLop();
            foreach (Lop lp in dsLop)
            {
                cbb_tenlop.Items.Add(lp.Tenlop);
                cbb_malop.Items.Add(lp.Malop);
            }
            //DataTable dt = qlsv.getTableLop(); 
            //foreach(DataRow r in dt.Rows) 
            //{ 
            //    cbb_tenlop.Items.Add(r["tenlop"].ToString()); 
            //    cbb_malop.Items.Add(r["malop"].ToString()); 
            //} 
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            getGridSinhvien();
            getLop();
        }

        private void btn_them_Click(object sender, EventArgs e)
        {
            // code cho sự kiện Click của nút Thêm: 
            if (checkInput())
            {
                Sinhvien s = new Sinhvien();
                s.Masv = txt_masv.Text;
                s.Hoten = txt_tensv.Text;
                if (rdb_nam.Checked)
                {
                    s.Gioitinh = true;
                }
                else
                {
                    s.Gioitinh = false;
                }
                s.Ngaysinh = dtp_ngsinh.Value;
                s.Diachi = txt_diachi.Text;
                s.Malop = cbb_malop.Items[cbb_tenlop.SelectedIndex].ToString();

                Boolean kq = qlsv.add_New_SV(s);
                if (!kq)
                {
                    MessageBox.Show("Thêm mới không thành công. Có thể mã sinh viên đã tồn tại!"); 
                }
            }
            else
            {
                MessageBox.Show("Bạn chưa nhập đủ dữ liệu!");
            }
        }
    }
}
