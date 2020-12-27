### 构建工程的方法

工程使用dotnet 3.0，与dotnet 2.x有很多语法和功能不兼容，请读者升级到dotnet 3.0

1. 创建命令行工程：`dotnet new console`
2. 添加依赖包：
   1. `dotnet add package Microsoft.EntityFrameworkCore`
   2. `dotnet add package Microsoft.EntityFrameworkCore.SqlServer`
   3. `dotnet add package Microsoft.Extensions.Logging.Console`
   4. `dotnet add package Microsoft.Extensions.Configuration`
   5. `dotnet add package Microsoft.Extensions.Configuration.FileExtensions`
   6. `dotnet add package Microsoft.Extensions.Configuration.Json`
   7. `dotnet add package Microsoft.Extensions.DependencyInjection`
3. 生成数据库：
   1. 先在工程里安装数据库处理工具：`dotnet tool install --global dotnet-ef`
   2. 再添加数据库设计工具包：`dotnet add package Microsoft.EntityFrameworkCore.Design`
   3. 在代码中生成数据库构建脚本代码：`dotnet ef migrations add V1`
   4. 生成数据库：`dotnet ef database update`，或者：`dotnet ef database update V1`
   5. 修改表结构：`dotnet ef migrations add V2`
   5. 升级数据库：`dotnet ef database update V2`