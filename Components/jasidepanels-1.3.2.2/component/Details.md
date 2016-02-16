JASidePanels is a UIViewController container designed for presenting a center panel with revealable side panels - one to the left and one to the right. 

The main inspiration for this project is the menuing system in Path 2.0 and Facebook's iOS apps.

##  Example 1 : Code


	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		// class-level declarations
		UIWindow window;
		JASidePanelController viewController;

		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			window = new UIWindow (UIScreen.MainScreen.Bounds);

			// set up the actual view controller here
			viewController = new JASidePanelController();
			viewController.ShouldDelegateAutorotateToVisiblePanel = false;

			// set up the controller panes here
			viewController.LeftPanel = new JALeftViewController();
			viewController.CenterPanel = new UINavigationController (new JACenterViewController ());
			viewController.RightPanel = new JARightViewController();

			// off we go
			window.RootViewController = viewController;
			window.MakeKeyAndVisible ();

			return true;
		}
	}

## Example 2 : Storyboards

1. Create a subclass of `JASidePanelController`. In this example we call it `MySidePanelController`.
2. In the Storyboard designer, the root view's owner as `MySidePanelController`.
3. Add more view controllers to your Storyboard, give them ids "leftViewController", "centerViewController" and "rightViewController". 
4. Override the method `AwakeFromNib` to `MySidePanelController.cs` with the following code:


    public override void AwakeFromNib ()
    {
        LeftPanel = Storyboard.InstantiateViewController ("leftViewController");
        CenterPanel = Storyboard.InstantiateViewController ("centerViewController");
        RightPanel = Storyboard.InstantiateViewController ("rightViewController");
    }

## Extension Method

An extension method is also provided in the project. This adds a single convenience method to `UIViewController`. The method provides access to the nearest `JASidePanelController` ancestor in your view controller heirarchy. It behaves similar to the NavigationController UIViewController property provided by Apple. Here's an example:

    public class JALeftViewController : UIViewController
    {
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
            // sweet, I can access my parent JASidePanelController as an extension method!
    		this.SidePanelController ().ShowCenterPanelAnimated (true);
		}
    }
    
## Requirements


JASidePanels requires iOS 5.0+


