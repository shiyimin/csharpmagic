# 运行说明

1. 可以先启动订阅者，打开一个或多个窗口。

```
cd subscriber
dotnet run TopicA  # 下面几个命令可以随意组合
dotnet run TopicB
dotnet run All
```

2. 启动发布者

```
cd publisher
dotnet run
```