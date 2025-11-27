using System.Diagnostics.CodeAnalysis;
using MvvmCross.Platforms.Ios.Core;
using Playground.Core;

namespace Playground.iOS;

[Register("SceneDelegate")]
[RequiresUnreferencedCode("Uses MvvmCross reflection based plugin loading")]
#pragma warning disable IL2026 // Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code
public class SceneDelegate : MvxSceneDelegate<Setup, App>;
#pragma warning restore IL2026 // Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code
