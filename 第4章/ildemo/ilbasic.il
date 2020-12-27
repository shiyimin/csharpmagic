.assembly ilbasic {}
.method public static void demo()
{
    .entrypoint
    ldstr "Hello, world"
    call void [mscorlib]System.Console::WriteLine(class System.String)
    ret
}