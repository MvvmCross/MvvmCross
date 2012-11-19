using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;

using MonoCross.Navigation;
using MonoCross.Droid;

using CustomerManagement.Shared.Model;
using System.IO;

namespace CustomerManagement.Droid
{
    [BroadcastReceiver]
    [IntentFilter(new string[] { "MonoCross.MainReceiver.CustomerManagement" })]
    public class MainReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            Android.Util.Log.Debug("MainReceiver", "OnReceive");

            CheckFiles(context);
			
			if (MXContainer.Instance == null)
			{
	            // initialize app
	            MXDroidContainer.Initialize(new CustomerManagement.App(), context.ApplicationContext);
	
	            // initialize views
	            MXDroidContainer.AddView<List<Company>>(typeof(CustomerListView), ViewPerspective.Default);
	            MXDroidContainer.AddView<Company>(typeof(Views.CustomerView), ViewPerspective.Default);
	            MXDroidContainer.AddView<Company>(typeof(Views.CustomerEditView), ViewPerspective.Update);
			}
			
            // navigate to first view
            MXDroidContainer.Navigate(null, MXContainer.Instance.App.NavigateOnLoad);
        }

        /// <summary>
        /// Copies the contents of input to output. Doesn't close either stream.
        /// </summary>
        public static void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[8 * 1024];
            int len;
            while ((len = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, len);
            }
        }

        public void CheckFiles(Context context)
        {
            string documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            string dataDirectory = Path.Combine(documents, "Xml");
            if (!Directory.Exists(dataDirectory))
                Directory.CreateDirectory(dataDirectory);

            string dataFile = Path.Combine(documents, @"Xml/Customers.xml");
            if (File.Exists(dataFile))
                return;

            Stream input = context.Assets.Open(@"Xml/Customers.xml");
            FileStream output = File.Create(dataFile);
            CopyStream(input, output);
            input.Close();
            output.Close();
        }
    }
}