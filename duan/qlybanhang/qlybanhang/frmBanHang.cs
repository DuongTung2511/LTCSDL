using System;
using System.Data;
using System.Windows.Forms;
using BUS;

namespace qlybanhang
{
    public partial class frmBanHang : Form
    {
        KhachHangBUS khBus = new KhachHangBUS();
        SanPhamBUS spBus = new SanPhamBUS();
        HoaDonBUS hdBus = new HoaDonBUS();

        public string MaNV { get; set; }
        private DataTable gioHang;

        public frmBanHang()
        {
            InitializeComponent();
        }

        private void frmBanHang_Load(object sender, EventArgs e)
        {
            // Load Khách Hàng (chỉ hiển thị khách hàng có trạng thái = 1)
            DataRow[] activeKHs = khBus.getFilter_KH("TrangThai = 1 OR TrangThai IS NULL");
            DataTable dtKH = khBus.getTableKhachHang().Clone();
            if (activeKHs.Length > 0) dtKH = activeKHs.CopyToDataTable();
            
            cboKhachHang.DataSource = dtKH;
            cboKhachHang.DisplayMember = "TenKH";
            cboKhachHang.ValueMember = "MaKH";

            // Load Sản Phẩm
            LoadDataSanPham();

            KhoiTaoGioHang();
            LoadmaHD();
        }

        private void KhoiTaoGioHang()
        {
            gioHang = new DataTable();
            gioHang.Columns.Add("MaSP", typeof(string));
            gioHang.Columns.Add("TenSP", typeof(string));
            gioHang.Columns.Add("SoLuong", typeof(int));
            gioHang.Columns.Add("DonGia", typeof(decimal));
            gioHang.Columns.Add("ThanhTien", typeof(decimal));
            dgvGioHang.DataSource = gioHang;
            
            if (dgvGioHang.Columns.Count > 0)
            {
                dgvGioHang.Columns["MaSP"].HeaderText = "Mã SP";
                dgvGioHang.Columns["TenSP"].HeaderText = "Tên sản phẩm";
                dgvGioHang.Columns["SoLuong"].HeaderText = "Số lượng";
                dgvGioHang.Columns["DonGia"].HeaderText = "Đơn giá";
                dgvGioHang.Columns["ThanhTien"].HeaderText = "Thành tiền";
            }
            dgvGioHang.ReadOnly = true;
        }

        private void LoadDataSanPham()
        {
            filter_dssp();

            if (dgvSanPham.Columns.Count > 0)
            {
                if (dgvSanPham.Columns.Contains("MaSP")) dgvSanPham.Columns["MaSP"].HeaderText = "Mã SP";
                if (dgvSanPham.Columns.Contains("TenSP")) dgvSanPham.Columns["TenSP"].HeaderText = "Tên sản phẩm";
                if (dgvSanPham.Columns.Contains("MaNCC")) dgvSanPham.Columns["MaNCC"].HeaderText = "Mã NCC";
                if (dgvSanPham.Columns.Contains("GiaBan")) dgvSanPham.Columns["GiaBan"].HeaderText = "Giá bán";
                if (dgvSanPham.Columns.Contains("SoLuongTon")) dgvSanPham.Columns["SoLuongTon"].HeaderText = "Số lượng tồn";
            }
            dgvSanPham.ReadOnly = true;
        }

        private void LoadmaHD()
        {
            string maHD = hdBus.LayNextMaHD();
            txtMaHD.Text = maHD;
        }

        private void filter_dssp()
        {
            // Lấy danh sách Nhà cung cấp đang giao dịch
            NhaCungCapBUS nccBus = new NhaCungCapBUS();
            DataTable dtNCC = nccBus.LayDanhSachNCCDangHoatDong();
            System.Collections.Generic.List<string> listMaNCC = new System.Collections.Generic.List<string>();
            foreach (DataRow r in dtNCC.Rows)
            {
                listMaNCC.Add("'" + r["MaNCC"].ToString() + "'");
            }
            string inClause = string.Join(",", listMaNCC);
            if (inClause == "") inClause = "''";

            string keyword = txtTimKiemSanPham.Text.Replace("'", "''");
            string strFilter = $"(TenSP LIKE '%{keyword}%') AND (TrangThai = 1 OR TrangThai IS NULL) AND (MaNCC IN ({inClause}))";

            DataRow[] rows = spBus.getFilter_SP(strFilter);
            if (rows.Length > 0)
            {
                dgvSanPham.DataSource = rows.CopyToDataTable();
            }
            else
            {
                dgvSanPham.DataSource = spBus.getTableSanPham().Clone();
            }
        }

