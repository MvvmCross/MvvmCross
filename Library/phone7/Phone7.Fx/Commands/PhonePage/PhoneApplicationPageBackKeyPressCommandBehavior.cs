namespace Phone7.Fx.Commands.PhonePage
{
    public class PhoneApplicationPageBackKeyPressCommandBehavior : CommandBehaviorBase<Microsoft.Phone.Controls.PhoneApplicationPage>
    {

        public PhoneApplicationPageBackKeyPressCommandBehavior(Microsoft.Phone.Controls.PhoneApplicationPage @object)
            : base(@object)
        {
            @object.BackKeyPress += (s, e) => ExecuteCommand(e);
            
        }

        private void ExecuteCommand(System.ComponentModel.CancelEventArgs e)
        {
            this.Command.Execute(e);
        }

        

       
    }
}