.assembly ilpointer {}
.method public hidebysig static void Main() cil managed
{
    .entrypoint
    .locals(int32 *v, int32 j)
    ldloca j
    stloc v
    ldloc v
    ldc.i4.2
    stobj int32
    ldloc j
    call void [mscorlib]System.Console::WriteLine(int32)
    ret
}