using Mono.Cecil;

public partial class ModuleWeaver
{
    public bool IsMvxMainThreadDispatchingObject(TypeReference type)
    {
        TypeDefinition definition;
        while ((definition = type?.Resolve()) != null)
        {
            if (IsSameAs(definition.BaseType, _mvxMainThreadDispatchingObject)) return true;

            type = definition.BaseType;
        }

        return false;
    }

    public static bool IsSameAs(TypeReference self, TypeReference other)
    {
        if (ReferenceEquals(self, null) || ReferenceEquals(other, null)) return false;

        return self.FullName == other.FullName && GetAssemblyName(self) == GetAssemblyName(other);
    }

    public static string GetAssemblyName(TypeReference self)
    {
        switch (self.Scope)
        {
            case AssemblyNameReference assemblyName:
                return assemblyName.FullName;
            case ModuleReference moduleReference:
                return moduleReference.Name;
            default:
                return ((ModuleDefinition)self.Scope).Assembly.FullName;
        }
    }
}
