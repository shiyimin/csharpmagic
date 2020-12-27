# dotnet core编译方法

**确保C.cs的版本号是：2.0**
csc /t:library C.cs
csc /t:library /r:C.dll B.cs
**修改C.cs的版本号为：1.0**
csc /t:library C.cs
csc /t:library /r:B.dll /r:C.dll A.cs
dotnet ReflectionCall.exe A.dll

# dotnet framework编译方法
sn -k key.ppk
**确保C.cs的版本号是：2.0**
csc /t:library /keyfile:key.ppk C.cs
csc /t:library /r:C.dll B.cs
**修改C.cs的版本号为：1.0**
csc /t:library /keyfile:key.ppk C.cs
csc /t:library /r:B.dll /r:C.dll A.cs
dotnet ReflectionCall.exe A.dll

# 调查方法
export COREHOST_TRACE=1
dotnet ReflectionCall.exe A.dll 2> load.log 

