### 构建工程的方法

工程使用dotnet 3.0，与dotnet 2.x有很多语法和功能不兼容，请读者升级到dotnet 3.0

1. 创建命令行工程：`dotnet new console`
2. 添加依赖包：
   1. `dotnet add .\TradeBot.Strategy.Band\TradeBot.Strategy.Band.csproj package Microsoft.EntityFrameworkCore --version 3.0.0`
   2. `dotnet add .\TradeBot.Strategy.Band\TradeBot.Strategy.Band.csproj package Microsoft.EntityFrameworkCore.SqlServer --version 3.0.0`
   3. `dotnet add .\TradeBot.Strategy.Band\TradeBot.Strategy.Band.csproj package Microsoft.Extensions.Logging.Console --version 3.0.0`
   4. `dotnet add .\TradeBot.Strategy.Band\TradeBot.Strategy.Band.csproj package Microsoft.Extensions.Configuration --version 3.0.0`
   5. `dotnet add .\TradeBot.Strategy.Band\TradeBot.Strategy.Band.csproj package Microsoft.Extensions.Configuration.FileExtensions --version 3.0.0`
   6. `dotnet add .\TradeBot.Strategy.Band\TradeBot.Strategy.Band.csproj package Microsoft.Extensions.Configuration.Json --version 3.0.0`
   7. `dotnet add .\TraderBot.Brokers\TraderBot.Brokers.csproj package Microsoft.Extensions.Logging --version 3.0.0`
   8. `dotnet add .\TraderBot.Test\TraderBot.Test.csproj package Microsoft.EntityFrameworkCore.InMemory --version 3.0.0`
   9. `dotnet add .\TraderBot.Test\TraderBot.Test.csproj package Microsoft.Extensions.Logging.Debug --version 3.0.0`
   10. `dotnet add .\TraderBot.Test\TraderBot.Test.csproj package Moq --version 4.13.1`