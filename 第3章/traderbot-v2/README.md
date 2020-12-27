# 重新创建工程和添加依赖包

```
dotnet new console
dotnet add package Microsoft.Extensions.Configuration
dotnet add package Microsoft.Extensions.Configuration.FileExtensions
dotnet add package Microsoft.Extensions.Configuration.Json
dotnet add package Microsoft.Extensions.Configuration.EnvironmentVariables
dotnet add package Microsoft.Extensions.Configuration.CommandLine
dotnet add package Microsoft.Extensions.Configuration.Binder
dotnet add package Microsoft.Extensions.DependencyInjection
dotnet add package Microsoft.Extensions.Logging
dotnet add package Microsoft.Extensions.Logging.Console
dotnet add package Serilog
dotnet add package Serilog.Sinks.File
dotnet add package Serilog.Extensions.Logging
```

# 编译

```
dotnet build
```

# 运行

```
TRADE_OPTION__APIKEY=905ec232-1hrfj6yhgg-6db5e875-62911 TRADE_OPTION__APISECRET=648cd3c1-1c9cf0d9-d403a63f-38fbb dotnet run -s hbm --option:quote BTC --option:coin ETH --option:price 0.01 --option:volumn 1
```