/*
using System;

class ManagedPoint
{
    unsafe static void Main()
    {
        int[] iArray = new int[10];
        int *pIter;
        fixed (int *pArray = iArray)
        {
            pIter = pArray;
            for (int i = 0; i < iArray.Length; ++i)
            {
                *pIter++ = i + 1;
            }
        }

        Console.WriteLine(iArray[8]);

        int j = 1;
        RefMethod(ref j);
        Console.WriteLine(j);
    }

    static void RefMethod(ref int i)
    {
        i++;
    }
}
*/
.assembly ilmanagedpointer {}
.custom instance void class [mscorlib]System.Security.UnverifiableCodeAttribute::'.ctor'() =  (01 00 00 00 ) // ....

.class private auto ansi beforefieldinit ManagedPoint
  	extends [mscorlib]System.Object
  {

    // method line 1
    .method private static hidebysig 
           default void Main ()  cil managed 
    {
        // Method begins at RVA 0x2050
	.entrypoint
	// Code size 106 (0x6a)
	.maxstack 3
	.locals init (
		int32[]	V_0,
		int32*	V_1,
		int32	V_2,
		int32*	V_3,
		int32[] pinned	V_4,
		int32	V_5,
		bool	V_6)
	IL_0000:  nop 
	IL_0001:  ldc.i4.s 0x0a
	IL_0003:  newarr [mscorlib]System.Int32
	IL_0008:  stloc.0 
	IL_0009:  ldloc.0 
	IL_000a:  dup 
	IL_000b:  stloc.s 4
	IL_000d:  brfalse.s IL_0015

	IL_000f:  ldloc.s 4
	IL_0011:  ldlen 
	IL_0012:  conv.i4 
	IL_0013:  brtrue.s IL_001a

	IL_0015:  ldc.i4.0 
	IL_0016:  conv.u 
	IL_0017:  stloc.3 
	IL_0018:  br.s IL_0024

	IL_001a:  ldloc.s 4
	IL_001c:  ldc.i4.0 
	IL_001d:  ldelema [mscorlib]System.Int32
	IL_0022:  conv.u 
	IL_0023:  stloc.3 
	IL_0024:  nop 
	IL_0025:  ldloc.3 
	IL_0026:  stloc.1 
	IL_0027:  ldc.i4.0 
	IL_0028:  stloc.s 5
	IL_002a:  br.s IL_003e

	IL_002c:  nop 
	IL_002d:  ldloc.1 
	IL_002e:  dup 
	IL_002f:  ldc.i4.4 
	IL_0030:  add 
	IL_0031:  stloc.1 
	IL_0032:  ldloc.s 5
	IL_0034:  ldc.i4.1 
	IL_0035:  add 
	IL_0036:  stind.i4 
	IL_0037:  nop 
	IL_0038:  ldloc.s 5
	IL_003a:  ldc.i4.1 
	IL_003b:  add 
	IL_003c:  stloc.s 5
	IL_003e:  ldloc.s 5
	IL_0040:  ldloc.0 
	IL_0041:  ldlen 
	IL_0042:  conv.i4 
	IL_0043:  clt 
	IL_0045:  stloc.s 6
	IL_0047:  ldloc.s 6
	IL_0049:  brtrue.s IL_002c

	IL_004b:  nop 
	IL_004c:  ldnull 
	IL_004d:  stloc.s 4
	IL_004f:  ldloc.0 
	IL_0050:  ldc.i4.8 
	IL_0051:  ldelem.i4 
	IL_0052:  call void class [mscorlib]System.Console::WriteLine(int32)
	IL_0057:  nop 
	IL_0058:  ldc.i4.1 
	IL_0059:  stloc.2 
	IL_005a:  ldloca.s 2
	IL_005c:  call void class ManagedPoint::RefMethod([out] int32&)
	IL_0061:  nop 
	IL_0062:  ldloc.2 
	IL_0063:  call void class [mscorlib]System.Console::WriteLine(int32)
	IL_0068:  nop 
	IL_0069:  ret 
    } // end of method ManagedPoint::Main

    // method line 2
    .method private static hidebysig 
           default void RefMethod (int32& i)  cil managed 
    {
        // Method begins at RVA 0x20c6
	// Code size 8 (0x8)
	.maxstack 8
	IL_0000:  nop 
	IL_0001:  ldarg.0 
	IL_0002:  ldarg.0 
	IL_0003:  ldind.i4 
	IL_0004:  ldc.i4.1 
	IL_0005:  add 
	IL_0006:  stind.i4 
	IL_0007:  ret 
    } // end of method ManagedPoint::RefMethod

    // method line 3
    .method public hidebysig specialname rtspecialname 
           instance default void '.ctor' ()  cil managed 
    {
        // Method begins at RVA 0x20cf
	// Code size 8 (0x8)
	.maxstack 8
	IL_0000:  ldarg.0 
	IL_0001:  call instance void object::'.ctor'()
	IL_0006:  nop 
	IL_0007:  ret 
    } // end of method ManagedPoint::.ctor

  } // end of class ManagedPoint

