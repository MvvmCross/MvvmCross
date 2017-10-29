using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;
using static Mono.Cecil.MethodAttributes;

public partial class ModuleWeaver
{
    // The approach used here is similar to the one used for local functions
    private void WeaveInvokeOnMainThread(MethodDefinition method, TypeDefinition containingType)
    {
        //Creates a private method, needed for calling InvokeOnMainThread
        var privateMethodName = $"_{method.Name}_Woven";
        var privateMethod = new MethodDefinition(privateMethodName, HideBySig | Private, ModuleDefinition.TypeSystem.Void);
        containingType.Methods.Add(privateMethod);

        //Moves the instructions from the annotated method into our recently created private method
        var il = method.Body.GetILProcessor();
        var privateIl = privateMethod.Body.GetILProcessor();
        foreach (var instruction in method.Body.Instructions.ToList())
            privateIl.Append(instruction);

        foreach (var variable in il.Body.Variables)
            privateMethod.Body.Variables.Add(variable);

        //Empties the body of the annotated method so that we can add the needed instructions
        il.Body.Variables.Clear();
        il.Body.Instructions.Clear();

        if (IsMvxMainThreadDispatchingObject(containingType))
        {
            //If the object inherits from MvxMainThreadDispatchingObject we can simply call InvokeOnMainThread
            il.Append(il.Create(OpCodes.Nop));
            il.Append(il.Create(OpCodes.Ldarg_0));
            il.Append(il.Create(OpCodes.Ldarg_0));
            il.Append(il.Create(OpCodes.Ldftn, privateMethod));
            il.Append(il.Create(OpCodes.Newobj, _actionConstructor));
            il.Append(il.Create(OpCodes.Call, _invokeOnMainThread));
            il.Append(il.Create(OpCodes.Nop));
            il.Append(il.Create(OpCodes.Ret));
        }
        else
        {
            //In all other cases we need to resolve the IMvxMainThreadDispatcher and then calls RequestMainThreadAction
            var variable = new VariableDefinition(_mainThreadDispatcher);
            method.Body.Variables.Add(variable);

            var nullBranch = il.Create(OpCodes.Ret);
            var nonNullBranch = il.Create(OpCodes.Ldloc_0);

            il.Append(il.Create(OpCodes.Nop));
            il.Append(il.Create(OpCodes.Call, _mvxResolve));
            il.Append(il.Create(OpCodes.Stloc_0));
            il.Append(il.Create(OpCodes.Ldloc_0));
            il.Append(il.Create(OpCodes.Brtrue_S, nonNullBranch));

            il.Append(il.Create(OpCodes.Br_S, nullBranch));

            il.Append(nonNullBranch);
            il.Append(il.Create(OpCodes.Ldarg_0));
            il.Append(il.Create(OpCodes.Ldftn, privateMethod));
            il.Append(il.Create(OpCodes.Newobj, _actionConstructor));
            il.Append(il.Create(OpCodes.Ldc_I4_1));
            il.Append(il.Create(OpCodes.Callvirt, _requestMainThreadAction));
            il.Append(il.Create(OpCodes.Pop));

            il.Append(nullBranch);
        }
    }
}
