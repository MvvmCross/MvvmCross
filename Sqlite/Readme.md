### Sqlite

The `Sqlite` plugin provide local Sqlite storage via the a modified version of the Sqlite-net library.

The version of this library MvvmCross forked from is at https://github.com/praeclarum/sqlite-net/.

In SQLite-net, database entities are mapped into RAM using ORM classes like:

    public class CollectedItem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        
        public string Caption { get; set; }
        public string Notes { get; set; }

        public DateTime WhenUtc { get; set; }
        
        public bool LocationKnown { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }

        public string ImagePath { get; set; }
    }

The MvvmCross plugin version allows local databases to be opened/created using the interface:

    public interface ISQLiteConnectionFactory
    {
        ISQLiteConnection Create(string address);
    }

This can be used:

    var factory = Mvx.Resolve<ISQLiteConnectionFactory>();
    // open or create the database
    var connection = factory.Create("mydb.sql");
    // ensure our tables exist
    connection.CreateTable<CollectedItem>();
    
Once opened/created, SQLite database connections can be used like:

    // Create
    connection.Insert(item);
    
    // Update
    connection.Update(item);
    
    // Delete
    connection.Delete(item);
    
    // Select
    var item = connection.Table<CollectedItem>.FirstOrDefault(x => x.Id == 42);

For more on the capabilities and use of sqlite-net, see https://github.com/praeclarum/sqlite-net/.

For samples using sqlite-net, see:

- N=10 - KittensDb - http://slodge.blogspot.co.uk/2013/05/n10-sqlite-persistent-data-storage-n1.html
- N=12 to N=17 - CollectABull - http://slodge.blogspot.co.uk/2013/05/n12-collect-bull-full-app-part-1-n1.html
 
The file location used by the sqlite-net `factory.Create` call is not ideal:

- Android and iOS - `Environment.SpecialFolder.Personal`
- WindowsPhone - IsolatedStorage root
- WindowsStore - Windows.Storage.ApplicationData.Current.LocalFolder
- Wpf - `Directory.GetCurrentDirectory()`

This may need changing and unifying in future versions of the plugin. To avoid these default locations, providing absolute paths is possible on some platforms.

Use of the Sqlite plugin on WindowsStore (and to some extent Wpf) is complicated by Windows native x86, x64 and ARM Sqlite.dll versions. For some discussion on this, see https://github.com/slodge/MvvmCross/issues/307

The future of this plugin is that we do want to reintegrate with the core Sqlite-net version - see https://github.com/praeclarum/sqlite-net/issues/135.

Until this reintegration occurs, we aren't actively extending this plugin - we don't want to diverge from the core platform if we can help it.

When this reintegration occurs, we do hope to include Sqlite-net async support.