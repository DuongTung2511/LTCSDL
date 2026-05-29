using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class DAL_sinhvien : DBConnect
    {
        // 1. LẤY DANH SÁCH SINH VIÊN
        public DataTable GetTableSinhVien()
        {
            string sql = @"SELECT sv.masv, sv.hoten, sv.gioitinh, sv.ngaysinh, sv.diachi, sv.malop, l.tenlop 
                           FROM sinhvien sv 
                           INNER JOIN lop l ON sv.malop = l.malop";

            // Khởi tạo đối tượng SqlCommand chứa câu lệnh truy vấn
            SqlCommand cmd = new SqlCommand(sql);
            
            // Gọi hàm hỗ trợ từ DBConnect để lấy dữ liệu ngắn gọn
            return GetDataTable(cmd);
        }

        // 2. THÊM SINH VIÊN SỬ DỤNG COMMAND
        public bool InsertSV(Sinhvien sv)
        {
            // Dùng Parameter (@) để tránh lỗi cú pháp và chống SQL Injection
            string sql = "INSERT INTO sinhvien(masv, hoten, gioitinh, ngaysinh, diachi, malop) " +
                         "VALUES(@masv, @hoten, @gioitinh, @ngaysinh, @diachi, @malop)";
            
            SqlCommand cmd = new SqlCommand(sql);
            // Gán giá trị thực tế cho các tham số
            cmd.Parameters.AddWithValue("@masv", sv.Masv);
            cmd.Parameters.AddWithValue("@hoten", sv.Hoten);
            cmd.Parameters.AddWithValue("@gioitinh", sv.Gioitinh);
            cmd.Parameters.AddWithValue("@ngaysinh", sv.Ngaysinh);
            cmd.Parameters.AddWithValue("@diachi", sv.Diachi);
            cmd.Parameters.AddWithValue("@malop", sv.Malop);

            return ExecuteNonQuery(cmd);
        }

        // 3. SỬA SINH VIÊN SỬ DỤNG COMMAND
        public bool UpdateSV(Sinhvien sv)
        {
            string sql = "UPDATE sinhvien SET hoten=@hoten, gioitinh=@gioitinh, ngaysinh=@ngaysinh, " +
                         "diachi=@diachi, malop=@malop WHERE masv=@masv";
            
            SqlCommand cmd = new SqlCommand(sql);
            cmd.Parameters.AddWithValue("@masv", sv.Masv);
            cmd.Parameters.AddWithValue("@hoten", sv.Hoten);
            cmd.Parameters.AddWithValue("@gioitinh", sv.Gioitinh);
            cmd.Parameters.AddWithValue("@ngaysinh", sv.Ngaysinh);
            cmd.Parameters.AddWithValue("@diachi", sv.Diachi);
            cmd.Parameters.AddWithValue("@malop", sv.Malop);

            return ExecuteNonQuery(cmd);
        }

        // 4. XÓA SINH VIÊN SỬ DỤNG COMMAND
        public bool DeleteSV(string masv)
        {
            string sql = "DELETE FROM sinhvien WHERE masv=@masv";
            SqlCommand cmd = new SqlCommand(sql);
            cmd.Parameters.AddWithValue("@masv", masv);

            return ExecuteNonQuery(cmd);
        }

        // 5. TÌM KIẾM SINH VIÊN SỬ DỤNG COMMAND
        public DataTable SearchSV(string keyword)
        {
            string sql = @"SELECT sv.masv, sv.hoten, sv.gioitinh, sv.ngaysinh, sv.diachi, sv.malop, l.tenlop 
                           FROM sinhvien sv 
                           INNER JOIN lop l ON sv.malop = l.malop 
                           WHERE sv.hoten LIKE @keyword";

            SqlCommand cmd = new SqlCommand(sql);
            // Thêm '%' để tìm kiếm theo dạng chứa từ khóa
            cmd.Parameters.AddWithValue("@keyword", "%" + keyword + "%");

            return GetDataTable(cmd);
        }
    }
}