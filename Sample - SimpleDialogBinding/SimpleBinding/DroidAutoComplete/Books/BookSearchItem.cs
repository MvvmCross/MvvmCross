namespace DroidAutoComplete.Books
{
    public class BookSearchItem : Java.Lang.Object
    {
        public string kind { get; set; }
        public string id { get; set; }
        public VolumeInfo volumeInfo { get; set; }

        public override string ToString()
        {
            return volumeInfo.title;
        }
    }
}