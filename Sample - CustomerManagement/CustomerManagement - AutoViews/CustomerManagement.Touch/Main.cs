using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace CustomerManagement.Touch
{
	public class Application
	{
		// This is the main entry point of the application.
		static void Main (string[] args)
		{
			var rootPath = string.Empty;
			rootPath = Path.Combine(rootPath, "Xml");
			var xmlPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			xmlPath = Path.Combine(xmlPath, "Xml");
			if (!Directory.Exists(xmlPath)) { Directory.CreateDirectory(xmlPath); }
			var filenames = new string[] { "Customers.xml" };
			foreach(string filename in filenames)
			{
				if (!File.Exists(Path.Combine(xmlPath, filename))) 
				{ 
					File.Copy(Path.Combine(rootPath, filename), Path.Combine(xmlPath, filename));
				}
			}			

			// Start the UI for the application
			UIApplication.Main (args, null, "AppDelegate");
		}
	}
}
