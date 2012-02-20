namespace Cirrious.MvvmCross.Interfaces.Services.Location
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