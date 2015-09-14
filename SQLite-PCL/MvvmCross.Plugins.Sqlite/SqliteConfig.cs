using SQLite.Net;
using SQLite.Net.Interop;

namespace MvvmCross.Plugins.Sqlite
{
    public class SqLiteConfig
    {
        public SqLiteConfig(
            string databaseName, 
            bool storeDateTimeAsTicks = true,
            IBlobSerializer serializer = null,
            IContractResolver resolver = null)
        {
            DatabaseName = databaseName;
            StoreDateTimeAsTicks = storeDateTimeAsTicks;

            BlobSerializer = serializer;
        }
        public string DatabaseName { get; set; }
        public bool StoreDateTimeAsTicks { get; set; }

        public IContractResolver ContractResolver { get; set; }
        public IBlobSerializer BlobSerializer { get; set; }
    }
}
