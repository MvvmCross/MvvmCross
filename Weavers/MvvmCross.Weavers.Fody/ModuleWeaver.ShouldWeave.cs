using System.Linq;
using Mono.Cecil;

public partial class ModuleWeaver
{
    private bool ShouldWeave(MethodDefinition method)
    {
        if (method.ReturnType != _void)
        {
            var sequencePoint = method.DebugInformation.SequencePoints.FirstOrDefault();
            LogErrorPoint(ReturnMustBeVoid, sequencePoint);

            return false;
        }

        return true;
    }
}
