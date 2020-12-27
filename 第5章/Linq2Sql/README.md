### 构建工程的方法

1. 创建命令行工程：`dotnet new console`
2. 添加依赖包：
   1. `dotnet add package System.Data.SqlClient`
   2. `dotnet add package --version 2.2 Microsoft.EntityFrameworkCore.SqlServer`
   3. `dotnet add package --version 2.2 Microsoft.Extensions.Logging.Console`

### 说明

示例代码采用的数据库是SQL Server的示例数据库**AdventureWorks**，下载地址：https://github.com/microsoft/sql-server-samples/tree/master/samples/databases/adventure-works，建议直接下载 [数据库备份](https://github.com/Microsoft/sql-server-samples/releases/tag/adventureworks) 还原安装。