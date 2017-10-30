using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Mono.Cecil;

public partial class ModuleWeaver
{
    public ModuleDefinition ModuleDefinition { get; set; }

    public void Execute()
    {
        Debug.WriteLine("Weaving file: " + ModuleDefinition.FileName);

        InitializeDefinitions();

        var typesToWeave =
            ModuleDefinition
                .GetTypes()
                .SelectMany(MethodsToWeave)
                .GroupBy(method => method.DeclaringType);

        foreach (var grouping in typesToWeave)
        {
            var declaringType = grouping.Key;
            
            foreach (var method in grouping.Where(ShouldWeave))
            {
                WeaveInvokeOnMainThread(method, declaringType);

                //Removes the reference to the RunOnMainThread attribute
                method.CustomAttributes.Remove(method.CustomAttributes
                    .First(a => a.AttributeType.Name == RunOnMainThreadAttributeTypeName));
            }
        }
    }

    private AssemblyNameReference FindAssembly(string assemblyName)
        => ModuleDefinition.AssemblyReferences.SingleOrDefault(a => a.Name == assemblyName);

    private IEnumerable<MethodDefinition> MethodsToWeave(TypeDefinition type)
        => type.Methods.Where(MethodHasMainThreadAttribute);

    private bool MethodHasMainThreadAttribute(MethodDefinition method)
        => method.CustomAttributes.Any(a => a.AttributeType.Name == RunOnMainThreadAttributeTypeName);
}