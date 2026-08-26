using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

public class JReXPatcher {
    public static void Main(string[] args) {
        if (args.Length < 2) {
            Console.WriteLine(""Usage: JReXPatcher.exe <input_dll> <output_dll>"");
            return;
        }

        try {
            var asm = AssemblyDefinition.ReadAssembly(args[0]);
            var pad = asm.MainModule.Types.FirstOrDefault(t => t.Name == ""ProtectedPad"");
            if (pad != null) {
                // 1. Bypass Offline Activation Check
                var mVerify = pad.Methods.FirstOrDefault(m => m.Name == ""verifyPaidVersion"");
                if (mVerify != null) {
                    mVerify.Body.Instructions.Clear();
                    mVerify.Body.ExceptionHandlers.Clear();
                    var il = mVerify.Body.GetILProcessor();
                    il.Emit(OpCodes.Ldc_I4_2);
                    il.Emit(OpCodes.Ret);
                }

                // 2. Bypass Activation Process Coroutine
                var mAct = pad.Methods.FirstOrDefault(m => m.Name == ""activationProcess"");
                if (mAct != null) {
                    mAct.Body.Instructions.Clear();
                    mAct.Body.ExceptionHandlers.Clear();
                    var il = mAct.Body.GetILProcessor();
                    il.Emit(OpCodes.Ldnull);
                    il.Emit(OpCodes.Ret);
                }

                // 3. Fix Overheat / Engine Physics Properties (Return Defaults)
                foreach (var m in pad.Methods) {
                    if (m.Name.StartsWith(""get_"")) {
                        if (m.ReturnType.Name == ""Single"" || m.ReturnType.Name == ""Int32"") {
                            m.Body.Instructions.Clear();
                            m.Body.ExceptionHandlers.Clear();
                            var il = m.Body.GetILProcessor();
                            if (m.ReturnType.Name == ""Single"") {
                                il.Emit(OpCodes.Ldc_R4, 1.0f);
                            } else {
                                il.Emit(OpCodes.Ldc_I4_1);
                            }
                            il.Emit(OpCodes.Ret);
                        }
                    }
                }

                // 4. Bypass Store Transactions (Local Buy/Sell)
                var f_sDW = pad.Fields.FirstOrDefault(f => f.Name == ""sDW"");
                var f_RDU = pad.Fields.FirstOrDefault(f => f.Name == ""RDU"");

                var mBuy = pad.Methods.FirstOrDefault(m => m.Name == ""buyItem"");
                if (mBuy != null && f_sDW != null && f_RDU != null) {
                    mBuy.Body.Instructions.Clear();
                    mBuy.Body.ExceptionHandlers.Clear();
                    var il = mBuy.Body.GetILProcessor();
                    il.Emit(OpCodes.Ldarg_0);
                    il.Emit(OpCodes.Ldfld, f_sDW);
                    il.Emit(OpCodes.Ldarg_1);
                    il.Emit(OpCodes.Ldelem_Ref);
                    il.Emit(OpCodes.Ldarg_2);
                    il.Emit(OpCodes.Ldc_I4, (int)'b');
                    il.Emit(OpCodes.Stelem_I2);
                    il.Emit(OpCodes.Ldarg_0);
                    il.Emit(OpCodes.Ldc_I4_1);
                    il.Emit(OpCodes.Stfld, f_RDU);
                    il.Emit(OpCodes.Ldc_I4_0);
                    il.Emit(OpCodes.Ret);
                }

                var mSell = pad.Methods.FirstOrDefault(m => m.Name == ""sellItem"");
                if (mSell != null && f_sDW != null && f_RDU != null) {
                    mSell.Body.Instructions.Clear();
                    mSell.Body.ExceptionHandlers.Clear();
                    var il = mSell.Body.GetILProcessor();
                    il.Emit(OpCodes.Ldarg_0);
                    il.Emit(OpCodes.Ldfld, f_sDW);
                    il.Emit(OpCodes.Ldarg_1);
                    il.Emit(OpCodes.Ldelem_Ref);
                    il.Emit(OpCodes.Ldarg_2);
                    il.Emit(OpCodes.Ldc_I4, (int)'u');
                    il.Emit(OpCodes.Stelem_I2);
                    il.Emit(OpCodes.Ldarg_0);
                    il.Emit(OpCodes.Ldc_I4_1);
                    il.Emit(OpCodes.Stfld, f_RDU);
                    il.Emit(OpCodes.Ldc_I4_0);
                    il.Emit(OpCodes.Ret);
                }
            }
            
            asm.Write(args[1]);
            Console.WriteLine(""Success!"");
        } catch (Exception e) {
            Console.WriteLine(e);
        }
    }
}
