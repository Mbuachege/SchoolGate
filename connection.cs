using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace SchoolGate
{
    public class connection
    {
        public static SqlConnection CONN()
        {
            SqlConnection sql = new SqlConnection(Properties.Settings.Default.Conn);
            sql.Open();

            return sql;
        }
    }
}
