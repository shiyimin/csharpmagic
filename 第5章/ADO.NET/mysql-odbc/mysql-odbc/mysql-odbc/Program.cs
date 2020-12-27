// 源码位置：第六章\ADO.NET\mysql-odbc\mysql-odbc\mysql-odbc\Program.cs
// 运行方式：在Visual Studio中启动运行
using System;
using System.Data;
using System.Data.Odbc;

namespace mysql_odbc
{
    static class Program
    {
        static void Main(string[] args)
        {
            using (var conn = new OdbcConnection("Dsn=MySQL"))
            {
                conn.Open();
                /*
                using (var cmd = new OdbcCommand("SELECT * FROM tblCSharp", conn))
                {
                    using (OdbcDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                            Console.WriteLine($"id: {reader.GetInt32(0)}, name: {reader.GetString(1)}");
                    }
                }

                using (var cmd = new OdbcCommand("SELECT COUNT(*) FROM tblCSharp", conn))
                    Console.WriteLine($"tblCSharp表总共有{cmd.ExecuteScalar()}");

                using (var cmd = new OdbcCommand("INSERT INTO tblCSharp (`name`) VALUES ('第一章')", conn))
                    Console.WriteLine($"插入成功的行数：{cmd.ExecuteNonQuery()}");

                using (var cmd = new OdbcCommand("SELECT id, name, chars FROM tblCSharp WHERE id = 6", conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        reader.Read();
                        Console.WriteLine($"名字索引: {reader["chars"]}，数字索引：{reader[2]}");
                        // 下面一句会抛出异常，因为值是空的
                        // Console.WriteLine($"GetInt32(2): {reader.GetInt32(2)}");
                        Console.WriteLine($"reader[2]??: {reader[2] ?? 0}");
                        Console.WriteLine($"Get<int>(2): {reader.Get<int>(2)}");
                        Console.WriteLine($"Get<int>(null, 1): {Get<int>(null, 1)}");
                    }
                }

                // SQL注入攻击
                // 在命令行中输入：6 OR id < 7;
                Console.Write("请输入要查询的id：");
                var id = Console.ReadLine();
                using (var cmd = new OdbcCommand($"SELECT id, name, chars FROM tblCSharp WHERE id > {id}", conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"id: {reader["id"]}");
                        }
                    }
                }

                using (var cmd = new OdbcCommand($"SELECT id, name, chars FROM tblCSharp WHERE id = ? OR `name` = ?", conn))
                {
                    cmd.Parameters.AddWithValue("@id", 1);
                    cmd.Parameters.AddWithValue("@1", "第六章");
                    // cmd.Parameters.AddWithValue("?", "第六章");
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"id: {reader["id"]}, name: {reader["name"]}");
                        }
                    }
                }
                */

                OdbcTransaction transaction = null;
                try
                {
                    transaction = conn.BeginTransaction();
                    using (var cmd1 = new OdbcCommand("INSERT INTO tblCSharp (`name`, chars) VALUES ('第七章', 50000)", conn))
                    // using (var cmd2 = new OdbcCommand("INSERT INTO tblCSharp (id, `name`, chars) VALUES (6, '第六章', 60000)", conn))
                    using (var cmd2 = new OdbcCommand("INSERT INTO tblCSharp (`name`, chars) VALUES ('第六章', 60000)", conn))
                    {
                        cmd1.Transaction = transaction;
                        cmd2.Transaction = transaction;

                        cmd1.ExecuteNonQuery();
                        cmd2.ExecuteNonQuery();
                        transaction.Commit();
                    }

                    Console.WriteLine("提交事务！");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"执行事务发生错误：{e.Message}，回滚事务！");
                    transaction?.Rollback();
                }
            }
        }

        static T Get<T>(this OdbcDataReader reader, int i)
        {
            var value = reader?.GetValue(i);

            return value == null || value == DBNull.Value ? 
                default(T) : (T)value;
        }
    }
}
