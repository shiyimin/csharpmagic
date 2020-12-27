# 启动说明

1. 首先启动ROUTER端：

```
cd router
dotnet run
```

2. 在两个窗口各启动一个DEALER端，参数分别是BTC和ETH：

```
cd dealer
dotnet run BTC
dotnet run ETH
```

3. Dealer端启动后，在Route端按回车开始发送消息