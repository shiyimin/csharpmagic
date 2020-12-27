using System;
using System.Data.SqlClient;

namespace adonet_basic
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new SqlConnectionStringBuilder();
            builder.DataSource = ".\\sqlexpress";
            builder.IntegratedSecurity = true;
            builder.InitialCatalog = "AdventureWorks";

            SqlConnection conn = null;
            try
            // using (var conn = new SqlConnection(builder.ConnectionString))
            {
                conn = new SqlConnection(builder.ConnectionString);
                var cmd = new SqlCommand("EXEC uspGetManagerEmployees @eid", conn);
                Console.Write("请输入要查询的经理Id：");
                var eid = Console.ReadLine();
                cmd.Parameters.AddWithValue("@eid", eid.Trim());

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                Console.WriteLine("递归层级\t组织节点\t经理姓\t经理名\t经理Id\t员工姓\t员工名");
                while (reader.Read())
                {
                    Console.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}",
                        reader[0], reader[1], reader[2], reader[3], reader[4], reader[5], reader[6]);
                }

                reader.Close();
                
                cmd = new SqlCommand("SELECT * FROM Production.Product WHERE ProductNumber LIKE @pn", conn);
                Console.Write("请输入要查询的产品前缀：");
                var prefix = Console.ReadLine();
                cmd.Parameters.AddWithValue("@pn", $"{prefix}%");

                reader = cmd.ExecuteReader();
                while (reader.Read())
                    Console.WriteLine("{0}\t{1}\t{2}", reader[0], reader[1], reader[2]);

                reader.Close();
            }
            finally
            {
                conn.Dispose();
            }
        }
    }
}
