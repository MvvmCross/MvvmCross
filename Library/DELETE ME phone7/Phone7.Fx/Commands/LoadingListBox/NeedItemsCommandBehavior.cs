namespace Phone7.Fx.Commands.LoadingListBox
{
    public class NeedItemsCommandBehavior : CommandBehaviorBase<Controls.LoadingListBox>
    {
        public NeedItemsCommandBehavior(Controls.LoadingListBox targetObject)
            : base(targetObject)
        {
            targetObject.NeedItems +=
                (o, args) => ExecuteCommand();

        }
    }
}