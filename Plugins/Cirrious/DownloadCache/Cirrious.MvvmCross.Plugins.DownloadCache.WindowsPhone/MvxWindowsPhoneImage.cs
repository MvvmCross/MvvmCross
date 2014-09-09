namespace Cirrious.MvvmCross.Plugins.DownloadCache.WindowsPhone
{
    public class MvxWindowsPhoneImage
        : MvxImage<byte[]>
    {
        public MvxWindowsPhoneImage(byte[] rawImage)
            : base(rawImage)
        {
        }

        public override int GetSizeInBytes()
        {
            if (RawImage == null)
                return 0;

            return RawImage.Length;
        }
    }
}