# 运行说明

1. 先启动工作进程，可以多打开几个窗口启动进程

```
cd consumer
dotnet run
```

2. 启动发布者

```
cd publisher
dotnet run
```

3. 启动结果收集进程

```
cd collector
dotnet run
```

4. 所有进程启动好之后，找到发布者进程，敲回车键启动生产过程