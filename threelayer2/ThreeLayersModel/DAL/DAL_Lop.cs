using DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DAL_Lop: DBConnect
    {
        // 1. LẤY DANH SÁCH LỚP
        public DataTable GetTableLop()
        {
            string sql = "SELECT * FROM lop";
            // Khởi tạo đối tượng SqlCommand chứa câu lệnh truy vấn
            SqlCommand cmd = new SqlCommand(sql);
            
            // Gọi hàm hỗ trợ từ DBConnect để lấy dữ liệu ngắn gọn
            return GetDataTable(cmd);
        }

        // 2. THÊM LỚP
        public bool InsertLop(Lop lp)
        {
            string sql = "INSERT INTO lop(malop, tenlop) VALUES(@malop, @tenlop)";
            SqlCommand cmd = new SqlCommand(sql);
            // Truyền giá trị vào các tham số để chống lỗi cú pháp và SQL Injection
            cmd.Parameters.AddWithValue("@malop", lp.Malop);
            cmd.Parameters.AddWithValue("@tenlop", lp.Tenlop);

            return ExecuteNonQuery(cmd);
        }

        // 3. SỬA LỚP
        public bool UpdateLop(string maLopCu, Lop lp)
        {
            // Lệnh 1: Cập nhật mã lớp mới cho toàn bộ sinh viên đang ở lớp cũ (Không dùng Transaction theo yêu cầu)
            string sqlSV = "UPDATE sinhvien SET malop=@maLopMoi WHERE malop=@maLopCu";
            SqlCommand cmdSV = new SqlCommand(sqlSV);
            cmdSV.Parameters.AddWithValue("@maLopMoi", lp.Malop);
            cmdSV.Parameters.AddWithValue("@maLopCu", maLopCu);
            ExecuteNonQuery(cmdSV); // Có thể không có sinh viên nào bị ảnh hưởng nên không cần kiểm tra kết quả

            // Lệnh 2: Cập nhật mã lớp mới và tên lớp mới cho bảng Lớp
            string sqlLop = "UPDATE lop SET malop=@maLopMoi, tenlop=@tenlop WHERE malop=@maLopCu";
            SqlCommand cmdLop = new SqlCommand(sqlLop);
            cmdLop.Parameters.AddWithValue("@maLopMoi", lp.Malop);
            cmdLop.Parameters.AddWithValue("@tenlop", lp.Tenlop);
            cmdLop.Parameters.AddWithValue("@maLopCu", maLopCu);

            return ExecuteNonQuery(cmdLop);
        }

        // 4. XÓA LỚP
        public bool DeleteLop(string malop)
        {
            // Lệnh 1: Xóa toàn bộ sinh viên thuộc lớp này trước (Không dùng Transaction theo yêu cầu)
            string sqlSV = "DELETE FROM sinhvien WHERE malop=@malop";
            SqlCommand cmdSV = new SqlCommand(sqlSV);
            cmdSV.Parameters.AddWithValue("@malop", malop);
            ExecuteNonQuery(cmdSV);

            // Lệnh 2: Xóa chính lớp này
            string sqlLop = "DELETE FROM lop WHERE malop=@malop";
            SqlCommand cmdLop = new SqlCommand(sqlLop);
            cmdLop.Parameters.AddWithValue("@malop", malop);
            
            return ExecuteNonQuery(cmdLop);
        }

        // 5. TÌM KIẾM LỚP SỬ DỤNG COMMAND
        public DataTable SearchLop(string keyword)
        {
            string sql = "SELECT * FROM lop WHERE malop LIKE @keyword OR tenlop LIKE @keyword";
            SqlCommand cmd = new SqlCommand(sql);
            cmd.Parameters.AddWithValue("@keyword", "%" + keyword + "%");

            return GetDataTable(cmd);
        }
    }
}
