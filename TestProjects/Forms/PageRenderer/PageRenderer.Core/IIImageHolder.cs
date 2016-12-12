
namespace PageRendererExample
{
    public interface IImageHolder
    {        
        byte[] ImageBytes { get; }
        string MimeType { get;  }

        void Update(byte[] imageBytes, string mimeType);
    }
}