        private void txtTimKiemSanPham_TextChanged(object sender, EventArgs e)
        {
            filter_dssp();
        }

        private void btnThemGioHang_Click(object sender, EventArgs e)
        {
            if (dgvSanPham.CurrentRow == null) return;
            DataRowView drvSP = dgvSanPham.CurrentRow.DataBoundItem as DataRowView;
            if (drvSP == null) return;

            string maSPStr = drvSP["MaSP"].ToString();
            string tenSP = drvSP["TenSP"].ToString();
            decimal donGia = Convert.ToDecimal(drvSP["GiaBan"]);
            int soLuongTon = Convert.ToInt32(drvSP["SoLuongTon"]);
            int soLuongThem = (int)nudSoLuong.Value;

            if (soLuongTon < soLuongThem)
            {
                MessageBox.Show("Số lượng tồn không đủ!");
                return;
            }

            // Check if exist
            DataRow[] existing = gioHang.Select("MaSP = '" + maSPStr.Replace("'", "''") + "'");
            if (existing.Length > 0)
            {
                int soLuongHienTai = Convert.ToInt32(existing[0]["SoLuong"]);
                if (soLuongTon < soLuongHienTai + soLuongThem)
                {
                    MessageBox.Show("Số lượng tồn không đủ!");
                    return;
                }
                existing[0]["SoLuong"] = soLuongHienTai + soLuongThem;
                existing[0]["ThanhTien"] = (soLuongHienTai + soLuongThem) * donGia;
            }
            else
            {
                DataRow r = gioHang.NewRow();
                r["MaSP"] = maSPStr;
                r["TenSP"] = tenSP;
                r["SoLuong"] = soLuongThem;
                r["DonGia"] = donGia;
                r["ThanhTien"] = soLuongThem * donGia;
                gioHang.Rows.Add(r);
            }

            CapNhatTongTien();
        }

        private void btnXoaGioHang_Click(object sender, EventArgs e)
        {
            if (dgvGioHang.CurrentRow == null) return;
            DataRowView drvGH = dgvGioHang.CurrentRow.DataBoundItem as DataRowView;
            if (drvGH != null)
            {
                drvGH.Row.Delete();
                gioHang.AcceptChanges();
                CapNhatTongTien();
            }
        }

        private Boolean checkInputThanhToan()
        {
            Boolean kq = true;
            if (gioHang.Rows.Count == 0)
            {
                kq = false;
                MessageBox.Show("Giỏ hàng trống!");
            }
            else if (cboKhachHang.SelectedIndex < 0)
            {
                kq = false;
                cboKhachHang.Focus();
                MessageBox.Show("Vui lòng chọn khách hàng!");
            }
            else if (txtMaHD.Text == "")
            {
                kq = false;
                txtMaHD.Focus();
                MessageBox.Show("Vui lòng nhập mã hóa đơn!");
            }
            return kq;
        }

        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            if (checkInputThanhToan())
            {
                if (!hdBus.MaHD_not_Exist(txtMaHD.Text))
                {
                    MessageBox.Show("Mã hóa đơn đã tồn tại!");
                    txtMaHD.Focus();
                    return;
                }

                try
                {
                    string maKH = cboKhachHang.SelectedValue.ToString();
                    string maHD = txtMaHD.Text.Trim();
                    hdBus.TaoHoaDon(maHD, maKH, MaNV, gioHang);
                    MessageBox.Show("Thanh toán thành công!", "Thông báo");
                    
                    gioHang.Rows.Clear();
                    txtMaHD.Clear();
                    CapNhatTongTien();
                    LoadDataSanPham();
                    LoadmaHD();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message, "Lỗi");
                }
            }
        }

        private void CapNhatTongTien()
        {
            decimal tong = 0;
            foreach (DataRow r in gioHang.Rows)
                tong += Convert.ToDecimal(r["ThanhTien"]);
            lblTongTien.Text = "Tổng tiền: " + tong.ToString("N0") + " VNĐ";
        }
    }
}
