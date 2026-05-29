using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DBConnect
    {
        protected SqlConnection conn = new SqlConnection(Properties.Settings.Default.strconn);

        // Hàm hỗ trợ lấy dữ liệu (Dành cho các câu lệnh SELECT)
        // Trả về một DataTable chứa dữ liệu
        protected DataTable GetDataTable(SqlCommand cmd)
        {
            cmd.Connection = conn;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        // Hàm hỗ trợ thực thi lệnh (Dành cho INSERT, UPDATE, DELETE)
        // Trả về true nếu thực thi thành công
        protected bool ExecuteNonQuery(SqlCommand cmd)
        {
            try
            {
                cmd.Connection = conn;
                conn.Open();
                int result = cmd.ExecuteNonQuery(); // Trả về số dòng bị ảnh hưởng
                return result > 0;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                // Luôn luôn đóng kết nối sau khi thực thi xong để giải phóng tài nguyên
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }
    }
}
