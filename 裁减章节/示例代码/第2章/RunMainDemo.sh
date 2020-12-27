#!/bin/bash

# dotnet MainDemo.exe
dotnet MainDemo.exe 1 2
# dotnet MainDemo.exe 3 3

case "$?" in
    "1") echo "执行程序的参数错误！";;
    "2") echo "结果是奇数！";;
    "0") echo "执行成功！";;
esac
