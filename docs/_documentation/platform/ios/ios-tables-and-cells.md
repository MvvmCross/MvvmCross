---
layout: documentation
title: iOS Tables and Cells
category: Platforms
---

## Available table source classes in MvvmCross

Abstract classes

- [MvxBaseTableViewSource](https://github.com/MvvmCross/MvvmCross/blob/master/MvvmCross/Platforms/Ios/Binding/Views/MvxBaseTableViewSource.cs)
  - base functionality only 
  - no `ItemsSource` - generally not used directly
- [MvxTableViewSource.cs](https://github.com/MvvmCross/MvvmCross/blob/master/MvvmCross/Platforms/Ios/Binding/Views/MvxTableViewSource.cs)
  - inherits from the basetable and adds `ItemsSource` for data-binding
  - inheriting classes need only to implement `protected abstract UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item);`
- [MvxExpandableTableViewSource.cs](https://github.com/MvvmCross/MvvmCross/blob/master/MvvmCross/Platforms/Ios/Views/MvxExpandableTableViewSource.cs)
  - inherits from [MvxTableViewSource.cs](https://github.com/MvvmCross/MvvmCross/blob/master/MvvmCross/Platforms/Ios/Binding/Views/MvxTableViewSource.cs)
  - changes `ItemSource` to `IEnumerable<TItemSource> ItemsSource` for data-binding groups
  - inheriting classes need to implement `protected abstract UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item);` and register a header cell and table cell

Concrete classes

- [MvxStandardTableViewSource.cs](https://github.com/MvvmCross/MvvmCross/blob/master/MvvmCross/Platforms/Ios/Binding/Views/MvxStandardTableViewSource.cs)
  - inherits from `MvxTableViewSource`
  - provides the 'standard iPhone cell types' via `UITableViewCellStyle` 
  - within these you can bind `TitleText`, `DetailText`, and (with some teasing) Accessory
- [MvxSimpleTableViewSource.cs](https://github.com/MvvmCross/MvvmCross/blob/master/MvvmCross/Platforms/Ios/Binding/Views/MvxSimpleTableViewSource.cs)
  - inherits from `MvxTableViewSource`
  - provides a single cell type for all items in the collection - via `string nibName` in the `ctor`
  - within these cells you can bind what you like - see videos (later)
- [MvxActionBasedTableViewSource.cs](https://github.com/MvvmCross/MvvmCross/blob/master/MvvmCross/Platforms/Ios/Binding/Views/MvxActionBasedTableViewSource.cs)
  - provides some `Func<>`style hooks to allow you to implement `GetOrCreateCellFor` without inheriting a new class from `MvxTableViewSource`

## Custom table source example

A typical TableSource with multiple cell types typically looks like:

```c#
public class PolymorphicListItemTypesView : MvxTableViewController
{
    public PolymorphicListItemTypesView()
    {
        Title = "Poly List";
    }

    public override void ViewDidLoad()
    {
        base.ViewDidLoad();

        var source = new TableSource(TableView);
        this.AddBindings(new Dictionary<object, string>
            {
                {source, "ItemsSource Animals"}
            });

        TableView.Source = source;
        TableView.RowHeight = KittenCell.GetCellHeight();
        TableView.ReloadData();
    }

    public class TableSource : MvxTableViewSource
    {
        private static readonly NSString KittenCellIdentifier = new NSString("KittenCell");
        private static readonly NSString DogCellIdentifier = new NSString("DogCell");

        public TableSource(UITableView tableView)
            : base(tableView)
        {
            tableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
            tableView.RegisterNibForCellReuse(UINib.FromName("KittenCell", NSBundle.MainBundle),
                                              KittenCellIdentifier);
            tableView.RegisterNibForCellReuse(UINib.FromName("DogCell", NSBundle.MainBundle), DogCellIdentifier);
        }

        protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath,
                                                              object item)
        {
            NSString cellIdentifier;
            if (item is Kitten)
            {
                cellIdentifier = KittenCellIdentifier;
            }
            else if (item is Dog)
            {
                cellIdentifier = DogCellIdentifier;
            }
            else
            {
                throw new ArgumentException("Unknown animal of type " + item.GetType().Name);
            }

            return (UITableViewCell) TableView.DequeueReusableCell(cellIdentifier, indexPath);
        }
    }
}
```
### Implementing sections

In the example above, `GetOrCreateCellFor` will try to bind your data 1 to 1 with your data source.
If you would like to implement sections, then you need to manually tell your `MvxTableViewSource` which data should be bound with which view.
To do this, override `GetItemAt` and return the `object` that you want to have bound to the `UITableViewCell` when `GetOrCreateCellFor` is called.

An example may be 
```c#
class MySection
{
    public string SectionTitle { get; set; }
    public List<MyRow> MyRows { get; set; }
}

class MyRow
{
    public string InterestingRowData { get; set; }
}

private List<MySection> MySections = new List<MySection>()
{
    new MySection(){SectionTitle = "Section1", MyRows = new List<MyRow>(){ new MyRow() { InterestingRowData = "Data for Section1 Row1"} } }
};

protected override object GetItemAt(NSIndexPath indexPath)
{
    return MySections[indexPath.Section].MyRows[indexPath.Row];
}

public override string TitleForHeader(UITableView tableView, nint section)
{
    return MySections[(int)section].SectionTitle;
}
```


## References

For examples of creating custom tables and cells:

- there's a lot of demos of table use in [the N+1 series](http://mvvmcross.wordpress.com)
  - N=2 and N=3 are very basic
  - N=6 and N=6.5 covers a book list (a good place to start)
  - N=11 covers collection views
  - N=12 to k=17 make a large app with a list/table from a database
- the [Working with Collections]((https://github.com/MvvmCross/MvvmCross-Samples/tree/master/WorkingWithCollections)) sample has quite a lot of Table and List code
- there are other samples available - see [MvvmCross-Samples](https://github.com/MvvmCross/MvvmCross-Samples) (or just search on the web for mvvmcross - others are also posting samples)
- Custom Cells Without Using NIB files - [benjaminhysell blog](http://benjaminhysell.com/archive/2014/04/mvvmcross-custom-mvxtableviewcell-without-a-nib-file/)
