
namespace PageRendererExample
{
    public class ImageHolder : IImageHolder
    {
        public byte[] ImageBytes { get; private set; }

        public string MimeType { get; private set; }

        public void Update(byte[] imageBytes, string mimeType)
        {
            ImageBytes = imageBytes;
            MimeType = mimeType;
        }
    }
}

