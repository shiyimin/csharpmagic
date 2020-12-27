#! /bin/bash

# 演示直接读取环境变量配置
CSHARP_FROMENV=EnvValue dotnet run

# 演示将前面一个程序的输出保存到环境变量，修改后一个程序的配置
echo "请输入环境变量CSHARP_FROMENV的值，以 CTRL+D 结束："
CSHARP_FROMENV=$(cat -) dotnet run

#
# 下面两行都是在当前shell里设置环境变量，再将dotnet当作子进程启动
# 而dotnet作为子进程并不访问父进程shell里的环境变量
# 
# CSHARP_FROMENV=EnvValue; dotnet run
# CSHARP_FROMENV=EnvValue && dotnet run
