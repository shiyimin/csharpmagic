using System;
using MySql.Data.MySqlClient;

namespace mysql_basic
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var connection = new MySqlConnection(
                "server=localhost;port=3306;database=DemoMysqlDb;user=root;password=Vikvon@123"))
            {
                connection.Open();

                using (var command = new MySqlCommand("SELECT * FROM tblCSharp", connection))
                using (var reader = command.ExecuteReader())
                    while (reader.Read())
                        Console.WriteLine($"id: {reader.GetInt32(0)}, name: {reader.GetString(1)}");
            }
        }
    }
}
