namespace Cirrious.MvvmCross.Interfaces.Platform.Location
{
    public class MvxLocationError
    {
        public MvxLocationErrorCode Code { get; private set; }

        public MvxLocationError(MvxLocationErrorCode code)
        {
            Code = code;
        }
    }
}