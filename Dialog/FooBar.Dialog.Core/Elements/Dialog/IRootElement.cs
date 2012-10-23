namespace Foobar.Dialog.Core.Elements
{
    public interface IRootElement
    {
        IGroup Group { get; set; }
        void Add(ISection section);
    }
}