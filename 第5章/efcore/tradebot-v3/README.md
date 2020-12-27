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