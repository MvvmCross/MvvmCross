
namespace FirstDemo.Mac
{
	// Should subclass MonoMac.AppKit.NSView
	[MonoMac.Foundation.Register("FirstView")]
	public partial class FirstView
	{
	}
	// Should subclass MonoMac.AppKit.NSViewController
	[MonoMac.Foundation.Register("FirstViewController")]
	public partial class FirstViewController
	{
	}
}

