cd /d "D:\workspace\writing\china-pub\C# Programming Magic\sample-code\��7��\demo\"
dotnet build

pushd TraderBot.Test
dotnet test
popd

pushd TraderBot.Web
dotnet publish -f netcoreapp3.0 -r linux-x64 --self-contained
popd