using System;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class DBContext
    {
        protected SqlConnection conn = new SqlConnection(Properties.Settings.Default.strconnect);
    }
}
