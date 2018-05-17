using Xamarin.Forms;

namespace MvvmCross.Forms.Core
{
    public interface IMvxFormsSetup
    {
        Application FormsApplication { get; }

        IMvxFormsSetupHelper FormsSetupHelper { get; }
    }
}
