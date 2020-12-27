/*
using System;

class DemoIlClass
{
    private int IntValue;

    static void Main()
    {
        var dc = new DemoIlClass();
        DemoMethod(dc, 2);
    }

    static void DemoMethod(DemoIlClass dc, int i)
    {
        dc.IntValue += i;
        Console.WriteLine("{0}", dc.IntValue);
    }
}
*/
.assembly extern mscorlib
{
  .ver 4:0:0:0
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 ) // .z\V.4..
}
.assembly ilmethodcall {}
.class private auto ansi beforefieldinit DemoIlClass
  	extends [mscorlib]System.Object
{
    .field  private  int32 IntValue

    .method private static hidebysig 
           default void Main ()  cil managed 
    {
        .entrypoint
        .locals init (class DemoIlClass	dc)
        newobj instance void class DemoIlClass::'.ctor'()
        stloc dc
        ldloc.0 
        ldc.i4.2 
        call void class DemoIlClass::DemoMethod(class DemoIlClass, int32)
        ret 
    } 

    .method private static hidebysig 
           default void DemoMethod (class DemoIlClass dc, int32 i)  cil managed 
    {
        ldarg dc 
        dup 
        ldfld int32 DemoIlClass::IntValue
        ldarg i
        add 
        stfld int32 DemoIlClass::IntValue
        ldstr "{0}"
        ldarg.0 
        ldfld int32 DemoIlClass::IntValue
        box [mscorlib]System.Int32
        call void class [mscorlib]System.Console::WriteLine(string, object)
        ret 
    } 

    .method public hidebysig specialname rtspecialname 
           instance default void '.ctor' ()  cil managed 
    {
        .maxstack 8
        ldarg.0 
        call instance void object::'.ctor'()
        ret 
    }
}

