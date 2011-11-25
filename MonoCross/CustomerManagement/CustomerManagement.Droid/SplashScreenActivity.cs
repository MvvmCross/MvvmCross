using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using MonoCross.Droid;
using MonoCross.Navigation;

using CustomerManagement.Shared.Model;

namespace CustomerManagement.Droid
{
    [Activity(Label = "SplashScreenActivity", Theme = "@android:style/Theme.Black.NoTitleBar", MainLauncher = true, Icon = "@drawable/icon", NoHistory = true)]
    public class SplashScreenActivity: Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // assign a layout with an image
            SetContentView(Resource.Layout.Splash);

            CheckFiles(ApplicationContext);

            // initialize app
            MXDroidContainer.Initialize(new CustomerManagement.App(), this.ApplicationContext);

            // initialize views
            MXDroidContainer.AddView<List<Customer>>(typeof(Views.CustomerListView), ViewPerspective.Default);
            MXDroidContainer.AddView<Customer>(typeof(Views.CustomerView), ViewPerspective.Default);
            MXDroidContainer.AddView<Customer>(typeof(Views.CustomerEditView), ViewPerspective.Update);

            // navigate to first view
            MXDroidContainer.Navigate(null, MXContainer.Instance.App.NavigateOnLoad);
        }

        protected override void OnResume()
        {
			base.OnResume();
			
        }

        /// <summary>
        /// Copies the contents of input to output. Doesn't close either stream.
        /// </summary>
        public void CopyStream(Stream input, Stream output)
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
            string documents = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);

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